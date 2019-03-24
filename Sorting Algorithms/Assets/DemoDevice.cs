using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDevice : MonoBehaviour {

    private Dictionary<string, SettingsMenuItem> buttons;

    private Section section;

    private void Start()
    {
        section = GetComponentInChildren<Section>();
        section.InitItem("Pause", true);
    }
}
