using BepInEx.Logging;
using LethalAPI.LibTerminal;
using LethalAPI.LibTerminal.Models;
using Matrix.TerminalExtensions.Commands;
using Matrix.TerminalExtensions.Configs;

namespace Matrix.TerminalExtensions;

[BepInPlugin(GeneratedPluginInfo.Identifier, GeneratedPluginInfo.Name, GeneratedPluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    #nullable disable
    
    internal static new ManualLogSource Logger;
    internal static new Config Config;

#nullable enable

    private readonly Harmony _harmony = new(GeneratedPluginInfo.Identifier);
    private readonly TerminalModRegistry _commands = TerminalRegistry.CreateTerminalRegistry();

    public void Awake()
    {
        Logger = base.Logger;
        Config = new(base.Config);

        try
        {
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo($"<!!!> {GeneratedPluginInfo.Name} Plugin has loaded. <!!!>");

            _commands.RegisterFrom<DoorCommands>();
            _commands.RegisterFrom<TeleporterCommands>();
            _commands.RegisterFrom<LightsCommands>();
            _commands.RegisterFrom<LaunchCommands>();

            if (Config.EnableCheatCommands)
            {
                Logger.LogInfo("Enabling Cheat Commands.");
                _commands.RegisterFrom<CheatCommands>();
            }

            Logger.LogInfo($"<!!!> {GeneratedPluginInfo.Name} Custom Terminal Commands loaded. <!!!>");
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
    }
}