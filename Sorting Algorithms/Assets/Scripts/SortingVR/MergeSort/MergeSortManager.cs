using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MergeSort))]
public class MergeSortManager : AlgorithmManagerBase {

    [SerializeField]
    private MergeSort mergeSort;

    public override string AlgorithmManager
    {
        get { return mergeSort.AlgorithmName + " Manager"; }
    }

    public override int MovesNeeded
    {
        get { return 1; }
    }

    public override int PrepareNextInstruction(InstructionBase instruction)
    {
        if (instruction is MergeSortInstruction)
        {
            MergeSortInstruction mergeSortInstruction = (MergeSortInstruction)instruction;

        }
        else if (instruction is MergeSortHolderInstruction)
        {

        }
        else
        {

        }

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
        MergeSortHolder holder = (MergeSortHolder)sortMain.HolderManager.GetHolder(index);
        return (holder != null) ? holder : mergeSort.GetExtraHolder(index).GetComponent<MergeSortHolder>();
    }

    public override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
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
            elementStates[i] = new MergeSortInstruction(Util.INIT_INSTRUCTION, 0, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, sortingElementID, holderID, UtilSort.NO_DESTINATION, value, isCompare, isSorted);
        }
        return elementStates;
    }

}
