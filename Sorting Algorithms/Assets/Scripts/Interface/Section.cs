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
    private bool isOneActiveOnlySection, onHideMakeInactive;
    private bool isHidden;

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
            if (!item.IsSectionMember)
            {
                // Item is under this section
                item.Section = this;
                item.IsSectionMember = true;

                // Add item to dictionary
                string buttonID = item.ItemID;
                sectionButtons[buttonID] = item;
            }
        }
    }

    // Section identification such as Algorithm, Teaching Mode, etc...
    public string SectionID
    {
        get { return sectionID; }
    }

    // A section of 2 or more buttons which requires only 1 active at a time
    public bool IsOneActiveOnlySection
    {
        get { return IsOneActiveOnlySection; }
    }

    // If a subsection stacks on top of another sub section
    public bool OnHideMakeInactive
    {
        get { return onHideMakeInactive; }
    }

    // A item (button) clicked reports their ID and description which then is sent to the class SettingsBase (GraphSettings/Sortsettings)
    public void ReportItemClicked(string sectionID, string itemID, string itemDescription)
    {
        settings.UpdateValueFromSettingsMenu(sectionID, itemID, itemDescription);
    }

    // ----------------------- Init item methods ----------------------- 
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
        if (onHideMakeInactive)
        {
            // Hide object makes them invisible + moves them under floor to avoid buttons to be clicked (setactive not used due to some stuff)
            if (!visible && isHidden)
                return;

            isHidden = !visible;
            Util.HideObject(gameObject, visible, true);
        }
        else
            Util.HideObject(gameObject, visible, false);
    }

}
