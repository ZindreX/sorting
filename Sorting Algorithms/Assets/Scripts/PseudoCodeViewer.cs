using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoCodeViewer : MonoBehaviour, IDisplay {

    //[SerializeField]
    private TextMesh[] codeLines;

    [SerializeField]
    private TextMesh startPosAndMat;

    private float seconds;

    private Algorithm algorithm;

    public void SetAlgorithm(Algorithm algorithm)
    {
        this.algorithm = algorithm;
        seconds = algorithm.Seconds;

        // If pseudo code added, put it to display
        PseudoCodeSetup();
    }

    private void PseudoCodeSetup()
    {
        // Dont draw pseudocode if difficulty is equal or higher than advanced
        if (algorithm.gameObject.GetComponent<AlgorithmManagerBase>().Difficulty >= Util.ADVANCED)
            return;

        int numberOfLines = algorithm.FinalInstructionCodeLine() + 1;
        codeLines = new TextMesh[numberOfLines];

        for (int x = 0; x < numberOfLines; x++)
        {
            // Create gameobject and add it to this gameobject
            GameObject codeLine = new GameObject("Line" + x);
            codeLine.transform.parent = gameObject.transform;

            // Change transformation position and scale //new Vector3(-4.23f, 3.36f - (x * SPACE_BETWEEN_CODE_LINES), -0.1f);
            Vector3 pos = startPosAndMat.transform.position;
            codeLine.transform.position = new Vector3(pos.x, pos.y - (x * Util.SPACE_BETWEEN_CODE_LINES), pos.z);
            codeLine.transform.eulerAngles = new Vector3(startPosAndMat.transform.eulerAngles.x, startPosAndMat.transform.eulerAngles.y, startPosAndMat.transform.eulerAngles.z);
            codeLine.transform.localScale = startPosAndMat.transform.localScale; // new Vector3(0.05f, 0.05f, 0.05f);

            // Change material and font
            codeLine.AddComponent<MeshRenderer>();
            codeLine.GetComponent<MeshRenderer>().material = startPosAndMat.GetComponent<MeshRenderer>().material; //material;
            codeLine.AddComponent<TextMesh>();
            codeLine.GetComponent<TextMesh>().font = startPosAndMat.font; //font;

            // Get line of code from algorithm
            codeLine.GetComponent<TextMesh>().text = algorithm.CollectLine(x); // ***
            codeLines[x] = codeLine.GetComponent<TextMesh>();
        }

        // Move/extend blackboard up if the pseudocode goes below the floor
        float codeBelowFloor = codeLines[numberOfLines - 1].transform.position.y;
        if (codeBelowFloor < 0.342f)
        {
            transform.position += new Vector3(0f, 0.5f -codeBelowFloor, 0f);
            Debug.Log("Working? move higher up? " + codeBelowFloor);
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


    private void PseudoCodeFirstFinal(string instruction, Color color)
    {
        switch (instruction)
        {
            case Util.FIRST_INSTRUCTION:
                CLEAN_HIGHTLIGHT(algorithm.FirstInstructionCodeLine() + 1, algorithm.FinalInstructionCodeLine());
                ChangeColorOfText(algorithm.FirstInstructionCodeLine(), color);
                break;

            case Util.FINAL_INSTRUCTION:
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

    public void ResetPseudoCode()
    {
        for (int x=0; x < codeLines.Length; x++)
        {
            Destroy(codeLines[x]);
        }
        PseudoCodeSetup();
    }
}
