using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : SettingsMenuButton {

    [SerializeField]
    private Material onMaterial, offMaterial;

    [SerializeField]
    private string onText, offText;

    private bool state;

    public void InitSettingsButton(bool state)
    {
        this.state = !state;
        Toggle();
    }

    // Toggles between two states
    public void Toggle()
    {
        state = !state;

        if (state)
            ChangeAppearance(onText, onMaterial);
        else
            ChangeAppearance(offText, offMaterial);

    }
    
}
