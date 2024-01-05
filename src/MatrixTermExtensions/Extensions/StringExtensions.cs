using System.Diagnostics.CodeAnalysis;

namespace MatrixTermExtensions.Extensions
{
    internal static partial class StringExtensions
    {
        internal static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
            => string.IsNullOrEmpty(str);

        internal static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str)
            => string.IsNullOrWhiteSpace(str);
    }
}
