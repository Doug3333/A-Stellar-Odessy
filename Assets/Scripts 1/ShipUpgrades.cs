using UnityEngine;

public class ShipUpgrades : MonoBehaviour
{
    [Header("Fuel and Heat Management")]
    public float fuelEfficiencyUpgrade = 1f; // Increases fuel collection efficiency
    public float overheatingThreshold = 75f; // Adjusts overheating proximity

    [Header("Offensive Upgrades")]
    public float damageAmount = 10f; // Base damage amount
    public float attackSpeed = 1f; // Time between attacks
    public float attackRange = 50f; // Range of attacks
    public float projectileSpeed = 20f; // Speed of projectiles
    public float missileSpeed = 30f; // Speed of missiles

    [Header("Defensive Upgrades")]
    public float movementSpeedIncrease = 10f; // Boost to ship movement speed
    public float evasionChance = 0.1f; // Chance to evade enemy projectiles and missiles
    public float missileDefenseTurretChance = 0.15f; // Chance for turrets to shoot down missiles

    [Header("Shield Upgrades")]
    public float energyShieldRechargeRate = 2f; // Rate of shield recharge per second
    public float energyShieldHitpoints = 50f; // Shield hitpoints

    [Header("Upgrade Progression")]
    public int upgradePoints = 0; // Points available to spend on upgrades

    public void ApplyUpgrade(string upgradeType, float value)
    {
        switch (upgradeType)
        {
            case "FuelEfficiency":
                fuelEfficiencyUpgrade += value;
                Debug.Log($"Fuel Efficiency upgraded to: {fuelEfficiencyUpgrade:F2}");
                break;

            case "OverheatingThreshold":
                overheatingThreshold += value;
                Debug.Log($"Overheating Threshold upgraded to: {overheatingThreshold:F2}");
                break;

            case "DamageAmount":
                damageAmount += value;
                Debug.Log($"Damage Amount upgraded to: {damageAmount:F2}");
                break;

            case "AttackSpeed":
                attackSpeed = Mathf.Max(attackSpeed - value, 0.1f); // Reduce time between attacks
                Debug.Log($"Attack Speed upgraded to: {attackSpeed:F2}");
                break;

            case "AttackRange":
                attackRange += value;
                Debug.Log($"Attack Range upgraded to: {attackRange:F2}");
                break;

            case "ProjectileSpeed":
                projectileSpeed += value;
                Debug.Log($"Projectile Speed upgraded to: {projectileSpeed:F2}");
                break;

            case "MissileSpeed":
                missileSpeed += value;
                Debug.Log($"Missile Speed upgraded to: {missileSpeed:F2}");
                break;

            case "MovementSpeed":
                movementSpeedIncrease += value;
                Debug.Log($"Movement Speed upgraded to: {movementSpeedIncrease:F2}");
                break;

            case "EvasionChance":
                evasionChance = Mathf.Clamp(evasionChance + value, 0f, 1f);
                Debug.Log($"Evasion Chance upgraded to: {evasionChance:P2}");
                break;

            case "MissileDefense":
                missileDefenseTurretChance = Mathf.Clamp(missileDefenseTurretChance + value, 0f, 1f);
                Debug.Log($"Missile Defense Turret Chance upgraded to: {missileDefenseTurretChance:P2}");
                break;

            case "ShieldRechargeRate":
                energyShieldRechargeRate += value;
                Debug.Log($"Shield Recharge Rate upgraded to: {energyShieldRechargeRate:F2}");
                break;

            case "ShieldHitpoints":
                energyShieldHitpoints += value;
                Debug.Log($"Shield Hitpoints upgraded to: {energyShieldHitpoints:F2}");
                break;

            default:
                Debug.LogWarning($"Unknown upgrade type: {upgradeType}");
                break;
        }
    }

    public void SpendUpgradePoint(string upgradeType, float value)
    {
        if (upgradePoints > 0)
        {
            ApplyUpgrade(upgradeType, value);
            upgradePoints--;
            Debug.Log($"Upgrade applied: {upgradeType}. Remaining points: {upgradePoints}");
        }
        else
        {
            Debug.LogWarning("Not enough upgrade points!");
        }
    }

    /*
    if(asteroidMining.GetMineralCount("Gold") >= 10 &&
    asteroidMining.GetMineralCount("Platinum") >= 5)
    {
    shipUpgrades.SpendUpgradePoint("DamageAmount", 5);
    asteroidMining.OffloadMinerals();
    }
     else
    {
    Debug.Log("Not enough resources for this upgrade!");

     } 


    1. Titanium (Ti)

    Use: Lightweight, strong, and resistant to corrosion. Often used in frames, landing gear, and critical structural parts.

2. Iron (Fe)

    Use: Widely used in alloys for structural integrity and durability. Common in steel and other heavy-duty materials.

3. Copper (Cu)

    Use: High electrical and thermal conductivity. Used in wiring, power systems, and electronics.

4. Gold (Au)

    Use: Excellent conductor and highly resistant to corrosion. Often used in connectors, wiring, and spacecraft coatings.

5. Silicon (Si)

    Use: Semiconductor material for electronic devices, solar panels, and sensors.

6. Carbon (C)

    Use: Used in the form of composites (carbon fiber) for its lightweight and high strength. Also used for heat shields and advanced propulsion systems.

7. Aluminum (Al)

    Use: Lightweight and strong; used extensively in spacecraft for frames, skin, and structural components.

8. Magnesium (Mg)

    Use: Lightweight and strong. Used in alloys to reduce spacecraft weight without compromising strength.

9. Nickel (Ni)

    Use: Corrosion-resistant and used in superalloys for engines, turbines, and exhaust systems.

10. Tungsten (W)

    Use: High density and melting point. Used for radiation shielding, ballast, and rocket nozzles.

11. Zirconium (Zr)

    Use: Corrosion-resistant and stable under high heat. Used in high-performance alloys and heat shields.

12. Beryllium (Be)

    Use: Very lightweight and strong. Often used in parts requiring high stiffness and low weight, such as structural components and heat shields.

13. Cobalt (Co)

    Use: Used in high-strength alloys and magnetic materials, such as in turbines and electrical systems.

14. Lithium (Li)

    Use: Lightweight and used in batteries (e.g., lithium-ion batteries) for energy storage.

15. Palladium (Pd)

    Use: Used in fuel cells and catalytic converters, particularly for hydrogen storage and conversion.

16. Vanadium (V)

    Use: Used in high-strength steel alloys for aerospace applications. Provides durability under stress and extreme conditions.

17. Rhodium (Rh)

    Use: Corrosion-resistant and used in electronics and catalytic systems for fuel efficiency.

18. Neodymium (Nd)

    Use: Strong permanent magnets, essential for motors, actuators, and sensors in spacecraft electronics.

19. Platinum (Pt)

    Use: Catalyst material in fuel cells and used in other advanced electronics due to its resistance to corrosion.

20. Hafnium (Hf)

    Use: High resistance to heat and corrosion, making it ideal for high-temperature alloys, control rods, and nuclear applications.

    21: Plotunium (Pu)
    */
}
