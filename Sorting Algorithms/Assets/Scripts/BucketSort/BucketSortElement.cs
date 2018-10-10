using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortElement : InsertionSortElement {

    private BucketSortInstruction bucketSortInstruction;
   
    protected override void Awake()
    {
        base.Awake();
        //ElementInstruction = new BucketSortInstruction(sortingElementID, sortingElementID, Util.NO_INSTRUCTION, Util.INIT_INSTRUCTION, value, false, false);)
    }

    public override InstructionBase ElementInstruction
    {
        get { return bucketSortInstruction; }
        set { bucketSortInstruction = (BucketSortInstruction)value; UpdateSortingElementState(); }
    }

    protected override void UpdateSortingElementState()
    {
        if (bucketSortInstruction != null)
        {
            // Debugging
            instruction = bucketSortInstruction.ElementInstruction;
            hID = bucketSortInstruction.HolderID;
            nextID = bucketSortInstruction.NextHolderID;

            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.PIVOT_START_INST: status = "Move to pivot holder"; break;
                case Util.PIVOT_END_INST: status = "Move down from pivot holder"; break;
                case Util.COMPARE_START_INST: status = "Comparing with pivot"; break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextID;
                    intermediateMove = true; // Too easy for the user?
                    break;
                case Util.EXECUTED_INST: status = "Performed"; break;
                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

            if (bucketSortInstruction.IsPivot)
                isPivot = true;
            else
                isPivot = false;

            if (bucketSortInstruction.IsCompare)
                isCompare = true;
            else
                isCompare = false;

            if (bucketSortInstruction.IsSorted)
                isSorted = true;
            else
                isSorted = false;
        }
    }

    protected override string IsCorrectlyPlaced()
    {
        throw new System.NotImplementedException();
    }


}
