using System.Text;
using UuidKeySharp.Utils.Blake2Sharp;
using UuidKeySharp.Enums;
using CrockfordBase32;

namespace UuidKeySharp;

public class UuidKey
{
    private const int EntropyBytesMultiplier = 8 / 5;
    private const int InitialEntropyBytes = 32;
    private const int ChecksumLength = 8;
    
    private readonly CrockfordBase32Encoding _encoder = new();
    private readonly Hasher _hasher = Blake2B.Create(new Blake2BConfig { OutputSizeInBytes = 32 });

    public ApiKey NewApiKey(string prefix, string uuid, NumOfCrock32Chars numOfCrock32Chars = NumOfCrock32Chars.EntropyBits160)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("prefix cannot be null, empty, or only whitespace.", nameof(prefix));

        var key = Encode(uuid, true);
        var entropy = GenerateEntropy(numOfCrock32Chars);

        return new ApiKey(prefix, key, entropy);
    }

    public ApiKey ParseApiKey(string apiKeyString)
    {
        if (string.IsNullOrWhiteSpace(apiKeyString))
            throw new ArgumentException("API key cannot be null, empty, or only whitespace.", nameof(apiKeyString));
        
        var parts = apiKeyString.Split('_');
        if (parts.Length != 3)
            throw new ArgumentException($"Invalid API key format: expected 3 parts, got {parts.Length}", nameof(apiKeyString));
        
        var prefix = parts[0];
        if (prefix == "")
            throw new ArgumentException("API key prefix cannot be empty", nameof(apiKeyString));

        var remainder = parts[1];

        if (remainder.Length < Key.KeyLengthWithoutHyphens)
            throw new ArgumentException("Invalid Key format: insufficient length");
        
        var keyPart = remainder[..Key.KeyLengthWithoutHyphens];
        var key = Parse(keyPart);

        var entropy = remainder[Key.KeyLengthWithoutHyphens..];

        var checksum = parts[2];
        if (checksum.Length != ChecksumLength)
            throw new ArgumentException("Invalid checksum format: must be 8 hexadecimal characters");
        
        if (Key.ApprovedCharRegex().IsMatch(checksum))
            throw new ArgumentException("Invalid checksum");
        
        var apiKey = new ApiKey(prefix, key, entropy);
        
        if (apiKey.Checksum != checksum)
            throw new ArgumentException($"Invalid checksum: expected {apiKey.Checksum}, got {checksum}");

        return apiKey;
    }
    
    public Guid GenerateUuid()
    {
        return Guid.CreateVersion7();
    }

    public Key Encode(Guid uuid)
    {
        return Encode(uuid.ToString());
    }
    
    public Key Encode(string u, bool withoutHyphens = false)
    {
        var uuid = u;
        List<string> parts = [
            EncodePart(uuid[..8]),
            EncodePart(uuid[9..13]+uuid[14..18]),
            EncodePart(uuid[19..23]+uuid[24..28]),
            EncodePart(uuid[28..36]),
        ];

        return new Key(withoutHyphens ? String.Join("", parts) : String.Join("-", parts));
    }
    
    private string EncodePart(string part)
    {
        var encoded = _encoder.Encode(Convert.ToUInt64(part, 16), false);
        var padding = 7 - encoded.Length;
        return new StringBuilder(7).Insert(0, "0", padding) + encoded;
    }

    public Key Parse(string key)
    {
        var apiKey = new Key(key, key.Length == Key.KeyLengthWithoutHyphens);
        if (!apiKey.IsValid())
            throw new ArgumentException("Invalid api key");
        
        return apiKey;
    }

    private string GenerateEntropy(NumOfCrock32Chars size)
    {
        var numOfRandomBytes = size.GetHashCode() * EntropyBytesMultiplier;
        
        var random = new Random();
        var inputBytes = new byte[InitialEntropyBytes];
        random.NextBytes(inputBytes);

        var entropy = new byte[numOfRandomBytes];
        for (var i = 0; i < numOfRandomBytes; i +=  _hasher.Length())
        {
            _hasher.Update(inputBytes, 0, numOfRandomBytes);
            var hash = _hasher.Finish();
            
            var copyLength = numOfRandomBytes - i < _hasher.Length() ? numOfRandomBytes - i : _hasher.Length();
            Buffer.BlockCopy(hash[..copyLength],0, entropy, i, copyLength);
            
            inputBytes = hash;
        }

        var entropyEncoded = "";

        for (var i = 0; i < entropy.Length; i += 8)
        {
            var end = i + 8 < entropy.Length ? i + 8 : entropy.Length;

            UInt64? n = null;
            var j = 0;
            foreach (var b in entropy[i..end])
            {
                n ??= Convert.ToUInt64(Convert.ToUInt64(b) * Math.Pow(2, 8 * (end - i - 1 - j)));

                j++;
            }
            
            var buf = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes(n!.Value), 0, buf, 0, 8);

            var start = 0;
            for (var k = 0; j < 8; j++)
            {
                if (buf[k] == 0) continue;
                start = k;
                break;
            }
            
            var encoded = _encoder.Encode(BitConverter.ToUInt64(entropy, start), false);
            entropyEncoded += encoded;
        }

        var result = entropyEncoded.ToUpper();
        if (result.Length < size.GetHashCode())
        {
            return result + new StringBuilder(7).Insert(0, "0", size.GetHashCode() - result.Length);
        }
        return result[..size.GetHashCode()];
    }
}
