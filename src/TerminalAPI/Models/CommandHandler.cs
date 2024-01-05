using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TerminalAPI.Models;

public static class CommandHandler
{
    private static readonly Regex _splitRegex = new("[\\\"](.+?)[\\\"]|([^ ]+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
    private static readonly CommandComparer _comparer = new();

    public static TerminalNode? TryExecute(string command, Terminal terminal)
    {
        var source1 = _splitRegex.Matches(command.Trim()).Select(x => x.Value.Trim('"', ' '));
        if (source1 is null || !source1.Any())
            return null;

        var commandName = source1.First();
        var arguments = source1.Skip(1).ToArray();
        var commandNodes = new List<(TerminalCommand, Func<TerminalNode>)>();
        foreach (var terminalCommand in TerminalRegistry.GetCommands(commandName).ToArray<TerminalCommand>())
        {
            if (terminalCommand.CheckAllowed()
                && terminalCommand.TryCreateInvoker(arguments, terminal, out var invoker))
            {
                commandNodes.Add((terminalCommand, invoker));
            }
        }

        foreach (var tuple in commandNodes.OrderByDescending(x => x.Item1, _comparer))
        {
            TerminalNode node = tuple.Item2();
            if (!Equals(node, null))
                return node;
        }

        return null;
    }
}
