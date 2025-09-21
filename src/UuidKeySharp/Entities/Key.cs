using System.Text.RegularExpressions;
using CrockfordBase32;
using UuidKeySharp.Utils;
using Regex = System.Text.RegularExpressions.Regex;

namespace UuidKeySharp.Entities;

public class Key(string apiKey, bool withoutHyphens = false)
{
    private const int KeyPartLength = 7;
    private const int KeyLengthWithHyphens = KeyPartLength * 4 + 3;
    public const int KeyLengthWithoutHyphens = KeyPartLength * 4;
    
    private readonly CrockfordBase32Encoding _encoder = new();

    public Guid Uuid()
    {
        if (!IsValid())
            throw new Exception("invalid UUID key");

        return Decode();
    }

    public new string ToString()
    {
        return apiKey;
    }

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

    private Guid Decode()
    {
        var parts = CreateParts()
            .Select(part => HexConverter.ToHex(_encoder.Decode(part, false)!.Value)).ToList();

        return Guid.Parse(
            string.Join("-", parts[0], parts[1][..4], parts[1][4..], parts[2][..4], parts[2][4..] + parts[3])
        );
    }

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
