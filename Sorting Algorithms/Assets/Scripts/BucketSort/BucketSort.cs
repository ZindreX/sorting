using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSort : Algorithm {

    private Dictionary<int, string> pseudoCode;

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
        throw new System.NotImplementedException();
    }

    #region Bucket Sort: Standard
    public GameObject[] BucketSortStandard(GameObject[] list, int numberOfBuckets)
    {
        // Create buckets
        BucketStandard[] buckets = new BucketStandard[numberOfBuckets];
        for (int x=0; x < numberOfBuckets; x++)
        {
            buckets[x] = new BucketStandard();
        }

        // Find the correct bucket for each element in the list
        for (int x=0; x <list.Length; x++)
        {
            int bucketIndex = list[x].GetComponent<BucketSortElement>().Value * numberOfBuckets / (Util.MAX_VALUE + 1); // int: floors down
            buckets[bucketIndex].AddObject(list[x]);
        }

        Debug.Log(BucketToString(buckets));

        // Sort each bucket by using insertion sort
        for (int x=0; x < list.Length; x++)
        {
            SortingElementBase[] sortedBucket = InsertionSort.InsertionSortStandard(buckets[x].SortingElements(), false);
        }

        return list;
    }
    #endregion

    private string BucketToString(BucketStandard[] buckets)
    {
        string bucketsCheck = "";
        for (int i = 0; i < buckets.Length; i++)
        {
            string temp = "> " + i + ": ";
            List<GameObject> bucket = buckets[i].Bucket;

            for (int j = 0; j < bucket.Count; j++)
            {
                temp += bucket[j] + ", ";
            }
            bucketsCheck += temp.Substring(0, temp.Length - 2) + "\n";
        }
        return bucketsCheck;
    }

    #region Bucket Sort: Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] list)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Bucket Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}

public class BucketStandard
{
    private List<GameObject> bucket = new List<GameObject>();

    public void AddObject(GameObject obj)
    {
        bucket.Add(obj);
    }

    public List<GameObject> Bucket
    {
        get { return bucket; }
    }

    public SortingElementBase[] SortingElements()
    {
        SortingElementBase[] result = new SortingElementBase[bucket.Count];
        for (int x=0; x < bucket.Count; x++)
        {
            result[x] = bucket[x].GetComponent<SortingElementBase>();
        }
        return result;
    }
}