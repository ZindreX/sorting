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

    protected override int MovesNeeded
    {
        get { return 1; }
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        MergeSortInstruction mergeSortInstruction = (MergeSortInstruction)instruction;
        Debug.Log("FIX");


        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            //if (instruction.NextHolderID == Util.NO_INSTRUCTION) // skipping until next move // TODO: FIX
            Debug.LogError("FIX");
            return 1;
        }
        return 0;
    }



    public override HolderBase GetCorrectHolder(int index)
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
            elementStates[i] = new MergeSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, isCompare, isSorted);
        }
        return elementStates;
    }

}
