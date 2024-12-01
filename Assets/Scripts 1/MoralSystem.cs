using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoraleSystem : MonoBehaviour
{
    [Header("Morale Settings")]
    public float maxMorale = 100f;          // Max morale level
    public float minMorale = 0f;           // Min morale level (where work strikes occur)
    public float currentMorale;            // Current morale level
    public float moraleDecrementRate = 0.1f; // Rate at which morale decreases over time due to negative events

    [Header("Event Impact Settings")]
    public float deathImpact = 20f;         // Impact on morale for crew death
    public float foodSupplyImpact = 10f;   // Impact on morale due to food supply problems
    public float entertainmentImpact = 5f; // Impact of entertainment modules on morale
    public float successfulMissionBonus = 15f; // Bonus for successful missions

    [Header("Ship Areas Affected by Strikes")]
    public List<ShipArea> shipAreas;       // List of areas that can be affected by strikes (e.g., engine bay, weapons bay)

    private bool isOnStrike = false;        // Flag to check if there is an ongoing strike
    private float strikeDuration = 10f;     // Duration of a work strike (in seconds)

    void Start()
    {
        currentMorale = maxMorale;           // Initialize morale to maximum
    }

    void Update()
    {
        // Decrease morale over time (simulating some negative effects or fatigue)
        currentMorale = Mathf.Max(currentMorale - moraleDecrementRate * Time.deltaTime, minMorale);

        // Check for work strikes if morale falls too low
        if (currentMorale <= minMorale && !isOnStrike)
        {
            StartCoroutine(InitiateWorkStrike());
        }
    }

    public void ApplyEventImpact(MoraleEvent moraleEvent)
    {
        switch (moraleEvent)
        {
            case MoraleEvent.CrewDeath:
                AdjustMorale(-deathImpact);
                break;
            case MoraleEvent.FoodShortage:
                AdjustMorale(-foodSupplyImpact);
                break;
            case MoraleEvent.EntertainingActivities:
                AdjustMorale(entertainmentImpact);
                break;
            case MoraleEvent.SuccessfulMission:
                AdjustMorale(successfulMissionBonus);
                break;
            default:
                break;
        }
    }

    private void AdjustMorale(float amount)
    {
        currentMorale = Mathf.Clamp(currentMorale + amount, minMorale, maxMorale);
        Debug.Log("Current Morale: " + currentMorale);
    }

    private IEnumerator InitiateWorkStrike()
    {
        isOnStrike = true;
        Debug.Log("Morale too low! Crew is striking...");

        // Disable ship areas affected by strike
        foreach (var area in shipAreas)
        {
            area.DisableArea();
        }

        // Wait for strike to resolve (you can trigger this based on external events, like morale improvements or missions)
        yield return new WaitForSeconds(strikeDuration);

        // Resolve strike and restore affected areas
        foreach (var area in shipAreas)
        {
            area.RestoreArea();
        }

        isOnStrike = false;
        Debug.Log("Strike resolved. Crew morale has stabilized.");
    }

    // Method to call when a crew member dies (can be triggered from another system)
    public void OnCrewDeath()
    {
        ApplyEventImpact(MoraleEvent.CrewDeath);
    }

    // Method to call when food supplies are low (can be triggered based on supply levels)
    public void OnFoodShortage()
    {
        ApplyEventImpact(MoraleEvent.FoodShortage);
    }

    // Method to call when entertainment activities occur
    public void OnEntertainment()
    {
        ApplyEventImpact(MoraleEvent.EntertainingActivities);
    }

    // Method to call after a successful mission (can be triggered when missions are completed)
    public void OnSuccessfulMission()
    {
        ApplyEventImpact(MoraleEvent.SuccessfulMission);
    }
}

// Enum for different morale events
public enum MoraleEvent
{
    CrewDeath,
    FoodShortage,
    EntertainingActivities,
    SuccessfulMission
}

// Ship area affected by strikes
[System.Serializable]
public class ShipArea
{
    public string areaName;           // Name of the ship area (e.g., Engine Bay, Weapons Bay)
    public bool isDisabled = false;   // Flag to indicate whether the area is disabled during a strike

    public void DisableArea()
    {
        isDisabled = true;
        Debug.Log(areaName + " has been disabled due to work strike.");
    }

    public void RestoreArea()
    {
        isDisabled = false;
        Debug.Log(areaName + " has been restored.");
    }
}
