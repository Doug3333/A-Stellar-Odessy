using UnityEngine;
using System.Collections.Generic;

public class ShipMovement : MonoBehaviour
{
    [Header("Ship Settings")]
    public float fuel = 100f; // Starting fuel
    public float fuelConsumptionPerTile = 1f; // Fuel used per tile
    public float warpFuelMultiplier = 3f; // Multiplier for warp travel
    public float moveSpeed = 5f; // Movement speed of the ship

    [Header("Hex Grid")]
    public HexGrid hexGrid; // Reference to the hexagonal grid system
    public HexTile currentTile; // The tile the ship is currently on
    public LayerMask tileLayer; // Layer for clickable tiles

    private bool isMoving = false;
    private Queue<HexTile> movementPath = new Queue<HexTile>();
    private float remainingFuel;

    private void Start()
    {
        remainingFuel = fuel;
        if (currentTile != null)
        {
            transform.position = currentTile.GetWorldPosition();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
        else if (Input.GetMouseButtonDown(0)) // Left-click to select a destination tile
        {
            SelectDestination();
        }
    }

    private void SelectDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayer))
        {
            HexTile targetTile = hit.collider.GetComponent<HexTile>();
            if (targetTile != null && targetTile != currentTile)
            {
                List<HexTile> path = hexGrid.GetPath(currentTile, targetTile); // Use pathfinding
                float pathCost = CalculateFuelCost(path);

                if (pathCost <= remainingFuel)
                {
                    PreviewPath(path);
                    StartMovement(path);
                }
                else
                {
                    Debug.Log("Not enough fuel for this journey!");
                }
            }
        }
    }

    private void PreviewPath(List<HexTile> path)
    {
        foreach (HexTile tile in path)
        {
            tile.Highlight(true); // Assumes HexTile has a Highlight method
        }
    }

    private void StartMovement(List<HexTile> path)
    {
        foreach (HexTile tile in path)
        {
            movementPath.Enqueue(tile);
        }
        isMoving = true;
    }

    private void MoveAlongPath()
    {
        if (movementPath.Count > 0)
        {
            HexTile nextTile = movementPath.Peek();
            Vector3 targetPosition = nextTile.GetWorldPosition();
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentTile = nextTile;
                movementPath.Dequeue();
                remainingFuel -= fuelConsumptionPerTile;
                UpdateUI();
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private float CalculateFuelCost(List<HexTile> path)
    {
        return path.Count * fuelConsumptionPerTile; // Modify for warp travel or other modifiers
    }

    private void UpdateUI()
    {
        // Update fuel display or other UI elements
        Debug.Log($"Fuel remaining: {remainingFuel}");
    }
}
