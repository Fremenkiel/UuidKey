using UuidKeySharp.Entities;
using UuidKeySharp.Enums;

namespace UuidKeySharp.Interfaces;

/// <summary>
/// Interface for UUID key operations including API key generation, parsing, and encoding/decoding of UUIDs and keys.
/// </summary>
public interface IUuidKey
{
    /// <summary>
    /// Creates a new API key with the specified prefix and UUID.
    /// </summary>
    /// <param name="prefix">The prefix to use for the API key</param>
    /// <param name="uuid">The UUID string to include in the API key</param>
    /// <param name="numOfCrock32Chars">The number of Crockford Base32 characters to use for entropy (default: 160 bits)</param>
    /// <returns>A new ApiKey instance</returns>
    ApiKey NewApiKey(string prefix, string uuid,
        NumOfCrock32Chars numOfCrock32Chars = NumOfCrock32Chars.EntropyBits160);

    /// <summary>
    /// Parses an API key string and returns the corresponding ApiKey object.
    /// </summary>
    /// <param name="apiKeyString">The API key string to parse</param>
    /// <returns>The parsed ApiKey object</returns>
    ApiKey ParseApiKey(string apiKeyString);

    /// <summary>
    /// Generates a new UUID.
    /// </summary>
    /// <returns>A new Guid instance</returns>
    Guid GenerateUuid();

    /// <summary>
    /// Encodes a UUID into a Key object.
    /// </summary>
    /// <param name="uuid">The UUID to encode</param>
    /// <param name="withoutHyphens">Whether the UUID string is formatted without hyphens (default: false)</param>
    /// <returns>A Key object representing the encoded UUID</returns>
    Key Encode(Guid uuid, bool withoutHyphens = false);

    /// <summary>
    /// Encodes a UUID string into a Key object.
    /// </summary>
    /// <param name="u">The UUID string to encode</param>
    /// <param name="withoutHyphens">Whether the UUID string is formatted without hyphens (default: false)</param>
    /// <returns>A Key object representing the encoded UUID</returns>
    Key Encode(string u, bool withoutHyphens = false);

    /// <summary>
    /// Parses a key string and returns the corresponding Key object.
    /// </summary>
    /// <param name="key">The key string to parse</param>
    /// <returns>The parsed Key object</returns>
    Key Parse(string key);
}