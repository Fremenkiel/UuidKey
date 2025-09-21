using UuidKeySharp.Entities;
using UuidKeySharp.Enums;
using UuidKeySharp.Utils;

namespace UuidKeySharp.Interfaces;

public interface IUuidKey
{
    ApiKey NewApiKey(string prefix, string uuid,
        NumOfCrock32Chars numOfCrock32Chars = NumOfCrock32Chars.EntropyBits160);

    ApiKey ParseApiKey(string apiKeyString);

    Guid GenerateUuid();

    Key Encode(Guid uuid);

    Key Encode(string u, bool withoutHyphens = false);

    Key Parse(string key);
}