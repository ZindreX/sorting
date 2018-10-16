using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BucketManager))]
public class BucketSort : Algorithm {

    public const string CHOOSE_BUCKET = "Choose bucket", PUT_BACK_TO_HOLDER = "Put back to holder", NONE = "None";

    private string status = "";

    private Dictionary<int, string> pseudoCode;
    private BucketManager bucketManager;

    void Awake()
    {
        bucketManager = GetComponent(typeof(BucketManager)) as BucketManager;
        status = NONE;
    }

    public override Dictionary<int, string> PseudoCode
    {
        get { return pseudoCode; }
        set { pseudoCode = value; }
    }

    public override string GetAlgorithmName()
    {
        return Util.BUCKET_SORT;
    }

    public override void ResetSetup()
    {
        Debug.Log("Nothing to reset?");
    }

    // For visuals on the blackboard
    public override string GetComparison()
    {
        switch (status)
        {
            case CHOOSE_BUCKET:
                int bucketID = BucketIndex(pivotValue, GetComponent<BucketSortManager>().NumberOfBuckets);
                return pivotValue + " into bucket " + bucketID;

            case PUT_BACK_TO_HOLDER:
                return pivotValue + " to holder " + compareValue;

            case NONE: return "";
            default: return "Nothing";
        }

    }

    #region Bucket Sort: Standard 1
    public static GameObject[] BucketSortStandard(GameObject[] sortingElements, int numberOfBuckets)
    {
        // Find min-/ max values
        int minValue = Util.MAX_VALUE, maxValue = 0;
        for (int i = 0; i < sortingElements.Length; i++)
        {
            int value = sortingElements[i].GetComponent<BucketSortElement>().Value;
            if (value > maxValue)
                maxValue = value;
            if (value < minValue)
                minValue = value;
        }

        // Create empty lists (buckets)
        List<GameObject>[] bucket = new List<GameObject>[maxValue - minValue + 1];
        for (int i = 0; i < bucket.Length; i++)
        {
            bucket[i] = new List<GameObject>();
        }

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            GameObject temp = sortingElements[i];
            bucket[temp.GetComponent<BucketSortElement>().Value - minValue].Add(temp);
        }

