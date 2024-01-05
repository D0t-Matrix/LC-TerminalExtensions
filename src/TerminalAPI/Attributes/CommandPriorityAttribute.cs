using System;

namespace TerminalAPI.Attributes;

public sealed class CommandPriorityAttribute : Attribute
{
    public int Priority { get; }

    public CommandPriorityAttribute(int priority) => this.Priority = priority;
}
