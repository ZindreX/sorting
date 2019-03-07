using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Section : MonoBehaviour {

    [SerializeField]
    private string sectionID;

    private Dictionary<string, SettingsButton> sectionButtons;

    private void Awake()
    {
        // Get all buttons in this section and add them to a dictionary
        sectionButtons = new Dictionary<string, SettingsButton>();

        Component[] buttons = GetComponentsInChildren<SettingsButton>();
        foreach (SettingsButton sButton in buttons)
        {
            string buttonID = sButton.ButtonID;
            sectionButtons[buttonID] = sButton;
        }
    }

    private void Update()
    {
        
    }


    public void ReportActivation(string buttonID)
    {
        foreach (KeyValuePair<string, SettingsButton> entry in sectionButtons)
        {
            if (entry.Key != buttonID)
                Debug.Log(entry.Value.ButtonID);
                //entry.Value.Deactivate();
        }
    }

    public void SetSectionVisible(bool visible)
    {
        Util.HideObject(gameObject, visible);
    }

}
