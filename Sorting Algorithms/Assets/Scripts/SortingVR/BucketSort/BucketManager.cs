﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketManager : MonoBehaviour, IManager {

    [SerializeField]
    private GameObject bucketPrefab;

    [SerializeField]
    private Transform firstBucketPosition, firstRowUserTest, secondRowUserTest;

    [SerializeField]
    private GameObject bucketContainer;

    [SerializeField]
    private BucketSort bucketSort;

    private GameObject[] buckets;
    private bool containsBuckets = false;

    private SortMain superElement;

    private WaitForSeconds emptyBucketDuration = new WaitForSeconds(0.5f);

    public void InitManager()
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
            buckets = Util.CreateObjects(bucketPrefab, numberOfElements, position, UtilSort.SPACE_BETWEEN_BUCKETS, superElement);
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
                bucket.transform.parent = bucketContainer.transform;
            }
        }
        containsBuckets = true;
    }

    public GameObject[] Buckets
    {
        get { return buckets; }
    }

    public Bucket GetBucket(int index)
    {
        if (index >= 0 && index < buckets.Length)
            return buckets[index].GetComponent<Bucket>();
        return null;
    }

    public void AutoSortBuckets()
    {
        for (int x = 0; x < buckets.Length; x++)
        {
            Bucket bucket = GetBucket(x);
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);
        }
    }

    //
    public IEnumerator PutElementsForDisplay(int bucketID)
    {
        //sortMain.UpdateCheckList(UtilSort.ALGORITHM_MANAGER, false);

        Bucket bucket = GetBucket(bucketID);
        bucket.SetEnterTrigger(false);
        int numberOfElements = bucket.CurrenHolding.Count;

        if (numberOfElements > 0)
        {
            StartCoroutine(bucket.Animation(Bucket.HIGHLIGHT, 2));
            yield return emptyBucketDuration;

            for (int y = 0; y < numberOfElements; y++)
            {
                SortingElementBase element = bucket.GetElementForDisplay(y);
                element.transform.position = bucket.transform.position + UtilSort.ABOVE_BUCKET_VR + (UtilSort.ABOVE_BUCKET_VR / 4) * y;
                element.transform.rotation = Quaternion.identity;
                element.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                //element.transform.position = new Vector3(0f, 2f, 0f);
                yield return emptyBucketDuration;
                element.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            bucket.Empty();
        }

        //sortMain.UpdateCheckList(UtilSort.ALGORITHM_MANAGER, true);
        superElement.WaitForSupportToComplete--;
    }

    public void ResetSetup()
    {
        DestroyAndReset();
    }

    public void DestroyAndReset()
    {
        UtilSort.DestroyObjects(buckets);
        containsBuckets = false;
        Bucket.BUCKET_NR = 0;
    }


}
