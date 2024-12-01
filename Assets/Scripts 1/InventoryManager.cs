using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxStorageCapacity = 100;            // Max total storage capacity
    public int resourceStorageCapacity = 50;        // Capacity for resources
    public int foodStorageCapacity = 20;            // Capacity for food items
    public int artifactStorageCapacity = 10;        // Capacity for artifacts
    public int fightersCapacity = 5;                // Capacity for fighters/bombers

    [Header("Inventory Data")]
    public List<ResourceItem> resources = new List<ResourceItem>();
    public List<FoodItem> foodItems = new List<FoodItem>();
    public List<ArtifactItem> artifacts = new List<ArtifactItem>();
    public List<FighterBomber> fightersAndBombers = new List<FighterBomber>();

    [Header("Crafting/Trade Settings")]
    public bool autoOrganize = true;               // Automatically organizes the inventory

    // Resource Item class
    [System.Serializable]
    public class ResourceItem
    {
        public string resourceName;
        public int quantity;
        public ResourceItem(string name, int qty)
        {
            resourceName = name;
            quantity = qty;
        }
    }

    // Food Item class
    [System.Serializable]
    public class FoodItem
    {
        public string foodName;
        public int quantity;
        public FoodItem(string name, int qty)
        {
            foodName = name;
            quantity = qty;
        }
    }

    // Artifact Item class
    [System.Serializable]
    public class ArtifactItem
    {
        public string artifactName;
        public string description;
        public ArtifactItem(string name, string desc)
        {
            artifactName = name;
            description = desc;
        }
    }

    // Fighter/Bomber class
    [System.Serializable]
    public class FighterBomber
    {
        public string fighterName;
        public int damage;
        public int health;
        public FighterBomber(string name, int dmg, int hp)
        {
            fighterName = name;
            damage = dmg;
            health = hp;
        }
    }

    private void Start()
    {
        if (autoOrganize)
        {
            OrganizeInventory();
        }
    }

    private void Update()
    {
        // Here, you could handle any time-based checks for the inventory (e.g., checking if crafting/upgrades are ready)
    }

    // Add resources to the inventory
    public bool AddResource(string resourceName, int quantity)
    {
        if (resources.Count < resourceStorageCapacity)
        {
            ResourceItem existingItem = resources.Find(item => item.resourceName == resourceName);
            if (existingItem != null)
            {
                existingItem.quantity += quantity;
            }
            else
            {
                resources.Add(new ResourceItem(resourceName, quantity));
            }
            return true;
        }
        return false; // Not enough space
    }

    // Add food to the inventory
    public bool AddFood(string foodName, int quantity)
    {
        if (foodItems.Count < foodStorageCapacity)
        {
            FoodItem existingFood = foodItems.Find(item => item.foodName == foodName);
            if (existingFood != null)
            {
                existingFood.quantity += quantity;
            }
            else
            {
                foodItems.Add(new FoodItem(foodName, quantity));
            }
            return true;
        }
        return false; // Not enough space
    }

    // Add artifacts to the inventory
    public bool AddArtifact(string artifactName, string description)
    {
        if (artifacts.Count < artifactStorageCapacity)
        {
            artifacts.Add(new ArtifactItem(artifactName, description));
            return true;
        }
        return false; // Not enough space
    }

    // Add fighter or bomber to the inventory
    public bool AddFighterBomber(string fighterName, int damage, int health)
    {
        if (fightersAndBombers.Count < fightersCapacity)
        {
            fightersAndBombers.Add(new FighterBomber(fighterName, damage, health));
            return true;
        }
        return false; // Not enough space
    }

    // Organize the inventory (sorting by type and quantity)
    public void OrganizeInventory()
    {
        resources.Sort((x, y) => x.resourceName.CompareTo(y.resourceName));
        foodItems.Sort((x, y) => x.foodName.CompareTo(y.foodName));
        artifacts.Sort((x, y) => x.artifactName.CompareTo(y.artifactName));
        fightersAndBombers.Sort((x, y) => x.fighterName.CompareTo(y.fighterName));
    }

    // Display inventory (for debugging or UI purposes)
    public void DisplayInventory()
    {
        Debug.Log("Resources:");
        foreach (var item in resources)
        {
            Debug.Log(item.resourceName + ": " + item.quantity);
        }

        Debug.Log("Food:");
        foreach (var item in foodItems)
        {
            Debug.Log(item.foodName + ": " + item.quantity);
        }

        Debug.Log("Artifacts:");
        foreach (var item in artifacts)
        {
            Debug.Log(item.artifactName + ": " + item.description);
        }

        Debug.Log("Fighters and Bombers:");
        foreach (var item in fightersAndBombers)
        {
            Debug.Log(item.fighterName + " - Damage: " + item.damage + ", Health: " + item.health);
        }
    }

    // Use a resource (e.g., craft upgrades or trade)
    public bool UseResource(string resourceName, int quantity)
    {
        ResourceItem item = resources.Find(r => r.resourceName == resourceName);
        if (item != null && item.quantity >= quantity)
        {
            item.quantity -= quantity;
            return true;
        }
        return false; // Not enough resources
    }

    // Trade resources with other factions or NPCs
    public bool TradeResources(string resourceName, int quantity, InventoryManager otherParty)
    {
        if (UseResource(resourceName, quantity))
        {
            otherParty.AddResource(resourceName, quantity);
            return true;
        }
        return false; // Not enough resources to trade
    }
}
