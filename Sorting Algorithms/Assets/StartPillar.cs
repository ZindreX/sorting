using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartPillar : MonoBehaviour {

    [SerializeField]
    private TextMeshPro displayText;

    [SerializeField]
    private ToggleButton startButton;


    public void InitStartPillar(bool selectNodes, bool endNode)
    {
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
            SetDisplayText("Click stop to return to settings menu.");
        else
            SetDisplayText("Click start to play");
    }

}
