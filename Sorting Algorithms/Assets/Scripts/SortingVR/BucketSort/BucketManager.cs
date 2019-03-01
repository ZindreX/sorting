using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketManager : MonoBehaviour, IManager {

    [SerializeField]
    private GameObject bucketPrefab;

    [SerializeField]
    private Transform firstBucketPosition, firstRowUserTest, secondRowUserTest;

    private GameObject[] buckets;
    private bool containsBuckets = false;

    private SortMain superElement;

    void Awake()
    {
        buckets = new GameObject[GetComponent<BucketSortManager>().NumberOfBuckets];
        superElement = GetComponentInParent<SortMain>();
    }

    public Vector3 FirstBucketPosition
    {
        get { return firstBucketPosition.position; }
    }

    public Transform UserTestBucketRowPosition(int rowId)
    {
        if (rowId == 1)
            return firstRowUserTest;
        else if (rowId == 2)
            return secondRowUserTest;
        return null;
    }

    public void CreateObjects(int numberOfElements, Vector3[] position)
    {
        if (containsBuckets)
            return;

        if (!superElement.SortSettings.IsUserTest())
        {
            buckets = UtilSort.CreateObjects(bucketPrefab, numberOfElements, position, UtilSort.SPACE_BETWEEN_BUCKETS, superElement);
        }
        else
        {
            buckets = new GameObject[numberOfElements];
            GameObject bucket;
            for (int x = 0; x < numberOfElements; x++)
            {
                if (x < (numberOfElements/2))
                {
                    bucket = Instantiate(bucketPrefab, firstRowUserTest.position + new Vector3(0f, 0f, x * UtilSort.SPACE_BETWEEN_BUCKETS), Quaternion.identity);
                    bucket.transform.Rotate(0f, -90f, 0f);
                    bucket.GetComponent<ISortSubElement>().SuperElement = superElement;
                    buckets[x] = bucket;
                }
                else
                {
                    bucket = Instantiate(bucketPrefab, secondRowUserTest.position + new Vector3(0f, 0f, (x - (numberOfElements/2)) * UtilSort.SPACE_BETWEEN_BUCKETS), Quaternion.identity);
                    bucket.GetComponent<ISortSubElement>().SuperElement = superElement;
                    bucket.transform.Rotate(0f, 90f, 0f);
                    buckets[x] = bucket;
                }
            }
        }



        containsBuckets = true;
    }

    public void ResetSetup()
    {
        DestroyObjects();
    }

    public void DestroyObjects()
    {
        UtilSort.DestroyObjects(buckets);
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

    public void AutoSortBuckets()
    {
        for (int x = 0; x < buckets.Length; x++)
        {
            Bucket bucket = GetBucket(x);
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);
        }
    }

    public IEnumerator PutElementsForDisplay(int bucketID)
    {
        Bucket bucket = GetBucket(bucketID);
        bucket.DisplayElements = true;

        int numberOfElements = bucket.CurrenHolding.Count;
        if (numberOfElements > 0)
        {
            for (int y = 0; y < numberOfElements; y++)
            {
                BucketSortElement element = (BucketSortElement)bucket.RemoveSoringElement();
                element.CanEnterBucket = false;
                
                element.transform.position = new Vector3(0f, 2f, 0f);
                yield return GetComponent<SortAlgorithm>().DemoStepDuration;
            }
        }
    }


}
