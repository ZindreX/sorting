using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR.InteractionSystem;

//[RequireComponent(typeof(Image))]
//[RequireComponent(typeof(Button))]
//[RequireComponent(typeof(Interactable))]
//[RequireComponent(typeof(UIElement))]
public abstract class SettingsMenuItem : MonoBehaviour {

    [SerializeField]
    protected string itemID;

    [SerializeField]
    protected string interactionDescription;

    protected Section section;


    public string ItemID
    {
        get { return itemID; }
    }

    public Section Section
    {
        get { return section; }
        set { section = value; }
    }

    public string InteractionDescription
    {
        get { return interactionDescription; }
    }



}
