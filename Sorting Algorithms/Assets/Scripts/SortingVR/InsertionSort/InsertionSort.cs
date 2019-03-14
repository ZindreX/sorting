using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(InsertionSortManager))]
public class InsertionSort : SortAlgorithm {

    /* --------------------------------------------------- Insertion Sort --------------------------------------------------- 
     * 1) Starts from 2nd element and checks if it is less than the first
     * 2) Moves if smaller
     * *) Increment start point -> Repeat, but now check all slots to the left of pivot
     */

    [SerializeField]
    private GameObject pivotHolderPrefab, sortingTableHoldersObj;
    private GameObject pivotHolderClone;

    [SerializeField]
    private InsertionSortManager insertionSortManager;

    [Space(2)]
    [Header("Info plate")]
    [SerializeField]
    private GameObject infoPlate;

    private InsertionSortHolder pivotHolder;
    private Vector3 pivotHolderPos = new Vector3(0f, 0.1f, UtilSort.SPACE_BETWEEN_HOLDERS), tutorialPivotElementHeight;


    public override void InitTeachingAlgorithm(float algorithmSpeed)
    {
        tutorialPivotElementHeight = pivotHolderPos + new Vector3(0f, 0.1f, 0f);
        element1Value = "list[i]";
        element2Value = "list[j]";
        base.InitTeachingAlgorithm(algorithmSpeed);
    }

    public override string AlgorithmName
    {
        get { return UtilSort.INSERTION_SORT; }
    }

    public override GameObject InfoPlate
    {
        get { return infoPlate; }
    }

