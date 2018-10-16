using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoCodeViewer : MonoBehaviour {

    [SerializeField]
    private TextMesh[] codeLines;

    public TextMesh CodeLine(int index)
    {
        return codeLines[index];
    }

    public void SetCodeLine(int index, string text, Color color)
    {
        if (ValidIndex(index))
        {
            codeLines[index].text = text;
            codeLines[index].color = color;
        }
    }

    public void ChangeColorOfText(int index, Color color)
    {
        if (ValidIndex(index))
            codeLines[index].color = color;
    }

    private bool ValidIndex(int index)
    {
        return index >= 0 && index < codeLines.Length;
    }

}
