﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PseudoCodeViewer))]
public class InsertionSort : Algorithm {

    /* --------------------------------------------------- Insertion Sort --------------------------------------------------- 
     * 1) Starts from 2nd and checks if it is less than the first
     * 2) Moves if smaller
     * *) Repeat, but now check all slots to the left of pivot
     */

    [SerializeField]
    private GameObject pivotHolderPrefab;
    private GameObject pivotHolderClone; // TODO: change to Holder
    private InsertionSortHolder pivotHolder;
    private Vector3 tutorialHeight1 = new Vector3(0f, 1f, 0f), tutorialHeight2;
    private Vector3 userTestHeight = new Vector3(0f, 1.5f, 0f);

    private PseudoCodeViewer pseudoCodeViewer;

    private void Awake()
    {
        tutorialHeight2 = tutorialHeight1 + new Vector3(0f, 0.2f, 0f);
        pseudoCodeViewer = pseudoCodeViewerObj.GetComponent(typeof(PseudoCodeViewer)) as PseudoCodeViewer;
    }

    public override string GetAlgorithmName()
    {
        return Util.INSERTION_SORT;
    }

    public HolderBase PivotHolder
    {
        get { return pivotHolder; }
    }

    protected string PseudoCode(int lineNr, int i, int j, int numberOfElements)
    {
        switch (lineNr)
        {
            case 0: return "InsertionSort( List<int> list )";
            case 1: return "i = 1";
            case 2: return "while ( " + i + " < " + numberOfElements + " )";
            case 3: return "    " + j + " = " + i  + " - 1";
            case 4: return "    while ( " + j + " > 0 and " + pivotValue + " < " + compareValue + " )";
            case 5: return "        swap " + pivotValue + " and " + compareValue;
            case 6: return "        " + j + " = " + (j + 1) + " - 1";
            case 7: return "    end while";
            case 8: return "    " + i + " = " + (i - 1) + " + 1";
            case 9: return "end while";
            default: return "X";
        }
    }

