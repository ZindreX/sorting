using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour, IDisplay {

    /* -------------------------------------------- Blackboard --------------------------------------------
     * 
     * 
    */

    [SerializeField]
    private TextMesh[] texts;
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
        foreach (TextMesh text in texts)
        {
            text.text = "";
        }
    }

    public void RemoveHightlight()
    {
        foreach (TextMesh line in texts)
        {
            line.color = Util.BLACKBOARD_TEXT_COLOR;
        }
    }
}
