namespace UuidKeySharp.Utils;

public static class HexConverter
{
    public static string ToHex(ulong u)
    {
        var bytes = BitConverter.GetBytes(u).Reverse().ToArray();
        var hex = Convert.ToHexStringLower(bytes);
        while (hex[..1] == "0" && hex.Length > 8)
        {
            hex = hex[1..];
        }

        return hex;
    }
}
