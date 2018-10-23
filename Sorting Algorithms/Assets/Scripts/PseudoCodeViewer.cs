﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoCodeViewer : MonoBehaviour {

    public static readonly float SPACE_BETWEEN_CODE_LINES = 0.7f;

    //[SerializeField]
    private TextMesh[] codeLines;

    [SerializeField]
    private TextMesh test;

    [SerializeField]
    private Font font;

    [SerializeField]
    private Material material;

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
        int numberOfLines = algorithm.FinalInstructionCodeLine() + 1;
        codeLines = new TextMesh[numberOfLines];

        for (int x = 0; x < numberOfLines; x++)
        {
            // Create gameobject and add it to this gameobject
            GameObject codeLine = new GameObject("Line" + x);
            codeLine.transform.parent = gameObject.transform;
            
            // Change transformation position and scale
            codeLine.transform.position = transform.position + new Vector3(-4.23f, 3.36f - (x * SPACE_BETWEEN_CODE_LINES), -0.1f);
            codeLine.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // Change material and font
            codeLine.AddComponent<MeshRenderer>();
            codeLine.GetComponent<MeshRenderer>().material = test.GetComponent<MeshRenderer>().material; //material;
            codeLine.AddComponent<TextMesh>();
            codeLine.GetComponent<TextMesh>().font = test.font; //font;

            // Get line of code from algorithm
            codeLine.GetComponent<TextMesh>().text = algorithm.CollectLine(x); // ***
            codeLines[x] = codeLine.GetComponent<TextMesh>();
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

    public IEnumerator HighlightText(int lineNr, string text)
    {
        SetCodeLine(lineNr, text, Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        ChangeColorOfText(lineNr, Util.BLACKBOARD_TEXT_COLOR);
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
}