using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSortHolderInstruction : InstructionBase {

    private int mergeHolderID;

    public MergeSortHolderInstruction(string instruction, int instNr, int mergeHolderID) : base(instruction, instNr)
    {
        this.mergeHolderID = mergeHolderID;
    }

    public int MergeHolderID
    {
        get { return mergeHolderID; }
    }


    public override string DebugInfo()
    {
        return base.DebugInfo() + " | Merge holder ID = " + mergeHolderID; 
    }

}
