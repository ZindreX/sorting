using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BucketSort))]
public class BucketSortManager : AlgorithmManagerBase {

    private BucketSort bucketSort;

    protected override void Awake()
    {
        bucketSort = GetComponent(typeof(BucketSort)) as BucketSort;
        base.Awake();

    }

    protected override Algorithm InstanceOfAlgorithm
    {
        get { return bucketSort; }
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        throw new System.NotImplementedException();
    }

    protected override Dictionary<int, string> CreatePseudoCode()
    {
        throw new System.NotImplementedException();
    }

    protected override HolderBase GetCorrectHolder(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override bool PrepareNextInstruction(int instructionNr)
    {
        throw new System.NotImplementedException();
    }

    protected override bool SkipOrHelp(InstructionBase instruction)
    {
        throw new System.NotImplementedException();
    }

}
