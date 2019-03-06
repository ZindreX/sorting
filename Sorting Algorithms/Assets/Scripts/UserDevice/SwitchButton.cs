using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(UIElement))]
public class SwitchButton : MonoBehaviour {

    [SerializeField]
    private GameObject section;

    [SerializeField]
    private TextMeshPro buttonText;

    [SerializeField]
    private string[] states;

    [SerializeField]
    private GameObject[] subMenus;

    private string buttonID;
    private int state;

    private void Awake()
    {
        buttonID = section.GetComponentInChildren<TextMeshPro>().text;
        state = -1;
        Toggle();
    }

    public void Toggle()
    {
        state++;
        if (state >= states.Length)
            state = 0;

        buttonText.text = states[state];
    }


}
