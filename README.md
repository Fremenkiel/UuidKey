```sh
                         _   _  _   _  ___ ____     _  __
                        | | | || | | ||_ _|  _ \   | |/ /___ _   _
                        | | | || | | | | || | | |  | ' // _ \ | | |
                        | |_| || |_| | | || |_| |  | . \  __/ |_| |
                         \___/  \___/ |___|____/   |_|\_\___|\__, |
                                                             |___/
```
[![License](http://img.shields.io/badge/license-mit-blue.svg?style=flat-square)](https://raw.githubusercontent.com/Fremenkiel/uuidkey/master/LICENSE)

A .NET library of the [agentstation/uuidkey](https://github.com/agentstation/uuidkey) library, originally written in Go.

## Overview

The `UuidKey` library generates secure, readable API key based on [UUID7](https://en.wikipedia.org/wiki/Universally_unique_identifier) and [Base32 Crockford](https://www.crockford.com/base32.html) hash algorithms, with some additional security measures.

The library can either be used to generate a new API key based on a given UUIOD, with a prefix, a random entropy string and a checksum using the NewApiKey() method, or to generate a more simple key based on a given UUID7 using the Encode() method.

## API Key Format

```
AGNTSTNP_38QARV01ET0G6Z2CJD9VA2ZZAR0XJJLSO7WBNWY3F_A1B2C3D8
└─────┘ └──────────────────────────┘└────────────┘ └──────┘
Prefix        Key (crock32 UUID)        Entropy      Checksum
```

### Components
1. **Prefix** - Company/application identifier (e.g., "AGNTSTNP")
2. **Key** - Base32-Crockford encoded UUID
3. **Entropy** - Additional random data (128, 160, or 256 bits)
4. **Checksum** - CRC32 checksum (8 characters) for validation

### Security Features
1. **Secret Scanning** - Formatted for GitHub Secret Scanning detection
2. **Validation** - CRC32 checksum for error detection and validation
3. **Entropy Options** - Configurable entropy levels that ensure UUIDv7 security (128, 160, or 256 bits)

## Usage
Create and parse API Keys:
```c#
var uuidKey = new UuidKey();
ApiKey apiKey;

// Create a new API Key with default settings (160-bit entropy)
apiKey = uuidKey.NewApiKey("AGNTSTNP", "d1756360-5da0-40df-9926-a76abff5601d");
Console.WriteLine(apiKey.ToString()); // Output: AGNTSTNP_38QARV01ET0G6Z2CJD9VA2ZZAR0X6M2F6CZSBVJ986M2F6CZS_9F2439A3

// Create an API Key with 128-bit entropy
apiKey = uuidKey.NewApiKey("AGNTSTNP", "d1756360-5da0-40df-9926-a76abff5601d", NumOfCrock32Chars.EntropyBits128);
Console.WriteLine(apiKey.ToString()); // Output: AGNTSTNP_38QARV01ET0G6Z2CJD9VA2ZZAR0X6M2F6CZSBVJ986M2F6CZS_9F2439A3

// Parse an existing API Key
apiKey = uuidKey.ParseApiKey("AGNTSTNP_38QARV01ET0G6Z2CJD9VA2ZZAR0XJJLSO7WBNWY3F_96FDB498");
Console.WriteLine($"Prefix: {apiKey.Prefix}, Key: {apiKey.Key.ToString()}, Entropy: {apiKey.Entropy}"); // Output: Prefix: AGNTSTNP, Key: 38QARV01ET0G6Z2CJD9VA2ZZAR0X, Entropy: JJLSO7WBNWY3F
```

Work with UUID Keys directly:
```c#
var uuidKey = new UuidKey();
Key key;
Guid uuid;

// Encoding with hyphens (default)
key = uuidKey.Encode("d1756360-5da0-40df-9926-a76abff5601d");
Console.WriteLine(key.ToString()); // Output: 38QARV0-1ET0G6Z-2CJD9VA-2ZZAR0X

// Encoding without hyphens
key = uuidKey.Encode("d1756360-5da0-40df-9926-a76abff5601d", true);
Console.WriteLine(key.ToString()); // Output: 38QARV01ET0G6Z2CJD9VA2ZZAR0X

// Decoding with hyphens
key = uuidKey.Parse("38QARV0-1ET0G6Z-2CJD9VA-2ZZAR0X");
uuid = key.Uuid();
Console.WriteLine(uuid.ToString()); // Output: d1756360-5da0-40df-9926-a76abff5601d

// Decoding without hyphens
key = uuidKey.Parse("38QARV01ET0G6Z2CJD9VA2ZZAR0X");
uuid = key.Uuid();
Console.WriteLine(uuid.ToString()); // Output: d1756360-5da0-40df-9926-a76abff5601d
```

### Credits
Credits to [agentstation](https://github.com/agentstation) for writing the base article and heading the development of the original [uuidkey](https://github.com/agentstation/uuidkey) library.

Credits to [Blake2](https://github.com/BLAKE2) for the [BLAKE2Sharp](https://github.com/blake2-net/Blake2) library.

Credits to [w3roman](https://github.com/w3roman) for the [Calculating crc32 in C#](https://stackhub.net/manuals/calculating-crc32-in-c-with-examples) hashing algorithm.

Credits to [jonsagara](https://github.com/jonsagara) for the [Crockford Base32 Core](https://github.com/jonsagara/crockford-base32-core) library.
