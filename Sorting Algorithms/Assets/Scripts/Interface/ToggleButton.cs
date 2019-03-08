using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : SettingsMenuButton {

    [SerializeField]
    private Material onMaterial, offMaterial;

    [SerializeField]
    private string onText, offText;

    private bool state;

    protected override void Awake()
    {
        base.Awake();

        state = false;
        ChangeAppearance(offText, offMaterial);
    }

    public void InitToggleButton(bool state)
    {
        if (state)
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

    public override string ItemRole()
    {
        return Util.TOGGLE_BUTTON;
    }

}
