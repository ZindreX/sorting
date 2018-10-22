using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BucketManager))]
[RequireComponent(typeof(BucketSortManager))]
public class BucketSort : Algorithm {

    public const string CHOOSE_BUCKET = "Choose bucket", PUT_BACK_TO_HOLDER = "Put back to holder", NONE = "None";

    private string status = "";

    private Dictionary<int, string> pseudoCode;
    private BucketManager bucketManager;
    private BucketSortManager bucketSortManager;

    protected override void Awake()
    {
        base.Awake();
        bucketManager = GetComponent(typeof(BucketManager)) as BucketManager;
        bucketSortManager = GetComponent(typeof(BucketSortManager)) as BucketSortManager;
        status = NONE;
    }

    public override string GetAlgorithmName()
    {
        return Util.BUCKET_SORT;
    }

    // Change back ???
    public override string CollectLine(int lineNr)
    {
        string temp = PseudoCode(lineNr, 0, true);
        switch (lineNr)
        {
            case 0: case 1: case 5: case 8: return temp;
            case 2: return temp.Replace((GetComponent<AlgorithmManagerBase>().NumberOfElements - 1).ToString(), "(len( list ) - 1)");
            case 3: return temp.Replace((Util.INIT_STATE - 1).ToString(), "index").Replace(Util.INIT_STATE.ToString(), "list[ i ] * n / max_value");
            case 4: return temp.Replace((Util.INIT_STATE - 1).ToString(), "index").Replace(Util.INIT_STATE.ToString(), "list[ i ]");
            case 6: return temp.Replace((bucketSortManager.NumberOfBuckets - 1).ToString(), "n-1");
            case 7: return temp.Replace("0", "i");
            default: return "lineNr " + lineNr + " not found!";
        }
    }

    private string PseudoCode(int lineNr, int i, bool increment)
    {
        switch (lineNr)
        {
            case 0: return "BucketSort( list, n):";
            case 1: return "    buckets = new array of n empty lists";
            case 2: return string.Format("        for i={0} to {1}:", i, (GetComponent<AlgorithmManagerBase>().NumberOfElements - 1));
            case 3: return string.Format("            {0} = {1} * {2} / {3}", value2, value1, bucketSortManager.NumberOfBuckets, Util.MAX_VALUE);
            case 4: return string.Format("            buckets[ {0} ] = {1}", value2, value1);
            case 5: return "        end for";
            case 6: return string.Format("    for i={0} to {1}:", i, (bucketSortManager.NumberOfBuckets - 1));
            case 7: return string.Format("        Sortbucket( buckets[ {0} ] ):", i);
            case 8: return "    end for";
            default: return "X";
        }
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 8;
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
                int bucketID = BucketIndex(value1, GetComponent<BucketSortManager>().NumberOfBuckets);
                return value1 + " into bucket " + bucketID;

            case PUT_BACK_TO_HOLDER:
                return value1 + " to holder " + value2;

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
        return value * numberOfBuckets / Util.MAX_VALUE; // max + 1 ~?
    }

    #endregion

    #region Bucket Sort: Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] sortingElements)
    {
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
            value1 = element.GetComponent<SortingElementBase>().Value;
            Bucket bucket = buckets[BucketIndex(value1, numberOfBuckets)].GetComponent<Bucket>(); // element.GetComponent<SortingElementBase>().Value - minValue);

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
                value1 = sortingElements[k].GetComponent<SortingElementBase>().Value;
                value2 = k;

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
            instructions.Add(instructionNr++, new BucketSortInstruction(element.SortingElementID, element.HolderID, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, bucketIndex, Util.MOVE_TO_BUCKET_INST, element.Value, false, false, false));
        }

        // Move player into insertion sort room, and let them do the sorting or skip ?
        instructions.Add(instructionNr++, new BucketSortInstruction(Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, Util.PHASING_INST, Util.NO_VALUE, false, false, false));

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
                instructions.Add(instructionNr++, new BucketSortInstruction(element.SortingElementID, bucket.BucketID, k++, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.MOVE_BACK_INST, element.Value, false, false, true));
            }
        }
        return instructions;
    }
    #endregion


    public override void ExecuteOrder(InstructionBase instruction, int instructionNr, bool increment)
    {

    }

}
