using UnityEngine;

public class Pulsar : MonoBehaviour
{
    public Vector3 Position { get; private set; } // Position of the pulsar
    public float Frequency { get; private set; } // Frequency of the pulsar

    public void Initialize(Vector3 position, float frequency)
    {
        Position = position;
        Frequency = frequency;
        transform.position = position;

        // Optional: Visualize the pulsar
        Debug.Log($"Pulsar spawned at {position} with frequency {frequency} Hz");
    }

    private void Update()
    {
        // Optional: Add a pulsing visual effect
        float scale = Mathf.Abs(Mathf.Sin(Time.time * Frequency)) * 0.5f + 0.5f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
