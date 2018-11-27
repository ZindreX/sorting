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
        BucketSortInstruction[] elementStates = new BucketSortInstruction[sortingElements.Length];

        for (int i = 0; i < sortingElements.Length; i++)
        {
            BucketSortElement element = sortingElements[i].GetComponent<BucketSortElement>();
            int sortingElementID = element.SortingElementID;
            int holderID = element.CurrentStandingOn.HolderID;
            int value = element.Value;
            bool isPivot = element.IsPivot;
            bool isCompare = element.IsCompare;
            bool isSorted = element.IsSorted;
            elementStates[i] = new BucketSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, isPivot, isCompare, isSorted);
        }
        return elementStates;
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        // Get the next instruction
        BucketSortInstruction bucketSortInstruction = (BucketSortInstruction)instruction;

        if (bucketSortInstruction.Instruction == Util.PHASING_INST)
        {
            // Phase into Insertion Sort (?)
            AutoSort();
            StartCoroutine(PutElementsForDisplay());
        }
        else
        {
            // Get the Sorting element
            BucketSortElement sortingElement = elementManager.GetSortingElement(bucketSortInstruction.SortingElementID).GetComponent<BucketSortElement>();

            // Hands out the next instruction
            sortingElement.Instruction = bucketSortInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            sortingElement.NextMove = true;

            Debug.Log("Round " + userTestManager.CurrentInstructionNr + ": " + bucketSortInstruction.DebugInfo());
        }

        // Display help on blackboard
        if (false) // help enabled
        {

        }
        else
        {
            if (bucketSortInstruction.NextHolderID == Util.NO_DESTINATION && bucketSortInstruction.BucketID == Util.NO_DESTINATION) // skipping until next (user) move
                return 1;
        }
        return 0;
    }


    private void AutoSort()
    {
        for (int x=0; x < numberOfBuckets; x++)
        {
            Bucket bucket = bucketManager.GetBucket(x);
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);
        }
    }

    private IEnumerator PutElementsForDisplay()
    {
        for (int x=0; x < numberOfBuckets; x++)
        {
            Bucket bucket = bucketManager.GetBucket(x);
            bucket.DisplayElements = true;

            int numberOfElements = bucket.CurrenHolding.Count;
            if (numberOfElements > 0)
            {
                for (int y=0; y < numberOfElements; y++)
                {
                    SortingElementBase element = bucket.RemoveSoringElement();
                    element.transform.position = new Vector3(0f, 2f, 0f);
                    yield return new WaitForSeconds(bucketSort.Seconds);
                }
            }
        }
    }

}
