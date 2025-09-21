using UuidKeySharp.Enums;

namespace UuidKeySharp.Entities;

public class KeyOptions {
    public bool Hyphens { get; set; } = false;
    public NumOfCrock32Chars NumOfCrock32Chars { get; set; } = NumOfCrock32Chars.EntropyBits160;
}
