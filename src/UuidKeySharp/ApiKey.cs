using UuidKeySharp.Utils;

namespace UuidKeySharp;

public class ApiKey
{
    private string Prefix { get; } 
    private Key Key { get; }
    private string Entropy { get; }
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
