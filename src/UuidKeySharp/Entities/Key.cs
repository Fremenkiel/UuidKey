using System.Text.RegularExpressions;
using CrockfordBase32;
using UuidKeySharp.Utils;
using Regex = System.Text.RegularExpressions.Regex;

namespace UuidKeySharp.Entities;

/// <summary>
/// Represents an Uuidkey that can be encoded/decoded between UUID and the key object.
/// Supports both hyphenated and non-hyphenated key formats.
/// </summary>
/// <param name="apiKey">The API key string to wrap</param>
/// <param name="withoutHyphens">Whether the key format excludes hyphens (default: false)</param>
public class Key(string apiKey, bool withoutHyphens = false)
{
    /// <summary>
    /// The length of each part in the key (7 characters).
    /// </summary>
    private const int KeyPartLength = 7;

    /// <summary>
    /// The total length of a key with hyphens (31 characters).
    /// </summary>
    private const int KeyLengthWithHyphens = KeyPartLength * 4 + 3;

    /// <summary>
    /// The total length of a key without hyphens (28 characters).
    /// </summary>
    public const int KeyLengthWithoutHyphens = KeyPartLength * 4;

    /// <summary>
    /// The Crockford Base32 encoder used for encoding/decoding operations.
    /// </summary>
    private readonly CrockfordBase32Encoding _encoder = new();

    /// <summary>
    /// Converts the key object to its corresponding UUID.
    /// </summary>
    /// <returns>The UUID represented by this key</returns>
    /// <exception cref="Exception">Thrown when the key is invalid</exception>
    public Guid Uuid()
    {
        if (!IsValid())
            throw new Exception("invalid UUID key");

        return Decode();
    }

    /// <summary>
    /// Returns the string representation of the key.
    /// </summary>
    /// <returns>The API key string</returns>
    public new string ToString()
    {
        return apiKey;
    }

    /// <summary>
    /// Validates whether the key has the correct format and contains valid characters.
    /// </summary>
    /// <returns>True if the key is valid, false otherwise</returns>
    public bool IsValid()
    {
        return apiKey.Length switch
        {
            KeyLengthWithHyphens when apiKey[7..8] != "-" || apiKey[15..16] != "-" || apiKey[23..24] != "-" => false,
            KeyLengthWithHyphens => IsValidPart(apiKey[..7]) && IsValidPart(apiKey[8..15]) && IsValidPart(apiKey[16..23]) &&
                                    IsValidPart(apiKey[24..31]),
            KeyLengthWithoutHyphens => IsValidPart(apiKey[..7]) && IsValidPart(apiKey[7..14]) && IsValidPart(apiKey[14..21]) &&
                                       IsValidPart(apiKey[21..28]),
            _ => false
        };
    }

    /// <summary>
    /// Validates whether a key part contains only valid Crockford Base32 characters.
    /// </summary>
    /// <param name="part">The key part to validate</param>
    /// <returns>True if the part is valid, false otherwise</returns>
    private static bool IsValidPart(string part)
    {
        if (part.Length != KeyPartLength)
        {
            return false;
        }

        for (var i = 0; i < KeyPartLength; i++)
        {
            var character = part[i..(i+1)];
            if (CharRegex.Approved().IsMatch(character) || CharRegex.Disapproved().IsMatch(character))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Decodes the key string back to its original UUID.
    /// </summary>
    /// <returns>The decoded UUID</returns>
    private Guid Decode()
    {
        var parts = CreateParts()
            .Select(part => HexConverter.ToHex(_encoder.Decode(part, false)!.Value)).ToList();

        return Guid.Parse(
            string.Join("-", parts[0], parts[1][..4], parts[1][4..], parts[2][..4], parts[2][4..] + parts[3])
        );
    }

    /// <summary>
    /// Splits the API key into its constituent parts based on whether hyphens are present.
    /// </summary>
    /// <returns>A list of key parts</returns>
    private List<string> CreateParts()
    {
        if (withoutHyphens)
        {
            return [
                apiKey[..7],
                apiKey[7..14],
                apiKey[14..21],
                apiKey[21..28],
            ];
        }
        return [
            apiKey[..7],
            apiKey[8..15],
            apiKey[16..23],
            apiKey[24..31],
        ];
    }
}
