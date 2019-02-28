using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BubbleSortManager))]
public class BubbleSort : SortAlgorithm {

    /* --------------------------------------------------- Bubble Sort --------------------------------------------------- 
     * Comparing 2 elements at a time
     * Moves the biggest until it reaches the end of the list
    */
    private Dictionary<int, string> pseudoCode;

    private BubbleSortManager bubbleSortManager;
    private List<string> pseudoCodeLines;

    protected override void Awake()
    {
        base.Awake();
        bubbleSortManager = GetComponent(typeof(BubbleSortManager)) as BubbleSortManager;
    }

    public override string AlgorithmName
    {
        get { return UtilSort.BUBBLE_SORT; }
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(UtilSort.SKIP_NO_DESTINATION, new List<string>());
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.FIRST_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.FINAL_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_START_INST);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.COMPARE_END_INST);
    }

    public override string CollectLine(int lineNr)
    {
        string temp = PseudoCode(lineNr, 0, 0);
        switch (lineNr)
        {
            case 0: case 6: case 7: case 8: return temp;
            case 1: return temp.Replace(GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements.ToString(), "len( list )");
            case 2: return temp.Replace((GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements - 1).ToString(), "n-1");
            case 3: return temp.Replace((GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements - 1).ToString(), "n-i-1");
            case 4: case 5: return temp.Replace(UtilSort.INIT_STATE.ToString(), "list[ j ]").Replace((UtilSort.INIT_STATE - 1).ToString(), "list[ j + 1 ]");
            default: return "lineNr " + lineNr + " not found!";
        }
    }

    private string PseudoCode(int lineNr, int i, int j)
    {
        int n = GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements;
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode += "BubbleSort( list ):"; break; // add case for this line(?): BubbleSort(4, 1, 10, ... etc.)
            case 1: lineOfCode += string.Format("  n = {0}", n); break; //"n = " + n;
            case 2: lineOfCode += string.Format("  for i={0} to {1}:", i, (n-1)); break; //"for i=" + i + " to " + (n - 1) + ":";
            case 3: lineOfCode += string.Format("      for j={0} to {1}:", j, (n-i-1)); break; // "    for j=" + j + " to " + (n - i - 1) + ":";
            case 4: lineOfCode += string.Format("          if ( {0} > {1} ):", value1, value2); break; //"          if ( " + value1 + " > " + value2 + " ):";
            case 5: lineOfCode += string.Format("              swap {0} and {1}", value1, value2); break; //"            swap " + value1 + " and " + value2;
            case 6: lineOfCode += "          end if"; break;
            case 7: lineOfCode += "      end for"; break;
            case 8: lineOfCode += "  end for"; break;
            default: return "lineNr " + lineNr + " not found!";
        }
        return lineOfCode;
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
        int N = list.Length, i = 0, j = 0;

        // Display pseudocode (list length)
        yield return HighlightPseudoCode(PseudoCode(1, i, j), Util.HIGHLIGHT_COLOR);

        for (i=0; i < N; i++)
        {
            // Display outer loop
            yield return HighlightPseudoCode(PseudoCode(2, i, j), Util.HIGHLIGHT_COLOR);

            for (j = 0; j < N - i - 1; j++)
            {
                // Display pseudocode (update for-loops)
                yield return HighlightPseudoCode(PseudoCode(3, i, j), Util.HIGHLIGHT_COLOR);

                // Choose sorting elements to compare
                BubbleSortElement p1 = list[j].GetComponent<BubbleSortElement>();
                BubbleSortElement p2 = list[j + 1].GetComponent<BubbleSortElement>();

                // Change status
                p1.IsCompare = true;
                p2.IsCompare = true;

                // Update color on holders
                UtilSort.IndicateElement(p1.gameObject);
                UtilSort.IndicateElement(p2.gameObject);

                // Get their values
                value1 = p1.Value;
                value2 = p2.Value;

                // Display pseudocode (list length)
                yield return HighlightPseudoCode(PseudoCode(4, i, j), Util.HIGHLIGHT_COLOR);

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
                    yield return HighlightPseudoCode(PseudoCode(5, i, j), Util.HIGHLIGHT_COLOR);
                }
                // Display pseudocode (comparison/if end)
                yield return HighlightPseudoCode(PseudoCode(6, i, j), Util.HIGHLIGHT_COLOR);

                p1.IsCompare = false;
                if (p1.CurrentStandingOn != null)
                    p1.CurrentStandingOn.CurrentColor = UtilSort.STANDARD_COLOR;
                p2.IsCompare = false;
                if (p2.CurrentStandingOn != null)
                    p2.CurrentStandingOn.CurrentColor = UtilSort.STANDARD_COLOR;
            }
            // Display pseudocode (end 2nd for-loop)
            yield return HighlightPseudoCode(PseudoCode(7, i, j), Util.HIGHLIGHT_COLOR);

            list[N - i - 1].GetComponent<BubbleSortElement>().IsSorted = true;
            UtilSort.IndicateElement(list[N - i - 1]); //list[N - i - 1].transform.position += Util.ABOVE_HOLDER_VR;
            yield return demoStepDuration;
        }
        isTaskCompleted = true;

        // Display pseudocode (end 1st for-loop)
        yield return HighlightPseudoCode(PseudoCode(8, i, j), Util.HIGHLIGHT_COLOR);
    }
    #endregion

    #region Execute order from user
    public override void ExecuteStepByStepOrder(InstructionBase instruction, bool gotSortingElement, bool increment)
    {
        // Gather information from instruction
        BubbleSortInstruction bubbleInstruction = null;
        BubbleSortElement se1 = null, se2 = null;
        int i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE, k = UtilSort.NO_VALUE;

        if (gotSortingElement)
        {
            bubbleInstruction = (BubbleSortInstruction)instruction;

            // Change internal state of following sorting elements
            if (instruction.Instruction != UtilSort.UPDATE_LOOP_INST && instruction.Instruction != UtilSort.END_LOOP_INST)
            {
                se1 = GetComponent<ElementManager>().GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
                se2 = GetComponent<ElementManager>().GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            }
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
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
            case UtilSort.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                    lineOfCode.Add(2);
                else if (k == UtilSort.INNER_LOOP)
                    lineOfCode.Add(3);
                break;

            case UtilSort.COMPARE_START_INST:
                if (increment)
                {
                    se1.IsCompare = bubbleInstruction.IsCompare;
                    se2.IsCompare = bubbleInstruction.IsCompare;
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

                lineOfCode.Add(4);
                value1 = se1.Value;
                value2 = se2.Value;
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode.Add(5);
                break;

            case UtilSort.COMPARE_END_INST:
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

                lineOfCode.Add(6);
                break;

            case UtilSort.END_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                    lineOfCode.Add(8);
                else if (k == UtilSort.INNER_LOOP)
                    lineOfCode.Add(7);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;

        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(PseudoCode(lineOfCode[x], i, j), UtilSort.HIGHLIGHT_COLOR);
        }

        // Move sorting element
        if (gotSortingElement)
        {
            switch (instruction.Instruction)
            {
                case UtilSort.SWITCH_INST:
                    if (increment)
                    {
                        se1.transform.position = bubbleSortManager.GetCorrectHolder(bubbleInstruction.HolderID2).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        se2.transform.position = bubbleSortManager.GetCorrectHolder(bubbleInstruction.HolderID1).transform.position + UtilSort.ABOVE_HOLDER_VR;
                    }
                    else
                    {
                        se1.transform.position = bubbleSortManager.GetCorrectHolder(bubbleInstruction.HolderID1).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        se2.transform.position = bubbleSortManager.GetCorrectHolder(bubbleInstruction.HolderID2).transform.position + UtilSort.ABOVE_HOLDER_VR;
                    }
                    break;
            }
        }
    }
    #endregion

    #region User test display pseudocode as support
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        // Gather information from instruction
        BubbleSortElement se1 = null, se2 = null;
        int i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE, k = UtilSort.NO_VALUE;

        if (gotSortingElement)
        {
            se1 = GetComponent<ElementManager>().GetSortingElement(((BubbleSortInstruction)instruction).SortingElementID1).GetComponent<BubbleSortElement>();
            se2 = GetComponent<ElementManager>().GetSortingElement(((BubbleSortInstruction)instruction).SortingElementID2).GetComponent<BubbleSortElement>();
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
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
            case UtilSort.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                    lineOfCode.Add(2);
                else if (k == UtilSort.INNER_LOOP)
                    lineOfCode.Add(3);
                break;

            case UtilSort.COMPARE_START_INST:
                lineOfCode.Add(4);
                value1 = se1.Value;
                value2 = se2.Value;

                UtilSort.IndicateElement(se1.gameObject);
                UtilSort.IndicateElement(se2.gameObject);
                break;

            case UtilSort.SWITCH_INST:
                lineOfCode.Add(5);
                useColor = UtilSort.HIGHLIGHT_MOVE_COLOR;
                break;

            case UtilSort.COMPARE_END_INST:
                lineOfCode.Add(6);
                break;

            case UtilSort.END_LOOP_INST:
                if (k == UtilSort.OUTER_LOOP)
                    lineOfCode.Add(8);
                else if (k == UtilSort.INNER_LOOP)
                    lineOfCode.Add(7);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(PseudoCode(lineOfCode[x], i, j), useColor);
        }

        yield return demoStepDuration;
        bubbleSortManager.BeginnerWait = false;
    }
    #endregion

    #region Bubble Sort: Instruction generator (User test / step-by-step)
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instructionNr = 0, i = 0, j = 0;
        // Add first instruction
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.FIRST_INSTRUCTION, instructionNr)); // new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.FIRST_INSTRUCTION, instructionNr, false, false));

        int N = sortingElements.Length; // line 1 
        for (i = 0; i < N; i++)
        {
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instructionNr, i, j, UtilSort.OUTER_LOOP));
            for (j = 0; j < N - i - 1; j++)
            {
                // Update for loop instruction
                instructions.Add(instructionNr++, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instructionNr, i, j, UtilSort.INNER_LOOP)); //new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.UPDATE_LOOP_INST, instructionNr, false, false));

                // Choose sorting elements to compare
                BubbleSortInstruction comparison = MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, UtilSort.COMPARE_START_INST, instructionNr, true, false);

                // Add this instruction
                instructions.Add(instructionNr++, comparison);

                if (comparison.Value1 > comparison.Value2)
                {
                    // Switch their positions
                    instructions.Add(instructionNr++, MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, UtilSort.SWITCH_INST, instructionNr, true, false));

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
                instructions.Add(instructionNr++, MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, UtilSort.COMPARE_END_INST, instructionNr, false, false));
            }
            if (instructions[instructions.Count - 1] is BubbleSortInstruction)
                ((BubbleSortInstruction)instructions[instructions.Count - 1]).IsSorted = true;

            // Update end 2nd for-loop instruction
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.END_LOOP_INST, instructionNr, i, j, UtilSort.INNER_LOOP)); // new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.END_LOOP_INST, instructionNr, false, false));
        }
        //instructions.Add(instructionNr++, new InstructionLoop(Util.UPDATE_LOOP_INST, instructionNr, i, j, Util.OUTER_LOOP)); // new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.UPDATE_LOOP_INST, instructionNr, false, false));

        // Final instruction
        instructions.Add(instructionNr, new InstructionBase(UtilSort.FINAL_INSTRUCTION, instructionNr)); // new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.FINAL_INSTRUCTION, instructionNr, false, false));
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
