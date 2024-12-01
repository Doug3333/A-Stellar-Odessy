using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    [Header("Research Settings")]
    public float researchTimeBase = 60f;               // Base time to research a technology (in seconds)
    public int resourceCostPerResearch = 100;          // Cost in resources to complete research
    public int dataCostPerResearch = 50;               // Cost in data to complete research
    public bool isResearching = false;                 // Flag to check if research is in progress
    public float researchProgress = 0f;                // Current progress of the research

    [Header("Research Tree")]
    public List<ResearchNode> researchTree;            // List of all research nodes (technologies)

    [Header("Resources")]
    public InventoryManager inventory;                 // Reference to the inventory manager to check resources

    // Research Node class
    [System.Serializable]
    public class ResearchNode
    {
        public string researchName;                     // Name of the research (e.g., new engine type)
        public string description;                       // Description of the technology
        public bool isUnlocked = false;                  // Whether this technology is unlocked
        public int dataRequired;                         // Data required to start research
        public int resourcesRequired;                    // Resources required to start research
        public float timeRequired;                       // Time required to complete the research (in seconds)
        public bool isCompleted = false;                 // Whether the technology has been completed

        // Constructor
        public ResearchNode(string name, string desc, int data, int resources, float time)
        {
            researchName = name;
            description = desc;
            dataRequired = data;
            resourcesRequired = resources;
            timeRequired = time;
        }
    }

    private void Start()
    {
        // Example of adding research nodes manually or from external configuration
        researchTree.Add(new ResearchNode("Advanced Engines", "Unlocks advanced engine for faster travel.", 100, 500, 120f));
        researchTree.Add(new ResearchNode("Shield Upgrades", "Improves energy shields to withstand stronger attacks.", 50, 300, 90f));
    }

    private void Update()
    {
        if (isResearching)
        {
            researchProgress += Time.deltaTime / researchTimeBase;

            if (researchProgress >= 1f)
            {
                CompleteResearch();
            }
        }
    }

    // Start research if conditions are met (resources and data)
    public bool StartResearch(int researchIndex)
    {
        if (researchIndex < 0 || researchIndex >= researchTree.Count)
        {
            Debug.LogWarning("Invalid research index!");
            return false;
        }

        ResearchNode selectedResearch = researchTree[researchIndex];

        if (selectedResearch.isUnlocked)
        {
            Debug.LogWarning("Research already unlocked!");
            return false;
        }

        // Check if enough resources and data are available
        if (inventory.UseResource("Data", selectedResearch.dataRequired) &&
            inventory.UseResource("Metal", selectedResearch.resourcesRequired)) // Assuming "Metal" is the required resource
        {
            isResearching = true;
            researchProgress = 0f;
            selectedResearch.timeRequired = Mathf.Max(1f, selectedResearch.timeRequired); // Prevent divide by zero
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough resources or data for research!");
            return false;
        }
    }

    // Complete the research when time is finished
    private void CompleteResearch()
    {
        foreach (ResearchNode node in researchTree)
        {
            if (!node.isUnlocked && !node.isCompleted && researchProgress >= 1f)
            {
                node.isUnlocked = true;
                node.isCompleted = true;
                isResearching = false;
                researchProgress = 0f;
                Debug.Log("Research completed: " + node.researchName);
                // Apply any unlocked benefits, such as upgrading ship systems here
                UnlockResearchBenefits(node);
                break;
            }
        }
    }

    // Apply the benefits of a researched technology (e.g., upgrading ship systems)
    private void UnlockResearchBenefits(ResearchNode research)
    {
        if (research.researchName == "Advanced Engines")
        {
            // Unlock new engine or increase ship speed
            // Example: Increase player ship speed or unlock new ability
            Debug.Log("New Engine unlocked! Ship speed increased.");
        }
        else if (research.researchName == "Shield Upgrades")
        {
            // Upgrade shields or increase shield capacity
            Debug.Log("New Shield Upgrade unlocked! Shield capacity increased.");
        }
        // Add more conditions for other research technologies here
    }

    // Display the current research tree and progress (for debugging or UI)
    public void DisplayResearchTree()
    {
        foreach (var node in researchTree)
        {
            string status = node.isUnlocked ? "Unlocked" : "Locked";
            Debug.Log("Research: " + node.researchName + " - Status: " + status);
            if (node.isUnlocked)
            {
                Debug.Log("  Description: " + node.description);
            }
        }
    }
}
