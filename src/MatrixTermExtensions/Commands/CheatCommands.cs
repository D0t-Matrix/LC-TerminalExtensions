using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class CheatCommands
{
    [TerminalCommand("GiveMoney", true)]
    [CommandInfo("Adds specified amount of money to shop balance.", "GiveMoney [value]")]
    public string GiveMoneyCommand(Terminal terminal, int amount)
    {
        terminal.groupCredits += amount;

        return $"${amount} has been added, ye dirty cheater!";
    }

    [TerminalCommand("SpawnLoot", true)]
    [CommandInfo("Spawns a random amount of loot, with the specified value at your feet.", "SpawnLoot [amount] [value]")]
    public string SpawnLootCommand(int amount, int value)
    {
        Plugin.Logger.LogInfo("Selecting random loot item to spawn.");
        var randIndex = RoundManager.Instance.LevelRandom.Next(RoundManager.Instance.currentLevel.spawnableScrap.Count);
        var randomLootItem = RoundManager.Instance.currentLevel.spawnableScrap[randIndex];

        Plugin.Logger.LogInfo($"Spawning {randomLootItem.spawnableItem.itemName}");

        var objToSpawn = randomLootItem.spawnableItem.spawnPrefab;
        var position = GameNetworkManager.Instance.localPlayerController.transform.position;

        for (int i  = 0; i < amount; i++)
        {
            Plugin.Logger.LogInfo($"Spawning item {i+1} of {amount}");
            var newObj = UnityEngine.Object.Instantiate(objToSpawn, position, Quaternion.identity);
            var component = newObj.GetComponent<GrabbableObject>();
            component.startFallingPosition = position;
            component.targetFloorPosition = component.GetItemFloorPosition(position);
            component.SetScrapValue(value);
            component.NetworkObject.Spawn();
            Plugin.Logger.LogInfo($"{component.name} spawned with value: {component.scrapValue}");
        }

        return "spawned items.";
    }

    [TerminalCommand("ResetInverse", false)]
    [CommandInfo("Resets the inverse teleporter cooldown")]
    public string ResetInverseTeleporterCooldownCommand()
    {
        if (TeleporterCommands.TryResetInverseTeleporter())
            return "Cooldown reset.";

        return "failed to reset cooldown";
    }

    [TerminalCommand("Kill", true)]
    [CommandInfo("Kills player with given name.", "Kill <Player Name>")]
    public string KillPlayer(string playerName)
    {
        return "failed to kill player";
    }
}