    public HolderBase PivotHolder
    {
        get { return pivotHolder; }
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(UtilSort.SKIP_NO_DESTINATION, new List<string>());
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.FIRST_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.FINAL_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_START_INST);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_END_INST);
        SkipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.SET_SORTED_INST);

        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.INCREMENT_VAR_I);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.SET_VAR_J);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_VAR_J);
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;

        if (pseudoCodeInitilized)
            UpdatePseudoValues(lineNr);

        switch (lineNr)
        {
            case 0: lineOfCode +=  "InsertionSort(list)"; break;
            case 1: lineOfCode +=  "i = 1"; break;
            case 2: lineOfCode +=  "while ( " + i_str + " < " + lengthOfList + " )"; break;
            case 3: lineOfCode +=  "   j = " + iMinus1; break;
            case 4: lineOfCode +=  "   pivot = " + element1Value; break;
            case 5: lineOfCode +=  "   while ( " + j_str + " >= 0 and pivot < " + element2Value + " )"; break;
            case 6: lineOfCode +=  "       move " + element2Value + " to list[" + jPlus1 + "]"; break; // (j_str + 1) 
            case 7: lineOfCode +=  "       j = " + jMinus1; break; // j_str = (j_str + 1) - 1
            case 8: lineOfCode +=  "   end while"; break;
            case 9: lineOfCode +=  "   list[" + jPlus1 + "] = pivot"; break; // (j_str + 1) 
            case 10: lineOfCode += "   i = " + iPlus1; break; // i_str = (i_str - 1) + 1 
            case 11: lineOfCode += "end while"; break;
            default: return "X";
        }
        return lineOfCode;
    }

    private string iMinus1 = "i - 1", iPlus1 = "i + 1", jMinus1 = "j - 1", jPlus1 = "j + 1";
    private void UpdatePseudoValues(int lineNr)
    {
        i_str = i.ToString();
        j_str = j.ToString();

        iMinus1 = (i - 1).ToString();
        iPlus1 = (i + 1).ToString();

        jMinus1 = (j - 1).ToString();
        jPlus1 = (j + 1).ToString();
        
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 11;
    }

    public override void ResetSetup()
    {
        Destroy(pivotHolderClone);
        iMinus1 = "i - 1";
        iPlus1 = "i + 1";
        jMinus1 = "j - 1";
        jPlus1 = "j + 1";
        base.ResetSetup();
    }

    public override void Specials(string method, int number, bool activate)
    {
        switch (method)
        {
            case UtilSort.INCREMENT: MovePivotHolder(true); break;
            case UtilSort.DECREMENT: MovePivotHolder(false); break;
        }
    }

    public void CreatePivotHolder()
    {
        Debug.Log("Creating pivot holder");
        // Instantiate
        Vector3 pos = GetComponentInParent<SortMain>().HolderPositions[1] + pivotHolderPos;

        pivotHolderClone = Instantiate(pivotHolderPrefab, pos, Quaternion.identity);
        pivotHolderClone.AddComponent<InsertionSortHolder>();
        pivotHolderClone.transform.parent = sortingTableHoldersObj.transform;
        pivotHolder = pivotHolderClone.GetComponent<InsertionSortHolder>();

        // Mark as pivotholder
        pivotHolder.IsPivotHolder = true;
        // Set gameobject parent
        pivotHolder.SuperElement = GetComponentInParent<SortMain>();
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

    #region Insertion Sort: Standard (inverted)
    public static int[] InsertionSortFixCase(int[] list, bool inverted)
    {
        int i = 1;
        while (i < list.Length)
        {
            int pivot = list[i];
            int j = i - 1;
            while (j >= 0 && (!inverted && pivot < list[j] || inverted && pivot > list[j]))
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

    #region Insertion Sort: All Moves Demo (Visuals)
    public override IEnumerator Demo(GameObject[] list)
    {
        // Create pivot holder
        CreatePivotHolder();
        Vector3 temp = new Vector3();

        // Testing
        list[0].GetComponent<SortingElementBase>().IsSorted = true;
        UtilSort.IndicateElement(list[0]);
        yield return demoStepDuration;

        i = 1;
        int listLength = list.Length;
        lengthOfList = listLength.ToString();
        // Display pseudocode (set i)
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        while (i < listLength)
        {
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            // Display pseudocode (1st while)
            yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            // Get index of first element to the left of the pivot and compare
            j = i - 1;

            // Display pseudocode (set j)
            yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            // Get pivot
            GameObject pivotObj = list[i];
            InsertionSortElement pivot = list[i].GetComponent<InsertionSortElement>();
            pivot.IsPivot = true;

            // Get pivot's initial position
            temp = pivot.transform.position;

            // Place pivot holder above the pivot element
            pivotHolder.transform.position = temp + pivotHolderPos;

            // Place the pivot on top of the pivot holder
            pivot.transform.position = temp + tutorialPivotElementHeight;

            // Set first values here to display on blackboard
            PreparePseudocodeValue(pivot.Value, 1);
            PreparePseudocodeValue(list[j].GetComponent<InsertionSortElement>().Value, 2);

            // Display pseudocode (set pivot)
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            // Start comparing until find the correct position is found
            // Prepare the element to compare with
            GameObject compareObj = list[j];
            InsertionSortElement compare = compareObj.GetComponent<InsertionSortElement>();
            compare.IsCompare = true;
            UtilSort.IndicateElement(compare.gameObject);

            // Display pseudocode (2nd while)
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            compare.IsCompare = false;
            compare.IsSorted = true;

            while (value1 < value2)
            {
                // Check if user wants to stop the demo
                if (sortMain.UserStoppedAlgorithm)
                    break;

                // Pivot is smaller, start moving compare element
                // Compare element's position
                Vector3 temp2 = compare.transform.position;

                // Moving other element one index to the right
                compare.transform.position = temp;
                // Updating list
                list[j + 1] = compareObj;

                // Display pseudocode (move compare element)
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedAlgorithm)
                    break;

                // Preparing for next step
                temp = temp2;

                // Display pseudocode (update j)
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);
                j -= 1;


                // Check if user wants to stop the demo
                if (sortMain.UserStoppedAlgorithm)
                    break;

                // Move pivot out and place it ontop of pivot holder (above holder it check whether it's put the element)
                pivotHolder.transform.position = temp + pivotHolderPos;
                pivot.transform.position = temp + tutorialPivotElementHeight;

                // Wait to show the pivot being moved
                yield return demoStepDuration;

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedAlgorithm)
                    break;

                // Check if there are more elements to compare the pivot with
                if (j >= 0)
                {
                    // Start comparing until find the correct position is found
                    // Prepare the element to compare with
                    compareObj = list[j];
                    compare = compareObj.GetComponent<InsertionSortElement>();
                    compare.IsCompare = true;
                    PreparePseudocodeValue(compare.Value, 2);
                    UtilSort.IndicateElement(compare.gameObject);

                    // Display pseudocode (2nd while new compare value)
                    yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

                    // Check if user wants to stop the demo
                    if (sortMain.UserStoppedAlgorithm)
                        break;

                    compare.IsCompare = false;
                    compare.IsSorted = true;

                    if (value1 >= value2)
                    {
                        UtilSort.IndicateElement(compare.gameObject);
                        // Display pseudocode (end 2nd while)
                        yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                        // Check if user wants to stop the demo
                        if (sortMain.UserStoppedAlgorithm)
                            break;
                    }
                }
                else
                {
                    // Display pseudocode (end 2nd while)
                    yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);
                    break;
                }
            }
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            if (i == 1 && value1 >= value2)
                compare.CurrentStandingOn.CurrentColor = UtilSort.SORTED_COLOR;

            // Finish off the pivots work
            pivot.IsSorted = true;
            pivot.IsPivot = false;
            pivot.transform.position = temp;

            // Put pivot object back into the list
            list[j + 1] = pivotObj;
            yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;

            // Display pseudocode (increment i)
            yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);
            i += 1;

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedAlgorithm)
                break;
        }
        // Display pseudocode (end 1st while)
        yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);

        // Mark the last element sorted
        InsertionSortElement lastElement = null;
        if (list[list.Length - 1] != null)
            lastElement = list[list.Length - 1].GetComponent<InsertionSortElement>();

        if (lastElement != null)
            lastElement.IsSorted = true;

        // Finished off; remove pivot holder
        PivotHolderVisible(false);
        IsTaskCompleted = true;
    }
    #endregion

    #region Execute order from user
    public override void ExecuteStepByStepOrder(InstructionBase instruction, bool gotSortingElement, bool increment)
    {
        // Gather information from instruction
        InsertionSortInstruction insertionInstruction = null;
        InsertionSortElement sortingElement = null;
        int i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE; // k = Util.NO_VALUE;

        if (gotSortingElement)
        {
            insertionInstruction = (InsertionSortInstruction)instruction;
            Debug.Log("Debug: " + insertionInstruction.DebugInfo() + "\n");

            // Change internal state of sorting element
            sortingElement = sortMain.ElementManager.GetSortingElement(insertionInstruction.SortingElementID).GetComponent<InsertionSortElement>();
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            //k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], UtilSort.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        List<int> lineOfCode = new List<int>();
        switch (instruction.Instruction)
        {
            case UtilSort.SET_SORTED_INST:
                if (increment)
                {
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                    UtilSort.IndicateElement(sortingElement.gameObject);
                }
                else
                {
                    sortingElement.IsSorted = !insertionInstruction.IsSorted;
                    UtilSort.IndicateElement(sortingElement.gameObject);
                }
                break;

            case UtilSort.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode.Add(2);
                break;

            case UtilSort.SET_VAR_J:
                lineOfCode.Add(3);
                break;

            case UtilSort.PIVOT_START_INST:
                if (increment)
                    sortingElement.IsPivot = insertionInstruction.IsPivot;
                else
                    sortingElement.IsPivot = !insertionInstruction.IsPivot;

                value1 = sortingElement.Value;
                UtilSort.IndicateElement(sortingElement.gameObject);

                lineOfCode.Add(4);
                break;

            case UtilSort.COMPARE_START_INST:
                if (increment)
                {
                    sortingElement.IsCompare = insertionInstruction.IsCompare;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                }
                else
                {
                    sortingElement.IsCompare = !insertionInstruction.IsCompare;
                    if (insertionInstruction.HolderID == sortingElement.SortingElementID) // works for worst case, none might be buggy
                        sortingElement.IsSorted = insertionInstruction.IsSorted;
                    else
                        sortingElement.IsSorted = !insertionInstruction.IsSorted;
                }

                value2 = sortingElement.Value;
                UtilSort.IndicateElement(sortingElement.gameObject);

                lineOfCode.Add(5);
                break;

            case UtilSort.SWITCH_INST:
                if (increment)
                {
                    sortingElement.IsCompare = insertionInstruction.IsCompare;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                }
                else
                    sortingElement.IsCompare = !insertionInstruction.IsCompare;

                lineOfCode.Add(6);
                break;

            case UtilSort.UPDATE_VAR_J:
                lineOfCode.Add(7);
                break;

            case UtilSort.COMPARE_END_INST:
                if (increment)
                {
                    sortingElement.IsCompare = insertionInstruction.IsCompare;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                }
                else
                {
                    sortingElement.IsCompare = !insertionInstruction.IsCompare;
                    sortingElement.IsSorted = !insertionInstruction.IsSorted;
                }

                UtilSort.IndicateElement(sortingElement.gameObject);
                lineOfCode.Add(8);
                break;

            case UtilSort.PIVOT_END_INST:
                if (increment)
                {
                    sortingElement.IsPivot = insertionInstruction.IsPivot;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                }
                else
                {
                    sortingElement.IsPivot = !insertionInstruction.IsPivot;
                    sortingElement.IsSorted = !insertionInstruction.IsSorted;
                }

                UtilSort.IndicateElement(sortingElement.gameObject);
                lineOfCode.Add(9);
                break;

            case UtilSort.INCREMENT_VAR_I:
                lineOfCode.Add(10);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode[x]), UtilSort.HIGHLIGHT_COLOR);
        }

        // Move sorting element
        if (gotSortingElement)
        {
            switch (insertionInstruction.Instruction)
            {
                case UtilSort.COMPARE_START_INST: // testing
                    if (increment)
                    {
                        // Positioning the pivot holder behind/slightly above comparing element
                        pivotHolder.transform.position = new Vector3(insertionSortManager.GetCorrectHolder(sortingElement.CurrentStandingOn.HolderID).transform.position.x, pivotHolder.transform.position.y, pivotHolder.transform.position.z);
                        // Postitioning the pivot element on top of the pivot holder
                        pivotHolder.CurrentHolding.transform.position = pivotHolder.transform.position + UtilSort.ABOVE_HOLDER_VR;
                    }
                    else
                    {
                        //sortingElement.transform.position = insertionSortManager.GetCorrectHolder(insertionInstruction.HolderID).transform.position + Util.ABOVE_HOLDER_VR;

                    }
                    break;

                case UtilSort.PIVOT_START_INST: // tesing (was combined with switch/pivot_end
                    if (increment)
                    {
                        pivotHolder.transform.position = new Vector3(insertionSortManager.GetCorrectHolder(sortingElement.CurrentStandingOn.HolderID).transform.position.x, pivotHolder.transform.position.y, pivotHolder.transform.position.z);
                        sortingElement.transform.position = pivotHolder.transform.position + UtilSort.ABOVE_HOLDER_VR;
                    }
                    else
                    {
                        //
                        sortingElement.transform.position = insertionSortManager.GetCorrectHolder(insertionInstruction.HolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                    }
                    break;

                //case Util.PIVOT_START_INST: // original working setup (non moving pivot holder)
                case UtilSort.SWITCH_INST:
                case UtilSort.PIVOT_END_INST:
                    if (increment)
                        sortingElement.transform.position = insertionSortManager.GetCorrectHolder(insertionInstruction.NextHolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                    else
                        sortingElement.transform.position = insertionSortManager.GetCorrectHolder(insertionInstruction.HolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                    break;
            }
        }
    }
    #endregion

    #region User test display pseudocode as support
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        //Debug.LogError("Instruction: " + instruction.Instruction);
        // Gather information from instruction
        InsertionSortElement sortingElement = null;
        //i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE; // k = Util.NO_VALUE;

        if (gotSortingElement)
            sortingElement = sortMain.ElementManager.GetSortingElement(((InsertionSortInstruction)instruction).SortingElementID).GetComponent<InsertionSortElement>();

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            // k = ((InstructionLoop)instruction).K;
        }


        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], UtilSort.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        List<int> lineOfCode = new List<int>(); // change back to int var? no need for list, or change pseudocode?
        Color useColor = UtilSort.HIGHLIGHT_COLOR;
        switch (instruction.Instruction)
        {
            case UtilSort.SET_SORTED_INST:
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                lengthOfList = sortMain.SortSettings.NumberOfElements.ToString();
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode.Add(2);
                break;

            case UtilSort.SET_VAR_J:
                lineOfCode.Add(3);
                break;

            case UtilSort.PIVOT_START_INST:
                PreparePseudocodeValue(sortingElement.Value, 1); // value1 = sortingElement.Value;
                UtilSort.IndicateElement(sortingElement.gameObject);
                useColor = Util.HIGHLIGHT_MOVE_COLOR;

                lineOfCode.Add(4);
                break;

            case UtilSort.COMPARE_START_INST:
                PreparePseudocodeValue(sortingElement.Value, 2); // value2 = sortingElement.Value;
                UtilSort.IndicateElement(sortingElement.gameObject);

                lineOfCode.Add(5);
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode.Add(6);
                useColor = Util.HIGHLIGHT_MOVE_COLOR;
                break;

            case UtilSort.UPDATE_VAR_J:
                lineOfCode.Add(7);
                break;

            case UtilSort.COMPARE_END_INST:
                lineOfCode.Add(8);
                break;

            case UtilSort.PIVOT_END_INST:
                lineOfCode.Add(9);
                useColor = UtilSort.HIGHLIGHT_MOVE_COLOR;
                break;

            case UtilSort.INCREMENT_VAR_I:
                lineOfCode.Add(10);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode[x]), useColor);
        }

        yield return demoStepDuration;
        sortMain.BeginnerWait = false;
    }
    #endregion

    #region Insertion Sort: User Test / Tutorial step by step --> Instructions creator
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        // Create pivot holder
        CreatePivotHolder();

        // Instructions for user test + pseudo code
        int instructionNr = 0;
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        InsertionSortInstruction compareElement;

        // Testing
        instructions.Add(instructionNr++, new InsertionSortInstruction(UtilSort.SET_SORTED_INST, instructionNr, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, 0, ((InsertionSortInstruction)sortingElements[0]).Value, false, true, false, 0, UtilSort.NO_DESTINATION));

        int i = 1; // Line 1
        // Add the first instruction which will be used for Pseudo code
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.FIRST_INSTRUCTION, instructionNr)); // new InstructionBase(Util.FIRST_INSTRUCTION, instructionNr, i, Util.NO_VALUE, false, false));

        while (i < sortingElements.Length)
        {
            // Line 2
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.FIRST_LOOP, instructionNr, i, UtilSort.NO_VALUE, UtilSort.NO_VALUE)); // new InstructionBase(Util.FIRST_LOOP, instructionNr, i, Util.NO_VALUE, false, false));
            
            // Line 3
            int j = i - 1;
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.SET_VAR_J, instructionNr, i, j, UtilSort.NO_VALUE)); // new InstructionBase(Util.SET_VAR_J, instructionNr, i, j, false, false));

            InsertionSortInstruction element = (InsertionSortInstruction)sortingElements[i];
            int temp1 = element.HolderID; //((InsertionSortInstruction)sortingElements[i]).HolderID; // pivot: temp1 -> temp2*

            InsertionSortInstruction pivot = new InsertionSortInstruction(UtilSort.PIVOT_START_INST, instructionNr, i, j, UtilSort.NO_VALUE, element.SortingElementID, element.Value, false, false, true, temp1, pivotHolder.HolderID); // new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[i]).SortingElementID, temp1, pivotHolder.HolderID, i, j, Util.PIVOT_START_INST, instructionNr, ((InsertionSortInstruction)sortingElements[i]).Value, true, false, false);
            sortingElements[i] = pivot;

            // Add this move (Pivot moved in pivot position)
            instructions.Add(instructionNr++, pivot);

            while (true)
            {
                InsertionSortInstruction element2 = (InsertionSortInstruction)sortingElements[j];

                // Choose a new compare element // Line 4
                compareElement = new InsertionSortInstruction(UtilSort.COMPARE_START_INST, instructionNr, i, j, UtilSort.NO_VALUE, element2.SortingElementID, element2.Value, true, element2.IsSorted, false, j, UtilSort.NO_DESTINATION); // new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[j]).SortingElementID, j, Util.NO_DESTINATION, i, j, Util.COMPARE_START_INST, instructionNr, ((InsertionSortInstruction)sortingElements[j]).Value, false, true, sortingElements[j].IsSorted);
                //use this instead?: compareElement = new InstructionSingleElementUpdate(Util.COMPARE_START_INST, instructionNr, i, j, Util.NO_VALUE, element2.SortingElementID, element2.Value, true, element2.IsSorted);
                    
                sortingElements[j] = compareElement;
                instructions.Add(instructionNr++, compareElement);

                // Pivot larger than compare element, place compare element
                if (pivot.Value >= compareElement.Value)
                {
                    // Line 7
                    instructions.Add(instructionNr++, new InsertionSortInstruction(UtilSort.COMPARE_END_INST, instructionNr, i, j, UtilSort.NO_VALUE, compareElement.SortingElementID, compareElement.Value, false, true, false, compareElement.HolderID, UtilSort.NO_DESTINATION)); // new InsertionSortInstruction(compareElement.SortingElementID, compareElement.HolderID, Util.NO_DESTINATION, i, j, Util.COMPARE_END_INST, instructionNr, compareElement.Value, false, false, true));
                    break;
                }

                // Pivot is less than compare element, switch their spots
                int temp2 = compareElement.HolderID;
                sortingElements[j + 1] = compareElement;
                sortingElements[j] = pivot;

                // Add this move (compare element switched to pivot/next position) // Line 6
                instructions.Add(instructionNr++, new InsertionSortInstruction(UtilSort.SWITCH_INST, instructionNr, i, j, UtilSort.NO_VALUE, compareElement.SortingElementID, compareElement.Value, false, true, false, compareElement.HolderID, temp1)); // new InsertionSortInstruction(compareElement.SortingElementID, compareElement.HolderID, temp1, i, j, Util.SWITCH_INST, instructionNr, compareElement.Value, false, false, true));

                // Line 7
                j -= 1;
                instructions.Add(instructionNr++, new InstructionLoop(UtilSort.UPDATE_VAR_J, instructionNr, i, j, UtilSort.NO_VALUE)); // new InstructionBase(Util.UPDATE_VAR_J, instructionNr, i, j, false, false));

                // temp2 is open spot, temp1 will be given to next compare element or place pivot there
                temp1 = temp2;

                if (j < 0)
                {
                    // Line 8
                    instructions.Add(instructionNr++, new InsertionSortInstruction(UtilSort.COMPARE_END_INST, instructionNr, i, j, UtilSort.NO_VALUE, compareElement.SortingElementID, compareElement.Value, false, true, false, j+2, UtilSort.NO_DESTINATION)); // new InsertionSortInstruction(compareElement.SortingElementID, j+2, Util.NO_DESTINATION, i, j, Util.COMPARE_END_INST, instructionNr, compareElement.Value, false, false, true));
                    break;
                }
            }

            sortingElements[j + 1] = pivot;

            // Add this move (pivot sorted) // Line 9
            instructions.Add(instructionNr++, new InsertionSortInstruction(UtilSort.PIVOT_END_INST, instructionNr, i, j, UtilSort.NO_VALUE, pivot.SortingElementID, pivot.Value, false, true, false, pivotHolder.HolderID, temp1)); // new InsertionSortInstruction(pivot.SortingElementID, pivotHolder.HolderID, temp1, i, j, Util.PIVOT_END_INST, instructionNr, pivot.Value, false, false, true));

            // Line 10
            i += 1;
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.INCREMENT_VAR_I, instructionNr, i, j, UtilSort.NO_VALUE)); // new InstructionBase(Util.INCREMENT_VAR_I, instructionNr, i, j, false, false));
        }

        // Line 2 (update loop before finishing)
        instructions.Add(instructionNr++, new InstructionLoop(UtilSort.FIRST_LOOP, instructionNr, i, UtilSort.NO_VALUE, UtilSort.NO_VALUE)); // new InstructionBase(Util.FIRST_LOOP, instructionNr, i, Util.NO_VALUE, false, false));

        // Add the final instruction which will be used for Pseudo code
        instructions.Add(instructionNr, new InstructionBase(UtilSort.FINAL_INSTRUCTION, instructionNr)); // new InsertionSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.FINAL_INSTRUCTION, instructionNr, Util.NO_VALUE, false, false, false));
        return instructions;
    }
    #endregion


    // ----------------------------- Help functions -----------------------------

    private void PivotHolderVisible(bool enable)
    {
        if (pivotHolderClone != null)
            pivotHolderClone.GetComponentInChildren<MeshRenderer>().enabled = enable;
    }

    public void MovePivotHolder(bool increment)
    {
        if (sortMain.ControllerReady)
        {
            if (increment && pivotHolder.PositionIndex < sortMain.HolderManager.Holders.Length - 1)
            {
                pivotHolder.transform.position += new Vector3(UtilSort.SPACE_BETWEEN_HOLDERS, 0f, 0f);
                pivotHolder.PositionIndex++;
                if (pivotHolder.CurrentHolding != null)
                    pivotHolder.CurrentHolding.transform.position += new Vector3(UtilSort.SPACE_BETWEEN_HOLDERS, 0f, 0f);
            }
            else if (!increment && pivotHolder.PositionIndex > 0)
            {
                pivotHolder.transform.position -= new Vector3(UtilSort.SPACE_BETWEEN_HOLDERS, 0f, 0f);
                pivotHolder.PositionIndex--;
                if (pivotHolder.CurrentHolding != null)
                    pivotHolder.CurrentHolding.transform.position -= new Vector3(UtilSort.SPACE_BETWEEN_HOLDERS, 0f, 0f);
            }
        }
            
    }
}
