using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackboard : MonoBehaviour, IDisplay {

    /* -------------------------------------------- Blackboard --------------------------------------------
     * 
     * 
    */

    [SerializeField]
    private TextMeshPro[] texts;
    private int titleIndex = 0, textIndex = 1;

    public int TitleIndex
    {
        get { return titleIndex; }
    }

    public int TextIndex
    {
        get { return textIndex; }
    }

    public void ChangeText(int textLine, string text)
    {
        if (textLine >= 0 && textLine < texts.Length)
            texts[textLine].text = text;
        else
            Debug.Log("Cant acces that line");
    }


    public void EmptyContent()
    {
        foreach (TextMeshPro text in texts)
        {
            text.text = "";
        }
    }

    public void RemoveHightlight()
    {
        foreach (TextMeshPro line in texts)
        {
            line.color = UtilSort.BLACKBOARD_TEXT_COLOR;
        }
    }

    public void DestroyContent()
    {
        // Nothing to destroy
    }
}
