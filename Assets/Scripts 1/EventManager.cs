using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    [Header("Event Settings")]
    public List<GameEvent> gameEvents;  // List to hold all possible game events
    public float eventTriggerInterval = 10f; // Interval at which new events will be triggered
    private float eventTimer;

    void Start()
    {
        eventTimer = eventTriggerInterval;
        InitializeGameEvents();
    }

    void Update()
    {
        eventTimer -= Time.deltaTime;
        if (eventTimer <= 0f)
        {
            TriggerRandomEvent();
            eventTimer = eventTriggerInterval; // Reset event timer
        }
    }

    private void InitializeGameEvents()
    {
        // Initialize with predefined events
        gameEvents = new List<GameEvent>
        {
            new GameEvent("Engine Breakdown", "Your ship's engine has broken down. You need to repair it.", EventType.EngineBreakdown),
            new GameEvent("Mutual Interests", "Two factions are interested in collaborating with you. Choose wisely.", EventType.MutualInterests),
            new GameEvent("Territorial Dispute", "A neighboring faction is claiming territory you explored. Prepare for conflict.", EventType.TerritorialDispute),
            new GameEvent("Trade Proposal", "A faction proposes a trade agreement. Will you accept?", EventType.TradeProposal),
            new GameEvent("Rescue Mission", "A stranded ship requests help. Do you want to assist?", EventType.RescueMission),
            new GameEvent("Resource Delivery", "A faction needs resources delivered to a remote station. Can you help?", EventType.ResourceDelivery),
            new GameEvent("Enemy Extermination", "A dangerous faction needs to be eliminated. Will you take on the task?", EventType.EnemyExtermination),
            new GameEvent("Artifact Retrieval", "A faction has information about a rare artifact. They need your help retrieving it.", EventType.ArtifactRetrieval),
            new GameEvent("Gravitational Waves", "Gravitational waves are affecting your ship’s systems. Prepare for instability.", EventType.SpaceEvent),
            new GameEvent("Gamma Flares", "Gamma flares have started affecting nearby planets, threatening to destroy them. Time to act.", EventType.SpaceEvent),
            new GameEvent("Pirate Ambush", "Pirates are attacking a peaceful faction in the system. Will you defend them?", EventType.PirateAmbush),
            new GameEvent("Resource Mining Opportunity", "You’ve found a valuable asteroid field rich in rare minerals. Start mining.", EventType.ResourceMining),
            new GameEvent("Solar Storm", "A massive solar storm is approaching. It may disrupt your ship's systems.", EventType.SpaceEvent),
            new GameEvent("Meteor Shower", "A meteor shower is heading your way! Seek shelter or brace for impact.", EventType.SpaceEvent),
            new GameEvent("Pirate Bounty", "A pirate group has a bounty on your head. Expect attacks from bounty hunters.", EventType.PirateBounty),
            new GameEvent("Scientific Discovery", "You’ve discovered a new lifeform on a distant planet. What will you do with this knowledge?", EventType.ScientificDiscovery),
            new GameEvent("Warp Core Malfunction", "Your ship’s warp core is malfunctioning. It could explode unless fixed.", EventType.EngineBreakdown),
            new GameEvent("Diplomatic Crisis", "A diplomatic crisis is brewing. How will you handle negotiations?", EventType.DiplomaticCrisis)
        };
    }

    private void TriggerRandomEvent()
    {
        // Randomly select an event from the list
        int randomIndex = Random.Range(0, gameEvents.Count);
        GameEvent selectedEvent = gameEvents[randomIndex];

        // Handle event based on its type
        HandleEvent(selectedEvent);
    }

    private void HandleEvent(GameEvent selectedEvent)
    {
        // Display the event message
        Debug.Log($"{selectedEvent.eventName}: {selectedEvent.eventDescription}");

        // Call specific methods based on the event type
        switch (selectedEvent.eventType)
        {
            case EventType.EngineBreakdown:
                // Call a method to handle engine breakdown
                HandleEngineBreakdown();
                break;

            case EventType.MutualInterests:
                // Handle mutual interests, factions, and trade relations
                HandleMutualInterests();
                break;

            case EventType.TerritorialDispute:
                // Handle conflict resolution for territorial disputes
                HandleTerritorialDispute();
                break;

            case EventType.TradeProposal:
                // Handle trade proposal between factions
                HandleTradeProposal();
                break;

            case EventType.RescueMission:
                // Trigger a rescue mission
                HandleRescueMission();
                break;

            case EventType.ResourceDelivery:
                // Handle resource delivery mission
                HandleResourceDelivery();
                break;

            case EventType.EnemyExtermination:
                // Handle enemy extermination mission
                HandleEnemyExtermination();
                break;

            case EventType.ArtifactRetrieval:
                // Handle artifact retrieval
                HandleArtifactRetrieval();
                break;

            case EventType.SpaceEvent:
                // Handle space-related anomalies (e.g., gravitational waves, gamma flares)
                HandleSpaceEvent(selectedEvent);
                break;

            default:
                Debug.LogWarning("Event type not handled yet.");
                break;
        }
    }

    // Example methods for handling specific events

    private void HandleEngineBreakdown()
    {
        Debug.Log("The engine is malfunctioning. Start the repair process.");
        // Implement repair mechanic
    }

    private void HandleMutualInterests()
    {
        Debug.Log("Two factions want to collaborate. Choose which one to support.");
        // Implement decision logic for choosing which faction to support
    }

    private void HandleTerritorialDispute()
    {
        Debug.Log("A faction is claiming the territory you discovered.");
        // Implement conflict resolution (combat or diplomacy)
    }

    private void HandleTradeProposal()
    {
        Debug.Log("Faction proposes a trade agreement.");
        // Implement logic for accepting or rejecting trade proposals
    }

    private void HandleRescueMission()
    {
        Debug.Log("A distress signal is coming from a stranded ship.");
        // Implement mission to rescue the stranded ship
    }

    private void HandleResourceDelivery()
    {
        Debug.Log("A faction needs resources delivered.");
        // Implement resource delivery mission
    }

    private void HandleEnemyExtermination()
    {
        Debug.Log("A faction wants you to eliminate an enemy.");
        // Implement combat logic for extermination mission
    }

    private void HandleArtifactRetrieval()
    {
        Debug.Log("A rare artifact is rumored to be in a distant system.");
        // Implement artifact retrieval mission
    }

    private void HandleSpaceEvent(GameEvent spaceEvent)
    {
        if (spaceEvent.eventName == "Gravitational Waves")
        {
            Debug.Log("Gravitational waves are affecting the ship's systems.");
            // Implement the effect of gravitational waves on the ship's navigation or power systems
        }
        else if (spaceEvent.eventName == "Gamma Flares")
        {
            Debug.Log("Gamma flares are destroying nearby planets.");
            // Implement the damage or effects on planets and ships
        }
    }
}

[System.Serializable]
public class GameEvent
{
    public string eventName;
    public string eventDescription;
    public EventType eventType;

    public GameEvent(string name, string description, EventType type)
    {
        eventName = name;
        eventDescription = description;
        eventType = type;
    }
}

public enum EventType
{
    EngineBreakdown,
    MutualInterests,
    TerritorialDispute,
    TradeProposal,
    RescueMission,
    ResourceDelivery,
    EnemyExtermination,
    ArtifactRetrieval,
    SpaceEvent,
    PirateAmbush,
    ResourceMining,
    SolarStorm,
    MeteorShower,
    PirateBounty,
    ScientificDiscovery,
    DiplomaticCrisis
}
