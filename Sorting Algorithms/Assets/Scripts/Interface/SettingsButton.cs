using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Image))]
//[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(UIElement))]
public class SettingsButton : MonoBehaviour {

    [SerializeField]
    protected string buttonID;

    protected TextMeshPro buttonText;

    private void Awake()
    {
        buttonText = GetComponent<TextMeshPro>();
        buttonText.text = buttonID;
    }

    public string ButtonID
    {
        get { return buttonID; }
    }




}
