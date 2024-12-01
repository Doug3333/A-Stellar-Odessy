using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class EngineSystem : MonoBehaviour
{
    [Header("Engine Settings")]
    public float maxSpeed = 100f;              // Maximum speed of the ship
    public float acceleration = 5f;            // How fast the ship accelerates
    public float deceleration = 2f;            // How fast the ship decelerates when not accelerating
    public float currentSpeed = 0f;            // Current speed of the ship
    public float maxWarpSpeed = 500f;          // Maximum speed during warp
    public float warpChargeTime = 5f;          // Time required to charge for a warp jump
    public float currentWarpSpeed = 0f;        // Current speed during warp
    public float currentFuel = 100f;           // Current fuel (percentage)
    public float fuelConsumptionRate = 2f;     // Fuel consumption per second while traveling
    public float warpFuelConsumptionRate = 10f; // Fuel consumption per second during warp

    [Header("References")]
    public CombatSystem combatSystem;          // Reference to CombatSystem for interaction with combat mechanics
    public InventoryManager inventoryManager;  // Reference to the inventory system for resource management
    public ResourceManager resourceManager;    // Reference to the resource manager system

    private bool isWarping = false;            // Is the ship currently warping
    private bool isAccelerating = false;       // Is the ship accelerating
    private bool isDecelerating = false;       // Is the ship decelerating
    private bool isInCombat = false;           // Whether the ship is in combat

    private void Update()
    {
        HandleMovement();
        //HandleWarp();
        HandleFuelConsumption();
    }

    // Handle basic ship movement: accelerate, decelerate, and turn
    private void HandleMovement()
    {
        if (!isWarping && !isInCombat)
        {
            if (isAccelerating)
            {
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }

            if (isDecelerating)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0);
            }

            // Implement ship turning if necessary
            // Here you could use input to change the ship's direction, for example:
            // transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }
    }

    // Start the warp process, consuming fuel and initializing the warp speed
    public void StartWarp()
    {
        if (!isWarping && currentFuel > 0)
        {
            StartCoroutine(WarpSequence());
        }
        else
        {
            Debug.LogWarning("Insufficient fuel or ship is already warping.");
        }
    }

    // Warp sequence: Charges up for warp, consumes fuel, and travels to a distant point
    private IEnumerator WarpSequence()
    {
        isWarping = true;
        Debug.Log("Warp drive charging...");

        // Charge up the warp drive (can be interrupted if needed)
        float warpChargeTimer = 0f;
        while (warpChargeTimer < warpChargeTime)
        {
            warpChargeTimer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Warp drive fully charged. Initiating warp...");

        // Start the warp
        currentWarpSpeed = maxWarpSpeed;
        isAccelerating = false;  // No acceleration during warp

        // Consume fuel while warping
        while (currentFuel > 0 && isWarping)
        {
            currentFuel -= warpFuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(currentFuel, 0);
            yield return null;
        }

        // If out of fuel or warp interrupted, stop the warp
        StopWarp();
    }

    // Stop the warp and return to normal speed
    public void StopWarp()
    {
        isWarping = false;
        currentWarpSpeed = 0f;
        Debug.Log("Warp complete or interrupted. Returning to normal space travel.");

        // Re-enable acceleration for regular travel
        isAccelerating = true;
    }

    // Handle fuel consumption (during movement and warp)
    private void HandleFuelConsumption()
    {
        if (!isWarping && !isInCombat && currentFuel > 0)
        {
            // Consume fuel for regular movement
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(currentFuel, 0);
        }

        if (currentFuel <= 0)
        {
            Debug.LogWarning("Out of fuel!");
            // Implement logic to handle running out of fuel (e.g., stop acceleration, force the ship to drift)
            currentSpeed = 0f;
        }
    }

    // Get the current time required for a warp jump (can be influenced by ship upgrades or research)
    public float GetWarpTime()
    {
        // Example: This can be influenced by ship upgrades or research
        return warpChargeTime;
    }

    // Enable ship acceleration
    public void EnableAcceleration()
    {
        isAccelerating = true;
        isDecelerating = false;
    }

    // Enable ship deceleration
    public void EnableDeceleration()
    {
        isDecelerating = true;
        isAccelerating = false;
    }

    // Disable both acceleration and deceleration
    public void StopMovement()
    {
        isAccelerating = false;
        isDecelerating = false;
    }

    #region Combat Interaction

    // Call when the ship is entering combat; prevent warp and movement
    public void EnterCombat()
    {
        isInCombat = true;
        StopMovement();
        StopWarp();
    }

    // Call when the ship exits combat; allow warp and movement again
    public void ExitCombat()
    {
        isInCombat = false;
    }

    #endregion
}
