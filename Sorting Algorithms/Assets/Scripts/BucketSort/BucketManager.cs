using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketManager : MonoBehaviour, IManager {

    [SerializeField]
    private GameObject bucketPrefab;

    [SerializeField]
    private GameObject firstBucketPosition;

    private GameObject[] buckets;

    void Awake()
    {
        buckets = new GameObject[GetComponent<BucketSortManager>().NumberOfBuckets];
    }

    public Vector3 FirstBucketPosition
    {
        get { return firstBucketPosition.transform.position; }
    }

    public void CreateObjects(int numberOfElements, Vector3[] positions)
    {
        buckets = Util.CreateObjects(bucketPrefab, numberOfElements, positions, gameObject);
    }

    public void DestroyObjects()
    {
        Util.DestroyObjects(buckets);
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
