using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text fuelText;                         // UI Text for displaying fuel
    public Text foodText;                         // UI Text for displaying food
    public Text rawMaterialText;                  // UI Text for displaying raw materials
    public Text energyText;                       // UI Text for displaying energy levels
    public Text crewCountText;                    // UI Text for displaying current crew count
    public Text eventLogText;                     // UI Text for displaying event logs
    public Text moraleText;                       // UI Text for displaying crew morale
    public Text researchProgressText;             // UI Text for displaying research progress
    public Text mapDataText;                      // UI Text for displaying map data (current sector)

    [Header("UI Layouts")]
    public GameObject basicLayout;                // Basic UI layout for showing core resources
    public GameObject advancedLayout;             // Advanced layout for additional stats and ship systems
    public GameObject eventLogLayout;             // Layout for event logs
    public GameObject researchLayout;             // Layout for research info

    [Header("References")]
    public ResourceManager resourceManager;       // Reference to the resource manager
    public CrewManager crewManager;               // Reference to the crew manager
    public EngineSystem engineSystem;             // Reference to the engine system
    public MoraleSystem moraleSystem;             // Reference to the morale system
    public StarMapGenerator starMapGenerator;     // Reference to the star map generator
    public ResearchManager researchManager;       // Reference to the research manager
    public EventManager eventManager;             // Reference to the event manager

    private bool isAdvancedLayout = false;

    private void Start()
    {
        // Initialize UI with default layout and resource values
        UpdateResourceUI();
        UpdateCrewUI();
        UpdateEventLogUI();
        UpdateMapDataUI();
        UpdateMoraleUI();
        UpdateResearchUI();

        // Set the basic layout as the default layout
        SwitchToBasicLayout();
    }

    private void Update()
    {
        // Continuously update the resource UI based on current values
        UpdateResourceUI();
        UpdateCrewUI();
        UpdateEventLogUI();
        UpdateMoraleUI();
        UpdateMapDataUI();
        UpdateResearchUI();
    }

    #region UI Update Methods

    // Update resource-related UI
    private void UpdateResourceUI()
    {
        fuelText.text = "Fuel: " + resourceManager.GetResourceAmount("Fuel") + " / " + resourceManager.fuelAmount;
        foodText.text = "Food: " + resourceManager.GetResourceAmount("Food") + " / " + resourceManager.foodAmount;
        rawMaterialText.text = "Raw Materials: " + resourceManager.GetResourceAmount("RawMaterials") + " / " + resourceManager.rawMaterialAmount;
        energyText.text = "Energy: " + resourceManager.GetResourceAmount("Energy") + " / " + resourceManager.energyAmount;
    }

    // Update crew count UI
    private void UpdateCrewUI()
    {
        crewCountText.text = "Crew: " + crewManager.GetCrewCount();
    }

    // Update event log UI
    private void UpdateEventLogUI()
    {
        eventLogText.text = eventManager.GetLatestEventLog();
    }

    // Update morale UI
    private void UpdateMoraleUI()
    {
        moraleText.text = "Morale: " + moraleSystem.GetMoraleLevel() + "%";
    }

    // Update map data UI
    private void UpdateMapDataUI()
    {
        mapDataText.text = "Current Sector: " + starMapGenerator.GetCurrentSector();
    }

    // Update research progress UI
    private void UpdateResearchUI()
    {
        researchProgressText.text = "Research Progress: " + researchManager.GetResearchProgress() + "%";
    }

    #endregion

    #region Layout Management

    // Switch to basic layout (core resource stats)
    public void SwitchToBasicLayout()
    {
        basicLayout.SetActive(true);
        advancedLayout.SetActive(false);
        eventLogLayout.SetActive(false);
        researchLayout.SetActive(false);
    }

    // Switch to advanced layout (additional stats, engine, crew, etc.)
    public void SwitchToAdvancedLayout()
    {
        basicLayout.SetActive(false);
        advancedLayout.SetActive(true);
        eventLogLayout.SetActive(false);
        researchLayout.SetActive(false);
    }

    // Switch to event log layout
    public void SwitchToEventLogLayout()
    {
        basicLayout.SetActive(false);
        advancedLayout.SetActive(false);
        eventLogLayout.SetActive(true);
        researchLayout.SetActive(false);
    }

    // Switch to research layout
    public void SwitchToResearchLayout()
    {
        basicLayout.SetActive(false);
        advancedLayout.SetActive(false);
        eventLogLayout.SetActive(false);
        researchLayout.SetActive(true);
    }

    #endregion

    #region User Actions (Buttons)

    // Toggle between basic and advanced layouts
    public void ToggleLayout()
    {
        isAdvancedLayout = !isAdvancedLayout;

        if (isAdvancedLayout)
        {
            SwitchToAdvancedLayout();
        }
        else
        {
            SwitchToBasicLayout();
        }
    }

    // Toggle event log view
    public void ToggleEventLog()
    {
        if (eventLogLayout.activeSelf)
        {
            SwitchToBasicLayout();
        }
        else
        {
            SwitchToEventLogLayout();
        }
    }

    // Toggle research view
    public void ToggleResearchView()
    {
        if (researchLayout.activeSelf)
        {
            SwitchToBasicLayout();
        }
        else
        {
            SwitchToResearchLayout();
        }
    }

    #endregion
}