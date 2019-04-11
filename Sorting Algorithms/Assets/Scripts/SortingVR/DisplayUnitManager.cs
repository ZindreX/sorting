using System.Collections;
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


    public void InitDisplayUnitManager()
    {
        leftInfoBoardText.text = "";
        sortingTableText.text = "";
        settingsMenuText.text = "";
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
        sortingTableText.text = "";
        settingsMenuText.text = "";
        centerBlackboard.EmptyContent();
        rightBlackboard.EmptyContent();
    }

    public void DestroyDisplaysContent()
    {
        rightBlackboard.DestroyContent();
        centerBlackboard.DestroyContent();
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
