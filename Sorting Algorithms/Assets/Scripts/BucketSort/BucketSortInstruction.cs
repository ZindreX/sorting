using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortInstruction : InsertionSortInstruction {

    /* ************************************ Bucket sort instruction ************************************
     * > Inheritages insertion sort incase it'll use insertion sort later in a "phashing" operation
     *  - Now it'll just automatically sort the buckets
     * 
    */


    private int bucketID;

    public BucketSortInstruction(int sortingElementID, int holderID, int nextHolderID, int i, int j, int bucketID, string instruction, int instructionNr, int value, bool isPivot, bool isCompare, bool isSorted)
        : base(sortingElementID, holderID, nextHolderID, i, j, instruction, instructionNr, value, isPivot, isCompare, isSorted)
    {
        this.bucketID = bucketID;
    }

    public int BucketID
    {
        get { return bucketID; }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + ": Bucket=" + bucketID;
    }
}
