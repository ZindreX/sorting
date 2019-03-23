using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSortElement : SortingElementBase {

    private MergeSortInstruction mergeSortInstruction;


    public override InstructionBase Instruction
    {
        get { return mergeSortInstruction; }
        set { mergeSortInstruction = (MergeSortInstruction)value; UpdateSortingElementState(); }
    }

    protected override string IsCorrectlyPlaced()
    {
        return "Implement this...";
    }

    protected override void UpdateSortingElementState()
    {

    }
}
