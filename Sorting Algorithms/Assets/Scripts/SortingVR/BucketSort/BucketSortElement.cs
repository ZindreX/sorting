using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortElement : SortingElementBase {

    private BucketSortInstruction bucketSortInstruction;
    private bool isPivot; // canEnterBucket = true;
    private int nextBucketID;
    private Bucket currentInside;

    private Rigidbody rb;
    private Vector3 returnPos;

    protected override void Awake()
    {
        base.Awake();
        Instruction = new BucketSortInstruction(UtilSort.INIT_INSTRUCTION, 0, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, sortingElementID, value, false, false, sortingElementID, UtilSort.NO_DESTINATION, UtilSort.NO_VALUE); // new BucketSortInstruction(sortingElementID, sortingElementID, Util.NO_DESTINATION, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, false, false, false);

        //returnPos = 
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }

    private void Update()
    {
        if (parent.AlgorithmInitialized)
        {
            if (currentInside == null && transform.position.y < 0.1f)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                transform.position = FindObjectOfType<SortingTable>().ReturnPosition.position;
                rb.constraints = RigidbodyConstraints.None;
            }
        }
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

    //public bool CanEnterBucket
    //{
    //    get { return canEnterBucket; }
    //    set { canEnterBucket = value; }
    //}

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
                case UtilSort.INIT_INSTRUCTION: status = "Init pos"; break;
                case UtilSort.BUCKET_INDEX_INST:
                    status = "Bucket index: " + nextBucketID;
                    break;

                case UtilSort.MOVE_TO_BUCKET_INST:
                    status = "Move to bucket" + nextBucketID;
                    intermediateMove = true;
                    //canEnterBucket = true; // ***
                    break;
                case UtilSort.MOVE_BACK_INST:
                    status = "Move to holder " + nextHolderID;
                    intermediateMove = true;
                    break;

                case UtilSort.EXECUTED_INST: status = "Performed"; break;

                case UtilSort.DISPLAY_ELEMENT:
                    break;

                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

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

                case UtilSort.MOVE_TO_BUCKET_INST:
                    if (!bucketSortInstruction.HasBeenExecuted())
                    {
                        if (currentStandingOn != null)
                        {
                            // Check if element is standing on holder (before any user action)
                            if (IntermediateMove && currentStandingOn.HolderID == bucketSortInstruction.HolderID)
                                return UtilSort.CORRECT_HOLDER;
                            else if (IntermediateMove && CurrentStandingOn.HolderID == bucketSortInstruction.HolderID) // Mistake, going back to "start"
                                return UtilSort.CORRECT_HOLDER;
                            return UtilSort.WRONG_HOLDER;
                        }
                    }
                    return (currentInside != null && currentInside.BucketID == bucketSortInstruction.BucketID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                case UtilSort.MOVE_BACK_INST:
                    if (!bucketSortInstruction.HasBeenExecuted())
                    {
                        if (IntermediateMove && currentStandingOn != null && CurrentStandingOn.HolderID == bucketSortInstruction.NextHolderID)
                        {
                            intermediateMove = false;
                            return UtilSort.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && currentInside != null && CurrentInside.BucketID == bucketSortInstruction.BucketID)
                            return UtilSort.CORRECT_HOLDER;
                        return UtilSort.WRONG_HOLDER;
                    }
                    else
                    {
                        return (currentStandingOn != null & CurrentStandingOn.HolderID == bucketSortInstruction.NextHolderID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;
                    }

                case UtilSort.DISPLAY_ELEMENT:
                    return UtilSort.WRONG_HOLDER;

                default: Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }


}
