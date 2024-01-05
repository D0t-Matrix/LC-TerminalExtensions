using System;

namespace TerminalAPI.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ConfigGroupAttribute : Attribute
{
    public string Name { get; }

    public string Description { get; }

    public ConfigGroupAttribute(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }
}
