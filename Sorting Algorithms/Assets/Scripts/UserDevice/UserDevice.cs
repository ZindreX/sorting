﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDevice : MonoBehaviour {

    /* ------------------------------- User device carried by player -------------------------------
     * > Can switch between tutorial and user test
     * > Can change rules all: number of elements, duplicates, best-/worst case
     *            > tutorial : speed
     *            > user test: (help enabled) ~> difficulty
     * > Can teleport between rooms (algorithms)
     * > Quit game
    */

    [SerializeField]
    private GameObject[] topButtons;

    [SerializeField]
    private GameObject[] buttonPositions1, buttonPositions2;

    [SerializeField]
    private GameObject positions1, positons2, positions3;

    private UserDeviceButtonBase[] alternativeButtons;

    // Algorithm settings
    private int numberOfElements = 8;
    private Dictionary<int, Dictionary<string, bool>> options;


    void Awake()
    {
        options = CreateOptions();
    }


    private void SetAlternativeButtons(string buttonMode)
    {
        switch (buttonMode)
        {
            case Util.TUTORIAL:

                break;
            case Util.USER_TEST: break;
            case Util.RULE_BUTTONS: break;
            case Util.QUIT: break;
        }
    }

    // Set up tutorial buttons










    private Dictionary<int, Dictionary<string, bool>> CreateOptions()
    {
        Dictionary<int, Dictionary<string, bool>> options = new Dictionary<int, Dictionary<string, bool>>();
        //options.Add(0, CreateValue(Util.TUTORIAL, true));
        options.Add(1, CreateValue(Util.HELP_ENABLED, false));
        options.Add(2, CreateValue(Util.DUPLICATES, true));
        //options.Add(3, CreateValue("Case", false)); // Best- / worst case
        options.Add(4, CreateValue(Util.BEST_CASE, false));
        //options.Add(5, CreateValue(Util.USER_TEST, false));
        options.Add(6, CreateValue("", false));
        options.Add(7, CreateValue(Util.WORST_CASE, false));
        return options;
    }

    private Dictionary<string, bool> CreateValue(string valueName, bool active)
    {
        Dictionary<string, bool> value = new Dictionary<string, bool>();
        value.Add(valueName, active);
        return value;
    }


    private void SetTextAlternativeButtons(string buttonSet)
    {
        switch (buttonSet)
        {
            case Util.NUMBER_BUTTONS:
                for (int x=0; x < alternativeButtons.Length; x++)
                {
                    alternativeButtons[x].GetComponentInChildren<TextMesh>().text = x.ToString();
                }
                break;
            case Util.RULE_BUTTONS:

                break;
            default: Debug.Log("No buttonSet called '" + buttonSet + "'."); break;
        }
    }

    private void SetColorAlternativeButtons(string buttonMode)
    {
        switch (buttonMode)
        {
            case Util.ON_OR_OFF:
                for (int x=0; x < options.Count; x++)
                {

                }

                break;
            case Util.PORTAL:
                for (int x=0; x < alternativeButtons.Length; x++)
                {
                    alternativeButtons[x].GetComponentInChildren<TextMesh>().text = Util.ConvertSceneBuildIndexToName(x);
                }
                break;
        }
    }


}