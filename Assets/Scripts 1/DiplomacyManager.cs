using UnityEngine;
using System.Collections.Generic;

public class DiplomacyManager : MonoBehaviour
{
    [Header("Diplomacy Settings")]
    public List<Faction> factions; // List of all factions in the game
    public float allianceThreshold = 80f; // Reputation value at which factions become allies
    public float hostilityThreshold = 20f; // Reputation value at which factions become hostile

    [Header("Trade Agreement Settings")]
    public float tradeAgreementBonus = 10f; // Reputation bonus for having a trade agreement

    private void Start()
    {
        // Initialize any faction relations if necessary
        InitializeFactions();
    }

    private void InitializeFactions()
    {
        foreach (Faction faction in factions)
        {
            faction.reputation = Random.Range(50, 100); // Initialize reputation (randomized for demo purposes)
        }
    }

    public void UpdateDiplomacy(Faction playerFaction, Faction targetFaction, float reputationChange)
    {
        targetFaction.reputation += reputationChange;
        CheckFactionStatus(targetFaction);

        Debug.Log($"Reputation with {targetFaction.name} changed by {reputationChange}. New reputation: {targetFaction.reputation}");
    }

    private void CheckFactionStatus(Faction faction)
    {
        if (faction.reputation >= allianceThreshold)
        {
            faction.status = FactionStatus.Allied;
            Debug.Log($"{faction.name} is now an ally.");
        }
        else if (faction.reputation <= hostilityThreshold)
        {
            faction.status = FactionStatus.Hostile;
            Debug.Log($"{faction.name} is now hostile.");
        }
        else
        {
            faction.status = FactionStatus.Neutral;
            Debug.Log($"{faction.name} is neutral.");
        }
    }

    public void ProposeTradeAgreement(Faction playerFaction, Faction targetFaction)
    {
        if (targetFaction.status != FactionStatus.Hostile)
        {
            playerFaction.reputation += tradeAgreementBonus;
            targetFaction.reputation += tradeAgreementBonus;
            Debug.Log($"Trade agreement established with {targetFaction.name}. Both factions' reputation increased.");
        }
        else
        {
            Debug.Log($"Cannot establish a trade agreement with hostile faction {targetFaction.name}.");
        }
    }

    public void SendMessage(Faction playerFaction, Faction targetFaction, string message)
    {
        Debug.Log($"{playerFaction.name} sent message to {targetFaction.name}: {message}");
    }

    public void DeclareWar(Faction playerFaction, Faction targetFaction)
    {
        if (playerFaction.status == FactionStatus.Hostile || targetFaction.status == FactionStatus.Hostile)
        {
            Debug.Log($"{playerFaction.name} has declared war on {targetFaction.name}!");
            // Implement war logic here, such as initiating combat, losing reputation, etc.
        }
    }
}

[System.Serializable]
public class Faction
{
    public string name;
    public float reputation;
    public FactionStatus status;

    public Faction(string name)
    {
        this.name = name;
        reputation = 50f; // Default neutral reputation
        status = FactionStatus.Neutral;
    }
}

public enum FactionStatus
{
    Neutral,
    Allied,
    Hostile
}
