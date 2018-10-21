using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoCodeViewer : MonoBehaviour {

    [SerializeField]
    private TextMesh[] codeLines;

    void Awake()
    {
        List<string> pseudoCode = new List<string>(); //= GetComponent<Algorithm>().GetPseudoCode();
        if (pseudoCode != null)
            PseudoCodeSetup(pseudoCode);
    }

    private void PseudoCodeSetup(List<string> pseudoCode)
    {
        codeLines = new TextMesh[pseudoCode.Count];
        for (int x = 0; x < pseudoCode.Count; x++)
        {
            GameObject codeLine = new GameObject("Line" + x);
            codeLine.AddComponent<TextMesh>();
            Instantiate(codeLine, gameObject.transform.position, Quaternion.identity);
            codeLines[x].text = pseudoCode[x];
        }
    }


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

    public void CLEAN_HIGHTLIGHT(int start, int end)
    {
        for (int x=start; x <= end; x++)
        {
            ChangeColorOfText(x, Util.BLACKBOARD_TEXT_COLOR);
        }
    }

}
