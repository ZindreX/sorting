using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsBase : StartExchangePosition, ISectionManager {

    [Header("Settings base")]
    protected TextMeshPro tooltips;

    protected Dictionary<string, Section> settingsSections;

    protected override void Awake()
    {
        // Position in scene
        base.Awake();
        InScenePosition = new Vector3(0f, -5f, 5f);
        SwapPositions();

        //
        Component[] textMeshes = GetComponentsInChildren<TextMeshPro>();
        tooltips = (TextMeshPro)textMeshes[1];

        // Get all sections from the settings menu
        settingsSections = new Dictionary<string, Section>();
        Component[] sectionComponents = GetComponentsInChildren<Section>();
        foreach (Section section in sectionComponents)
        {
            string sectionID = section.SectionID;
            section.SectionManager = this;
            if (!settingsSections.ContainsKey(sectionID))
                settingsSections[sectionID] = section;
            else
                Debug.LogError("Section already in dict: " + sectionID);
        }
    }

    protected abstract void InitButtons();

    // All input from interactable settings menu goes through here
    public abstract void UpdateInteraction(string sectionID, string itemID, string itemDescription);

    // Set thje settings menu text
    public void FillTooltips(string text, bool leftAlignment)
    {
        if (tooltips != null)
        {
            if (leftAlignment)
                tooltips.alignment = TextAlignmentOptions.Left;
            else
                tooltips.alignment = TextAlignmentOptions.Center;

            tooltips.text = text;

        }
    }

    // ----------------------- Init settings menu buttons -----------------------
    protected void InitButtonState(string sectionID, string itemID)
    {
        if (settingsSections.ContainsKey(sectionID))
        {
            Section section = settingsSections[sectionID];

            section.InitItem(itemID);
        }
        else
            Debug.LogError("No section: " + sectionID);
    }

    protected void InitButtonState(string sectionID, string itemID, int value)
    {
        if (settingsSections.ContainsKey(sectionID))
        {
            Section section = settingsSections[sectionID];

            section.InitItem(itemID, value);

        }
        else
            Debug.LogError("No section: " + sectionID);
    }

    protected void InitButtonState(string sectionID, string itemID, bool value)
    {
        if (settingsSections.ContainsKey(sectionID))
        {
            Section section = settingsSections[sectionID];

            section.InitItem(itemID, value);

        }
        else
            Debug.LogError("No section: " + sectionID);
    }

}
