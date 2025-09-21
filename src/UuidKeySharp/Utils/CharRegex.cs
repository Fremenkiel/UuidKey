using System.Text.RegularExpressions;

namespace UuidKeySharp.Utils;

public static partial class CharRegex
{
    [GeneratedRegex("[^0-9A-Z]")]
    public static partial Regex Approved();

    [GeneratedRegex("[ILOU]")]
    public static partial Regex Disapproved();
}