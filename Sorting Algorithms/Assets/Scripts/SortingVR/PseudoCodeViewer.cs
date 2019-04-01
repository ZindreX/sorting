using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PseudoCodeViewer : MonoBehaviour, IDisplay {

    [SerializeField]
    private Transform pseudocodeLinesObj;
    private Vector3 objectStartPos, containerStartPos;

    private TextMeshPro[] codeLines;

    private WaitForSeconds demoStepDuration;

    private TeachingAlgorithm algorithm;

    private void Awake()
    {
        objectStartPos = transform.position;
        containerStartPos = pseudocodeLinesObj.transform.position;
    }

    public void InitPseudoCodeViewer(TeachingAlgorithm algorithm)
    {
        this.algorithm = algorithm;
        algorithm.PseudoCodeViewer = this;

        demoStepDuration = algorithm.DemoStepDuration;
    }

    // Initializes the pseudocode (Instantiating each line of code and making them fit among each other/within the blackboard)
    public void PseudoCodeSetup()
    {
        int numberOfLines = algorithm.FinalInstructionCodeLine() + 1;
        codeLines = new TextMeshPro[numberOfLines];

        float spaceBetweenLines = algorithm.LineSpacing;
        float fontSize = algorithm.FontSize;
        Vector2 rtDelta = algorithm.LineRTDelta;

        for (int x = 0; x < numberOfLines; x++)
        {
            // Create gameobject and add it to this gameobject
            GameObject codeLine = new GameObject("Line" + x);
            codeLine.transform.parent = pseudocodeLinesObj.transform;

            // Change transformation position and scale
            Vector3 pos = pseudocodeLinesObj.position;
            codeLine.transform.position = NextPseudoCodeLine(x, spaceBetweenLines); //new Vector3(pos.x, pos.y - (x * spaceBetweenLines), pos.z);
            //codeLine.transform.eulerAngles = new Vector3(startPosAndMat.transform.eulerAngles.x, startPosAndMat.transform.eulerAngles.y, startPosAndMat.transform.eulerAngles.z);
            //codeLine.transform.localScale = startPosAndMat.transform.localScale;

            // Change material and font
            codeLine.AddComponent<TextMeshPro>();
            TextMeshPro codelinePro = codeLine.GetComponent<TextMeshPro>();
            codelinePro.fontSize = fontSize; // startPosAndMat.fontSize;
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

    public void ChangeSizeOfPseudocode(Vector3 playerPos)
    {
        if (codeLines != null)
        {

            float lengthFromPseudocode = transform.position.z - playerPos.z;
            float lengthFromCenter = transform.position.x - playerPos.x;

            float xOffset = playerPos.x;

            float fontSize = 0f;
            float spaceBetweenLines = algorithm.LineSpacing;

            Debug.Log("Changing size of pseudocode::: Length = " + lengthFromPseudocode);
            if (lengthFromPseudocode < 5f)
            {
                Debug.Log("length < 5");
                fontSize = 1.5f;
                spaceBetweenLines /= 4.5f;

                if (xOffset < 0f)
                    pseudocodeLinesObj.position = new Vector3(xOffset + 4f, containerStartPos.y - 3f, containerStartPos.z);
                else
                    pseudocodeLinesObj.position = new Vector3(xOffset + 4f, containerStartPos.y - 4f, containerStartPos.z);
            }
            else if (lengthFromPseudocode < 10f)
            {
                Debug.Log("length < 10");
                fontSize = 3f;
                pseudocodeLinesObj.position = new Vector3(xOffset + 2f, containerStartPos.y - 2f, containerStartPos.z);
                spaceBetweenLines /= 3f;
            }
            else if (lengthFromPseudocode < 15f)
            {
                Debug.Log("length < 15");
                fontSize = 4.5f;

                if (xOffset < -4f)
                    xOffset += 3f;
                else if (xOffset > 2f)
                    xOffset -= 2f;

                pseudocodeLinesObj.position = new Vector3(xOffset, containerStartPos.y + 2f, containerStartPos.z);
                spaceBetweenLines /= 1.5f;
            }
            else
            {
                Debug.Log("length >= 15");
                fontSize = 6f;
                pseudocodeLinesObj.position = new Vector3(0f, containerStartPos.y + 4f, containerStartPos.z);
            }

            // Update pseudocode lines
            for (int i=0; i < codeLines.Length; i++)
            {
                codeLines[i].fontSize = fontSize;
                codeLines[i].transform.position = NextPseudoCodeLine(i, spaceBetweenLines);
            }

            //float aboveFloorDiff = codeLines[codeLines.Length - 1].transform.position.y;
            //if (aboveFloorDiff < 0f)
            //{
            //    pseudocodeLinesObj.transform.position += new Vector3(0f, aboveFloorDiff, 0f);
            //}
        }
    }


    private Vector3 NextPseudoCodeLine(int i, float spaceBetweenLines)
    {
        return new Vector3(pseudocodeLinesObj.transform.position.x, pseudocodeLinesObj.transform.position.y - (i * spaceBetweenLines), pseudocodeLinesObj.transform.position.z);
    }

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

        // Reset positions
        transform.position = objectStartPos;
        pseudocodeLinesObj.position = containerStartPos;
    }
}
