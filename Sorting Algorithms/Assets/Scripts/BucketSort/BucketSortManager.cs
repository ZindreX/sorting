using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BucketSort))]
[RequireComponent(typeof(BucketManager))]
public class BucketSortManager : AlgorithmManagerBase {

    [SerializeField]
    private int numberOfBuckets = 10;

    private BucketSort bucketSort;
    private BucketManager bucketManager;

    protected override void Awake()
    {
        bucketSort = GetComponent(typeof(BucketSort)) as BucketSort;
        bucketManager = GetComponent(typeof(BucketManager)) as BucketManager;
        base.Awake();

    }

    public int NumberOfBuckets
    {
        get { return numberOfBuckets; }
    }

    protected override Algorithm InstanceOfAlgorithm
    {
        get { return bucketSort; }
    }

    protected override int MovesNeeded
    {
        get { return 1; }
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        throw new System.NotImplementedException();
    }

    protected override Dictionary<int, string> CreatePseudoCode()
    {
        Dictionary<int, string> pseudoCode = new Dictionary<int, string>();
        return pseudoCode;
    }

    protected override HolderBase GetCorrectHolder(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        throw new System.NotImplementedException();
    }

    protected override int SkipOrHelp(InstructionBase instruction)
    {
        throw new System.NotImplementedException();
    }

}