        // Put elements back into list
        int k = 0;
        for (int i = 0; i < bucket.Length; i++)
        {
            if (bucket[i].Count > 0)
            {
                for (int j = 0; j < bucket[i].Count; j++)
                {
                    sortingElements[k] = bucket[i][j];
                    k++;
                }
            }
        }
        return sortingElements;
    }
    #endregion

    #region Bucket Sort: Standard 2
    public static GameObject[] BucketSortStandard2(GameObject[] sortingElements, int numberOfBuckets)
    {
        // Find number of elements per bucket, + counter for later use
        int[] numberOfElementsPerBucket = new int[numberOfBuckets], counters = new int[numberOfBuckets];
        for (int i=0; i < sortingElements.Length; i++)
        {
            int index = BucketIndex(sortingElements[i].GetComponent<SortingElementBase>().Value, numberOfBuckets);
            numberOfElementsPerBucket[index] += 1;
            counters[index] += 1;
        }

        // Create empty lists (buckets)
        Dictionary<int, GameObject[]> buckets = new Dictionary<int, GameObject[]>();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            buckets[i] = new GameObject[numberOfElementsPerBucket[i]];
        }

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            int index = BucketIndex(sortingElements[i].GetComponent<SortingElementBase>().Value, numberOfBuckets);
            int placeIn = numberOfElementsPerBucket[index] - counters[index];
            counters[index]--;
            buckets[index][placeIn] = sortingElements[i];
        }

        // Sort each bucket by using Insertion Sort
        for (int i=0; i < numberOfBuckets; i++)
        {
            if (numberOfElementsPerBucket[i] > 0)
                buckets[i] = InsertionSort.InsertionSortStandard(buckets[i]);
        }

        // Put elements back into list
        int k = 0;
        for (int i = 0; i < numberOfBuckets; i++)
        {
            if (buckets[i].Length > 0)
            {
                for (int j = 0; j < buckets[i].Length; j++)
                {
                    sortingElements[k] = buckets[i][j];
                    k++;
                }
            }
        }
        return sortingElements;
    }

    private static int BucketIndex(int value, int numberOfBuckets)
    {
        return value * numberOfBuckets / Util.MAX_VALUE;
    }

    #endregion

    #region Bucket Sort: Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] sortingElements)
    {
        // Find min-/ max values
        //int minValue = Util.MAX_VALUE, maxValue = 0;
        //for (int i = 0; i < sortingElements.Length; i++)
        //{
        //    int value = sortingElements[i].GetComponent<BucketSortElement>().Value;
        //    if (value > maxValue)
        //        maxValue = value;
        //    if (value < minValue)
        //        minValue = value;
        //}

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = GetComponent<BucketSortManager>().NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);
        yield return new WaitForSeconds(seconds);

        // Buckets
        GameObject[] buckets = bucketManager.Buckets;

        status = CHOOSE_BUCKET;
        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            // Get element
            GameObject element = sortingElements[i];

            // Get bucket
            pivotValue = element.GetComponent<SortingElementBase>().Value;
            Bucket bucket = buckets[BucketIndex(pivotValue, numberOfBuckets)].GetComponent<Bucket>(); // element.GetComponent<SortingElementBase>().Value - minValue);

            // Move element above the bucket and put it inside
            element.transform.position = bucket.transform.position + new Vector3(0f, 2f, 0f);
            yield return new WaitForSeconds(seconds);
        }

        status = NONE;

        // Display elements
        for (int x=0; x < numberOfBuckets; x++)
        {
            Bucket bucket = buckets[x].GetComponent<Bucket>();
            bucket.DisplayElements = true;
            // Sort bucket *** TODO: go to insertion sort scene
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);

            int numberOfElementsInBucket = bucket.CurrenHolding.Count;
            for (int y=0; y < numberOfElementsInBucket; y++)
            {
                SortingElementBase element = bucket.GetElementForDisplay(y);
                element.gameObject.active = true;
                element.transform.position += new Vector3(0f, 2f, 0f);
                yield return new WaitForSeconds(seconds);
            }
        }

        // Put elements back into list
        status = PUT_BACK_TO_HOLDER;
        int k = 0;
        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = GetComponent<HolderManager>().GetHolderPositions();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            Bucket bucket = buckets[i].GetComponent<Bucket>();
            int numberOfElementsInBucket = bucket.CurrenHolding.Count;
            for (int j = 0; j < numberOfElementsInBucket; j++)
            {
                sortingElements[k] = bucket.RemoveSoringElement().gameObject;
                
                // Display on blackboard
                pivotValue = sortingElements[k].GetComponent<SortingElementBase>().Value;
                compareValue = k;

                sortingElements[k].transform.position = holderPos[k] + new Vector3(0f, 2f, 0f);
                sortingElements[k].GetComponent<SortingElementBase>().IsSorted = true;
                k++;
                yield return new WaitForSeconds(seconds);
            }
        }
        status = NONE;
        IsSortingComplete = true;
    }
    #endregion


    #region Bucket Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instructionNr = 0;

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = GetComponent<BucketSortManager>().NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);

        // Buckets
        GameObject[] buckets = bucketManager.Buckets;

        // Move sorting elements to the correct bucket instructions
        for (int i = 0; i < sortingElements.Length; i++)
        {
            // Get element
            BucketSortInstruction element = (BucketSortInstruction)sortingElements[i];

            // Get bucket
            int bucketIndex = BucketIndex(element.Value, numberOfBuckets);

            // Move element above the bucket and put it inside
            instructions.Add(instructionNr++, new BucketSortInstruction(element.SortingElementID, element.HolderID, Util.NO_DESTINATION, bucketIndex, Util.MOVE_TO_BUCKET_INST, element.Value, false, false, false));
        }

        // Move player into insertion sort room, and let them do the sorting or skip ?
        instructions.Add(instructionNr++, new BucketSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.PHASING_INST, Util.NO_VALUE, false, false, false));

        // Put elements back into list
        int k = 0;
        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = GetComponent<HolderManager>().GetHolderPositions();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            Bucket bucket = buckets[i].GetComponent<Bucket>();
            int numberOfElementsInBucket = bucket.CurrenHolding.Count;
            for (int j = 0; j < numberOfElementsInBucket; j++)
            {
                BucketSortElement element = (BucketSortElement)bucket.GetElementForDisplay(j);
                instructions.Add(instructionNr++, new BucketSortInstruction(element.SortingElementID, bucket.BucketID, k++, Util.NO_DESTINATION, Util.MOVE_BACK_INST, element.Value, false, false, true));
            }
        }
        return instructions;
    }
    #endregion
}
