using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionDoubleElement : InstructionLoop {

    protected int sortingElementID1, sortingElementID2, holderID1, holderID2, value1, value2;
    protected bool isCompare, isSorted;
    protected bool part1Executed = false, part2Executed = false;

    public InstructionDoubleElement(string instruction, int instructionNr, int i, int j, int k, int seID1, int seID2, int hID1, int hID2, int value1, int value2, bool isCompare, bool isSorted)
    : base(instruction, instructionNr, i, j, k)
    {
        sortingElementID1 = seID1;
        sortingElementID2 = seID2;
        holderID1 = hID1;
        holderID2 = hID2;
        this.value1 = value1;
        this.value2 = value2;
        this.isCompare = isCompare;
        this.isSorted = isSorted;
    }

    public int SortingElementID1
    {
        get { return sortingElementID1; }
    }

    public int SortingElementID2
    {
        get { return sortingElementID2; }
    }

    public int HolderID1
    {
        get { return holderID1; }
        set { holderID1 = value; }
    }

    public int HolderID2
    {
        get { return holderID2; }
    }

    public int Value1
    {
        get { return value1; }
    }

    public int Value2
    {
        get { return value2; }
    }

    public bool IsCompare
    {
        get { return isCompare; }
        set { isCompare = value; }
    }

    public bool IsSorted
    {
        get { return isSorted; }
        set { isSorted = value; }
    }

    // Checks whether the parameter ID is the 1st element in the instruction
    public bool IsMain(int sortingElementID)
    {
        return SortingElementID1 == sortingElementID;
    }

    public int GetValueFor(int sortingElementID, bool other)
    {
        if (other)
            return (sortingElementID == SortingElementID1) ? value2 : value1;
        return (sortingElementID == SortingElementID1) ? value1 : value2;
    }

    public void ElementExecuted(int sortingElementID)
    {
        if (sortingElementID == sortingElementID1)
            part1Executed = true;
        else
            part2Executed = true;
    }

    public bool ElementHasBeenExecuted(int sortingElementID)
    {
        return (sortingElementID == SortingElementID1) ? part1Executed : part2Executed;
    }

    public override bool HasBeenExecuted()
    {
        return part1Executed && part2Executed;
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + "| se1: " + sortingElementID1 + ": [" + value1 + "]" + ", se2: " + sortingElementID2 + ": [" + value2 + "]" + ", C=" + isCompare + ", S=" + isSorted;
    }

}
