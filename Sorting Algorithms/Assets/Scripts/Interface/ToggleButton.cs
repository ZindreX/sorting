using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : SettingsButton {

    [SerializeField]
    private Material onMaterial, offMaterial;

    [SerializeField]
    private string onText, offText;

    private bool state;

    public void Toggle()
    {
        state = !state;

        if (state)
        {
            buttonText.text = onText;
            GetComponentInChildren<Renderer>().material = onMaterial;
        }
        else
        {
            buttonText.text = offText;
            GetComponentInChildren<Renderer>().material = offMaterial;
        }
    }


}
