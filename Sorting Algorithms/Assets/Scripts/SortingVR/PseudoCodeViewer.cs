using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PseudoCodeViewer : MonoBehaviour, IDisplay {

    //[SerializeField]
    private TextMeshPro[] codeLines;

    [SerializeField]
    private TextMeshPro startPosAndMat;

    private float spaceBetweenLines;
    private WaitForSeconds demoStepDuration;

    private TeachingAlgorithm algorithm;
    private Vector2 rtDelta;

    public void InitPseudoCodeViewer(TeachingAlgorithm algorithm)
    {
        this.algorithm = algorithm;
        algorithm.PseudoCodeViewer = this;

        demoStepDuration = algorithm.DemoStepDuration;
        spaceBetweenLines = algorithm.GetLineSpacing();
        rtDelta = algorithm.GetLineRTDelta();
    }

    // Initializes the pseudocode (Instantiating each line of code and making them fit among each other/within the blackboard)
    public void PseudoCodeSetup()
    {
        int numberOfLines = algorithm.FinalInstructionCodeLine() + 1;
        codeLines = new TextMeshPro[numberOfLines];

        for (int x = 0; x < numberOfLines; x++)
        {
            // Create gameobject and add it to this gameobject
            GameObject codeLine = new GameObject("Line" + x);
            codeLine.transform.parent = gameObject.transform;

            // Change transformation position and scale
            Vector3 pos = startPosAndMat.transform.position;
            codeLine.transform.position = new Vector3(pos.x, pos.y - (x * spaceBetweenLines), pos.z);
            codeLine.transform.eulerAngles = new Vector3(startPosAndMat.transform.eulerAngles.x, startPosAndMat.transform.eulerAngles.y, startPosAndMat.transform.eulerAngles.z);
            codeLine.transform.localScale = startPosAndMat.transform.localScale;

            // Change material and font
            codeLine.AddComponent<TextMeshPro>();
            TextMeshPro codelinePro = codeLine.GetComponent<TextMeshPro>();
            codelinePro.fontSize = startPosAndMat.fontSize;
            codelinePro.enableWordWrapping = false;
            
            // Rectangle shape
            RectTransform rt = codelinePro.rectTransform;
            rt.sizeDelta = rtDelta;
            rt.transform.position = new Vector3(rt.position.x + 1f, rt.position.y, rt.position.z);

            // Get line of code from algorithm
            if (algorithm.IncludeLineNr)
                codeLine.GetComponent<TextMeshPro>().text = algorithm.CollectLine(x);
            else
                codeLine.GetComponent<TextMeshPro>().text = algorithm.CollectLine(x).Split(Util.PSEUDO_SPLIT_LINE_ID)[1]; // Insertionsort / bucketsort: update pseudocode (as in bubble-/graph)

            codeLines[x] = codeLine.GetComponent<TextMeshPro>();
        }

        // Move/extend blackboard up if the pseudocode goes below the floor
        float codeBelowFloor = codeLines[numberOfLines - 1].transform.position.y;
        if (codeBelowFloor < 0.342f)
        {
            transform.position += new Vector3(0f, 0.5f - codeBelowFloor, 0f);
        }
    }

    //public TextMeshPro CodeLine(int index)
    //{
    //    return codeLines[index];
    //}

    public void SetCodeLine(int lineNr, string text, Color color)
    {
        // Check if lineNr exists
        if (ValidIndex(lineNr))
        {
            // Change text (with or without lineNr)
            if (algorithm.IncludeLineNr)
                codeLines[lineNr].text = lineNr + text;
            else
                codeLines[lineNr].text = text;

            // Change color
            codeLines[lineNr].color = color;
        }
    }

    public void SetCodeLine(string text, Color color)
    {
        string[] lineOfCodeSplit = text.Split(Util.PSEUDO_SPLIT_LINE_ID);
        int index = UtilGraph.ConvertCostToInt(lineOfCodeSplit[0]);

        if (ValidIndex(index))
        {
            if (algorithm.IncludeLineNr) // optimize?
                codeLines[index].text = text;
            else
                codeLines[index].text = lineOfCodeSplit[1];

            codeLines[index].color = color;
        }
    }


    // Used to remove highlight
    public void ChangeColorOfText(int index, Color color)
    {
        if (ValidIndex(index))
            codeLines[index].color = color;
    }

    private bool ValidIndex(int index)
    {
        if (codeLines != null)
            return index >= 0 && index < codeLines.Length;
        return false;
    }

    //public void CLEAN_HIGHTLIGHT(int start, int end)
    //{
    //    for (int x=start; x <= end; x++)
    //    {
    //        ChangeColorOfText(x, Util.BLACKBOARD_TEXT_COLOR);
    //    }
    //}


    //private void PseudoCodeFirstFinal(string instruction, Color color)
    //{
    //    switch (instruction)
    //    {
    //        case UtilSort.FIRST_INSTRUCTION:
    //            CLEAN_HIGHTLIGHT(algorithm.FirstInstructionCodeLine() + 1, algorithm.FinalInstructionCodeLine());
    //            ChangeColorOfText(algorithm.FirstInstructionCodeLine(), color);
    //            break;

    //        case UtilSort.FINAL_INSTRUCTION:
    //            CLEAN_HIGHTLIGHT(0, algorithm.FinalInstructionCodeLine() - 1);
    //            ChangeColorOfText(algorithm.FinalInstructionCodeLine(), color);
    //            break;
    //    }
    //}

    public void EmptyContent()
    {
        if (codeLines != null)
        {
            foreach (TextMeshPro line in codeLines)
            {
                line.text = "";
            }
        }
    }

    public void RemoveHightlight()
    {
        if (codeLines != null)
        {
            foreach (TextMeshPro line in codeLines)
            {
                line.color = Util.BLACKBOARD_TEXT_COLOR;
            }
        }
    }

    public void DestroyPseudoCode()
    {
        if (codeLines != null)
        {
            for (int x=0; x < codeLines.Length; x++)
            {
                if (codeLines[x] != null)
                    Destroy(codeLines[x].gameObject);
            }
        }
    }
}
