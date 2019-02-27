using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoCodeViewer : MonoBehaviour, IDisplay {

    //[SerializeField]
    private TextMesh[] codeLines;

    [SerializeField]
    private TextMesh startPosAndMat;

    private float seconds, spaceBetweenLines;

    private TeachingAlgorithm algorithm;

    public void InitPseudoCodeViewer(TeachingAlgorithm algorithm, float spaceBetweenLines)
    {
        this.algorithm = algorithm;
        seconds = algorithm.Seconds;
        this.spaceBetweenLines = spaceBetweenLines;
    }

    public void PseudoCodeSetup()
    {
        int numberOfLines = algorithm.FinalInstructionCodeLine() + 1;
        codeLines = new TextMesh[numberOfLines];

        for (int x = 0; x < numberOfLines; x++)
        {
            // Create gameobject and add it to this gameobject
            GameObject codeLine = new GameObject("Line" + x);
            codeLine.transform.parent = gameObject.transform;

            // Change transformation position and scale //new Vector3(-4.23f, 3.36f - (x * SPACE_BETWEEN_CODE_LINES), -0.1f);
            Vector3 pos = startPosAndMat.transform.position;
            codeLine.transform.position = new Vector3(pos.x, pos.y - (x * spaceBetweenLines), pos.z);
            codeLine.transform.eulerAngles = new Vector3(startPosAndMat.transform.eulerAngles.x, startPosAndMat.transform.eulerAngles.y, startPosAndMat.transform.eulerAngles.z);
            codeLine.transform.localScale = startPosAndMat.transform.localScale; // new Vector3(0.05f, 0.05f, 0.05f);

            // Change material and font
            codeLine.AddComponent<MeshRenderer>();
            codeLine.GetComponent<MeshRenderer>().material = startPosAndMat.GetComponent<MeshRenderer>().material; //material;
            codeLine.AddComponent<TextMesh>();
            codeLine.GetComponent<TextMesh>().font = startPosAndMat.font; //font;

            // Get line of code from algorithm
            if (algorithm.IncludeLineNr)
                codeLine.GetComponent<TextMesh>().text = algorithm.CollectLine(x);
            else
                codeLine.GetComponent<TextMesh>().text = algorithm.CollectLine(x).Split(Util.PSEUDO_SPLIT_LINE_ID)[1]; // Insertionsort / bucketsort: update pseudocode (as in bubble-/graph)

            codeLines[x] = codeLine.GetComponent<TextMesh>();
        }

        // Move/extend blackboard up if the pseudocode goes below the floor
        float codeBelowFloor = codeLines[numberOfLines - 1].transform.position.y;
        if (codeBelowFloor < 0.342f)
        {
            transform.position += new Vector3(0f, 0.5f - codeBelowFloor, 0f);
        }
    }

    public TextMesh CodeLine(int index)
    {
        return codeLines[index];
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


    private void PseudoCodeFirstFinal(string instruction, Color color)
    {
        switch (instruction)
        {
            case UtilSort.FIRST_INSTRUCTION:
                CLEAN_HIGHTLIGHT(algorithm.FirstInstructionCodeLine() + 1, algorithm.FinalInstructionCodeLine());
                ChangeColorOfText(algorithm.FirstInstructionCodeLine(), color);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                CLEAN_HIGHTLIGHT(0, algorithm.FinalInstructionCodeLine() - 1);
                ChangeColorOfText(algorithm.FinalInstructionCodeLine(), color);
                break;
        }
    }

    public void EmptyContent()
    {
        foreach (TextMesh line in codeLines)
        {
            line.text = "";
        }
    }

    public void RemoveHightlight()
    {
        foreach (TextMesh line in codeLines)
        {
            line.color = Util.BLACKBOARD_TEXT_COLOR;
        }
    }

    public void DestroyPseudoCode()
    {
        for (int x=0; x < codeLines.Length; x++)
        {
            Destroy(codeLines[x]);
        }
        PseudoCodeSetup();
    }
}
