using System.Collections.Generic;
using UnityEngine;

public class PulsarSpawner : MonoBehaviour
{
    [Header("Pulsar Settings")]
    public GameObject pulsarPrefab; // Prefab for the pulsar object
    public int numberOfPulsars = 5; // Number of pulsars to spawn
    public Vector3 universeBounds = new Vector3(1000, 1000, 1000); // Bounds of the universe
    public float minFrequency = 1f; // Minimum pulsar frequency
    public float maxFrequency = 10f; // Maximum pulsar frequency

    [Header("References")]
    public Transform player; // Reference to the player's ship

    private List<Pulsar> pulsars = new List<Pulsar>(); // List of spawned pulsars

    private void Start()
    {
        SpawnPulsars();
    }

    private void SpawnPulsars()
    {
        for (int i = 0; i < numberOfPulsars; i++)
        {
            // Generate a random position within bounds
            Vector3 position = new Vector3(
                Random.Range(-universeBounds.x / 2, universeBounds.x / 2),
                Random.Range(-universeBounds.y / 2, universeBounds.y / 2),
                Random.Range(-universeBounds.z / 2, universeBounds.z / 2)
            );

            // Instantiate the pulsar
            GameObject pulsarObj = Instantiate(pulsarPrefab, position, Quaternion.identity);

            // Set unique frequency for the pulsar
            float frequency = Random.Range(minFrequency, maxFrequency);

            // Assign pulsar properties
            Pulsar pulsar = pulsarObj.GetComponent<Pulsar>();
            if (pulsar == null)
            {
                Debug.LogError("Pulsar prefab must have a Pulsar component!");
                continue;
            }
            pulsar.Initialize(position, frequency);

            // Add to the list for triangulation and tracking
            pulsars.Add(pulsar);
        }
    }

    public Pulsar LockOntoClosestPulsar()
    {
        if (player == null || pulsars.Count == 0)
        {
            Debug.LogWarning("Player or pulsars not available for locking!");
            return null;
        }

        // Find the closest pulsar to the player
        Pulsar closestPulsar = null;
        float closestDistance = Mathf.Infinity;

        foreach (Pulsar pulsar in pulsars)
        {
            float distance = Vector3.Distance(player.position, pulsar.Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPulsar = pulsar;
            }
        }

        if (closestPulsar != null)
        {
            Debug.Log($"Locked onto pulsar at {closestPulsar.Position} with frequency {closestPulsar.Frequency} Hz");
        }

        return closestPulsar;
    }
}
