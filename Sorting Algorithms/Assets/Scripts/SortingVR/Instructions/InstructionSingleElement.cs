using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSingleElement : InstructionLoop {

    protected int sortingElementID, holderID, nextHolderID, value;
    protected bool isCompare, isSorted;

    public InstructionSingleElement(string instruction, int instructionNr, int i, int j, int k, int sortingElementID, int holderID, int nextHolderID, int value, bool isCompare, bool isSorted)
        : base(instruction, instructionNr, i, j, k)
    {
        this.sortingElementID = sortingElementID;
        this.value = value;
        this.holderID = holderID;
        this.nextHolderID = nextHolderID;
        this.isCompare = isCompare;
        this.isSorted = isSorted;
    }

    public int SortingElementID
    {
        get { return sortingElementID; }
    }

    public int HolderID
    {
        get { return holderID; }
        set { holderID = value; }
    }

    public int NextHolderID
    {
        get { return nextHolderID; }
        set { nextHolderID = value; }
    }

    public int Value
    {
        get { return value; }
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

    public override string DebugInfo()
    {
        return base.DebugInfo() + "| ElementID: " + sortingElementID + ", [" + value + "]" + ", C=" + isCompare + ", S=" + isSorted;
    }
}