    protected IEnumerator HighlightText(int lineNr, string text)
    {
        pseudoCodeViewer.SetCodeLine(lineNr, text, Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.ChangeColorOfText(lineNr, Util.BLACKBOARD_COLOR);
    }

    public override void ResetSetup()
    {
        Destroy(pivotHolder);
    }

    public void CreatePivotHolder()
    {
        // Instantiate
        Vector3 pos;
        if (GetComponent<AlgorithmManagerBase>().IsTutorial)
            pos = GetComponent<AlgorithmManagerBase>().HolderPositions[1] + tutorialHeight1;
        else
            pos = GetComponent<AlgorithmManagerBase>().HolderPositions[1] + userTestHeight;

        pivotHolderClone = Instantiate(pivotHolderPrefab, pos, Quaternion.identity);
        pivotHolder = pivotHolderClone.GetComponent<InsertionSortHolder>();
        // Mark as pivotholder
        pivotHolder.IsPivotHolder = true;
        // Set gameobject parent
        pivotHolder.Parent = gameObject;
        // Make the pivot holder position visible
        PivotHolderVisible(true);
    }

    #region Insertion Sort: Standard (No visuals)
    public static GameObject[] InsertionSortStandard(GameObject[] list)
    {
        int i = 1;
        while (i < list.Length)
        {
            GameObject pivot = list[i];
            int j = i - 1;
            while (j >= 0 && pivot.GetComponent<SortingElementBase>().Value < list[j].GetComponent<SortingElementBase>().Value)
            {
                list[j + 1] = list[j];
                j -= 1;
            }
            list[j + 1] = pivot;
            i += 1;
        }
        return list;
    }
    #endregion

    #region Insertion Sort: Standard 2 (No visuals)
    public static List<SortingElementBase> InsertionSortStandard2(List<SortingElementBase> list)
    {
        int i = 1;
        while (i < list.Count)
        {
            SortingElementBase pivot = list[i];
            int j = i - 1;
            while (j >= 0 && pivot.Value < list[j].Value)
            {
                list[j + 1] = list[j];
                j -= 1;
            }
            list[j + 1] = pivot;
            i += 1;
        }
        return list;
    }
    #endregion

    #region Insertion Sort: All Moves Tutorial (Visuals)
    public override IEnumerator Tutorial(GameObject[] list)
    {
        // Create pivot holder
        CreatePivotHolder();
        Vector3 temp = new Vector3();

        int i = 1, listLength = list.Length;
        // Display pseudocode (set i)
        StartCoroutine(HighlightText(1, PseudoCode(1, i, Util.NO_VALUE, listLength)));
        yield return new WaitForSeconds(seconds);

        while (i < listLength)
        {
            // Display pseudocode (1st while)
            StartCoroutine(HighlightText(2, PseudoCode(2, i, Util.NO_VALUE, listLength)));
            yield return new WaitForSeconds(seconds);

            // Get pivot
            GameObject pivotObj = list[i];
            InsertionSortElement pivot = list[i].GetComponent<InsertionSortElement>();
            pivot.IsPivot = true;

            // Get pivot's initial position
            temp = pivot.transform.position;

            // Place pivot holder above the pivot element
            pivotHolder.transform.position = temp + tutorialHeight1;

            // Place the pivot on top of the pivot holder
            pivot.transform.position = temp + tutorialHeight2;

            // Wait to show the pivot
            yield return new WaitForSeconds(seconds);

            // Get index of first element to the left of the pivot and compare
            int j = i - 1;

            // Display pseudocode (set j)
            StartCoroutine(HighlightText(3, PseudoCode(3, i, j, listLength)));
            yield return new WaitForSeconds(seconds);

            // Start comparing until find the correct position is found
            // Set first values here to display on blackboard
            pivotValue = pivot.Value;
            compareValue = list[j].GetComponent<InsertionSortElement>().Value;

            // Display pseudocode (2nd while)
            StartCoroutine(HighlightText(4, PseudoCode(4, i, j, listLength)));
            yield return new WaitForSeconds(seconds);

            while (pivotValue < compareValue)
            {
                // Pivot is smaller, start moving compare element
                // Prepare the element to compare with
                GameObject compareObj = list[j];
                InsertionSortElement compare = compareObj.GetComponent<InsertionSortElement>();

                // Compare element's position
                Vector3 temp2 = compare.transform.position;

                // Moving other element one index to the right
                compare.transform.position = temp;
                // Updating list
                list[j + 1] = compareObj;

                // Display pseudocode (swap)
                StartCoroutine(HighlightText(5, PseudoCode(5, i, j, listLength)));
                yield return new WaitForSeconds(seconds);

                // Preparing for next step
                temp = temp2;
                j -= 1;

                // Display pseudocode (decrement j)
                StartCoroutine(HighlightText(6, PseudoCode(6, i, j, listLength)));
                yield return new WaitForSeconds(seconds);

                // Check if there are more elements to compare the pivot with
                if (j >= 0)
                {
                    // More elements to evaluate
                    compareValue = list[j].GetComponent<InsertionSortElement>().Value;

                    // Display pseudocode (2nd while new compare value)
                    StartCoroutine(HighlightText(4, PseudoCode(4, i, j, listLength)));
                    yield return new WaitForSeconds(seconds);
                }
                else
                {
                    // Make sure that the last compare element is marked as sorted
                    compare.IsSorted = true; //list[j + 1].GetComponent<SortingElement>().IsSorted = true;

                    // Display pseudocode (end 2nd while)
                    StartCoroutine(HighlightText(7, PseudoCode(7, i, j, listLength)));
                    yield return new WaitForSeconds(seconds);
                    break;
                }
                // Move pivot out and place it ontop of pivot holder (above holder it check whether it's put the element)
                pivotHolder.transform.position = temp + tutorialHeight1;
                pivot.transform.position = temp + tutorialHeight2;

                // Wait to show the pivot being moved
                yield return new WaitForSeconds(seconds);
            }
            // Make sure the 1st element is marked sorted in the first round
            if (i == 1 && pivotValue >= compareValue)
            {
                InsertionSortElement firstSortingElement = list[0].GetComponent<InsertionSortElement>();
                firstSortingElement.IsSorted = true;
                firstSortingElement.transform.position += tutorialHeight1;
            }

            // Finish off the pivots work
            pivot.IsSorted = true;
            pivot.IsPivot = false;
            pivot.transform.position = temp;
            // Return pivot object to list, and increment 'i'
            list[j + 1] = pivotObj;
            i += 1;

            // Display pseudocode (increment i)
            StartCoroutine(HighlightText(8, PseudoCode(8, i, j, listLength)));
        }
        // Display pseudocode (end 1st while)
        StartCoroutine(HighlightText(9, PseudoCode(9, i, Util.NO_VALUE, listLength)));
        yield return new WaitForSeconds(seconds);

        // Mark the last element sorted
        list[list.Length - 1].GetComponent<InsertionSortElement>().IsSorted = true;

        // Finished off; remove pivot holder
        PivotHolderVisible(false);
        IsSortingComplete = true;
    }
    #endregion

    #region Insertion Sort: All Moves (Debug log: OK!)
    // ElementState[] deep copy of our list we want to sort
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        // Create pivot holder
        CreatePivotHolder();

        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();

        InsertionSortInstruction compareElement;

        int i = 1, instructionNr = 0;
        while (i < sortingElements.Length)
        {
            int temp1 = ((InsertionSortInstruction)sortingElements[i]).HolderID; // pivot: temp1 -> temp2*
            InsertionSortInstruction pivot = new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[i]).SortingElementID, temp1, pivotHolder.HolderID, Util.PIVOT_START_INST, ((InsertionSortInstruction)sortingElements[i]).Value, true, false, false);
            sortingElements[i] = pivot;

