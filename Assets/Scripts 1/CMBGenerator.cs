using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CMBGenerator : MonoBehaviour
{
    [Header("Settings")]
    public int textureResolution = 512; // Resolution of the generated texture
    public float noiseScale = 10f; // Scale of the noise patterns
    public float distortionIntensity = 1f; // Intensity of distortion effects
    public Color baseColor = Color.black; // Base background color
    public Color noiseColor = Color.white; // Color for the noise patterns

    private Material cmbMaterial; // Material to apply the CMB texture
    private RenderTexture cmbTexture; // Dynamically generated texture

    private void Start()
    {
        // Set up the RenderTexture
        cmbTexture = new RenderTexture(textureResolution, textureResolution, 0);
        cmbTexture.enableRandomWrite = true;
        cmbTexture.Create();

        // Assign a material to this object's renderer
        cmbMaterial = GetComponent<Renderer>().material;

        // Set up shader with dynamic texture
        if (cmbMaterial != null)
        {
            cmbMaterial.mainTexture = cmbTexture;
        }

        // Generate initial CMB pattern
        GenerateCMB();
    }

    private void Update()
    {
        // Allow dynamic changes via inspector or gameplay
        GenerateCMB();
    }

    private void GenerateCMB()
    {
        // Create a Perlin-based noise texture
        Texture2D noiseTexture = new Texture2D(textureResolution, textureResolution);
        for (int x = 0; x < textureResolution; x++)
        {
            for (int y = 0; y < textureResolution; y++)
            {
                float xCoord = (float)x / textureResolution * noiseScale;
                float yCoord = (float)y / textureResolution * noiseScale;

                // Generate noise with distortion
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord)
                                   + Mathf.PerlinNoise(xCoord * distortionIntensity, yCoord * distortionIntensity) * 0.5f;
                noiseValue = Mathf.Clamp01(noiseValue);

                // Blend noise color with the base color
                Color pixelColor = Color.Lerp(baseColor, noiseColor, noiseValue);
                noiseTexture.SetPixel(x, y, pixelColor);
            }
        }

        noiseTexture.Apply();

        // Apply to RenderTexture
        Graphics.Blit(noiseTexture, cmbTexture);
        Destroy(noiseTexture); // Clean up temporary texture
    }

    public void SetDistortion(float intensity)
    {
        distortionIntensity = intensity;
        GenerateCMB();
    }

    public void SetNoiseScale(float scale)
    {
        noiseScale = scale;
        GenerateCMB();
    }
}
