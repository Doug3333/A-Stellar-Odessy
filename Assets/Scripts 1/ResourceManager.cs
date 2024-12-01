using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")]
    public float fuelAmount = 1000f;              // Total fuel amount
    public float foodAmount = 500f;               // Total food amount (in kg or units)
    public float rawMaterialAmount = 1000f;       // Raw materials (used for repairs/upgrades)
    public float energyAmount = 1000f;            // Energy for ship systems

    [Header("Consumption Rates")]
    public float fuelConsumptionRate = 0.1f;      // Fuel consumed per unit of time (e.g., per second)
    public float foodConsumptionRate = 0.05f;     // Food consumed per unit of time (e.g., per second)
    public float rawMaterialDecayRate = 0.1f;     // Decay rate for raw materials (e.g., per minute)
    public float foodDecayRate = 0.02f;           // Food decay rate per minute (due to spoilage)
    public float fighterFuelConsumptionRate = 0.3f; // Fuel consumption for launching fighters and bombers

    [Header("References")]
    public CrewManager crewManager;               // Reference to crew manager to track consumption
    public EngineSystem engineSystem;             // Reference to engine system to track ship's fuel consumption for travel
    public FighterManager fighterManager;         // Reference to fighter management system to track fighter fuel usage

    private void Start()
    {
        // Start a repeating update function to consume resources
        StartCoroutine(ConsumeResources());
    }

    private IEnumerator ConsumeResources()
    {
        while (true)
        {
            // Decrease fuel based on ship usage (engine consumption, fighter fuel)
            if (engineSystem.IsEngineActive())
            {
                fuelAmount -= fuelConsumptionRate * Time.deltaTime; // General fuel consumption by ship
            }

            // Decrease food based on crew consumption
            foodAmount -= foodConsumptionRate * Time.deltaTime * crewManager.GetCrewCount(); // Multiply by crew size

            // Apply decay to food (spoilage over time)
            foodAmount -= foodDecayRate * Time.deltaTime;

            // Apply decay to raw materials (e.g., materials used for repairs)
            rawMaterialAmount -= rawMaterialDecayRate * Time.deltaTime;

            // Fuel consumption for fighter and bomber launches
            if (fighterManager.AreFightersDeployed())
            {
                fuelAmount -= fighterFuelConsumptionRate * Time.deltaTime;
            }

            // Ensure resource amounts don't go below zero
            fuelAmount = Mathf.Max(fuelAmount, 0f);
            foodAmount = Mathf.Max(foodAmount, 0f);
            rawMaterialAmount = Mathf.Max(rawMaterialAmount, 0f);
            energyAmount = Mathf.Max(energyAmount, 0f);

            // Wait for the next frame to continue resource consumption
            yield return null;
        }
    }

    // Method to add resources (e.g., after mining, trading, or gathering)
    public void AddResource(string resourceType, float amount)
    {
        switch (resourceType)
        {
            case "Fuel":
                fuelAmount += amount;
                break;

            case "Food":
                foodAmount += amount;
                break;

            case "RawMaterials":
                rawMaterialAmount += amount;
                break;

            case "Energy":
                energyAmount += amount;
                break;

            default:
                Debug.LogWarning("Unknown resource type: " + resourceType);
                break;
        }

        // Ensure no resource value goes negative after addition
        fuelAmount = Mathf.Max(fuelAmount, 0f);
        foodAmount = Mathf.Max(foodAmount, 0f);
        rawMaterialAmount = Mathf.Max(rawMaterialAmount, 0f);
        energyAmount = Mathf.Max(energyAmount, 0f);
    }

    // Method to consume resources directly (used in various actions like upgrades, repairs)
    public bool ConsumeResource(string resourceType, float amount)
    {
        bool success = false;

        switch (resourceType)
        {
            case "Fuel":
                if (fuelAmount >= amount)
                {
                    fuelAmount -= amount;
                    success = true;
                }
                break;

            case "Food":
                if (foodAmount >= amount)
                {
                    foodAmount -= amount;
                    success = true;
                }
                break;

            case "RawMaterials":
                if (rawMaterialAmount >= amount)
                {
                    rawMaterialAmount -= amount;
                    success = true;
                }
                break;

            case "Energy":
                if (energyAmount >= amount)
                {
                    energyAmount -= amount;
                    success = true;
                }
                break;

            default:
                Debug.LogWarning("Unknown resource type: " + resourceType);
                break;
        }

        return success;
    }

    // Method to get the current amounts of resources (can be used in UI)
    public float GetResourceAmount(string resourceType)
    {
        switch (resourceType)
        {
            case "Fuel":
                return fuelAmount;

            case "Food":
                return foodAmount;

            case "RawMaterials":
                return rawMaterialAmount;

            case "Energy":
                return energyAmount;

            default:
                Debug.LogWarning("Unknown resource type: " + resourceType);
                return 0f;
        }
    }

    // Method to check if we have enough resources for a particular action (e.g., engine start, fighter launch)
    public bool HasEnoughResources(string resourceType, float amount)
    {
        return GetResourceAmount(resourceType) >= amount;
    }

    // Update resource consumption based on specific gameplay actions
    public void OnFighterLaunched()
    {
        if (HasEnoughResources("Fuel", fighterFuelConsumptionRate))
        {
            ConsumeResource("Fuel", fighterFuelConsumptionRate);
        }
        else
        {
            Debug.LogWarning("Not enough fuel to launch fighters!");
        }
    }

    // Method for crew to consume food (e.g., daily consumption)
    public void OnCrewFoodConsumption()
    {
        if (HasEnoughResources("Food", foodConsumptionRate * crewManager.GetCrewCount()))
        {
            ConsumeResource("Food", foodConsumptionRate * crewManager.GetCrewCount());
        }
        else
        {
            Debug.LogWarning("Not enough food for the crew!");
        }
    }
}
