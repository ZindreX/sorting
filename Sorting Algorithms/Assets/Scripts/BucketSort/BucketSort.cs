using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BucketManager))]
public class BucketSort : Algorithm {

    private Dictionary<int, string> pseudoCode;

    private BucketManager bucketManager;

    void Awake()
    {
        bucketManager = GetComponent(typeof(BucketManager)) as BucketManager;
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

    // Not complete, insertion sort freezes
    #region Bucket Sort: Standard 2
    public static GameObject[] BucketSortStandard2(GameObject[] sortingElements, int numberOfBuckets)
    {
        // Find number of elements per bucket
        int[] numberOfElementsPerBucket = new int[numberOfBuckets];
        for (int i=0; i < sortingElements.Length; i++)
        {
            numberOfElementsPerBucket[BucketIndex(sortingElements[i], numberOfBuckets)] += 1;
        }

        // Create empty lists (buckets)
        Dictionary<int, GameObject[]> buckets = new Dictionary<int, GameObject[]>();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            buckets[i] = new GameObject[numberOfElementsPerBucket[i]];
        }

        // Add elements to buckets
        int[] counters = numberOfElementsPerBucket;
        for (int i = 0; i < sortingElements.Length; i++)
        {
            int index = BucketIndex(sortingElements[i], numberOfBuckets);
            int placeIn = numberOfElementsPerBucket[index] - counters[index];
            counters[index]--;
            buckets[index][placeIn] = sortingElements[i];
        }

        // Sort each bucket by using Insertion Sort
        for (int i=0; i < buckets.Count; i++)
        {
            buckets[i] = InsertionSort.InsertionSortStandard(buckets[i], false);
        }

        // Put elements back into list
        int k = 0;
        for (int i = 0; i < buckets.Count; i++)
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

    private static int BucketIndex(GameObject obj, int numberOfBuckets)
    {
        return obj.GetComponent<SortingElementBase>().Value * numberOfBuckets / Util.MAX_VALUE;
    }

    #endregion


    #region Bucket Sort: Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] sortingElements)
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

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        bucketManager.CreateObjects(GetComponent<BucketSortManager>().NumberOfBuckets, pos);
        yield return new WaitForSeconds(seconds);

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            // Get element
            GameObject element = sortingElements[i];

            // Get bucket
            Bucket bucket = bucketManager.GetBucket(element.GetComponent<SortingElementBase>().Value - minValue);

            // Move element above the bucket and put it inside
            element.transform.position = bucket.transform.position + new Vector3(0f, 2f, 0f);
            yield return new WaitForSeconds(seconds);
        }

        // Put elements back into list
        int k = 0;
        GameObject[] buckets = bucketManager.Buckets;
        Vector3[] holderPos = GetComponent<HolderManager>().GetHolderPositions();
        for (int i = 0; i < buckets.Length; i++)
        {
            List<SortingElementBase> bucketElements = buckets[i].GetComponent<Bucket>().CurrenHolding;
            if (bucketElements.Count > 0)
            {
                for (int j = 0; j < bucketElements.Count; j++)
                {
                    sortingElements[k] = bucketElements[j].gameObject;
                    sortingElements[k].transform.position = holderPos[k] + new Vector3(0f, 2f, 0f);
                    k++;
                    yield return new WaitForSeconds(seconds);
                }
            }
        }
    }
    #endregion

    #region Bucket Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
