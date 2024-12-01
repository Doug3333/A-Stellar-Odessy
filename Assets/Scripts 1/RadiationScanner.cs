using UnityEngine;

public class RadiationScanner : MonoBehaviour
{
    [Header("Settings")]
    public float scanSpeed = 0.5f; // Speed of the scanning effect
    public float baseDistortion = 0.2f; // Base distortion intensity
    public float maxDistortion = 1f; // Maximum distortion level
    public float celestialInterference = 0f; // Distortion caused by celestial objects
    public float distanceMultiplier = 0.1f; // Additional distortion based on distance

    [Header("References")]
    public Material cmbMaterial; // Material for the CMB map
    public Transform scannerOrigin; // Origin of the scanner (e.g., the ship)
    public Transform target; // Target being scanned (optional, could represent celestial objects)

    private float scanProgress = 0f; // Progress of the scan (0 to 1)

    private void Start()
    {
        if (cmbMaterial == null)
        {
            Debug.LogError("CMB Material not assigned!");
        }
    }

    private void Update()
    {
        PerformScan();
        ApplyDistortion();
    }

    private void PerformScan()
    {
        // Simulate a scan progressing over time
        scanProgress += scanSpeed * Time.deltaTime;
        if (scanProgress > 1f) scanProgress = 0f;

        // Update a shader property for a scan line effect
        if (cmbMaterial != null)
        {
            cmbMaterial.SetFloat("_ScanProgress", scanProgress);
        }
    }

    private void ApplyDistortion()
    {
        if (cmbMaterial == null) return;

        // Calculate distortion based on sensor quality, distance, and celestial interference
        float distance = target != null ? Vector3.Distance(scannerOrigin.position, target.position) : 0f;
        float totalDistortion = baseDistortion
                              + (distance * distanceMultiplier)
                              + celestialInterference;

        totalDistortion = Mathf.Clamp(totalDistortion, baseDistortion, maxDistortion);

        // Pass the distortion level to the shader
        cmbMaterial.SetFloat("_DistortionLevel", totalDistortion);
    }

    // Method to simulate nearby celestial interference
    public void AddInterference(float intensity)
    {
        celestialInterference += intensity;
    }

    public void ReduceInterference(float intensity)
    {
        celestialInterference = Mathf.Max(0f, celestialInterference - intensity);
    }
}
