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

    private InsertionSortHolder pivotHolder;
    private Vector3 pivotHolderPos = new Vector3(0f, 0.1f, UtilSort.SPACE_BETWEEN_HOLDERS), tutorialPivotElementHeight;

    private string iMinus1 = "i - 1", iPlus1 = "i + 1", jMinus1 = "j - 1", jPlus1 = "j + 1";

    public override void InitTeachingAlgorithm(float algorithmSpeed)
    {
        tutorialPivotElementHeight = pivotHolderPos + new Vector3(0f, 0.1f, 0f);
        element1Value = "list[i]";
        element2Value = "list[j]";
        base.InitTeachingAlgorithm(algorithmSpeed);
    }

    public override string AlgorithmName
    {
        get { return Util.INSERTION_SORT; }
    }

    public HolderBase PivotHolder
    {
        get { return pivotHolder; }
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FINAL_INSTRUCTION);
        //skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_START_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_END_INST);
        SkipDict[Util.SKIP_NO_DESTINATION].Add(UtilSort.SET_SORTED_INST);

        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.INCREMENT_VAR_I);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.SET_VAR_J);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.UPDATE_VAR_J);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_LOOP_INST);
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode +=  "InsertionSort(" + listValues + ")"; break;
            case 1: lineOfCode +=  "    i = 1"; break;
            case 2: lineOfCode +=  "    while ( " + i_str + " < " + lengthOfList + " )"; break;
            case 3: lineOfCode +=  "        j = " + iMinus1; break;
            case 4: lineOfCode +=  "        pivot = " + element1Value; break;
            case 5: lineOfCode +=  "        while ( " + j_str + " >= 0 and pivot < " + element2Value + " )"; break;
            case 6: lineOfCode +=  "            move " + element2Value + " to list[" + jPlus1 + "]"; break; // (j_str + 1) 
            case 7: lineOfCode +=  "            j = " + jMinus1; break; // j_str = (j_str + 1) - 1
            case 8: lineOfCode +=  "        end while"; break;
            case 9: lineOfCode +=  "        list[" + jPlus1 + "] = pivot"; break; // (j_str + 1) 
            case 10: lineOfCode += "        i = " + iPlus1; break; // i_str = (i_str - 1) + 1 
            case 11: lineOfCode += "    end while"; break;
            default: return Util.INVALID_PSEUDO_CODE_LINE;
        }
        return lineOfCode;
    }

    // Extra
    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        switch (lineNr)
        {
            case 2: return init ? "    while ( i < len(list) )" : "    while ( " + i + " < " + lengthOfList + " )";
            case 3: return init ? "        j = i - 1" : "        j = " + i + " - 1";
            case 4: return init ? "        pivot = list[i]" : "        pivot = list[" + i + "]";
            case 5: return init ? "        while ( j >= 0 and pivot < list[j] )" : "        while ( " + j + " >= 0 and pivot < list[" + j + "] )";
            case 6: return init ? "            move list[j] to list[j + 1]" : "            move list[" + j + "] to list[" + j + " + 1]";
            case 7: return init ? "            j = j - 1" : "            j = " + j + " - 1";
            case 9: return init ? "        list[j + 1] = pivot" : "        list[" + j + " + 1] = pivot";
            case 10: return init ? "        i = i + 1" : "        i = " + i + " + 1";
            default: return Util.INVALID_PSEUDO_CODE_LINE;
        }
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
            case UtilSort.INIT: CreatePivotHolder(); break;
            case Util.INCREMENT: MovePivotHolder(true); break;
            case Util.DECREMENT: MovePivotHolder(false); break;
        }
    }

    public void CreatePivotHolder()
    {
        Debug.Log("Creating pivot holder");
        // Instantiate
        Vector3 pos = sortMain.HolderPositions[1] + pivotHolderPos;

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

    #region Insertion Sort: All Moves Demo (Visuals, OLD)
    public override IEnumerator Demo(GameObject[] list)
    {
        Vector3 temp = new Vector3();

        // Testing
        list[0].GetComponent<SortingElementBase>().IsSorted = true;
        UtilSort.IndicateElement(list[0]);
        yield return demoStepDuration;

        i = 1;
        int listLength = list.Length;
        lengthOfList = listLength.ToString();
        // Display pseudocode (set i)
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_STANDARD_COLOR);

        while (i < listLength)
        {
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Display pseudocode (1st while)
            yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Get index of first element to the left of the pivot and compare
            j = i - 1;

            // Display pseudocode (set j)
            yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
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
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Start comparing until find the correct position is found
            // Prepare the element to compare with
            GameObject compareObj = list[j];
            InsertionSortElement compare = compareObj.GetComponent<InsertionSortElement>();
            compare.IsCompare = true;
            UtilSort.IndicateElement(compare.gameObject);

            // Display pseudocode (2nd while)
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            compare.IsCompare = false;
            compare.IsSorted = true;

            while (value1 < value2)
            {
                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Pivot is smaller, start moving compare element
                // Compare element's position
                Vector3 temp2 = compare.transform.position;

                // Moving other element one index to the right
                compare.transform.position = temp;
                // Updating list
                list[j + 1] = compareObj;

                // Display pseudocode (move compare element)
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_STANDARD_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Preparing for next step
                temp = temp2;

                // Display pseudocode (update j)
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_STANDARD_COLOR);
                j -= 1;


                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Move pivot out and place it ontop of pivot holder (above holder it check whether it's put the element)
                pivotHolder.transform.position = temp + pivotHolderPos;
                pivot.transform.position = temp + tutorialPivotElementHeight;

                // Wait to show the pivot being moved
                yield return demoStepDuration;

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
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
                    yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_STANDARD_COLOR);

                    // Check if user wants to stop the demo
                    if (sortMain.UserStoppedTask)
                        break;

                    compare.IsCompare = false;
                    compare.IsSorted = true;

                    if (value1 >= value2)
                    {
                        UtilSort.IndicateElement(compare.gameObject);
                        // Display pseudocode (end 2nd while)
                        yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_STANDARD_COLOR);

                        // Check if user wants to stop the demo
                        if (sortMain.UserStoppedTask)
                            break;
                    }
                }
                else
                {
                    // Display pseudocode (end 2nd while)
                    yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_STANDARD_COLOR);
                    break;
                }
            }
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            if (i == 1 && value1 >= value2)
                compare.CurrentStandingOn.CurrentColor = UtilSort.SORTED_COLOR;

            // Finish off the pivots work
            pivot.IsSorted = true;
            pivot.IsPivot = false;
            pivot.transform.position = temp;

            // Put pivot object back into the list
            list[j + 1] = pivotObj;
            yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Display pseudocode (increment i)
            yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_STANDARD_COLOR);
            i += 1;

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;
        }
        // Display pseudocode (end 1st while)
        yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_STANDARD_COLOR);

        // Mark the last element sorted
        InsertionSortElement lastElement = null;
        if (list[list.Length - 1] != null)
            lastElement = list[list.Length - 1].GetComponent<InsertionSortElement>();

        if (lastElement != null)
            lastElement.IsSorted = true;

        // Finished off; remove pivot holder
        PivotHolderVisible(false);

        if (sortMain.UserStoppedTask)
            sortMain.UpdateCheckList(Util.DEMO, true);
        else
            isTaskCompleted = true;
    }
    #endregion

    #region New Demo / Step-by-step
    public override IEnumerator ExecuteDemoInstruction(InstructionBase instruction, bool increment)
    {
        // Gather information from instruction
        InsertionSortInstruction insertionInstruction = null;
        InsertionSortElement sortingElement = null;

        if (instruction is InsertionSortInstruction)
        {
            insertionInstruction = (InsertionSortInstruction)instruction;

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
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
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

            case Util.FIRST_INSTRUCTION:
                lineOfCode = FirstInstructionCodeLine();
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode = 2;

                if (increment)
                {
                    i_str = i.ToString();
                    SetLengthOfList();

                    if (i >= lengthOfListInteger)
                        useHighlightColor = Util.HIGHLIGHT_CONDITION_NOT_FULFILLED;
                }
                else
                {
                    if (i > 1)
                        i_str = (i - 1).ToString();
                    else
                    {
                        i_str = "i";
                        lengthOfList = "len(list)";
                    }

                }
                break;

            case Util.SET_VAR_J:
                lineOfCode = 3;

                if (increment)
                    iMinus1 = (i - 1).ToString();
                else
                {
                    if (i > 1)
                        iMinus1 = (i - 2).ToString();
                    else
                        iMinus1 = "i - 1";
                }
                break;

            case UtilSort.PIVOT_START_INST:
                lineOfCode = 4;

                if (increment)
                {
                    PreparePseudocodeValue(sortingElement.Value, 1);
                    sortingElement.IsPivot = insertionInstruction.IsPivot;
                }
                else
                {
                    element1Value = "list[i]";
                    sortingElement.IsPivot = !insertionInstruction.IsPivot;
                }
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.COMPARE_START_INST:
                lineOfCode = 5;
                if (increment)
                {
                    PreparePseudocodeValue(sortingElement.Value, 2);
                    sortingElement.IsCompare = insertionInstruction.IsCompare;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                    j_str = j.ToString();

                    if (j < 0 || sortingElement.Value <= value1)
                        useHighlightColor = Util.HIGHLIGHT_CONDITION_NOT_FULFILLED;
                }
                else
                {
                    element2Value = "list[j]";
                    sortingElement.IsCompare = !insertionInstruction.IsCompare;
                    sortingElement.IsSorted = !insertionInstruction.IsSorted;

                    if (j > 0)
                        j_str = j.ToString();
                    else
                        j_str = "j";
                }
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.UPDATE_LOOP_INST: // Update 2nd while loop when j < 0
                lineOfCode = 5;

                if (increment)
                {
                    j_str = j.ToString();
                    element2Value = "X";
                    useHighlightColor = Util.HIGHLIGHT_CONDITION_NOT_FULFILLED;
                }
                else
                {
                    j_str = "j";
                    element2Value = "list[j]";
                }
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode = 6;

                if (increment)
                {
                    //PreparePseudocodeValue(sortingElement.Value, 2); // testing
                    sortingElement.IsCompare = insertionInstruction.IsCompare;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                    jPlus1 = (j + 1).ToString();
                }
                else
                {
                    element2Value = "list[j]";
                    jPlus1 = "j + 1";
                    sortingElement.IsCompare = !insertionInstruction.IsCompare;
                }
                break;

            case Util.UPDATE_VAR_J:
                lineOfCode = 7;

                if (increment)
                    jMinus1 = (j - 1).ToString();
                else
                {
                    if (i > 1)
                        jMinus1 = j.ToString();
                    else
                        jMinus1 = "j - 1";

                }
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
                lineOfCode = 8;
                break;

            case UtilSort.PIVOT_END_INST:
                lineOfCode = 9;

                if (increment)
                {
                    sortingElement.IsPivot = insertionInstruction.IsPivot;
                    sortingElement.IsSorted = insertionInstruction.IsSorted;
                    jPlus1 = (j + 1).ToString();
                }
                else
                {
                    PreparePseudocodeValue(sortingElement.Value, 1);
                    sortingElement.IsPivot = !insertionInstruction.IsPivot;
                    sortingElement.IsSorted = !insertionInstruction.IsSorted;
                    jPlus1 = "j + 1";
                }
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case Util.INCREMENT_VAR_I:
                lineOfCode = 10;

                if (increment)
                    iPlus1 = (i + 1).ToString();
                else
                {
                    if (i > 1)
                        iPlus1 = i.ToString();
                    else
                        iPlus1 = "i + 1";
                }
                break;

            case Util.FINAL_INSTRUCTION:
                lineOfCode = FinalInstructionCodeLine();
                IsTaskCompleted = increment;
                break;
        }

        // Highlight part of code in pseudocode
        yield return HighlightPseudoCode(CollectLine(lineOfCode), useHighlightColor);

        // Mark prev for next round
        prevHighlightedLineOfCode = lineOfCode;

        // Move sorting element
        if (instruction is InsertionSortInstruction)
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
        sortMain.WaitForSupportToComplete--;
    }
    #endregion


    #region User test display pseudocode as support
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        // Gather information from instruction
        InsertionSortElement sortingElement = null;

        if (gotSortingElement)
            sortingElement = sortMain.ElementManager.GetSortingElement(((InsertionSortInstruction)instruction).SortingElementID).GetComponent<InsertionSortElement>();

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            // k = ((InstructionLoop)instruction).K;
        }


        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case UtilSort.SET_SORTED_INST:
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case Util.FIRST_INSTRUCTION:
                lineOfCode = FirstInstructionCodeLine();
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode = 2;
                i_str = i.ToString();
                SetLengthOfList();

                if (i >= lengthOfListInteger)
                    useHighlightColor = Util.HIGHLIGHT_CONDITION_NOT_FULFILLED;
                break;

            case Util.SET_VAR_J:
                lineOfCode = 3;
                iMinus1 = (i - 1).ToString();
                break;

            case UtilSort.PIVOT_START_INST:
                lineOfCode = 4;
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;

                PreparePseudocodeValue(sortingElement.Value, 1);
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.COMPARE_START_INST:
                lineOfCode = 5;
                j_str = j.ToString();
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                PreparePseudocodeValue(sortingElement.Value, 2);
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.UPDATE_LOOP_INST: // Update 2nd while loop when j < 0
                lineOfCode = 5;
                useHighlightColor = Util.HIGHLIGHT_CONDITION_NOT_FULFILLED;
                j_str = j.ToString();
                element2Value = "X";
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode = 6;
                jPlus1 = (j + 1).ToString();
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                break;

            case Util.UPDATE_VAR_J:
                lineOfCode = 7;
                jMinus1 = (j - 1).ToString();
                break;

            case UtilSort.COMPARE_END_INST:
                lineOfCode = 8;
                break;

            case UtilSort.PIVOT_END_INST:
                lineOfCode = 9;
                jPlus1 = (j + 1).ToString();
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                break;

            case Util.INCREMENT_VAR_I:
                lineOfCode = 10;
                iPlus1 = (i + 1).ToString();
                break;

            case Util.FINAL_INSTRUCTION:
                lineOfCode = FinalInstructionCodeLine();
                break;
        }

        // Highlight part of code in pseudocode
        yield return HighlightPseudoCode(CollectLine(lineOfCode), useHighlightColor);

        // Mark prev for next round
        prevHighlightedLineOfCode = lineOfCode;

        sortMain.WaitForSupportToComplete--;
    }
    #endregion

    #region Insertion Sort: User Test / Tutorial step by step --> Instructions creator
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        // Instructions for user test + pseudo code
        int instNr = 0;
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        InsertionSortInstruction compareElement;

        instructions.Add(instNr, new InsertionSortInstruction(UtilSort.SET_SORTED_INST, instNr++, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, 0, ((InsertionSortInstruction)sortingElements[0]).Value, false, true, false, 0, UtilSort.NO_DESTINATION));

        int i = 1; // Line 1
        // Add the first instruction which will be used for Pseudo code
        instructions.Add(instNr, new InstructionBase(Util.FIRST_INSTRUCTION, instNr++));

        while (i < sortingElements.Length)
        {
            // Line 2
            instructions.Add(instNr, new InstructionLoop(UtilSort.FIRST_LOOP, instNr++, i, Util.NO_VALUE, Util.NO_VALUE));
            
            // Line 3
            int j = i - 1;
            instructions.Add(instNr, new InstructionLoop(Util.SET_VAR_J, instNr++, i, j, Util.NO_VALUE));

            InsertionSortInstruction element = (InsertionSortInstruction)sortingElements[i];
            int temp1 = element.HolderID;
                                                                                                                                                                      // compare, sorted, pivot
            InsertionSortInstruction pivot = new InsertionSortInstruction(UtilSort.PIVOT_START_INST, instNr, i, j, Util.NO_VALUE, element.SortingElementID, element.Value, false, false, true, temp1, pivotHolder.HolderID);
            sortingElements[i] = pivot;

            // Line 4: Add this move (Pivot moved in pivot position)
            instructions.Add(instNr++, pivot);

            while (true)
            {
                InsertionSortInstruction element2 = (InsertionSortInstruction)sortingElements[j];

                // Line 5: Choose a new compare element
                compareElement = new InsertionSortInstruction(UtilSort.COMPARE_START_INST, instNr, i, j, Util.NO_VALUE, element2.SortingElementID, element2.Value, true, element2.IsSorted, false, j, UtilSort.NO_DESTINATION);
                    
                sortingElements[j] = compareElement;
                instructions.Add(instNr++, compareElement);

                // Pivot larger than compare element, place compare element
                if (pivot.Value >= compareElement.Value)
                {
                    // Line 7
                    instructions.Add(instNr, new InsertionSortInstruction(UtilSort.COMPARE_END_INST, instNr++, i, j, Util.NO_VALUE, compareElement.SortingElementID, compareElement.Value, false, true, false, compareElement.HolderID, UtilSort.NO_DESTINATION));
                    break;
                }

                // Pivot is less than compare element, switch their spots
                int temp2 = compareElement.HolderID;
                sortingElements[j + 1] = compareElement;
                sortingElements[j] = pivot;

                // Add this move (compare element switched to pivot/next position) // Line 6
                instructions.Add(instNr, new InsertionSortInstruction(UtilSort.SWITCH_INST, instNr++, i, j, Util.NO_VALUE, compareElement.SortingElementID, compareElement.Value, false, true, false, compareElement.HolderID, temp1));

                // Line 7
                instructions.Add(instNr, new InstructionLoop(Util.UPDATE_VAR_J, instNr++, i, j, Util.NO_VALUE));
                j -= 1;

                // temp2 is open spot, temp1 will be given to next compare element or place pivot there
                temp1 = temp2;

                if (j < 0)
                {
                    // Line 8
                    instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j));
                    instructions.Add(instNr, new InsertionSortInstruction(UtilSort.COMPARE_END_INST, instNr++, i, j, Util.NO_VALUE, compareElement.SortingElementID, compareElement.Value, false, true, false, j+2, UtilSort.NO_DESTINATION));
                    break;
                }
            }

            sortingElements[j + 1] = pivot;

            // Add this move (pivot sorted) // Line 9
            instructions.Add(instNr, new InsertionSortInstruction(UtilSort.PIVOT_END_INST, instNr++, i, j, Util.NO_VALUE, pivot.SortingElementID, pivot.Value, false, true, false, pivotHolder.HolderID, temp1));

            // Line 10
            instructions.Add(instNr, new InstructionLoop(Util.INCREMENT_VAR_I, instNr++, i, j, Util.NO_VALUE));
            i += 1;
        }

        // Line 2 (update loop before finishing)
        instructions.Add(instNr, new InstructionLoop(UtilSort.FIRST_LOOP, instNr++, i, Util.NO_VALUE, Util.NO_VALUE));

        // Add the final instruction which will be used for Pseudo code
        instructions.Add(instNr, new InstructionBase(Util.FINAL_INSTRUCTION, instNr++));
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
