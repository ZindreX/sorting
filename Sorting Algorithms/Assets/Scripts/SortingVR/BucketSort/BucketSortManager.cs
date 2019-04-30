using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BucketSort))]
//[RequireComponent(typeof(BucketManager))]
public class BucketSortManager : AlgorithmManagerBase {

    [SerializeField]
    private int numberOfBuckets = 10;

    [SerializeField]
    private BucketSort bucketSort;

    [SerializeField]
    private BucketManager bucketManager;

    public override string AlgorithmManager
    {
        get { return bucketSort.AlgorithmName + " Manager"; }
    }

    public int NumberOfBuckets
    {
        get { return numberOfBuckets; }
    }

    public override int MovesNeeded
    {
        get { return 1; }
    }

    public override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
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
            elementStates[i] = new BucketSortInstruction(UtilSort.INIT_INSTRUCTION, 0, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, sortingElementID, value, isCompare, isSorted, holderID, UtilSort.NO_DESTINATION, UtilSort.NO_VALUE); // new BucketSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, isPivot, isCompare, isSorted);
        }
        return elementStates;
    }

    public override int PrepareNextInstruction(InstructionBase instruction)
    {
        Debug.Log(instruction.DebugInfo());

        bool gotSortingElement = !bucketSort.SkipDict[UtilSort.SKIP_NO_ELEMENT].Contains(instruction.Instruction);
        bool noDestination = bucketSort.SkipDict[UtilSort.SKIP_NO_DESTINATION].Contains(instruction.Instruction);

        switch (instruction.Instruction)
        {
            case UtilSort.PHASING_INST:
                // Phase into Insertion Sort?
                bucketManager.AutoSortBuckets();
                break;

            case UtilSort.DISPLAY_ELEMENT:
                Debug.Log("Display elements");

                // Display elements on top of bucket
                sortMain.WaitForSupportToComplete++;
                StartCoroutine(bucketManager.PutElementsForDisplay(((BucketSortInstruction)instruction).BucketID));
                return 1; // Nothing to do for the player, nor any pseudocode
        }

        if (instruction is BucketSortInstruction)
        {
            // Get the next instruction
            BucketSortInstruction bucketSortInstruction = (BucketSortInstruction)instruction;

            // Get the Sorting element
            BucketSortElement sortingElement = sortMain.ElementManager.GetSortingElement(bucketSortInstruction.SortingElementID).GetComponent<BucketSortElement>();

            // Hands out the next instruction
            sortingElement.Instruction = bucketSortInstruction;

            Bucket bucket = bucketManager.GetBucket(bucketSortInstruction.BucketID);
            bucket.BucketSortInstruction = bucketSortInstruction;

            // Give this sorting element permission to give feedback to progress to next intstruction
            if (instruction.Instruction == UtilSort.MOVE_TO_BUCKET_INST || instruction.Instruction == UtilSort.MOVE_BACK_INST)
                sortingElement.NextMove = true;
        }

        // Display help on blackboard
        if (sortMain.SortSettings.Difficulty <= Util.BEGINNER)
        {
            sortMain.WaitForSupportToComplete++;
            StartCoroutine(sortMain.GetTeachingAlgorithm().UserTestHighlightPseudoCode(instruction, gotSortingElement));
        }


        Debug.Log("Element: " + gotSortingElement + ", no destination: " + noDestination);
        if (gotSortingElement && !noDestination)
            return 0;

        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>> Nothing for player to do, continuing to next instruction");
        return 1;
    }

}
