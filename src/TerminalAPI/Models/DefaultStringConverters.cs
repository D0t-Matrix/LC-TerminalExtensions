using GameNetcodeStuff;
using TerminalAPI.Attributes;
using System;
using System.Linq;

namespace TerminalAPI.Models;

public static class DefaultStringConverters
{
    [StringConverter]
    public static string ParseString(string input) => input;

    [StringConverter]
    public static sbyte ParseSByte(string input)
    {
        if (sbyte.TryParse(input, out sbyte result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static byte ParseByte(string input)
    {
        if (byte.TryParse(input, out byte result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static short ParseShort(string input)
    {
        if (short.TryParse(input, out short result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static ushort ParseUShort(string input)
    {
        if (ushort.TryParse(input, out ushort result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static int ParseInt(string input)
    {
        if (int.TryParse(input, out int result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static uint ParseUInt(string input)
    {
        if (uint.TryParse(input, out uint result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static long ParseLong(string input)
    {
        if (long.TryParse(input, out long result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static ulong ParseULong(string input)
    {
        if (ulong.TryParse(input, out ulong result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static float ParseFloat(string input)
    {
        if (float.TryParse(input, out float result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static double ParseDouble(string input)
    {
        if (double.TryParse(input, out double result))
            return result;
        throw new ArgumentException(input);
    }

    [StringConverter]
    public static Decimal ParseDecimal(string input)
    {
        if (decimal.TryParse(input, out decimal result))
            return result;
        throw new ArgumentException(input);
    }

    public static PlayerControllerB ParsePlayerControllerB(string value)
    {
        if (Equals(StartOfRound.Instance, null))
            throw new ArgumentException("Game has not started");

        PlayerControllerB? playerControllerB = null;
        if (ulong.TryParse(value, out ulong steamID))
            playerControllerB = StartOfRound.Instance.allPlayerScripts
                .FirstOrDefault(x => (long)x.playerSteamId == (long)steamID);

        if (Equals(playerControllerB, null))
            playerControllerB = StartOfRound.Instance.allPlayerScripts
                .FirstOrDefault(x => x.playerUsername.Contains(value, StringComparison.InvariantCultureIgnoreCase));

        return !Equals(playerControllerB, null) 
            ? playerControllerB 
            : throw new ArgumentException("Failed to find player");
    }
}
