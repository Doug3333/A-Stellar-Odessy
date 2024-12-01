using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab; // Prefab for hex tile
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float hexSize = 1f; // Size of each hex tile

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        float xOffset = hexSize * Mathf.Sqrt(3);
        float yOffset = hexSize * 1.5f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float xPos = x * xOffset;
                float yPos = y * yOffset;

                // Offset every second column for a staggered hex grid
                if (y % 2 == 1) xPos += xOffset / 2f;

                // Instantiate the hex tile at the calculated position
                GameObject hex = Instantiate(hexTilePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                hex.transform.parent = this.transform;
            }
        }
    }
}
