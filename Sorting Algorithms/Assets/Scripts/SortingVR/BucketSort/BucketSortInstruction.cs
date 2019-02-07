using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortInstruction : InstructionSingleElement {

    /* ************************************ Bucket sort instruction ************************************
     * > Inheritages insertion sort incase it'll use insertion sort later in a "phashing" operation
     *  - Now it'll just automatically sort the buckets
     * 
    */

    private int bucketID;

    public BucketSortInstruction(string instruction, int instructionNr, int i, int j, int k, int sortingElementID, int value, bool isCompare, bool isSorted, int holderID, int nextHolderID, int bucketID)
        : base(instruction, instructionNr, i, j, k, sortingElementID, holderID, nextHolderID, value, isCompare, isSorted)
    {
        this.bucketID = bucketID;
    }

    public int BucketID
    {
        get { return bucketID; }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + "| " + holderID + " -> " + UtilSort.TranslateNextHolder(nextHolderID) + " or -> Bucket=" + bucketID;
    }
}
