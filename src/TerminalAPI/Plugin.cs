using TerminalAPI.Commands;
using TerminalAPI.Configs;
using TerminalAPI.Models;

namespace TerminalAPI;

[BepInPlugin("Matrix.TerminalAPI", "Matrix.TerminalAPI", "1.1.0")]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new("Matrix.TerminalAPI");

    private TerminalModRegistry? _terminal;

    #pragma warning disable IDE0052 // Remove unread private members
    private TerminalConfig? _terminalConfig;
#pragma warning restore IDE0052 // Remove unread private members

    #pragma warning disable IDE0051 // Remove unused private members
    void Awake()
    {
        Logger.LogInfo("Matrix.TerminalAPI is loading...");
        Logger.LogInfo("Installing patches..");
        _harmony.PatchAll(typeof(Plugin).Assembly);
        Logger.LogInfo("Registering build-in Commands..");
        _terminal = TerminalRegistry.CreateTerminalRegistry();
        _terminal.RegisterFrom<CommandInfoCommands>();
        _terminalConfig = _terminal.RegisterFrom<TerminalConfig>();
        DontDestroyOnLoad(this);
        Logger.LogInfo("Matrix.TerminalAPI is loaded!");
    }
    #pragma warning restore IDE0051 // Remove unused private members
}
