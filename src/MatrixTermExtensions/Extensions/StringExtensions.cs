using System.Diagnostics.CodeAnalysis;

namespace Matrix.TerminalExtensions.Extensions
{
    internal static partial class StringExtensions
    {
        internal static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
            => string.IsNullOrEmpty(str);

        internal static bool IsNullOrWhitespace([NotNullWhen(false)] this string? str)
            => string.IsNullOrWhiteSpace(str);
    }
}
