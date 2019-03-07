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

    private Dictionary<string, SettingsMenuItem> sectionButtons;

    private void Awake()
    {
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

    public bool IsOneActiveOnlySection
    {
        get { return IsOneActiveOnlySection; }
    }

    public bool IsSubSection
    {
        get { return isSubSection; }
    }

    // If one item activates within a group, then deactivate the others within this group
    public void ReportActivation(string itemID)
    {
        if (isOneActiveOnlySection)
        {
            foreach (KeyValuePair<string, SettingsMenuItem> entry in sectionButtons)
            {
                if (entry.Key != itemID)
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
