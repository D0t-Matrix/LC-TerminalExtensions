using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TerminalAPI.Models;

public class ArgumentStream
{
    public string[] Arguments { get; }

    public int Index { get; set; }

    public bool EndOfStream => Index >= Arguments.Length;

    public ArgumentStream(string[] arguments) => Arguments = arguments;

    public void Reset() => Index = 0;

    public bool TryReadNext(Type type, [NotNullWhen(true)] out object? value)
    {
        if (!EndOfStream)
            return StringConverter.TryConvert(this.Arguments[this.Index++], type, out value);

        value = null;
        return false;
    }

    public bool TryReadRemaining([NotNullWhen(true)] out string? result)
    {
        if (EndOfStream)
        {
            result = null;
            return false;
        }

        result = string.Join(" ", ((IEnumerable<string>)Arguments).Skip(Index));
        return true;
    }

    public bool TryReadNext([NotNullWhen(true)] out string? value)
    {
        if (EndOfStream)
        {
            value = null;
            return false;
        }

        value = Arguments[Index++];
        return true;
    }

    public bool TryReadNext([NotNullWhen(true)] out sbyte? value)
    {
        if (!EndOfStream && sbyte.TryParse(Arguments[Index++], out sbyte result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out byte? value)
    {
        if (!EndOfStream && byte.TryParse(Arguments[Index++], out byte result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out short? value)
    {
        if (!EndOfStream && short.TryParse(Arguments[Index++], out short result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out ushort? value)
    {
        if (!EndOfStream && ushort.TryParse(Arguments[Index++], out ushort result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out int? value)
    {
        if (!EndOfStream && int.TryParse(Arguments[Index++], out int result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out uint? value)
    {
        if (!EndOfStream && uint.TryParse(Arguments[Index++], out uint result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out long? value)
    {
        if (!EndOfStream && long.TryParse(Arguments[Index++], out long result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out ulong? value)
    {
        if (!EndOfStream && ulong.TryParse(Arguments[Index++], out ulong result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out float? value)
    {
        if (!EndOfStream && float.TryParse(Arguments[Index++], out float result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryReadNext([NotNullWhen(true)] out double? value)
    {
        if (!EndOfStream && double.TryParse(Arguments[Index++], out double result))
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }
}
