using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnOffButton : MonoBehaviour {

    [SerializeField]
    private string buttonID, sectionID;
    private bool state;

    private TextMeshPro buttonText;


    [SerializeField]
    private Material onMaterial, offMaterial, nonMaterial;

    [SerializeField]
    private bool isOnOffButton;

    [SerializeField]
    private List<OnOffButton> sectionButtons;

    private void Awake()
    {
        state = false;
        buttonText = GetComponentInChildren<TextMeshPro>();
        
        // litt lat
        if (sectionButtons.Count > 0)
        {
            for (int i=0; i < sectionButtons.Count; i++)
            {
                sectionButtons[i].AddSectionButton(this);
                sectionButtons[i].SectionID = sectionID;
                for (int j=0; j < sectionButtons.Count; j++)
                {
                    sectionButtons[i].AddSectionButton(sectionButtons[j]);
                }
            }
        }
    }

    private void Start()
    {
        
    }

    public string ButtonID
    {
        get { return buttonID;}
    }

    public string SectionID
    {
        get { return sectionID; }
        set { sectionID = value; }
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
        state = true;
        GetComponentInChildren<Renderer>().material = onMaterial;

        foreach (OnOffButton button in sectionButtons)
        {
            if (button != this)
                button.DeactivateButton();
        }
    }

    public void ToggleState()
    {
        state = !state;
        if (state)
        {
            buttonText.text = "ON";
            GetComponentInChildren<Renderer>().material = onMaterial;
        }
        else
        {
            buttonText.text = "OFF";
            GetComponentInChildren<Renderer>().material = offMaterial;
        }
    }

    public void DeactivateButton()
    {
        state = false;
        if (isOnOffButton)
            GetComponentInChildren<Renderer>().material = offMaterial;
        else
            GetComponentInChildren<Renderer>().material = nonMaterial;
    }

    public void AddSectionButton(OnOffButton button)
    {
        if (!sectionButtons.Contains(button))
            sectionButtons.Add(button);
    }


    

}
