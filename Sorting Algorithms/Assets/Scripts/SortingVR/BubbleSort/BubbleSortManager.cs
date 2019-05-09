using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BubbleSort))]
public class BubbleSortManager : SortAlgorithmManager {

    [SerializeField]
    private BubbleSort bubbleSort;

    public override string AlgorithmManager
    {
        get { return bubbleSort.AlgorithmName + " Manager"; }
    }

    public override int MovesNeeded
    {
        get { return 2; }
    }

    public override int PrepareNextInstruction(InstructionBase instruction)
    {
        Debug.Log(instruction.DebugInfo());

        bool gotSortingElement = !bubbleSort.SkipDict[UtilSort.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = bubbleSort.SkipDict[UtilSort.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (gotSortingElement)
        {
            // Get the next two instructions
            BubbleSortInstruction bubbleInstruction = (BubbleSortInstruction)instruction;

            // Find the sorting elements for this instruction
            BubbleSortElement s1 = sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
            BubbleSortElement s2 = sortMain.ElementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            
            // Hand the instructions out
            s1.Instruction = bubbleInstruction;
            s2.Instruction = bubbleInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == UtilSort.COMPARE_START_INST || instruction.Instruction == UtilSort.SWITCH_INST)
            {
                s1.NextMove = true;
                s2.NextMove = true;
            }
        }

        if (sortMain.SortSettings.Difficulty <= Util.BEGINNER)
        {
            sortMain.WaitForSupportToComplete++;
            StartCoroutine(sortMain.GetTeachingAlgorithm().UserTestHighlightPseudoCode(instruction, gotSortingElement));
        }

        if (gotSortingElement && !noDestination)
            return 0;
        return 2;
    }

    public override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        BubbleSortInstruction[] elementStates = new BubbleSortInstruction[sortingElements.Length];

        for (int i = 0; i < sortingElements.Length; i++)
        {
            BubbleSortElement element = sortingElements[i].GetComponent<BubbleSortElement>();
            int sortingElementID = element.SortingElementID;
            int holderID = element.CurrentStandingOn.HolderID;
            int value = element.Value;
            bool isCompare = element.IsCompare;
            bool isSorted = element.IsSorted;
            elementStates[i] = new BubbleSortInstruction(UtilSort.INIT_INSTRUCTION, 0, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, sortingElementID, UtilSort.NO_VALUE, holderID, UtilSort.NO_VALUE, value, UtilSort.NO_VALUE, false, false);
// new BubbleSortInstruction(sortingElementID, Util.NO_DESTINATION, holderID, Util.NO_DESTINATION, value, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, isCompare, isSorted);
        }
        return elementStates;
    }
}
