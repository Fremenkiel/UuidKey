using UuidKeySharp.Enums;

namespace UuidKeySharp.Entities;

/// <summary>
/// Configuration options for key generation and formatting operations.
/// </summary>
public class KeyOptions {
    /// <summary>
    /// Gets or sets whether hyphens should be included in the key format.
    /// Default value is false.
    /// </summary>
    public bool Hyphens { get; set; } = false;

    /// <summary>
    /// Gets or sets the number of Crockford Base32 characters to use for entropy.
    /// Default value is EntropyBits160.
    /// </summary>
    public NumOfCrock32Chars NumOfCrock32Chars { get; set; } = NumOfCrock32Chars.EntropyBits160;
}
