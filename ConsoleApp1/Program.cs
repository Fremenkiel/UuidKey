// See https://aka.ms/new-console-template for more information

using UuidKeySharp;
using UuidKeySharp.Enums;

Console.WriteLine("Hello, World!");

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