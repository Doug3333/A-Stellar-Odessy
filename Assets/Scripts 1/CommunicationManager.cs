using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommunicationManager : MonoBehaviour
{
    [Header("Communication Settings")]
    public float messageDisplayTime = 5f;        // Time in seconds to display each message
    public AudioClip incomingMessageSound;       // Sound for incoming message
    public AudioClip outgoingMessageSound;       // Sound for sending a response

    [Header("Message Options")]
    public List<string> potentialResponses;      // List of possible player responses (choices)
    public Dictionary<string, string> responseConsequences;  // Consequences for each response

    [Header("References")]
    public CrewManager crewManager;              // To affect crew morale based on responses
    public DiplomacyManager diplomacyManager;    // To affect faction relationships
    public InventoryManager inventoryManager;    // Can affect resources and supplies
    public EventTrigger eventTrigger;            // To trigger events based on the player response
    public ResourceManager resourceManager;      // To modify resources

    private bool isCommunicating = false;        // Is the communication currently active?
    private string currentMessage = "";          // Current message being displayed

    private void Start()
    {
        potentialResponses = new List<string> { "Yes", "No", "Maybe" };
        responseConsequences = new Dictionary<string, string>
        {
            { "Yes", "Increased morale, gained resources" },
            { "No", "Decreased morale, loss of trust with faction" },
            { "Maybe", "Neutral impact, event triggered" }
        };
    }

    // Start a communication sequence
    public void StartCommunication(string message)
    {
        if (isCommunicating)
            return;

        currentMessage = message;
        isCommunicating = true;
        StartCoroutine(DisplayMessageSequence());
    }

    // Coroutine to handle displaying the message and waiting for player input
    private IEnumerator DisplayMessageSequence()
    {
        // Play the sound for an incoming message
        AudioSource.PlayClipAtPoint(incomingMessageSound, transform.position);

        // Display the message
        Debug.Log("Incoming Message: " + currentMessage);
        yield return new WaitForSeconds(messageDisplayTime);

        // Offer choices to the player
        Debug.Log("Choose a response:");
        for (int i = 0; i < potentialResponses.Count; i++)
        {
            Debug.Log($"{i + 1}. {potentialResponses[i]}");
        }

        // Wait for the player to choose a response (for now, simulate with an index)
        int choiceIndex = GetPlayerChoice(); // You can replace this with actual player input in the game

        string chosenResponse = potentialResponses[choiceIndex];

        // Handle the response consequences
        HandleResponseConsequences(chosenResponse);

        // Play the sound for sending a response
        AudioSource.PlayClipAtPoint(outgoingMessageSound, transform.position);

        isCommunicating = false;
    }

    // Get player choice (replace this with actual input handling)
    private int GetPlayerChoice()
    {
        // For now, randomly select a response to simulate player choice
        return Random.Range(0, potentialResponses.Count); // In the real game, replace this with player input logic
    }

    // Handle consequences based on the player's response
    private void HandleResponseConsequences(string response)
    {
        string consequence = responseConsequences[response];

        Debug.Log("Consequences: " + consequence);

        // Example consequences: Change crew morale, affect diplomacy, or trigger events
        switch (response)
        {
            case "Yes":
                crewManager.ModifyMorale(10);  // Increase morale
                inventoryManager.AddResource("Fuel", 50);  // Gain fuel
                break;

            case "No":
                crewManager.ModifyMorale(-20);  // Decrease morale
                diplomacyManager.AlterFactionRelation("Hostile", -10);  // Decrease relationship with faction
                break;

            case "Maybe":
                eventTrigger.TriggerEvent("ArtifactRecovery");  // Trigger a neutral event
                break;
        }
    }

    // Method for sending a diplomatic message
    public void SendDiplomaticMessage(string message, string factionName)
    {
        string fullMessage = $"Message to {factionName}: {message}";
        StartCommunication(fullMessage);
        diplomacyManager.AlterFactionRelation(factionName, 5); // Example of increasing diplomatic relations
    }

    // Method for sending a mission proposal
    public void SendMissionProposal(string missionDetails, string targetFaction)
    {
        string fullMessage = $"Mission Proposal to {targetFaction}: {missionDetails}";
        StartCommunication(fullMessage);
        eventTrigger.TriggerEvent("MissionProposal");  // Trigger event for mission proposal
    }

    // Example method to notify the crew of a situation, affecting morale
    public void SendCrewMessage(string message)
    {
        string fullMessage = $"Crew Alert: {message}";
        StartCommunication(fullMessage);
        crewManager.ModifyMorale(-10);  // Example of crew morale impact based on the situation
    }
}
