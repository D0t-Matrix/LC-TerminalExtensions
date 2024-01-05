using System;

namespace TerminalAPI.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TerminalConfigAttribute : Attribute
{
    public string? Name { get; }

    public string Description { get; }

    public TerminalConfigAttribute(string description, string? name = null)
    {
        Name = name;
        Description = description;
    }
}
