using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortElement : SortingElementBase {

    private BucketSortInstruction bucketSortInstruction;
    private bool isPivot;
    private int nextBucketID;
    private Bucket currentInside;

    protected override void Awake()
    {
        base.Awake();
        Instruction = new BucketSortInstruction(sortingElementID, sortingElementID, Util.NO_DESTINATION, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, false, false, false);
    }

    public override InstructionBase Instruction
    {
        get { return bucketSortInstruction; }
        set { bucketSortInstruction = (BucketSortInstruction)value; UpdateSortingElementState(); }
    }

    public bool IsPivot
    {
        get { return isPivot; }
        set { isPivot = value; }
    }

    public Bucket CurrentInside
    {
        get { return currentInside; }
        set { currentInside = value; }
    }

    protected override void UpdateSortingElementState()
    {
        if (bucketSortInstruction != null)
        {
            // Debugging
            instruction = bucketSortInstruction.Instruction;
            hID = bucketSortInstruction.HolderID;
            nextHolderID = bucketSortInstruction.NextHolderID;
            nextBucketID = bucketSortInstruction.BucketID;

            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.PIVOT_START_INST: status = "Move to pivot holder"; break;
                case Util.PIVOT_END_INST: status = "Move down from pivot holder"; break;
                case Util.COMPARE_START_INST: status = "Comparing with pivot"; break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextHolderID;
                    intermediateMove = true; // Too easy for the user?
                    break;

                // Bucket sort
                case Util.MOVE_TO_BUCKET_INST:
                    status = "Move to bucket" + nextBucketID;
                    intermediateMove = true;
                    break;
                case Util.MOVE_BACK_INST:
                    status = "Move to holder " + nextHolderID;
                    intermediateMove = true;
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
        if (CanValidate())
        {
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return (currentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                case Util.PIVOT_START_INST:
                    return (currentStandingOn.HolderID == bucketSortInstruction.HolderID || ((BucketSortHolder)currentStandingOn).IsPivotHolder) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.PIVOT_END_INST:
                    return (((BucketSortHolder)currentStandingOn).IsPivotHolder || currentStandingOn.HolderID == bucketSortInstruction.NextHolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.COMPARE_START_INST:
                    break;

                case Util.COMPARE_END_INST:
                    return currentStandingOn.HolderID == bucketSortInstruction.HolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.SWITCH_INST:
                    if (intermediateMove && currentStandingOn.HolderID == bucketSortInstruction.HolderID) //insertionSortInstruction.HolderID == currentStandingOn.HolderID && insertionSortInstruction.NextHolderID != Util.NO_INSTRUCTION && IsSorted)
                    {
                        return Util.CORRECT_HOLDER; //Util.MOVE_INTERMEDIATE;
                    }
                    else if (intermediateMove && bucketSortInstruction.NextHolderID == currentStandingOn.HolderID)
                    {
                        intermediateMove = false;
                        return Util.CORRECT_HOLDER;
                    }
                    else
                        return Util.WRONG_HOLDER;

                case Util.MOVE_TO_BUCKET_INST:
                    return intermediateMove && (currentStandingOn.HolderID == bucketSortInstruction.HolderID || currentInside.BucketID == bucketSortInstruction.BucketID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.MOVE_BACK_INST:
                    return intermediateMove && (currentInside.BucketID == bucketSortInstruction.BucketID || currentStandingOn.HolderID == bucketSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER; // collapse cases?


                case Util.EXECUTED_INST:
                    if (bucketSortInstruction.NextHolderID != Util.NO_DESTINATION)
                        return (currentStandingOn.HolderID == bucketSortInstruction.NextHolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;
                    return (currentStandingOn.HolderID == bucketSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
            }
        }
        return Util.CANNOT_VALIDATE_ERROR;
    }


}
