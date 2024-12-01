using UnityEngine;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour
{
    public List<HexTile> allTiles = new List<HexTile>();

    public List<HexTile> GetPath(HexTile start, HexTile end)
    {
        // Placeholder for pathfinding logic
        List<HexTile> path = new List<HexTile> { start, end };
        return path;
    }
}