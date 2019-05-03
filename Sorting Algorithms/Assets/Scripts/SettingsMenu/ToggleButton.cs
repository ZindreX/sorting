using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : SettingsMenuButton {

    [SerializeField]
    protected Material onMaterial, offMaterial;

    [SerializeField]
    protected string onText, offText;

    protected bool state;

    protected override void Awake()
    {
        base.Awake();

        state = false;
        ChangeAppearance(offText, offMaterial);
    }

    public bool State
    {
        get { return state; }
    }

    public void InitToggleButton(bool state)
    {
        if (!this.state && state)
            Toggle();
    }

    // Toggles between two states
    public virtual void Toggle()
    {
        state = !state;

        interactionDescription = IsOn(state);
        if (state)
            ChangeAppearance(onText, onMaterial);
        else
            ChangeAppearance(offText, offMaterial);
    }

    private string IsOn(bool state)
    {
        return state ? Util.ON : Util.OFF;
    }

    public override string ItemRole()
    {
        return Util.TOGGLE_BUTTON;
    }

}
