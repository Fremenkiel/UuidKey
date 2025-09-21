using Microsoft.Extensions.DependencyInjection;
using UuidKeySharp.Entities;
using UuidKeySharp.Enums;
using UuidKeySharp.Interfaces;
using UuidKeySharp.Test.Setup;
using Xunit;

namespace UuidKeySharp.Test;

public class UuidKeyTest : IClassFixture<TestFixture>
{
    private readonly IUuidKey _uuidKey;
    public UuidKeyTest(TestFixture fixture)
    {
        _uuidKey = fixture.ServiceProvider.GetService<IUuidKey>()!;
    }
    
    [Fact]
    public void Generate_Uuid7()
    {
        // Act
        var uuid = _uuidKey.GenerateUuid();
        
        // Assert
        Assert.IsType<Guid>(uuid);
    }
    
    [Fact]
    public void Generate_New_Api_key()
    {
        // Arrange
        const string prefix = "KSWA";
        var uuid = Guid.Parse("d1756360-5da0-40df-9926-a76abff5601d");
        var key = _uuidKey.Encode(uuid.ToString(), true);
        
        // Act
        var apiKey = _uuidKey.NewApiKey(prefix, uuid.ToString());
        var apiKeyString = apiKey.ToString();
        var apiKeyArray = apiKeyString.Split('_');
        
        // Assert
        Assert.IsType<ApiKey>(apiKey);
        Assert.NotNull(apiKey);
        Assert.Equal(prefix, apiKey.Prefix);
        Assert.Equal(key.ToString(), apiKey.Key.ToString());
        Assert.Equal(3, apiKeyArray.Length);
        Assert.Equal(prefix, apiKeyArray[0]);
        Assert.Equal(key.ToString(), apiKeyArray[1][..28]);
        Assert.Equal(uuid, apiKey.Key.Uuid());
        Assert.Equal(apiKey.Checksum, apiKeyArray[2]);
    }
    
    [Fact]
    public void Number_Of_Crock32_Chars()
    {
        // Arrange
        const string prefix = "AGNTSTNP";
        var uuid = Guid.Parse("d1756360-5da0-40df-9926-a76abff5601d");
        
        // Act
        var apiKey128 = _uuidKey.NewApiKey(prefix, uuid.ToString(), NumOfCrock32Chars.EntropyBits128);
        var apiKey160 = _uuidKey.NewApiKey(prefix, uuid.ToString());
        var apiKey256 = _uuidKey.NewApiKey(prefix, uuid.ToString(), NumOfCrock32Chars.EntropyBits256);
        
        var apiKey128Array = apiKey128.ToString().Split('_');
        var apiKey160Array = apiKey160.ToString().Split('_');
        var apiKey256Array = apiKey256.ToString().Split('_');
        
        // Assert
        Assert.IsType<ApiKey>(apiKey128);
        Assert.IsType<ApiKey>(apiKey256);
        Assert.IsType<ApiKey>(apiKey160);
        
        Assert.NotNull(apiKey128);
        Assert.NotNull(apiKey160);
        Assert.NotNull(apiKey256);
        
        Assert.NotEqual(apiKey128, apiKey160);
        Assert.NotEqual(apiKey160, apiKey256);
        Assert.NotEqual(apiKey128, apiKey256);
        
        Assert.Equal(apiKey128Array[0], apiKey160Array[0]);
        Assert.Equal(apiKey160Array[0], apiKey256Array[0]);
        Assert.Equal(apiKey128Array[0], apiKey256Array[0]);
        
        Assert.Equal(apiKey128Array[1][..28], apiKey160Array[1][..28]);
        Assert.Equal(apiKey160Array[1][..28], apiKey256Array[1][..28]);
        Assert.Equal(apiKey128Array[1][..28], apiKey256Array[1][..28]);
        
        Assert.Equal(14, apiKey128Array[1][28..].Length);
        Assert.Equal(21, apiKey160Array[1][28..].Length);
        Assert.Equal(42, apiKey256Array[1][28..].Length);
        
        
        Assert.NotEqual(apiKey128Array[2], apiKey160Array[2]);
        Assert.NotEqual(apiKey160Array[2], apiKey256Array[2]);
        Assert.NotEqual(apiKey128Array[2], apiKey256Array[2]);
    }
    
    [Fact]
    public void Parse_Api_key()
    {
        // Arrange
        const string apiKeyString = "AGNTSTNP_38QARV01ET0G6Z2CJD9VA2ZZAR0XJJLSO7WBNWY3F_96FDB498";
        var uuid = Guid.Parse("d1756360-5da0-40df-9926-a76abff5601d");
        var apiKeyArray = apiKeyString.Split('_');
        
        // Act
        var apiKey = _uuidKey.ParseApiKey(apiKeyString);
        
        // Assert
        Assert.IsType<ApiKey>(apiKey);
        Assert.NotNull(apiKey);
        Assert.Equal(apiKeyArray[0], apiKey.Prefix);
        Assert.Equal(apiKeyArray[1][..28], apiKey.Key.ToString());
        Assert.Equal(apiKeyArray[2], apiKey.Checksum);
        Assert.Equal(apiKeyString, apiKey.ToString());
        Assert.Equal(uuid, apiKey.Key.Uuid());
    }
    
    [Fact]
    public void Encode_Uuid()
    {
        // Arrange
        var uuid = Guid.Parse("d1756360-5da0-40df-9926-a76abff5601d");
        
        // Act
        var key = _uuidKey.Encode(uuid);
        var keyWithoutHyphens = _uuidKey.Encode(uuid, true);
        
        // Assert
        Assert.IsType<Key>(key);
        Assert.NotNull(key);
        Assert.Equal("38QARV0-1ET0G6Z-2CJD9VA-2ZZAR0X", key.ToString());
        Assert.Equal(uuid, key.Uuid());
        
        Assert.IsType<Key>(keyWithoutHyphens);
        Assert.NotNull(keyWithoutHyphens);
        Assert.Equal("38QARV01ET0G6Z2CJD9VA2ZZAR0X", keyWithoutHyphens.ToString());
        Assert.Equal(uuid, keyWithoutHyphens.Uuid());
    }
    
    [Fact]
    public void Decode_Uuid()
    {
        // Arrange
        var keyString = "38QARV0-1ET0G6Z-2CJD9VA-2ZZAR0X";
        var keyStringWithoutHyphens = "38QARV01ET0G6Z2CJD9VA2ZZAR0X";
        
        // Act
        var key = _uuidKey.Parse(keyString);
        var keyWithoutHyphens = _uuidKey.Parse(keyStringWithoutHyphens);
        
        // Assert
        Assert.IsType<Key>(key);
        Assert.NotNull(key);
        Assert.Equal(keyString, key.ToString());
        Assert.Equal(Guid.Parse("d1756360-5da0-40df-9926-a76abff5601d"), key.Uuid());
        
        Assert.IsType<Key>(keyWithoutHyphens);
        Assert.NotNull(keyWithoutHyphens);
        Assert.Equal(keyStringWithoutHyphens, keyWithoutHyphens.ToString());
        Assert.Equal(Guid.Parse("d1756360-5da0-40df-9926-a76abff5601d"), keyWithoutHyphens.Uuid());
    }
}