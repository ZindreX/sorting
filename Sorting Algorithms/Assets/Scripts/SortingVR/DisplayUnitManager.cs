﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayUnitManager : MonoBehaviour {

    [SerializeField]
    private PseudoCodeViewer centerBlackboard;

    [SerializeField]
    private Blackboard rightBlackboard;


    [SerializeField]
    private TextMeshPro leftInfoBoardText, sortingTableText, settingsMenuText;

    private SortAlgorithm algorithm;

    public void InitDisplayUnitManager(SortAlgorithm algorithm, bool includeLineNr, bool inDetailStep)
    {
        centerBlackboard.InitPseudoCodeViewer(algorithm, includeLineNr, inDetailStep);
        leftInfoBoardText.text = "";
        sortingTableText.text = "";
        settingsMenuText.text = "";
    }

    public SortAlgorithm Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; }
    }

    // Algorithm will control this blackboard
    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return centerBlackboard; }
    }

    public Blackboard BlackBoard
    {
        get { return rightBlackboard; }
    }

    public void ResetDisplays()
    {
        leftInfoBoardText.text = "";
        centerBlackboard.EmptyContent();
        rightBlackboard.EmptyContent();
    }

    public void DestroyDisplaysContent()
    {
        centerBlackboard.DestroyPseudoCode();
    }

    public void SetTextWithIndex(string display, string text, int index)
    {
        switch (display)
        {
            case UtilSort.LEFT_BLACKBOARD: leftInfoBoardText.text = text; break;
            case UtilSort.RIGHT_BLACKBOARD: rightBlackboard.ChangeText(index, text); break;
            case UtilSort.SORT_TABLE_TEXT: sortingTableText.text = text; break;
            case UtilSort.SETTINGS_MENU_TEXT: break;
        }
    }

    public void SetText(string display, string text)
    {
        SetTextWithIndex(display, text, Util.NO_INDEX_VALUE);
    }


}
