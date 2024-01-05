using TerminalAPI.Attributes;
using TerminalAPI.Models;

namespace MatrixTermExtensions.Commands;

public class CheatCommands
{
    [TerminalCommand("GiveMoney", clearText: true)]
    public string GiveMoney(int amount)
    {
        var terminal = UnityEngine.Object.FindObjectOfType<Terminal>();

        terminal.groupCredits += amount;

        return $"${amount} has been added, ye cheater";
    }
}
