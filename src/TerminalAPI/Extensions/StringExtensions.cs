using System.Diagnostics.CodeAnalysis;

namespace TerminalAPI.Extensions;

internal static partial class StringExtensions
{
    internal static bool IsNullOrEmpty([NotNullWhen(true)] this string? value)
        => string.IsNullOrEmpty(value);

    internal static bool IsNullOrWhiteSpace([NotNullWhen(true)] this string? value)
        => string.IsNullOrWhiteSpace(value);
}
