using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class CheatCommands
{
    #region Strings

    const string AmountAdded = "{0} has been added, ye dirty cheater!";
    const string ItemSpawned = "Item Spawned";
    const string ItemsSpawned = "Items Spawned";
    const string ResetCooldown = "Cooldown Reset";
    const string FailedReset = "Failed to reset cooldown";

    #endregion

    #region Commands

    [TerminalCommand("GiveMoney", true)]
    [CommandInfo("Adds specified amount of money to shop balance.", "GiveMoney [value]")]
    public string GiveMoneyCommand(Terminal terminal, int amount)
    {
        terminal.groupCredits += amount;

        return string.Format(AmountAdded, amount);
    }

    [TerminalCommand("SpawnLoot", true)]
    [CommandInfo("Spawns a random amount of loot, with the specified value at your feet.", "SpawnLoot [amount] [value]")]
    public string SpawnLootCommand(int amount, int value)
    {
        var randIndex = RoundManager.Instance.LevelRandom.Next(RoundManager.Instance.currentLevel.spawnableScrap.Count);
        var randomLootItem = RoundManager.Instance.currentLevel.spawnableScrap[randIndex];

        var objToSpawn = randomLootItem.spawnableItem.spawnPrefab;
        var position = GameNetworkManager.Instance.localPlayerController.transform.position;

        for (int i  = 0; i < amount; i++)
        {
            var newObj = UnityEngine.Object.Instantiate(objToSpawn, position, Quaternion.identity);
            var component = newObj.GetComponent<GrabbableObject>();
            component.startFallingPosition = position;
            component.targetFloorPosition = component.GetItemFloorPosition(position);
            component.SetScrapValue(value);
            component.NetworkObject.Spawn();
        }

        return amount == 1
            ? ItemSpawned
            : ItemsSpawned;
    }

    [TerminalCommand("ResetInverse", false)]
    [CommandInfo("Resets the inverse teleporter cooldown")]
    public string ResetInverseTeleporterCooldownCommand()
    {
        if (TeleporterCommands.TryResetInverseTeleporter())
            return ResetCooldown;

        return FailedReset;
    }

    //[TerminalCommand("Kill", true)]
    //[CommandInfo("Kills player with given name.", "Kill <Player Name>")]
    //public string KillPlayer(string playerName)
    //{
    //    return "failed to kill player";
    //}

    #endregion
}
