using System;
using TerminalAPI.Models;

namespace TerminalAPI.Attributes;

public class ConfigPersistAttribute : Attribute
{
    public PersistType PersistType { get; }

    public string? ConfigPath { get; }

    public ConfigPersistAttribute(PersistType persistType, string? configPath = null)
    {
        this.PersistType = persistType;
        this.ConfigPath = configPath;
    }
}
