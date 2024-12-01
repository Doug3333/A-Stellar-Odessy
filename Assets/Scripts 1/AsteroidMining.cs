using UnityEngine;

public class AsteroidMining : MonoBehaviour
{
    [Header("Mining Settings")]
    public float miningRate = 1f; // Rate of mining (minerals per second)
    public int miningCapacity = 50; // Maximum amount of minerals that can be stored at once
    public float miningRange = 20f; // Maximum distance to mine asteroids

    [Header("Mineral Types")]
    public string[] minerals = new string[10] {
        "Iron", "Nickel", "Copper", "Gold", "Platinum",
        "Silicon", "Cobalt", "Uranium", "Tungsten", "Lithium"
    };
    public int[] collectedMinerals; // Stores the count of each mineral type

    [Header("References")]
    public Transform ship; // Reference to the player's ship
    public Transform asteroidField; // Reference to the asteroid field

    private float miningCooldown = 0f;

    private void Start()
    {
        collectedMinerals = new int[minerals.Length];
    }

    private void Update()
    {
        if (miningCooldown > 0)
            miningCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.M)) // Example input for mining action
        {
            StartMining();
        }
    }

    public void StartMining()
    {
        float distanceToField = Vector3.Distance(ship.position, asteroidField.position);

        if (distanceToField <= miningRange)
        {
            if (miningCooldown <= 0 && GetTotalMinerals() < miningCapacity)
            {
                MineMinerals();
                miningCooldown = 1f / miningRate;
            }
            else if (GetTotalMinerals() >= miningCapacity)
            {
                Debug.Log("Mining capacity reached! Return to base to offload minerals.");
            }
        }
        else
        {
            Debug.Log("You are too far from the asteroid field to mine.");
        }
    }

    private void MineMinerals()
    {
        int minedIndex = Random.Range(0, minerals.Length);
        collectedMinerals[minedIndex]++;
        Debug.Log($"Mined 1 unit of {minerals[minedIndex]}. Total: {collectedMinerals[minedIndex]}");
    }

    public int GetTotalMinerals()
    {
        int total = 0;
        foreach (int count in collectedMinerals)
        {
            total += count;
        }
        return total;
    }

    public void OffloadMinerals()
    {
        Debug.Log("Offloading minerals...");
        for (int i = 0; i < collectedMinerals.Length; i++)
        {
            if (collectedMinerals[i] > 0)
            {
                Debug.Log($"Offloaded {collectedMinerals[i]} units of {minerals[i]}.");
                collectedMinerals[i] = 0;
            }
        }
    }

    public int GetMineralCount(string mineralName)
    {
        int index = System.Array.IndexOf(minerals, mineralName);
        return index >= 0 ? collectedMinerals[index] : 0;
    }
}
