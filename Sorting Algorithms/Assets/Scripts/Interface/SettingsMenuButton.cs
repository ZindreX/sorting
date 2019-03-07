using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsMenuButton : SettingsMenuItem {

    protected TextMeshPro buttonText;

    private void Awake()
    {
        buttonText.text = itemID;
    }

    protected void ChangeAppearance(string text, Material material)
    {
        buttonText.text = text;
        GetComponentInChildren<Renderer>().material = material;
    }


}
