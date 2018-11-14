using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BubbleSortManager))]
public class BubbleSort : Algorithm {

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
        get { return Util.BUBBLE_SORT; }
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FINAL_INSTRUCTION);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.COMPARE_START_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.COMPARE_END_INST);
    }

    public override string CollectLine(int lineNr)
    {
        string temp = PseudoCode(lineNr, 0, 0, true);
        switch (lineNr)
        {
            case 0: case 6: case 7: case 8: return temp;
            case 1: return temp.Replace(GetComponent<AlgorithmManagerBase>().NumberOfElements.ToString(), "len( list )");
            case 2: return temp.Replace((GetComponent<AlgorithmManagerBase>().NumberOfElements - 1).ToString(), "n-1");
            case 3: return temp.Replace((GetComponent<AlgorithmManagerBase>().NumberOfElements - 1).ToString(), "n-i-1");
            case 4: case 5: return temp.Replace(Util.INIT_STATE.ToString(), "list[ j ]").Replace((Util.INIT_STATE - 1).ToString(), "list[ j + 1 ]");
            default: return "lineNr " + lineNr + " not found!";
        }
    }

    private string PseudoCode(int lineNr, int i, int j, bool increment)
    {
        int n = GetComponent<AlgorithmManagerBase>().NumberOfElements;
        switch (lineNr)
        {
            case 0: return "BubbleSort( list ):"; // add case for this line(?): BubbleSort(4, 1, 10, ... etc.)
            case 1: return string.Format("  n = {0}", n); //"n = " + n;
            case 2: return string.Format("  for i={0} to {1}:", i, (n-1)); //"for i=" + i + " to " + (n - 1) + ":";
            case 3: return string.Format("      for j={0} to {1}:", j, (n-i-1)); // "    for j=" + j + " to " + (n - i - 1) + ":";
            case 4: return string.Format("          if ( {0} > {1} ):", value1, value2); //"          if ( " + value1 + " > " + value2 + " ):";
            case 5: return string.Format("              swap {0} and {1}", value1, value2); //"            swap " + value1 + " and " + value2;
            case 6: return "          end if";
            case 7: return "      end for";
            case 8: return "  end for";
            default: return "lineNr " + lineNr + " not found!";
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

    #region Bubble Sort: All Moves Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] list)
    {
        int N = list.Length, i = 0, j = 0;

        // Display pseudocode (list length)
        pseudoCodeViewer.SetCodeLine(1, PseudoCode(1, i, j, true), Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(1, PseudoCode(1, i, j, true), Util.BLACKBOARD_TEXT_COLOR);

        for (i=0; i < N; i++)
        {
            for (j = 0; j < N - i - 1; j++)
            {
                // Display pseudocode (update for-loops)
                pseudoCodeViewer.SetCodeLine(2, PseudoCode(2, i, j, true), Util.HIGHLIGHT_COLOR);
                pseudoCodeViewer.SetCodeLine(3, PseudoCode(3, i, j, true), Util.HIGHLIGHT_COLOR);
                yield return new WaitForSeconds(seconds);
                pseudoCodeViewer.SetCodeLine(2, PseudoCode(2, i, j, true), Util.BLACKBOARD_TEXT_COLOR);
                pseudoCodeViewer.SetCodeLine(3, PseudoCode(3, i, j, true), Util.BLACKBOARD_TEXT_COLOR);

                // Choose sorting elements to compare
                BubbleSortElement p1 = list[j].GetComponent<BubbleSortElement>();
                BubbleSortElement p2 = list[j + 1].GetComponent<BubbleSortElement>();

                // Change status
                p1.IsCompare = true;
                p2.IsCompare = true;

                // Update color on holders
                Util.IndicateElement(p1.gameObject);
                Util.IndicateElement(p2.gameObject);

                // Get their values
                value1 = p1.Value;
                value2 = p2.Value;

                // Display pseudocode (list length)
                pseudoCodeViewer.SetCodeLine(4, PseudoCode(4, i, j, true), Util.HIGHLIGHT_COLOR);
                yield return new WaitForSeconds(seconds);
                pseudoCodeViewer.SetCodeLine(4, PseudoCode(4, i, j, true), Util.BLACKBOARD_TEXT_COLOR);

                if (value1 > value2)
                {
                    // Switch their positions
                    GameObject temp = list[j];
                    GameObject temp2 = list[j + 1];
                    //Vector3 pos1 = list[j].transform.position, pos2 = list[j + 1].transform.position; // p1 & p2 old positions
                    //p1.transform.position = pos2;
                    //list[j] = temp2;

                    //p2.transform.position = pos1;
                    //list[j + 1] = temp;

                    // Switch their positions (added some stuff such that the elements get back in position)
                    Vector3 pos1 = list[j].GetComponent<SortingElementBase>().PlacementAboveHolder, pos2 = list[j + 1].GetComponent<SortingElementBase>().PlacementAboveHolder;
                    p1.transform.position = pos2;
                    list[j] = temp2;
                    p2.transform.position = pos1;
                    list[j + 1] = temp;
                    Util.ResetRotation(temp);
                    Util.ResetRotation(temp2);

                    // Display pseudocode (swap)
                    pseudoCodeViewer.SetCodeLine(5, PseudoCode(5, i, j, true), Util.HIGHLIGHT_COLOR);
                    yield return new WaitForSeconds(seconds);
                    pseudoCodeViewer.SetCodeLine(5, PseudoCode(5, i, j, true), Util.BLACKBOARD_TEXT_COLOR);
                }
                // Display pseudocode (comparison/if end)
                pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, i, j, true), Util.HIGHLIGHT_COLOR);
                yield return new WaitForSeconds(seconds);
                pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, i, j, true), Util.BLACKBOARD_TEXT_COLOR);

                p1.IsCompare = false;
                if (p1.CurrentStandingOn != null)
                    p1.CurrentStandingOn.CurrentColor = Util.STANDARD_COLOR;
                p2.IsCompare = false;
                if (p2.CurrentStandingOn != null)
                    p2.CurrentStandingOn.CurrentColor = Util.STANDARD_COLOR;
            }
            // Display pseudocode (end 2nd for-loop)
            pseudoCodeViewer.SetCodeLine(7, PseudoCode(7, i, j, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(7, PseudoCode(7, i, j, true), Util.BLACKBOARD_TEXT_COLOR);

            list[N - i - 1].GetComponent<BubbleSortElement>().IsSorted = true;
            Util.IndicateElement(list[N - i - 1]); //list[N - i - 1].transform.position += Util.ABOVE_HOLDER_VR;
            yield return new WaitForSeconds(seconds);
        }
        isSortingComplete = true;

        // Display pseudocode (end 1st for-loop)
        pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, i, j, true), Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, i, j, true), Util.BLACKBOARD_TEXT_COLOR);
    }
    #endregion

    #region Bubble Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instructionNr = 0, i = 0, j = 0;
        // Add first instruction
        instructions.Add(instructionNr++, new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.FIRST_INSTRUCTION, false, false));

        int N = sortingElements.Length; // line 1 
        for (i = 0; i < N; i++)
        {
            for (j = 0; j < N - i - 1; j++)
            {
                // Update for loop instruction
                instructions.Add(instructionNr++, new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.UPDATE_LOOP_INST, false, false));

                // Choose sorting elements to compare
                BubbleSortInstruction comparison = MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, Util.COMPARE_START_INST, true, false);

                // Add this instruction
                instructions.Add(instructionNr++, comparison);

                if (comparison.Value1 > comparison.Value2)
                {
                    // Switch their positions
                    instructions.Add(instructionNr++, MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, Util.SWITCH_INST, true, false));

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
                instructions.Add(instructionNr++, MakeInstruction(sortingElements[j], sortingElements[j + 1], i, j, Util.COMPARE_END_INST, false, false));
            }
            //sortingElements[N - i - 1].IsSorted = true;
            instructions[instructions.Count - 1].IsSorted = true;

            // Update end 2nd for-loop instruction
            instructions.Add(instructionNr++, new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.END_LOOP_INST, false, false));

        }
        // Final instruction
        instructions.Add(instructionNr, new BubbleSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, i, j, Util.FINAL_INSTRUCTION, false, false));
        return instructions;
    }
    #endregion

    #region Execute order from user
    public override void ExecuteStepByStepOrder(InstructionBase instruction, int instructionNr, bool increment)
    {
        // Gather information from instruction
        BubbleSortInstruction inst = (BubbleSortInstruction)instruction;
        Debug.Log("Debug: " + inst.DebugInfo() + "\n");

        // Change internal state of following sorting elements
        BubbleSortElement se1 = null, se2 = null;
        if (instruction.Instruction != Util.UPDATE_LOOP_INST && instruction.Instruction != Util.END_LOOP_INST)
        {
            se1 = GetComponent<ElementManager>().GetSortingElement(inst.SortingElementID1).GetComponent<BubbleSortElement>();
            se2 = GetComponent<ElementManager>().GetSortingElement(inst.SortingElementID2).GetComponent<BubbleSortElement>();
        }

        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], Util.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        int i = inst.I, j = inst.J;
        List<int> lineOfCode = new List<int>();
        switch (inst.Instruction)
        {
            case Util.UPDATE_LOOP_INST:
                lineOfCode.Add(2);
                lineOfCode.Add(3);
                break;

            case Util.COMPARE_START_INST:
                if (increment)
                {
                    se1.IsCompare = inst.IsCompare;
                    se2.IsCompare = inst.IsCompare;
                }
                else
                {
                    se1.IsCompare = !inst.IsCompare;
                    se2.IsCompare = !inst.IsCompare;
                }

                lineOfCode.Add(4);
                value1 = se1.Value;
                value2 = se2.Value;
                break;

            case Util.SWITCH_INST:
                if (increment)
                {
                    se1.IsCompare = inst.IsCompare;
                    se2.IsCompare = inst.IsCompare;
                    se1.IsSorted = inst.IsSorted;
                    se2.IsSorted = inst.IsSorted;
                }
                else
                {
                    se1.IsCompare = !inst.IsCompare;
                    se2.IsCompare = !inst.IsCompare;
                    se1.IsSorted = !inst.IsSorted;
                    se2.IsSorted = !inst.IsSorted;
                }

                lineOfCode.Add(5);
                break;

            case Util.COMPARE_END_INST:
                if (increment)
                {
                    se1.IsCompare = inst.IsCompare;
                    se2.IsCompare = inst.IsCompare;
                    if (inst.IsSorted)
                        se2.IsSorted = inst.IsSorted; // ***
                }
                else
                {
                    se1.IsCompare = !inst.IsCompare;
                    se2.IsCompare = !inst.IsCompare;
                    if (inst.IsSorted)
                        se2.IsSorted = !inst.IsSorted;
                }

                lineOfCode.Add(6);
                break;

            case Util.END_LOOP_INST:
                lineOfCode.Add(7);
                break;

        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(lineOfCode[x], PseudoCode(lineOfCode[x], i, j, increment), Util.HIGHLIGHT_COLOR);
        }

        // Move sorting element
        switch (inst.Instruction)
        {
            case Util.COMPARE_START_INST:
                if (se1.CurrentStandingOn != null && se2.CurrentStandingOn != null)
                {
                    se1.CurrentStandingOn.CurrentColor = Util.COMPARE_COLOR;
                    se2.CurrentStandingOn.CurrentColor = Util.COMPARE_COLOR;
                }
                else
                    Debug.Log("Too fast!?");
                break;

            case Util.SWITCH_INST:
                if (increment)
                {
                    se1.transform.position = bubbleSortManager.GetCorrectHolder(inst.HolderID2).transform.position + Util.ABOVE_HOLDER_VR;
                    se2.transform.position = bubbleSortManager.GetCorrectHolder(inst.HolderID1).transform.position + Util.ABOVE_HOLDER_VR;
                }
                else
                {
                    se1.transform.position = bubbleSortManager.GetCorrectHolder(inst.HolderID1).transform.position + Util.ABOVE_HOLDER_VR;
                    se2.transform.position = bubbleSortManager.GetCorrectHolder(inst.HolderID2).transform.position + Util.ABOVE_HOLDER_VR;
                }
                break;

            case Util.COMPARE_END_INST:
                if (se1.CurrentStandingOn != null && se2.CurrentStandingOn != null)
                {
                    se1.CurrentStandingOn.CurrentColor = Util.STANDARD_COLOR;
                    if (!se2.IsSorted)
                        se2.CurrentStandingOn.CurrentColor = Util.STANDARD_COLOR;
                    else
                        se2.CurrentStandingOn.CurrentColor = Util.SORTED_COLOR;
                }
                else
                    Debug.Log("Too fast!?");
                break;
        }
    }
    #endregion

    #region User test display pseudocode as support
    public override IEnumerator UserTestDisplayHelp(InstructionBase instruction, bool gotSortingElement)
    {
        // Gather information from instruction
        BubbleSortElement se1 = null, se2 = null;
        if (gotSortingElement)
        {
            se1 = GetComponent<ElementManager>().GetSortingElement(((BubbleSortInstruction)instruction).SortingElementID1).GetComponent<BubbleSortElement>();
            se2 = GetComponent<ElementManager>().GetSortingElement(((BubbleSortInstruction)instruction).SortingElementID2).GetComponent<BubbleSortElement>();
        }

        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], Util.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        int i = instruction.I, j = instruction.J;
        List<int> lineOfCode = new List<int>(); // change back to int var? no need for list, or change pseudocode?
        switch (instruction.Instruction)
        {
            case Util.UPDATE_LOOP_INST:
                lineOfCode.Add(2);
                lineOfCode.Add(3);
                break;

            case Util.COMPARE_START_INST:
                lineOfCode.Add(4);
                value1 = se1.Value;
                value2 = se2.Value;

                Util.IndicateElement(se1.gameObject);
                Util.IndicateElement(se2.gameObject);
                break;

            case Util.SWITCH_INST:
                lineOfCode.Add(5);
                break;

            case Util.COMPARE_END_INST:
                lineOfCode.Add(6);
                break;

            case Util.END_LOOP_INST:
                lineOfCode.Add(7);
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(lineOfCode[x], PseudoCode(lineOfCode[x], i, j, true), Util.HIGHLIGHT_COLOR);
        }

        yield return new WaitForSeconds(seconds);
        bubbleSortManager.BeginnerWait = false;
    }
    #endregion

    // Help functions

    private BubbleSortInstruction MakeInstruction(InstructionBase inst1, InstructionBase inst2, int i, int j, string instruction, bool isCompare, bool isSorted)
    {
        int seID1 = ((BubbleSortInstruction)inst1).SortingElementID1;
        int seID2 = ((BubbleSortInstruction)inst2).SortingElementID1;
        int hID1 = ((BubbleSortInstruction)inst1).HolderID1;
        int hID2 = ((BubbleSortInstruction)inst2).HolderID1;
        int value1 = ((BubbleSortInstruction)inst1).Value1;
        int value2 = ((BubbleSortInstruction)inst2).Value1;
        return new BubbleSortInstruction(seID1, seID2, hID1, hID2, value1, value2, i, j, instruction, isCompare, isSorted);
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
