using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BubbleSortManager))]
public class BubbleSort : SortAlgorithm {

    /* --------------------------------------------------- Bubble Sort --------------------------------------------------- 
     * Comparing 2 elements at a time
     * Moves the biggest until it reaches the end of the list
    */

    [SerializeField]
    private BubbleSortManager bubbleSortManager;

    private string n, numberOfElementsMinusI1;

    public override void InitTeachingAlgorithm(float algorithmSpeed)
    {
        element1Value = "list[j]";
        element2Value = "list[j + 1]";
        n = "n";
        numberOfElementsMinusI1 = "n-i-1";
        base.InitTeachingAlgorithm(algorithmSpeed);
    }

    public override string AlgorithmName
    {
        get { return Util.BUBBLE_SORT; }
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FINAL_INSTRUCTION);
        //skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_START_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_END_INST);
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;

        switch (lineNr)
        {
            case 0: lineOfCode += string.Format("BubbleSort({0}):", listValues); break;
            case 1: lineOfCode += string.Format("  n = {0}", lengthOfList); break;
            case 2: lineOfCode += string.Format("  for i={0} to {1}:", i, n); break;
            case 3: lineOfCode += string.Format("      for j={0} to {1}:", j, numberOfElementsMinusI1); break;
            case 4: lineOfCode += string.Format("          if ( {0} > {1} ):", element1Value, element2Value); break;
            case 5: lineOfCode += string.Format("              swap {0} and {1}", element1Value, element2Value); break;
            case 6: lineOfCode += "          end if"; break;
            case 7: lineOfCode += "      end for"; break;
            case 8: lineOfCode += "  end for"; break;
            default: return Util.INVALID_PSEUDO_CODE_LINE;
        }
        return lineOfCode;
    }

    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        switch (lineNr)
        {
            case 4: return init ? "          if ( list[j] > list[j+1] ):" : "          if ( list[" + j + "] > list[" + (j + 1) + "] ):";
            case 5: return init ? "              swap list[j] and list[j+1]" : "              swap list[" + j + "] and list[" + (j + 1) + "]";
            default: return Util.INVALID_PSEUDO_CODE_LINE;
        }
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 8;
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
        Debug.Log("Nothing to reset (?)");
    }

    public override void Specials(string method, int number, bool activate)
    {
        switch (method)
        {
            case "Somemethod": FirstInstructionCodeLine(); break; // example: some void method
        }
    }

    #region Bubble Sort 1
    public static GameObject[] BubbleSort1(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list.Length - i - 1; j++)
            {
                GameObject pivot = list[j];

                if (pivot.GetComponent<SortingElementBase>().Value > list[j + 1].GetComponent<SortingElementBase>().Value)
                {
                    list[j] = list[j + 1];
                    list[j + 1] = pivot;
                }
            }
        }
        return list;
    }
    #endregion

    #region Bubble Sort 2
    public static GameObject[] BubbleSort2(GameObject[] list)
    {
        bool exchanges = true;
        int N = list.Length;
        while (N > 0 && exchanges)
        {
            exchanges = false;
            for (int i = 0; i < list.Length - 1; i++)
            {
                if (list[i].GetComponent<SortingElementBase>().Value > list[i + 1].GetComponent<SortingElementBase>().Value)
                {
                    exchanges = true;
                    GameObject temp = list[i];
                    list[i] = list[i + 1];
                    list[i + 1] = temp;
                }
            }
            N -= 1;
        }
        return list;
    }
    #endregion

    #region Bubble Sort: All Moves Demo (Visual)
    public override IEnumerator Demo(GameObject[] list)
    {     
        int N = list.Length;
        i = 0;
        j = 0;

        // Display pseudocode (list length)
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_STANDARD_COLOR);

        for (i=0; i < N; i++)
        {
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Display outer loop
            yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_STANDARD_COLOR);

            for (j = 0; j < N - i - 1; j++)
            {
                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Display pseudocode (update for-loops)
                yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_STANDARD_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Choose sorting elements to compare
                BubbleSortElement p1 = list[j].GetComponent<BubbleSortElement>();
                BubbleSortElement p2 = list[j + 1].GetComponent<BubbleSortElement>();

                // Change status
                if (p1 != null)
                    p1.IsCompare = true;
                if (p2 != null)
                    p2.IsCompare = true;

                // Update color on holders
                UtilSort.IndicateElement(p1.gameObject);
                UtilSort.IndicateElement(p2.gameObject);

                // Get their values
                if (p1 != null)
                    PreparePseudocodeValue(p1.Value, 1);
                if (p2 != null)
                    PreparePseudocodeValue(p2.Value, 2);

                // Display pseudocode (list length)
                yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_STANDARD_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                if (value1 > value2)
                {
                    // Switch their positions
                    GameObject temp = list[j];
                    GameObject temp2 = list[j + 1];

                    // Switch their positions (added some stuff such that the elements get back in position)
                    Vector3 pos1 = list[j].GetComponent<SortingElementBase>().PlacementAboveHolder, pos2 = list[j + 1].GetComponent<SortingElementBase>().PlacementAboveHolder;
                    p1.transform.position = pos2;
                    list[j] = temp2;
                    p2.transform.position = pos1;
                    list[j + 1] = temp;
                    UtilSort.ResetRotation(temp);
                    UtilSort.ResetRotation(temp2);

                    // Display pseudocode (swap)
                    yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_STANDARD_COLOR);

                    // Check if user wants to stop the demo
                    if (sortMain.UserStoppedTask)
                        break;
                }
                // Display pseudocode (comparison/if end)
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_STANDARD_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                p1.IsCompare = false;
                if (p1.CurrentStandingOn != null)
                    p1.CurrentStandingOn.CurrentColor = UtilSort.STANDARD_COLOR;
                p2.IsCompare = false;
                if (p2.CurrentStandingOn != null)
                    p2.CurrentStandingOn.CurrentColor = UtilSort.STANDARD_COLOR;
            }
            // Display pseudocode (end 2nd for-loop)
            yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Biggest element moved to the last* index, mark it as sorted
            if (list[N - i - 1] != null)
            {
                BubbleSortElement lastElement = list[N - i - 1].GetComponent<BubbleSortElement>();
                if (lastElement != null)
                    lastElement.IsSorted = true;
            }

            UtilSort.IndicateElement(list[N - i - 1]); //list[N - i - 1].transform.position += Util.ABOVE_HOLDER_VR;
            yield return demoStepDuration;
        }

        // Display pseudocode (end 1st for-loop)
        yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_STANDARD_COLOR);

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
        BubbleSortInstruction bubbleInstruction = null;
        BubbleSortElement se1 = null, se2 = null;

        if (instruction is BubbleSortInstruction)
        {
            bubbleInstruction = (BubbleSortInstruction)instruction;

            // Change internal state of following sorting elements
            if (instruction.Instruction != UtilSort.UPDATE_LOOP_INST && instruction.Instruction != UtilSort.END_LOOP_INST) // no need to check for this anymore?
            {
                //Debug.Log("Inst. debug: " + bubbleInstruction.DebugInfo());
                //Debug.Log("SortmMain         : " + (sortMain != null));
                //Debug.Log("ElementManager   : " + (sortMain.ElementManager != null));
                //Debug.Log("Element 1: " + bubbleInstruction.SortingElementID1 + "; Element 2: " + bubbleInstruction.SortingElementID2);
                //Debug.Log("Element 1        : " + (sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID1) != null));
                //Debug.Log("Element 2        : " + (sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID2) != null));
                //Debug.Log("Element 1        : " + (sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>() != null));
                //Debug.Log("Element 2        : " + (sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>() != null));        // Nullpointer solved: static element number not resetted (Remember to reset on destroy)

                se1 = sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
                se2 = sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            }
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = FirstInstructionCodeLine();
                if (increment)
                    SetLengthOfList();
                else
                    lengthOfList = "len(list)";
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                {
                    lineOfCode = 2;
                    if (increment)
                    {
                        n = lengthOfList;
                        useHighlightColor = UseConditionColor(i != lengthOfListInteger);
                    }
                    else
                    {
                        if (i == 0)
                            n = "n";
                        else
                            i -= 1;
                    }
                }
                else if (k == UtilSort.INNER_LOOP)
                {
                    lineOfCode = 3;
                    if (increment)
                    {
                        numberOfElementsMinusI1 = (lengthOfListInteger - i - 1).ToString();
                        useHighlightColor = UseConditionColor(j != lengthOfListInteger - i - 1);
                    }
                    else
                    {
                        if (i == 0 && j == 0)
                            numberOfElementsMinusI1 = "n-i-1";
                        else if (j == 0)
                        {
                            j = lengthOfListInteger - i;
                            numberOfElementsMinusI1 = j.ToString();
                        }
                        else
                            j -= 1;
                    }
                }
                break;

            case UtilSort.COMPARE_START_INST:
                lineOfCode = 4;
                PreparePseudocodeValue(se1.Value, 1);
                PreparePseudocodeValue(se2.Value, 2);

                if (increment)
                {
                    se1.IsCompare = bubbleInstruction.IsCompare;
                    se2.IsCompare = bubbleInstruction.IsCompare;
                    useHighlightColor = UseConditionColor(se1.Value > se2.Value);
                }
                else
                {
                    se1.IsCompare = !bubbleInstruction.IsCompare;
                    se2.IsCompare = !bubbleInstruction.IsCompare;

                    if (bubbleInstruction.IsElementSorted(se1.SortingElementID) != se1.IsSorted)
                        se1.IsSorted = bubbleInstruction.IsElementSorted(se1.SortingElementID);

                    if (bubbleInstruction.IsElementSorted(se2.SortingElementID) != se2.IsSorted)
                        se2.IsSorted = bubbleInstruction.IsElementSorted(se2.SortingElementID);
                }
                UtilSort.IndicateElement(se1.gameObject);
                UtilSort.IndicateElement(se2.gameObject);
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode = 5;

                if (increment)
                {
                    PreparePseudocodeValue(se1.Value, 1);
                    PreparePseudocodeValue(se2.Value, 2);
                }
                else
                {
                    element1Value = "list[j]";
                    element2Value = "list[j + 1]";
                }
                break;

            case UtilSort.COMPARE_END_INST:
                lineOfCode = 6;

                if (increment)
                {
                    se1.IsCompare = bubbleInstruction.IsCompare;
                    se2.IsCompare = bubbleInstruction.IsCompare;
                    se1.IsSorted = bubbleInstruction.IsElementSorted(se1.SortingElementID);
                    se2.IsSorted = bubbleInstruction.IsElementSorted(se2.SortingElementID);
                }
                else
                {
                    se1.IsCompare = !bubbleInstruction.IsCompare;
                    se2.IsCompare = !bubbleInstruction.IsCompare;
                    if (bubbleInstruction.IsElementSorted(se1.SortingElementID) != se1.IsSorted)
                        se1.IsSorted = bubbleInstruction.IsElementSorted(se1.SortingElementID);

                    if (bubbleInstruction.IsElementSorted(se2.SortingElementID) != se2.IsSorted)
                        se2.IsSorted = bubbleInstruction.IsElementSorted(se2.SortingElementID);
                }
                UtilSort.IndicateElement(se1.gameObject);
                UtilSort.IndicateElement(se2.gameObject);
                break;

            case UtilSort.END_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                    lineOfCode = 8;
                else if (k == UtilSort.INNER_LOOP)
                    lineOfCode = 7;
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
        if (instruction is BubbleSortInstruction)
        {
            switch (instruction.Instruction)
            {
                case UtilSort.SWITCH_INST:
                    if (increment)
                    {
                        se1.transform.position = sortMain.AlgorithmManagerBase.GetCorrectHolder(bubbleInstruction.HolderID2).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        se1.transform.rotation = Quaternion.identity;
                        se2.transform.position = sortMain.AlgorithmManagerBase.GetCorrectHolder(bubbleInstruction.HolderID1).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        se2.transform.rotation = Quaternion.identity;
                    }
                    else
                    {
                        se1.transform.position = sortMain.AlgorithmManagerBase.GetCorrectHolder(bubbleInstruction.HolderID1).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        se1.transform.rotation = Quaternion.identity;
                        se2.transform.position = sortMain.AlgorithmManagerBase.GetCorrectHolder(bubbleInstruction.HolderID2).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        se2.transform.rotation = Quaternion.identity;
                    }
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
        BubbleSortElement se1 = null, se2 = null;

        if (gotSortingElement)
        {
            se1 = sortMain.ElementManager.GetSortingElement(((BubbleSortInstruction)instruction).SortingElementID1).GetComponent<BubbleSortElement>();
            se2 = sortMain.ElementManager.GetSortingElement(((BubbleSortInstruction)instruction).SortingElementID2).GetComponent<BubbleSortElement>();
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = FirstInstructionCodeLine();
                SetLengthOfList();
                n = lengthOfList;
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                {
                    lineOfCode = 2;
                    useHighlightColor = UseConditionColor(i != lengthOfListInteger);
                }
                else if (k == UtilSort.INNER_LOOP)
                {
                    lineOfCode = 3;
                    numberOfElementsMinusI1 = (lengthOfListInteger - i - 1).ToString();
                    useHighlightColor = UseConditionColor(j != lengthOfListInteger - i - 1);
                }
                break;

            case UtilSort.COMPARE_START_INST:
                lineOfCode = 4;
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;

                PreparePseudocodeValue(se1.Value, 1);
                PreparePseudocodeValue(se2.Value, 2);

                UtilSort.IndicateElement(se1.gameObject);
                UtilSort.IndicateElement(se2.gameObject);
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode = 5;
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                break;

            case UtilSort.COMPARE_END_INST:
                lineOfCode = 6;
                break;

            case UtilSort.END_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                    lineOfCode = 8;
                else if (k == UtilSort.INNER_LOOP)
                    lineOfCode = 7;
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

    #region Bubble Sort: Instructions
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0, i = 0, j = 0;

        // Add first instruction
        instructions.Add(instNr, new InstructionBase(Util.FIRST_INSTRUCTION, instNr++));

        int N = sortingElements.Length; // line 1 
        for (i = 0; i < N; i++)
        {
            instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j, UtilSort.OUTER_LOOP));
            for (j = 0; j < N - i - 1; j++)
            {
                // Update for loop instruction
                instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j, UtilSort.INNER_LOOP));

                // Choose sorting elements to compare
                BubbleSortInstruction comparison = MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, UtilSort.COMPARE_START_INST, instNr, true, false);

                // Add this instruction
                instructions.Add(instNr++, comparison);

                if (comparison.Value1 > comparison.Value2)
                {
                    // Switch their positions
                    instructions.Add(instNr, MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, UtilSort.SWITCH_INST, instNr++, true, false));

                    int holder1 = ((BubbleSortInstruction)sortingElements[j]).HolderID1;
                    int holder2 = ((BubbleSortInstruction)sortingElements[j + 1]).HolderID1;
                    InstructionBase temp = sortingElements[j];
                    sortingElements[j] = sortingElements[j + 1];
                    sortingElements[j + 1] = temp;

                    // Update holderID
                    ((BubbleSortInstruction)sortingElements[j]).HolderID1 = holder1;
                    ((BubbleSortInstruction)sortingElements[j + 1]).HolderID1 = holder2;
                }
                // Add this instruction
                instructions.Add(instNr, MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, UtilSort.COMPARE_END_INST, instNr++, false, false));
            }
            if (instructions[instructions.Count - 1] is BubbleSortInstruction)
                ((BubbleSortInstruction)instructions[instructions.Count - 1]).IsSorted = true;

            // Update end 2nd for-loop instruction
            instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j, UtilSort.INNER_LOOP));
            instructions.Add(instNr, new InstructionLoop(UtilSort.END_LOOP_INST, instNr++, i, j, UtilSort.INNER_LOOP));
        }
        instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j, UtilSort.OUTER_LOOP));
        instructions.Add(instNr, new InstructionLoop(UtilSort.END_LOOP_INST, instNr++, i, j, UtilSort.OUTER_LOOP));

        // Final instruction
        instructions.Add(instNr, new InstructionBase(Util.FINAL_INSTRUCTION, instNr++));
        return instructions;
    }
    #endregion


    // Help functions

    private BubbleSortInstruction MakeInstruction(InstructionBase inst1, InstructionBase inst2, int i, int j, string instruction, int instructionNr, bool isCompare, bool isSorted)
    {
        int seID1 = ((BubbleSortInstruction)inst1).SortingElementID1;
        int seID2 = ((BubbleSortInstruction)inst2).SortingElementID1;
        int hID1 = ((BubbleSortInstruction)inst1).HolderID1;
        int hID2 = ((BubbleSortInstruction)inst2).HolderID1;
        int value1 = ((BubbleSortInstruction)inst1).Value1;
        int value2 = ((BubbleSortInstruction)inst2).Value1;
        return new BubbleSortInstruction(instruction, instructionNr, i, j, UtilSort.NO_VALUE, seID1, seID2, hID1, hID2, value1, value2, isCompare, isSorted);
    }





    // Debugging
    private void DebugCheck(InstructionBase[] elements, Dictionary<int, InstructionBase> dict)
    {
        string test1 = "", test2 = "";
        for (int x = 0; x < elements.Length; x++)
        {
            test1 += "[" + ((BubbleSortInstruction)elements[x]).Value1 + "|" + ((BubbleSortInstruction)elements[x]).IsSorted + "] ";
        }
        Debug.Log(test1);
        for (int x = 0; x < dict.Count; x++)
        {
            //test2 += "[" + ((BubbleSortInstruction)dict[x]).Value1 + "|" + dict[x].IsSorted + "|" + ((BubbleSortInstruction)dict[x]).Value2 + "] ";
            test2 += dict[x].DebugInfo() + "\n";
        }
        Debug.Log(test2 + "\n");

    }


}
