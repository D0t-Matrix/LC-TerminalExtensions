using TerminalAPI.Attributes;
using TerminalAPI.Models;
using System.Linq;
using System.Text;
using TerminalAPI.Extensions;

namespace TerminalAPI.Commands;

public class CommandInfoCommands
{
    [TerminalCommand("Other", clearText: true)]
    public static string CommandList()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Other commands:");
        sb.AppendLine();
        sb.AppendLine(">VIEW MONITOR");
        sb.AppendLine("To toggle on/off the main monitor's map cam");
        sb.AppendLine();
        sb.AppendLine(">SWITCH {RADAR}");
        sb.AppendLine("To switch the player view on the main monitor");
        sb.AppendLine();
        sb.AppendLine(">PING [Radar booster name]");
        sb.AppendLine("To switch the player view on the main monitor");
        sb.AppendLine();
        sb.AppendLine(">SCAN");
        sb.AppendLine("To scan for the number of items left on the current planet");
        sb.AppendLine();

        foreach(var enumerateCommand in TerminalRegistry.EnumerateCommands())
        {
            if (!enumerateCommand.Description.IsNullOrEmpty() && enumerateCommand.CheckAllowed())
            {
                sb.AppendLine(">" + enumerateCommand.Name.ToUpper() + " " + enumerateCommand.Syntax?.ToUpper());
                sb.AppendLine(enumerateCommand.Description);
                sb.AppendLine();
            }
        }
        return sb.ToString();
    }

    [TerminalCommand("Help", false)]
    [CommandInfo("Shows further information about a command", "[Command]")]
    public static string HelpCommand(string name)
    {
        var sb = new StringBuilder();
        var array = TerminalRegistry.EnumerateCommands(name).ToArray();
        if (array.IsNullOrEmpty())
            return "Unknown command: '" + name + "'";

        foreach (var terminalCommand in array)
        {
            sb.AppendLine(">" + terminalCommand.Name.ToUpper() + " " + terminalCommand.Syntax?.ToUpper());
            sb.AppendLine(terminalCommand.Description);
            if (!terminalCommand.CheckAllowed())
                sb.AppendLine("[Host Only]");

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
