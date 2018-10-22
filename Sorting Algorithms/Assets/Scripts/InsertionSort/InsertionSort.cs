using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InsertionSortManager))]
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

    private InsertionSortManager insertionSortManager;

    protected override void Awake()
    {
        base.Awake();
        tutorialHeight2 = tutorialHeight1 + new Vector3(0f, 0.2f, 0f);
        insertionSortManager = GetComponent(typeof(InsertionSortManager)) as InsertionSortManager;
    }

    public override string GetAlgorithmName()
    {
        return Util.INSERTION_SORT;
    }

    public HolderBase PivotHolder
    {
        get { return pivotHolder; }
    }

    public override string CollectLine(int lineNr)
    {
        switch (lineNr)
        {
            case 0: return "InsertionSort( list ):";
            case 1: return "i = 1";
            case 2: return "while ( i < len( list )):";
            case 3: return "    j = i - 1";
            case 4: return "    while ( j >= 0 and list[ i ] < list[ j ]):";
            case 5: return "        swap list[ i ] and list[ j ]";
            case 6: return "        j -= 1";
            case 7: return "    end while";
            case 8: return "    i += 1";
            case 9: return "end while";
            default: return "X";
        }
    }

    private string PseudoCode(int lineNr, int i, int j, bool increment)
    {
        switch (lineNr)
        {
            case 0: return "InsertionSort( List<int> list )";
            case 1: return "i = 1";
            case 2: return "while ( " + i + " < " + GetComponent<AlgorithmManagerBase>().NumberOfElements + " )";
            case 3: return "    " + j + " = " + i  + " - 1";
            case 4: return "    while ( " + j + " >= 0 and " + value1 + " < " + value2 + " )";
            case 5: return "        swap " + value1 + " and " + value2;
            case 6: return "        " + j + " = " + (j + 1) + " - 1";
            case 7: return "    end while";
            case 8: return "    " + i + " = " + (i - 1) + " + 1";
            case 9: return "end while";
            default: return "X";
        }
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 9;
    }

    public override void ResetSetup()
    {
        Destroy(pivotHolder);
    }

    public void CreatePivotHolder()
    {
        // Instantiate
        Vector3 pos;
        if (GetComponent<AlgorithmManagerBase>().IsTutorial())
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
        StartCoroutine(pseudoCodeViewer.HighlightText(1, PseudoCode(1, i, Util.NO_VALUE, true)));
        yield return new WaitForSeconds(seconds);

        while (i < listLength)
        {
            // Display pseudocode (1st while)
            StartCoroutine(pseudoCodeViewer.HighlightText(2, PseudoCode(2, i, Util.NO_VALUE, true)));
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
            StartCoroutine(pseudoCodeViewer.HighlightText(3, PseudoCode(3, i, j, true)));
            yield return new WaitForSeconds(seconds);

            // Start comparing until find the correct position is found
            // Set first values here to display on blackboard
            value1 = pivot.Value;
            value2 = list[j].GetComponent<InsertionSortElement>().Value;

            // Display pseudocode (2nd while)
            StartCoroutine(pseudoCodeViewer.HighlightText(4, PseudoCode(4, i, j, true)));
            yield return new WaitForSeconds(seconds);

            while (value1 < value2)
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
                StartCoroutine(pseudoCodeViewer.HighlightText(5, PseudoCode(5, i, j, true)));
                yield return new WaitForSeconds(seconds);

                // Preparing for next step
                temp = temp2;
                j -= 1;

                // Display pseudocode (decrement j)
                StartCoroutine(pseudoCodeViewer.HighlightText(6, PseudoCode(6, i, j, true)));
                yield return new WaitForSeconds(seconds);

                // Check if there are more elements to compare the pivot with
                if (j >= 0)
                {
                    // More elements to evaluate
                    value2 = list[j].GetComponent<InsertionSortElement>().Value;

                    // Display pseudocode (2nd while new compare value)
                    StartCoroutine(pseudoCodeViewer.HighlightText(4, PseudoCode(4, i, j, true)));
                    yield return new WaitForSeconds(seconds);
                }
                else
                {
                    // Make sure that the last compare element is marked as sorted
                    compare.IsSorted = true; //list[j + 1].GetComponent<SortingElement>().IsSorted = true;

                    // Display pseudocode (end 2nd while)
                    StartCoroutine(pseudoCodeViewer.HighlightText(7, PseudoCode(7, i, j, true)));
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
            if (i == 1 && value1 >= value2)
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
            StartCoroutine(pseudoCodeViewer.HighlightText(8, PseudoCode(8, i, j, true)));
        }
        // Display pseudocode (end 1st while)
        StartCoroutine(pseudoCodeViewer.HighlightText(9, PseudoCode(9, i, Util.NO_VALUE, true)));
        yield return new WaitForSeconds(seconds);

        // Mark the last element sorted
        list[list.Length - 1].GetComponent<InsertionSortElement>().IsSorted = true;

        // Finished off; remove pivot holder
        PivotHolderVisible(false);
        IsSortingComplete = true;
    }
    #endregion


    #region Execute order from user
    public override void ExecuteOrder(InstructionBase instruction, int instructionNr, bool increment)
    {
        // Gather information from instruction
        InsertionSortInstruction inst = (InsertionSortInstruction)instruction;
        Debug.Log("Debug: " + inst.DebugInfo() + "\n");

        // Change internal state of sorting element
        InsertionSortElement sortingElement = GetComponent<ElementManager>().GetSortingElement(inst.SortingElementID).GetComponent<InsertionSortElement>();

        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], Util.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        int i = inst.I, j = inst.J;
        List<int> lineOfCode = new List<int>();
        switch (inst.ElementInstruction)
        {
            case Util.PIVOT_START_INST:
                if (increment)
                    sortingElement.IsPivot = inst.IsPivot;   // Test, and maybe change to: = increment ?
                else
                    sortingElement.IsPivot = !inst.IsPivot;

                value1 = sortingElement.Value;
                lineOfCode.Add(2);
                lineOfCode.Add(3);
                break;

            case Util.PIVOT_END_INST:
                if (increment)
                {
                    sortingElement.IsPivot = inst.IsPivot;
                    sortingElement.IsSorted = inst.IsSorted;
                }
                else
                {
                    sortingElement.IsPivot = !inst.IsPivot;
                    sortingElement.IsSorted = !inst.IsSorted;
                }

                lineOfCode.Add(8);
                break;

            case Util.COMPARE_START_INST:
                if (increment)
                    sortingElement.IsCompare = inst.IsCompare;
                else
                    sortingElement.IsCompare = !inst.IsCompare;

                lineOfCode.Add(4);
                value2 = sortingElement.Value;
                break;

            case Util.COMPARE_END_INST:
                if (increment)
                {
                    sortingElement.IsCompare = inst.IsCompare;
                    sortingElement.IsSorted = inst.IsSorted;
                }
                else
                {
                    sortingElement.IsCompare = !inst.IsCompare;
                    sortingElement.IsSorted = !inst.IsSorted;
                }
                lineOfCode.Add(7);
                break;

            case Util.SWITCH_INST:
                if (increment)
                {
                    sortingElement.IsCompare = inst.IsCompare;
                    sortingElement.IsSorted = inst.IsSorted;
                }
                else
                {
                    sortingElement.IsCompare = !inst.IsCompare;
                    sortingElement.IsSorted = !inst.IsSorted;
                }
                lineOfCode.Add(5);
                lineOfCode.Add(6);
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(lineOfCode[x], PseudoCode(lineOfCode[x], i, j, increment), Util.HIGHLIGHT_COLOR);
        }

        // Move sorting element
        switch (inst.ElementInstruction)
        {
            case Util.PIVOT_START_INST:
            case Util.SWITCH_INST:
            case Util.PIVOT_END_INST:
                if (increment)
                    sortingElement.transform.position = insertionSortManager.GetCorrectHolder(inst.NextHolderID).transform.position + aboveHolder;
                else
                    sortingElement.transform.position = insertionSortManager.GetCorrectHolder(inst.HolderID).transform.position + aboveHolder;
                break;
        }
    }
    #endregion


    #region Insertion Sort: User Test / Tutorial step by step
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        // Create pivot holder
        CreatePivotHolder();

        // Instructions for user test + pseudo code
        int instructionNr = 0;
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        // Add the first instruction which will be used for Pseudo code
        instructions.Add(instructionNr++, new InsertionSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.FIRST_INSTRUCTION, Util.NO_VALUE, false, false, false));

        InsertionSortInstruction compareElement;

        int i = 1; // Line 1
        while (i < sortingElements.Length) // Line 2
        {
            int j = i - 1; // Line 3

            int temp1 = ((InsertionSortInstruction)sortingElements[i]).HolderID; // pivot: temp1 -> temp2*
            InsertionSortInstruction pivot = new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[i]).SortingElementID, temp1, pivotHolder.HolderID, i, j, Util.PIVOT_START_INST, ((InsertionSortInstruction)sortingElements[i]).Value, true, false, false);
            sortingElements[i] = pivot;

            // Add this move (Pivot moved in pivot position)
            instructions.Add(instructionNr++, pivot);

            while (true)
            {
                // Choose a new compare element // Line 4
                compareElement = new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[j]).SortingElementID, j, Util.NO_DESTINATION, i, j, Util.COMPARE_START_INST, ((InsertionSortInstruction)sortingElements[j]).Value, false, true, sortingElements[j].IsSorted);
                sortingElements[j] = compareElement;
                instructions.Add(instructionNr++, compareElement);

                // Pivot larger than compare element, place compare element
                if (pivot.Value >= compareElement.Value)
                {
                    // Line 7
                    instructions.Add(instructionNr++, new InsertionSortInstruction(compareElement.SortingElementID, j, Util.NO_DESTINATION, i, j, Util.COMPARE_END_INST, compareElement.Value, false, false, true));
                    break;
                }

                // Pivot is less than compare element, switch their spots
                int temp2 = compareElement.HolderID;
                sortingElements[j + 1] = compareElement;
                sortingElements[j] = pivot;
                j -= 1; // Line 6

                // Add this move (compare element switched to pivot/next position) // Line 5
                instructions.Add(instructionNr++, new InsertionSortInstruction(compareElement.SortingElementID, compareElement.HolderID, temp1, i, j, Util.SWITCH_INST, compareElement.Value, false, false, true));

                // temp2 is open spot, temp1 will be given to next compare element or place pivot there
                temp1 = temp2;

                if (j < 0)
                {
                    // Added *** Test: user test
                    instructions.Add(instructionNr++, new InsertionSortInstruction(compareElement.SortingElementID, j, Util.NO_DESTINATION, i, j, Util.COMPARE_END_INST, compareElement.Value, false, false, true));
                    break; // Line 7
                }
            }

            sortingElements[j + 1] = pivot;
            i += 1; // Line 8

            // Add this move (pivot sorted)
            instructions.Add(instructionNr++, new InsertionSortInstruction(pivot.SortingElementID, pivotHolder.HolderID, temp1, i, j, Util.PIVOT_END_INST, pivot.Value, false, false, true));
        }
        // Add the final instruction which will be used for Pseudo code
        instructions.Add(instructionNr, new InsertionSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.FINAL_INSTRUCTION, Util.NO_VALUE, false, false, false));
        return instructions;
    }
    #endregion



    // ----------------------------- Help functions -----------------------------

    private void PivotHolderVisible(bool enable)
    {
        pivotHolderClone.GetComponentInChildren<MeshRenderer>().enabled = enable;
    }
}
