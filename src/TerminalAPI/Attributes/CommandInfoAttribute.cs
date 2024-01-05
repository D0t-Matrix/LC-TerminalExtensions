using System;

namespace TerminalAPI.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandInfoAttribute : Attribute
{
    public string Syntax { get; }

    public string Description { get; }

    public CommandInfoAttribute(string description, string syntax = "")
    {
        Syntax = syntax;
        Description = description;
    }
}