            // Add this move (Pivot moved in pivot position)
            instructions.Add(instructionNr++, pivot);

            int j = i - 1;
            while (true)
            {
                // Choose a new compare element
                compareElement = new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[j]).SortingElementID, j, Util.NO_DESTINATION, Util.COMPARE_START_INST, ((InsertionSortInstruction)sortingElements[j]).Value, false, true, sortingElements[j].IsSorted);
                sortingElements[j] = compareElement;
                //
                instructions.Add(instructionNr++, compareElement);

                // Pivot larger than compare element, place compare element
                if (pivot.Value >= compareElement.Value)
                {
                    instructions.Add(instructionNr++, new InsertionSortInstruction(compareElement.SortingElementID, j, Util.NO_DESTINATION, Util.COMPARE_END_INST, compareElement.Value, false, false, true));
                    break;
                }

                // Pivot is less than compare element, switch their spots
                int temp2 = compareElement.HolderID;
                sortingElements[j + 1] = compareElement;
                sortingElements[j] = pivot;
                j -= 1;

                // Add this move (compare element switched to pivot/next position)
                instructions.Add(instructionNr++, new InsertionSortInstruction(compareElement.SortingElementID, compareElement.HolderID, temp1, Util.SWITCH_INST, compareElement.Value, false, false, true));

                // temp2 is open spot, temp1 will be given to next compare element or place pivot there
                temp1 = temp2;

                if (j < 0)
                    break;
            }

            sortingElements[j + 1] = pivot;
            i += 1;

            // Add this move (pivot sorted)
            instructions.Add(instructionNr++, new InsertionSortInstruction(pivot.SortingElementID, pivotHolder.HolderID, temp1, Util.PIVOT_END_INST, pivot.Value, false, false, true));
        }
        return instructions;
    }
    #endregion


    // ----------------------------- Help functions -----------------------------

    private void PivotHolderVisible(bool enable)
    {
        pivotHolderClone.GetComponentInChildren<MeshRenderer>().enabled = enable;
    }
}
