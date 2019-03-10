﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(UIElement))]
public abstract class SettingsMenuItem : MonoBehaviour {

    [SerializeField]
    protected string itemID;

    [SerializeField]
    protected string interactionDescription;

    protected bool isSectionMember;

    protected Section section;

    public string ItemID
    {
        get { return itemID; }
    }

    // Section is set by the section itself
    public Section Section
    {
        get { return section; }
        set { section = value; }
    }

    public bool IsSectionMember
    {
        get { return isSectionMember; }
        set { isSectionMember = value; }
    }

    public string InteractionDescription
    {
        get { return interactionDescription; }
    }

    public void OnClickDo()
    {
        // Perform item role first
        switch (ItemRole())
        {
            case Util.ONE_ACTIVE_BUTTON: ((OneActiveButton)this).ActivateItem(); break;
            case Util.STATIC_BUTTON: ((StaticButton)this).DoSomething(); break;
            case Util.TOGGLE_BUTTON: ((ToggleButton)this).Toggle(); break;
            case Util.MULTI_STATE_BUTTON: ((MultiStateButton)this).ToggleNextState(); break;
        }
        Debug.Log("Section: " + section.SectionID + ", item: " + itemID + ", description: " + interactionDescription);
        // Report click to section
        section.ReportItemClicked(section.SectionID, itemID, interactionDescription);
    }


    public abstract string ItemRole();


}
