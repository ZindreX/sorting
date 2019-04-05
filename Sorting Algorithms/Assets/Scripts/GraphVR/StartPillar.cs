using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartPillar : MonoBehaviour {

    [SerializeField]
    private TextMeshPro displayText;

    [SerializeField]
    private ToggleButton startButton;

    private string teachingMode;

    private Pointer pointer;

    private void Awake()
    {
        Section startPillarSection = GetComponentInChildren<Section>();
        startPillarSection.SectionManager = FindObjectOfType<SettingsBase>();
        pointer = FindObjectOfType<Pointer>();
    }

    public void InitStartPillar(string teachingMode, bool selectNodes, bool endNode)
    {
        this.teachingMode = teachingMode;

        if (selectNodes)
        {
            if (endNode)
                SetDisplayText("Select start and end node");
            else
                SetDisplayText("Select start node");
                
            SetButtonActive(false);
        }
        else
        {
            SetDisplayText("Click start to play");
            SetButtonActive(true);
        }
    }

    public void SetDisplayText(string text)
    {
        displayText.text = text;
    }

    public void SetButtonActive(bool active)
    {
        startButton.gameObject.SetActive(active);
    }

    public void ButtonClicked()
    {
        if (startButton.State)
        {
            SetDisplayText("Click stop to return to settings menu.");
            pointer.CurrentTask = teachingMode;
        }
        else
            SetDisplayText("Click start to play");
    }

}
