using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsBase : MonoBehaviour, ISectionManager {

    [Header("Settings base")]
    protected TextMeshPro tooltips;

    protected Dictionary<string, Section> settingsSections;

    protected void Awake()
    {
        Component[] textMeshes = GetComponentsInChildren<TextMeshPro>();
        tooltips = (TextMeshPro)textMeshes[1];

        // Get all sections from the settings menu
        settingsSections = new Dictionary<string, Section>();
        Component[] sectionComponents = GetComponentsInChildren<Section>();
        foreach (Section section in sectionComponents)
        {
            string sectionID = section.SectionID;
            Debug.Log("Section: " + sectionID);

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
    public void FillTooltips(string text)
    {
        if (tooltips != null)
            tooltips.text = text;
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

    public void SetSettingsActive(bool active)
    {
        Util.HideObject(gameObject, active, true);
    }

}
