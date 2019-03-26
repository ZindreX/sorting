using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDevice : MonoBehaviour {

    private Dictionary<string, SettingsMenuItem> buttons;

    private Section section;
    private ToggleButton pauseButton;
    private bool pauseState;

    private void Start()
    {
        section = GetComponentInChildren<Section>();
        section.InitItem(Util.PAUSE, true);

        Component[] components = GetComponentsInChildren<SettingsMenuItem>();
        foreach (Component component in components)
        {
            SettingsMenuItem item = component.GetComponent<SettingsMenuItem>();
            buttons.Add(item.ItemID, item);
        }

        pauseButton = (ToggleButton)buttons[Util.PAUSE];
        buttons["Reduce speed"].gameObject.SetActive(false);
        buttons["Increase speed"].gameObject.SetActive(false);
    }

    private void Update()
    {
        if (pauseState != pauseButton.State)
        {

        }
    }
}
