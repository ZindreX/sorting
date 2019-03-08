using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Section : MonoBehaviour {

    /* --------------------------------- Section ---------------------------------
     * - Managing grouped buttons
     * 
     * 
    */

    [SerializeField]
    private string sectionID;

    [SerializeField]
    private bool isOneActiveOnlySection, isSubSection;

    [SerializeField]
    private SettingsBase settings;

    private Dictionary<string, SettingsMenuItem> sectionButtons;

    private void Awake()
    {
        // Set title
        GetComponentInChildren<TextMeshPro>().text = sectionID;

        // Get all buttons in this section and add them to a dictionary
        sectionButtons = new Dictionary<string, SettingsMenuItem>();

        Component[] buttons = GetComponentsInChildren<SettingsMenuItem>();
        foreach (SettingsMenuItem item in buttons)
        {
            // Item is under this section
            item.Section = this;

            // Add item to dictionary
            string buttonID = item.ItemID;
            sectionButtons[buttonID] = item;

        }
    }

    public string SectionID
    {
        get { return sectionID; }
    }

    public bool IsOneActiveOnlySection
    {
        get { return IsOneActiveOnlySection; }
    }

    public bool IsSubSection
    {
        get { return isSubSection; }
    }

    public void ReportItemClicked(string itemID)
    {
        settings.UpdateValueFromSettingsMenu(sectionID, itemID);
    }

    public void InitItem(string itemID)
    {
        SettingsMenuItem item = sectionButtons[itemID];

        switch (item.ItemRole())
        {
            case Util.ONE_ACTIVE_BUTTON: ((OneActiveButton)item).ActivateItem(); break;
            case Util.STATIC_BUTTON: break;
        }
    }

    public void InitItem(string itemID, int value)
    {
        SettingsMenuItem item = sectionButtons[itemID];

        switch (item.ItemRole())
        {
            case Util.MULTI_STATE_BUTTON: ((MultiStateButton)item).InitMultiStateButton(value); break;
        }
    }

    public void InitItem(string itemID, bool value)
    {
        SettingsMenuItem item = sectionButtons[itemID];

        switch (item.ItemRole())
        {
            case Util.TOGGLE_BUTTON: ((ToggleButton)item).InitToggleButton(value); break;
        }
    }

    // If one item activates within a group, then deactivate the others within this group
    public void ReportActivation(string itemID)
    {
        foreach (KeyValuePair<string, SettingsMenuItem> entry in sectionButtons)
        {
            if (entry.Key != itemID)
            {
                SettingsMenuItem item = entry.Value;
                if (item.ItemRole() == Util.ONE_ACTIVE_BUTTON)
                {
                    OneActiveButton button = (OneActiveButton)entry.Value;
                    button.Deactivateitem();
                }
            }

        }
    }

    public void SetSectionVisible(bool visible)
    {
        Util.HideObject(gameObject, visible);
    }

}
