using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortInstruction : InsertionSortInstruction {

    private int bucketID;

    public BucketSortInstruction(int sortingElementID, int holderID, int nextHolderID, int bucketID, string instruction, int value, bool isPivot, bool isCompare, bool isSorted)
        : base(sortingElementID, holderID, nextHolderID, instruction, value, isPivot, isCompare, isSorted)
    {
        this.bucketID = bucketID;
    }

    public override string DebugInfo()
    {
        throw new System.NotImplementedException();
    }
}
