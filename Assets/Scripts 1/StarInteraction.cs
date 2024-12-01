using UnityEngine;

public class StarInteraction : MonoBehaviour
{
    [Header("Fuel Siphoning Settings")]
    public float baseFuelRate = 5f; // Base fuel collection rate
    public float maxProximity = 55f; // Maximum distance for siphoning
    public float overheatingThreshold = 45f; // Distance at which overheating begins
    public float overheatingRate = 5f; // Damage rate when overheating
    public float solarFlareDamage = 20f; // Damage from a solar flare

    [Header("Ship Settings")]
    public float currentFuel = 0f; // Current fuel level
    public float maxFuel = 100f; // Maximum fuel capacity
    public float currentHullIntegrity = 100f; // Current hull integrity
    public float maxHullIntegrity = 100f; // Maximum hull integrity
    public float fuelEfficiencyUpgrade = 1f; // Multiplier for upgraded fuel efficiency

    [Header("References")]
    public Transform ship; // Reference to the player's ship
    public Transform star; // Reference to the star

    private void Update()
    {
        SiphonFuel();
        CheckForOverheating();
    }

    private void SiphonFuel()
    {
        float distanceToStar = Vector3.Distance(ship.position, star.position);

        // Check if within siphoning range
        if (distanceToStar <= maxProximity)
        {
            // Calculate fuel siphoning rate based on distance
            float efficiency = Mathf.Clamp01(1 - (distanceToStar / maxProximity));
            float fuelToAdd = baseFuelRate * efficiency * fuelEfficiencyUpgrade * Time.deltaTime;

            currentFuel = Mathf.Min(currentFuel + fuelToAdd, maxFuel);
            Debug.Log($"Siphoning fuel: {fuelToAdd:F2} | Current Fuel: {currentFuel:F2}");
        }
    }

    private void CheckForOverheating()
    {
        float distanceToStar = Vector3.Distance(ship.position, star.position);

        // Check if ship is overheating
        if (distanceToStar <= overheatingThreshold)
        {
            float damage = overheatingRate * (1 - (distanceToStar / overheatingThreshold)) * Time.deltaTime;
            currentHullIntegrity = Mathf.Max(currentHullIntegrity - damage, 0);
            Debug.Log($"Overheating! Damage taken: {damage:F2} | Hull Integrity: {currentHullIntegrity:F2}");
        }
    }

    public void TriggerSolarFlare()
    {
        float distanceToStar = Vector3.Distance(ship.position, star.position);

        if (distanceToStar <= maxProximity)
        {
            currentHullIntegrity = Mathf.Max(currentHullIntegrity - solarFlareDamage, 0);
            Debug.Log($"Solar flare hit! Damage: {solarFlareDamage} | Hull Integrity: {currentHullIntegrity:F2}");
        }
        else
        {
            Debug.Log("Solar flare occurred, but the ship is too far away to take damage.");
        }
    }
}
