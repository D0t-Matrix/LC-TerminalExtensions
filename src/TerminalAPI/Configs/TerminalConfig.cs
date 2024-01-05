using TerminalAPI.Attributes;
using TerminalAPI.Models;

namespace TerminalAPI.Configs;

[ConfigGroup("Terminal", "Configure the behavior of the terminal")]
public class TerminalConfig
{
    [TerminalConfig("Enables/Disables terminal verb commands", null)]
    [ConfigPersist(PersistType.LocalPlayer, null)]
    public bool VerbsEnabled { get; set; }

    [TerminalConfig("Specifies if the Confirm/Deny pop-up should be shown", null)]
    [ConfigPersist(PersistType.LocalPlayer, null)]
    public bool AutoConfirm { get; set; }
}
