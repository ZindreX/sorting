using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BubbleSort))]
public class BubbleSortManager : AlgorithmManagerBase {

    private BubbleSort bubbleSort;

    protected override void Awake()
    {
        bubbleSort = GetComponent(typeof(BubbleSort)) as BubbleSort;
        base.Awake();
    }

    protected override Algorithm InstanceOfAlgorithm
    {
        get { return bubbleSort; }
    }

    protected override int MovesNeeded
    {
        get { return 2; }
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        // Get the next two instructions
        BubbleSortInstruction bubbleInstruction = (BubbleSortInstruction)instruction;

        // Find the sorting elements for this instruction
        BubbleSortElement s1 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
        BubbleSortElement s2 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            
        // Hand the instructions out
        s1.ElementInstruction = bubbleInstruction;
        s2.ElementInstruction = bubbleInstruction;

        // Give next move permission
        s1.NextMove = true;
        s2.NextMove = true;

        Debug.Log("Round " + userTestManager.CurrentInstructionNr + ": " + bubbleInstruction.DebugInfo());

        return SkipOrHelp(bubbleInstruction);
    }

    protected override int SkipOrHelp(InstructionBase instruction)
    {
        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            if (instruction.ElementInstruction == Util.COMPARE_START_INST || instruction.ElementInstruction == Util.COMPARE_END_INST) // skipping until next user move
                return 2;
        }
        return 0;
    }

    protected override HolderBase GetCorrectHolder(int index) // todo: override only in insertion sort etc?
    {
        return holderManager.GetHolder(index);
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
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
            elementStates[i] = new BubbleSortInstruction(sortingElementID, Util.NO_INSTRUCTION, holderID, Util.NO_INSTRUCTION, value, Util.NO_INSTRUCTION, Util.INIT_INSTRUCTION, isCompare, isSorted);
        }
        return elementStates;
    }

    protected override Dictionary<int, string> CreatePseudoCode()
    {
        Dictionary<int, string> pseudoCode = new Dictionary<int, string>();
        // TODO: fill in

        return pseudoCode;
    }

}
