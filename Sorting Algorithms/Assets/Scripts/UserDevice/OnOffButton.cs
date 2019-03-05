using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnOffButton : MonoBehaviour {

    // On/Off/any... button

    [SerializeField]
    private string buttonID;
    private bool state;

    [SerializeField]
    private string onText, offText;

    private TextMeshPro buttonText;

    [SerializeField]
    private Material onMaterial, offMaterial, nonMaterial;

    [SerializeField]
    private bool isOnOffButton, staticColor;

    [SerializeField]
    private GameObject section;

    [SerializeField]
    private OnOffButton[] sectionButtons;

    private void Awake()
    {
        state = false;
        buttonText = GetComponentInChildren<TextMeshPro>();

        if (section != null)
            sectionButtons = section.GetComponentsInChildren<OnOffButton>();
    }

    public string ButtonID
    {
        get { return buttonID;}
    }

    public bool State
    {
        get { return state; }
    }

    public bool IsOnOffButton
    {
        get { return isOnOffButton; }
    }

    public void ChangeState()
    {
        // Deactivate other button(s) in same section
        if (sectionButtons != null)
        {
            foreach (OnOffButton button in sectionButtons)
            {
                if (button.State)
                    button.DeactivateButton();
            }
        }

        // Activate this button
        state = true;

        if (!staticColor)
            GetComponentInChildren<Renderer>().material = onMaterial;
    }

    public void ToggleState()
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

    public void DeactivateButton()
    {
        state = false;

        if (!staticColor)
        {
            if (isOnOffButton)
                GetComponentInChildren<Renderer>().material = offMaterial;
            else
                GetComponentInChildren<Renderer>().material = nonMaterial;
        }
    }


    

}
