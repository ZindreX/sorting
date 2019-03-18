using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsMenuButton : SettingsMenuItem {

    protected TextMeshPro buttonText;

    protected virtual void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshPro>();

        if (!this is StaticButton)
            buttonText.text = itemID;
    }

    protected void ChangeAppearance(Material material)
    {
        GetComponentInChildren<Renderer>().material = material;
    }

    protected void ChangeAppearance(string text, Material material)
    {
        buttonText.text = text;
        GetComponentInChildren<Renderer>().material = material;
    }
}
