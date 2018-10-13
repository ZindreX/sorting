using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortInstructionSingle : InstructionBase {

    /* -------------------------------------------- Instruction class for user test --------------------------------------------
     * 
     * 
    */

    private int sortingElementID, holderID, nextHolderID, value;

    public BubbleSortInstructionSingle(int sortingElementID, int holderID, int nextHolderID, int value, string instruction, bool isCompare, bool isSorted)
        : base(instruction, isCompare, isSorted)
    {
        this.sortingElementID = sortingElementID;
        this.holderID = holderID;
        this.nextHolderID = nextHolderID;
        this.value = value;
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
    }

    public int Value
    {
        get { return value; }
    }

    public override string DebugInfo()
    {
        switch (instruction)
        {
            case Util.COMPARE_START_INST: return "";
            case Util.SWITCH_INST: return "";
            case Util.COMPARE_END_INST: return "";
            default: return instruction + " has no case.";
        }
    }
}
