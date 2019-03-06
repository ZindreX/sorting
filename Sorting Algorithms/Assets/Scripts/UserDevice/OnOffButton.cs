using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnOffButton : MonoBehaviour {

    // Messy class, should be improved --> split into on/off (toggle), switch etc.

    [SerializeField]
    private string buttonID;
    private bool state;

    [SerializeField]
    private string onText, offText;

    private TextMeshPro buttonText;

    [SerializeField]
    private Material onMaterial, offMaterial, nonMaterial;

    [SerializeField]
    private bool isOnOffButton, staticAppearance;

    [SerializeField]
    private GameObject section;

    private OnOffButton[] sectionButtons;

    [SerializeField]
    private GameObject subSectionButtons;

    private void Awake()
    {
        state = false;
        buttonText = GetComponentInChildren<TextMeshPro>();

        //if (!staticAppearance)
        //    buttonText.text = buttonID;

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

    public bool HasSubSection()
    {
        return subSectionButtons != null;
    }

    public void HideSubsection()
    {
        subSectionButtons.SetActive(false);
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

        if (!staticAppearance)
            GetComponentInChildren<Renderer>().material = onMaterial;

        // Display sub buttons
        if (subSectionButtons != null)
            subSectionButtons.SetActive(state);
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

        if (!staticAppearance)
        {
            if (isOnOffButton)
                GetComponentInChildren<Renderer>().material = offMaterial;
            else
                GetComponentInChildren<Renderer>().material = nonMaterial;
        }

        if (subSectionButtons != null)
            subSectionButtons.SetActive(state);
    }


    

}
