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
        bool gotSortingElement = !bucketSort.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = bucketSort.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        if (instruction.Instruction == Util.PHASING_INST)
        {
            // Phase into Insertion Sort?
            AutoSortBuckets();
        }
        else if (instruction.Instruction == Util.DISPLAY_ELEMENT)
        {
            // Display elements on top of bucket
            StartCoroutine(PutElementsForDisplay(instruction.I));
        }
        else if (gotSortingElement)
        {
            // Get the next instruction
            BucketSortInstruction bucketSortInstruction = (BucketSortInstruction)instruction;

            // Get the Sorting element
            InsertionSortElement sortingElement = elementManager.GetSortingElement(bucketSortInstruction.SortingElementID).GetComponent<InsertionSortElement>();

            // Hands out the next instruction
            sortingElement.Instruction = bucketSortInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == Util.MOVE_TO_BUCKET_INST || instruction.Instruction == Util.MOVE_BACK_INST)//(instruction.Instruction == Util.PIVOT_START_INST || instruction.Instruction == Util.PIVOT_END_INST || instruction.Instruction == Util.SWITCH_INST)
                sortingElement.NextMove = true;
        }

        // Display help on blackboard
        if (Difficulty <= Util.BEGINNER)
        {
            BeginnerWait = true;
            StartCoroutine(algorithm.UserTestDisplayHelp(instruction, gotSortingElement));
        }
        if (gotSortingElement && !noDestination)
            return 0;
        return 1;
    }

    // copied to BucketManager
    private void AutoSortBuckets()
    {
        for (int x=0; x < numberOfBuckets; x++)
        {
            Bucket bucket = bucketManager.GetBucket(x);
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);
        }
    }

    // copied **
    public IEnumerator PutElementsForDisplay(int bucketID)
    {
        Bucket bucket = bucketManager.GetBucket(bucketID);
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
