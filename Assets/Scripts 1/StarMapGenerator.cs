using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMapGenerator : MonoBehaviour
{
    [Header("Star Map Settings")]
    public int mapWidth = 100;               // Width of the star map (number of sectors across)
    public int mapHeight = 100;              // Height of the star map (number of sectors down)
    public int maxStarsPerSector = 5;        // Max number of stars per sector
    public int maxPlanetsPerStar = 3;        // Max number of planets per star
    public int maxAnomaliesPerSector = 2;    // Max anomalies per sector
    public float sectorSize = 50f;           // The size of each sector in world space

    [Header("Prefab References")]
    public GameObject starPrefab;            // Prefab for a star object
    public GameObject planetPrefab;          // Prefab for a planet object
    public GameObject anomalyPrefab;         // Prefab for anomalies

    [Header("Exploration Settings")]
    public Transform player;                 // Player's transform to track exploration
    public float revealDistance = 150f;      // Distance at which new sectors are revealed

    private Vector2 playerPosition;          // Player's current position in terms of sectors
    private Dictionary<Vector2, Sector> sectors = new Dictionary<Vector2, Sector>(); // Store generated sectors

    private void Start()
    {
        playerPosition = new Vector2(Mathf.FloorToInt(player.position.x / sectorSize), Mathf.FloorToInt(player.position.z / sectorSize));
        GenerateMap();
    }

    private void Update()
    {
        Vector2 currentPlayerSector = new Vector2(Mathf.FloorToInt(player.position.x / sectorSize), Mathf.FloorToInt(player.position.z / sectorSize));
        if (currentPlayerSector != playerPosition)
        {
            playerPosition = currentPlayerSector;
            RevealNearbySectors();
        }
    }

    private void GenerateMap()
    {
        // Generate stars, planets, and anomalies for each sector
        for (int x = -mapWidth / 2; x < mapWidth / 2; x++)
        {
            for (int z = -mapHeight / 2; z < mapHeight / 2; z++)
            {
                Vector2 sectorPosition = new Vector2(x, z);
                GenerateSector(sectorPosition);
            }
        }
    }

    private void GenerateSector(Vector2 sectorPosition)
    {
        if (sectors.ContainsKey(sectorPosition)) return;

        // Create a new sector and generate its contents
        Sector newSector = new Sector();
        newSector.position = sectorPosition;

        // Generate stars for the sector
        int starsToGenerate = Random.Range(1, maxStarsPerSector + 1);
        for (int i = 0; i < starsToGenerate; i++)
        {
            Vector3 starPosition = new Vector3(sectorPosition.x * sectorSize + Random.Range(-sectorSize / 2, sectorSize / 2),
                                               Random.Range(0f, 10f), // Star height can be adjusted
                                               sectorPosition.y * sectorSize + Random.Range(-sectorSize / 2, sectorSize / 2));
            GameObject star = Instantiate(starPrefab, starPosition, Quaternion.identity);
            star.name = "Star_" + sectorPosition + "_" + i;
            newSector.stars.Add(star);

            // Generate planets around the star
            int planetsToGenerate = Random.Range(1, maxPlanetsPerStar + 1);
            for (int j = 0; j < planetsToGenerate; j++)
            {
                Vector3 planetPosition = starPosition + Random.onUnitSphere * Random.Range(10f, 50f); // Planets at random distance from star
                GameObject planet = Instantiate(planetPrefab, planetPosition, Quaternion.identity);
                planet.name = "Planet_" + star.name + "_" + j;
                newSector.planets.Add(planet);
            }
        }

        // Generate anomalies for the sector
        int anomaliesToGenerate = Random.Range(0, maxAnomaliesPerSector + 1);
        for (int i = 0; i < anomaliesToGenerate; i++)
        {
            Vector3 anomalyPosition = new Vector3(sectorPosition.x * sectorSize + Random.Range(-sectorSize / 2, sectorSize / 2),
                                                  Random.Range(0f, 10f),
                                                  sectorPosition.y * sectorSize + Random.Range(-sectorSize / 2, sectorSize / 2));
            GameObject anomaly = Instantiate(anomalyPrefab, anomalyPosition, Quaternion.identity);
            anomaly.name = "Anomaly_" + sectorPosition + "_" + i;
            newSector.anomalies.Add(anomaly);
        }

        sectors.Add(sectorPosition, newSector);
    }

    private void RevealNearbySectors()
    {
        // Reveal sectors in the surrounding area based on the player's position
        int revealRadius = Mathf.CeilToInt(revealDistance / sectorSize);
        for (int x = -revealRadius; x <= revealRadius; x++)
        {
            for (int z = -revealRadius; z <= revealRadius; z++)
            {
                Vector2 sectorPosition = playerPosition + new Vector2(x, z);
                if (!sectors.ContainsKey(sectorPosition))
                {
                    GenerateSector(sectorPosition);
                }
            }
        }
    }

    // Helper class to store sector data
    private class Sector
    {
        public Vector2 position;
        public List<GameObject> stars = new List<GameObject>();
        public List<GameObject> planets = new List<GameObject>();
        public List<GameObject> anomalies = new List<GameObject>();
    }
}
