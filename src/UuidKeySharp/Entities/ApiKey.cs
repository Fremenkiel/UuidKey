using UuidKeySharp.Utils;

namespace UuidKeySharp.Entities;

/// <summary>
/// Represents an API key composed of a prefix, UUID key, entropy, and CRC32 checksum.
/// The API key format is: {Prefix}_{Key}{Entropy}_{Checksum}
/// </summary>
public class ApiKey
{
    /// <summary>
    /// Gets the prefix portion of the API key.
    /// </summary>
    public string Prefix { get; }

    /// <summary>
    /// Gets the UUID key portion of the API key.
    /// </summary>
    public Key Key { get; }

    /// <summary>
    /// Gets the entropy portion of the API key for additional randomness.
    /// </summary>
    public string Entropy { get; }

    /// <summary>
    /// Gets or sets the CRC32 checksum for validating the API key integrity.
    /// </summary>
    public string Checksum { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the ApiKey class with the specified components.
    /// </summary>
    /// <param name="prefix">The prefix for the API key</param>
    /// <param name="key">The UUID key component</param>
    /// <param name="entropy">The entropy string for additional randomness</param>
    public ApiKey(string prefix, Key key, string entropy)
    {
        Prefix = prefix;
        Key = key;
        Entropy = entropy;
        CalculateChecksum();
    }

    /// <summary>
    /// Returns the complete API key string in the format: {Prefix}_{Key}{Entropy}_{Checksum}
    /// </summary>
    /// <returns>The formatted API key string</returns>
    public new string ToString()
    {
        return $"{Prefix}_{Key.ToString()}{Entropy}_{Checksum}";
    }

    /// <summary>
    /// Calculates and sets the CRC32 checksum for the API key components.
    /// The checksum is computed from the concatenated prefix, key, and entropy.
    /// </summary>
    private void CalculateChecksum()
    {
        var data = $"{Prefix}_{Key.ToString()}{Entropy}";
        var crc32 = new Crc32();
        var crc = crc32.ComputeChecksum(data);
        Checksum = HexConverter.ToHex(crc).ToUpper();
    }
}
