using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortInstruction : InstructionBase {

    /* -------------------------------------------- Instruction class for user test --------------------------------------------
     * 
     * 
    */

    private int nextHolderID;

    public BubbleSortInstruction(int sortingElementID, int holderID, int nextHolderID, int value, string instruction, bool isCompare, bool isSorted)
        : base(sortingElementID, holderID, value, instruction, isCompare, isSorted)
    {
        this.nextHolderID = nextHolderID;
    }

    public int NextHolderID
    {
        get { return nextHolderID; }
    }

    public override string DebugInfo()
    {
        return "[" + value + "]: " + holderID + " -> " + Util.TranslateNextHolder(nextHolderID) + ", C=" + isCompare + ", S=" + isSorted + ", Inst: " + instruction;
    }
}
