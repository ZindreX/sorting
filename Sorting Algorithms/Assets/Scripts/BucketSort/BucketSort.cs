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
    public static GameObject[] BucketSortStandard(GameObject[] list, int numberOfBuckets)
    {
        Bucket[] buckets = new Bucket[numberOfBuckets];
        for (int x=0; x < numberOfBuckets; x++)
        {
            
        }

        for (int x=0; x < list.Length; x++)
        {
            
        }



        return list;
    }
    #endregion

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
