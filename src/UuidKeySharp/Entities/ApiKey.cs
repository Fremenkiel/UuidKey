using UuidKeySharp.Utils;

namespace UuidKeySharp.Entities;

public class ApiKey
{
    public string Prefix { get; } 
    public Key Key { get; }
    public string Entropy { get; }
    public string Checksum { get; set; } = null!;

    public ApiKey(string prefix, Key key, string entropy)
    {
        Prefix = prefix;
        Key = key;
        Entropy = entropy;
        CalculateChecksum();
    }

    public new string ToString()
    {
        return $"{Prefix}_{Key.ToString()}{Entropy}_{Checksum}";
    }

    private void CalculateChecksum()
    {
        var data = $"{Prefix}_{Key.ToString()}{Entropy}";
        var crc32 = new Crc32();
        var crc = crc32.ComputeChecksum(data);
        Checksum = HexConverter.ToHex(crc).ToUpper();
    }
}
