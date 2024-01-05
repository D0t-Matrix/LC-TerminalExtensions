namespace TerminalAPI.Models;

public enum AllowedCaller
{
    None = -1, // 0xFFFFFFFF
    Player = 0,
    Host = 1,
}
