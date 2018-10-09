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

    protected override bool PrepareNextInstruction(int instNr)
    {
        // Get the next two instructions
        BubbleSortInstruction bubbleInstruction = (BubbleSortInstruction)userTestManager.GetInstruction(instNr);

        // Find the sorting elements for this instruction
        BubbleSortElement s1 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID1).GetComponent<BubbleSortElement>();
        BubbleSortElement s2 = elementManager.GetSortingElement(bubbleInstruction.SortingElementID2).GetComponent<BubbleSortElement>();
            
        // Hand the instructions out
        s1.ElementInstruction = bubbleInstruction;
        s2.ElementInstruction = bubbleInstruction;

        // Give next move permission (TODO: move to sorting element instead?)
        s1.NextMove = true;
        s2.NextMove = true;

        Debug.Log("Round " + instNr + ": " + userTestManager.GetInstruction(instNr).DebugInfo());

        return SkipOrHelp(bubbleInstruction);
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

    protected override bool SkipOrHelp(InstructionBase instruction)
    {
        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            if (instruction.ElementInstruction == Util.COMPARE_START_INST || instruction.ElementInstruction == Util.COMPARE_END_INST) // skipping until next move // TODO: FIX
                return true;
        }
        return false;
    }
}
