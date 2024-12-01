using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("Combat Settings")]
    public float shieldRechargeRate = 5f;          // Rate at which the shield regenerates per second
    public float missileDamage = 50f;              // Damage of a single missile
    public float laserDamage = 25f;                // Damage of laser weapons
    public float fighterDamage = 10f;              // Damage from fighters/bombers
    public float bomberDamage = 30f;               // Damage from bombers
    public float boardingActionTime = 10f;         // Time required for boarding a ship

    [Header("Combat States")]
    public bool isCombatActive = false;            // Whether the combat is active
    public bool isBoarding = false;                // Whether the ship is attempting to board
    public bool isWarping = false;                 // Whether the ship is attempting to warp away
    public bool isInvasionActive = false;          // Whether planetary invasion is active

    [Header("Ship Stats")]
    public float currentShield;                    // Current shield value
    public float maxShield = 100f;                 // Maximum shield value
    public float hullIntegrity = 100f;             // Hull integrity (health)
    public float maxHullIntegrity = 100f;          // Maximum hull integrity (health)

    [Header("Fighters and Bombers")]
    public GameObject fighterPrefab;               // Fighter prefab
    public GameObject bomberPrefab;                // Bomber prefab
    public int maxFighters = 5;                    // Maximum number of fighters
    public int maxBombers = 3;                     // Maximum number of bombers
    public Transform fighterLaunchPoint;           // Launch point for fighters

    [Header("Other Systems")]
    public InventoryManager inventory;             // Reference to inventory for resources
    public EngineSystem engineSystem;              // Reference to the engine system for movement and warping
    public ResearchManager researchManager;        // Reference to the research system

    private List<GameObject> launchedFighters = new List<GameObject>();
    private List<GameObject> launchedBombers = new List<GameObject>();

    private void Update()
    {
        if (isCombatActive)
        {
            // Regenerate shields over time if combat is active
            if (currentShield < maxShield)
            {
                currentShield += shieldRechargeRate * Time.deltaTime;
                currentShield = Mathf.Min(currentShield, maxShield);
            }
        }
    }

    #region Combat Actions

    // Engage ship-to-ship combat
    public void StartCombatWithShip(CombatSystem enemyShip)
    {
        isCombatActive = true;
        Debug.Log("Engaging in combat with " + enemyShip.name);
        // Additional combat logic can be implemented (e.g., weapon targeting, damage calculation)
    }

    // Engage in planetary invasion (for planet-based combat scenarios)
    public void StartPlanetaryInvasion()
    {
        isInvasionActive = true;
        Debug.Log("Planetary invasion in progress...");
        // Implement logic for planetary invasion (e.g., sending soldiers, taking control of planet)
    }

    // Launch fighters (can be called in combat or other situations)
    public void LaunchFighters()
    {
        if (launchedFighters.Count < maxFighters)
        {
            GameObject fighter = Instantiate(fighterPrefab, fighterLaunchPoint.position, Quaternion.identity);
            launchedFighters.Add(fighter);
            Debug.Log("Fighter launched!");
        }
        else
        {
            Debug.LogWarning("Maximum number of fighters reached.");
        }
    }

    // Launch bombers (can be called in combat or other situations)
    public void LaunchBombers()
    {
        if (launchedBombers.Count < maxBombers)
        {
            GameObject bomber = Instantiate(bomberPrefab, fighterLaunchPoint.position, Quaternion.identity);
            launchedBombers.Add(bomber);
            Debug.Log("Bomber launched!");
        }
        else
        {
            Debug.LogWarning("Maximum number of bombers reached.");
        }
    }

    // Fire missiles at a target
    public void FireMissiles(CombatSystem target)
    {
        if (inventory.UseResource("Missiles", 1)) // Example: Reduce missile count in inventory
        {
            target.ReceiveMissileDamage(missileDamage);
            Debug.Log("Missile fired at " + target.name);
        }
        else
        {
            Debug.LogWarning("Not enough missiles to fire!");
        }
    }

    // Fire laser weapons at a target
    public void FireLaser(CombatSystem target)
    {
        target.ReceiveLaserDamage(laserDamage);
        Debug.Log("Laser fired at " + target.name);
    }

    // Receive missile damage
    public void ReceiveMissileDamage(float damage)
    {
        if (currentShield > 0)
        {
            float shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            damage -= shieldDamage;
            Debug.Log("Shield absorbed " + shieldDamage + " damage");
        }

        hullIntegrity -= damage;
        hullIntegrity = Mathf.Max(hullIntegrity, 0);
        Debug.Log("Hull integrity reduced by " + damage + ", remaining hull integrity: " + hullIntegrity);
        CheckHullIntegrity();
    }

    // Receive laser damage
    public void ReceiveLaserDamage(float damage)
    {
        if (currentShield > 0)
        {
            float shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            damage -= shieldDamage;
            Debug.Log("Shield absorbed " + shieldDamage + " damage");
        }

        hullIntegrity -= damage;
        hullIntegrity = Mathf.Max(hullIntegrity, 0);
        Debug.Log("Hull integrity reduced by " + damage + ", remaining hull integrity: " + hullIntegrity);
        CheckHullIntegrity();
    }

    // Check if hull integrity is critical
    private void CheckHullIntegrity()
    {
        if (hullIntegrity <= 0)
        {
            DestroyShip();
        }
    }

    // Destroy the ship when hull integrity reaches 0
    private void DestroyShip()
    {
        Debug.Log("Ship destroyed!");
        // Implement ship destruction logic, such as exploding or disabling the ship
        isCombatActive = false;
    }

    // Attempt to board another ship
    public void AttemptBoardShip(CombatSystem enemyShip)
    {
        if (enemyShip.isCombatActive)
        {
            isBoarding = true;
            StartCoroutine(BoardShip(enemyShip));
        }
    }

    // Boarding coroutine to simulate the time required to board an enemy ship
    private IEnumerator BoardShip(CombatSystem enemyShip)
    {
        Debug.Log("Boarding ship...");
        yield return new WaitForSeconds(boardingActionTime);
        enemyShip.isCombatActive = false;  // Disable combat for the enemy ship as it's boarded
        Debug.Log("Boarding successful, enemy ship now under control!");
    }

    // Attempt to warp away (disengage from combat)
    public void AttemptWarpAway()
    {
        if (!isWarping)
        {
            isWarping = true;
            Debug.Log("Attempting to warp away from combat...");
            StartCoroutine(WarpAway());
        }
    }

    // Warp away from combat (can be interrupted by enemy actions)
    private IEnumerator WarpAway()
    {
        float warpTime = engineSystem.GetWarpTime();  // Assume the engine system has a method to get warp time
        yield return new WaitForSeconds(warpTime);
        Debug.Log("Warp successful, disengaged from combat!");
        isCombatActive = false;
    }

    // Interrupt or sabotage an enemy ship's warp
    public void SabotageEnemyWarp(CombatSystem enemyShip)
    {
        enemyShip.isWarping = true;
        Debug.Log("Enemy ship's warp capabilities have been sabotaged!");
        // Additional sabotage logic goes here (e.g., reducing warp time or preventing warp)
    }

    #endregion

    #region Tactical Controls

    // Shield Control: Activate or deactivate shields during combat
    public void ToggleShields(bool activate)
    {
        if (activate)
        {
            currentShield = maxShield;
            Debug.Log("Shields activated.");
        }
        else
        {
            currentShield = 0;
            Debug.Log("Shields deactivated.");
        }
    }

    // Maneuver: Dodge or evade enemy attacks
    public void EvadeManeuver()
    {
        Debug.Log("Performing evasive maneuver!");
        // Implement logic for evading enemy projectiles or missiles
    }

    #endregion
}
