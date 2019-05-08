using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayUnitManager : MonoBehaviour {

    [SerializeField]
    private PseudoCodeViewer centerBlackboard;

    [SerializeField]
    private Blackboard leftBlackboard, rightBlackboard;

    [SerializeField]
    private TextMeshPro sortingTableText, settingsMenuText;


    public void InitDisplayUnitManager()
    {
        sortingTableText.text = "";
        settingsMenuText.text = "";
    }

    public Blackboard LeftBlackboard
    {
        get { return leftBlackboard; }
    }

    // Algorithm will control this blackboard
    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return centerBlackboard; }
    }

    public Blackboard RightBlackboard
    {
        get { return rightBlackboard; }
    }

    public void ResetDisplays()
    {
        sortingTableText.text = "";
        settingsMenuText.text = "";

        leftBlackboard.EmptyContent();
        centerBlackboard.EmptyContent();
        rightBlackboard.EmptyContent();
    }

    public void DestroyDisplaysContent()
    {
        centerBlackboard.DestroyContent();
    }

    public void SetTextWithIndex(string display, string text, int index)
    {
        switch (display)
        {
            case UtilSort.LEFT_BLACKBOARD: leftBlackboard.ChangeText(index, text); break;
            case UtilSort.RIGHT_BLACKBOARD: rightBlackboard.ChangeText(index, text); break;
            case UtilSort.SORT_TABLE_TEXT: sortingTableText.text = text; break;
            case UtilSort.SETTINGS_MENU_TEXT: break;
        }
    }

    public void SetText(string display, string text)
    {
        SetTextWithIndex(display, text, Util.NO_INDEX_VALUE);
    }

    public TextMeshPro SortingTableTextMeshPro
    {
        get { return sortingTableText; }
    }


}
