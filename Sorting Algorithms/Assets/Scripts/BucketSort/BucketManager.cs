using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketManager : MonoBehaviour, IManager {

    [SerializeField]
    private GameObject bucketPrefab;

    [SerializeField]
    private GameObject firstBucketPosition;

    private GameObject[] buckets;
    private bool containsBuckets = false;

    void Awake()
    {
        buckets = new GameObject[GetComponent<BucketSortManager>().NumberOfBuckets];
    }

    public Vector3 FirstBucketPosition
    {
        get { return firstBucketPosition.transform.position; }
    }

    public void CreateObjects(int numberOfElements, Vector3[] position)
    {
        if (containsBuckets)
            return;

        buckets = Util.CreateObjects(bucketPrefab, numberOfElements, position, 1f, gameObject);
        containsBuckets = true;
    }

    public void DestroyObjects()
    {
        Util.DestroyObjects(buckets);
        containsBuckets = false;
        Bucket.BUCKET_NR = 0;
    }

    public GameObject[] Buckets
    {
        get { return buckets; }
    }

    public Bucket GetBucket(int index)
    {
        return buckets[index].GetComponent<Bucket>();
    }


}
