using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MergeSort))]
public class MergeSortManager : AlgorithmManagerBase {

    private MergeSort mergeSort;

    protected override void Awake()
    {
        mergeSort = GetComponent(typeof(MergeSort)) as MergeSort;
        base.Awake();
    }

    protected override Algorithm InstanceOfAlgorithm
    {
        get { return mergeSort; }
    }

    protected override bool PrepareNextInstruction(int instructionNr)
    {
        MergeSortInstruction mergeSortInstruction = (MergeSortInstruction)userTestManager.GetInstruction(instructionNr);
        Debug.Log("FIX");


        return SkipOrHelp(mergeSortInstruction);
    }

    protected override bool SkipOrHelp(InstructionBase instruction)
    {
        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            //if (instruction.NextHolderID == Util.NO_INSTRUCTION) // skipping until next move // TODO: FIX
            Debug.LogError("FIX");
            return true;
        }
        return false;
    }

    protected override HolderBase GetCorrectHolder(int index)
    {
        MergeSortHolder holder = (MergeSortHolder)holderManager.GetHolder(index);
        return (holder != null) ? holder : mergeSort.GetExtraHolder(index).GetComponent<MergeSortHolder>();
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        MergeSortInstruction[] elementStates = new MergeSortInstruction[sortingElements.Length];

        for (int i = 0; i < sortingElements.Length; i++)
        {
            MergeSortElement element = sortingElements[i].GetComponent<MergeSortElement>();
            int sortingElementID = element.SortingElementID;
            int holderID = element.CurrentStandingOn.HolderID;
            int value = element.Value;
            bool isCompare = element.IsCompare;
            bool isSorted = element.IsSorted;
            elementStates[i] = new MergeSortInstruction(sortingElementID, holderID, Util.NO_INSTRUCTION, Util.INIT_INSTRUCTION, value, isCompare, isSorted);
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
