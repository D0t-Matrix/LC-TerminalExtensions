using System.Collections.Generic;

namespace TerminalAPI.Models;

public class CommandComparer : IComparer<TerminalCommand>
{
    public int Compare(TerminalCommand? x, TerminalCommand? y)
    {
        if (x is null && y is null)
            return 0;
        else if (y is null || x?.Priority > y.Priority)
            return 1;
        else if (x is null || x.Priority < y.Priority)
            return -1;
        else return x.ArgumentCount.CompareTo(y.ArgumentCount);
    }
}
