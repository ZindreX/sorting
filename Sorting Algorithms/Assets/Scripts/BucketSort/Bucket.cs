using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour, IChild {

    /* ---------------------------------------- Bucket object ----------------------------------------
     * bucket index = value * numberOfBuckets / (maxSize + 1)
     * 
     * 
    */


    private static int BUCKET_NR = 0, BUCKET_SIZE = 10; // change size internal based on how many buckets exists? max - max/2 - max/3 etc.

    private int bucketID;
    private GameObject parent;

    private int[] bucketCapacity;
    private List<SortingElementBase> currentHolding;

    void Awake()
    {
        bucketID = BUCKET_NR++;
        bucketCapacity = CreateBucketCapacity();
        currentHolding = new List<SortingElementBase>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public string MyRole()
    {
        return Util.BUCKET_TAG + BUCKET_NR;
    }

    public static int BucketSize(int numberOfBuckets, int maxValue)
    {
        return maxValue / numberOfBuckets;
    }

    // Capacity: itemValue >= start && itemValue < end
    private int[] CreateBucketCapacity()
    {
        int start = bucketID * BUCKET_SIZE;
        int end = bucketID * BUCKET_SIZE + BUCKET_SIZE;
        return new int[2] { start, end };
    }

    private bool ValidateSortingElement(SortingElementBase element)
    {
        return (element.Value >= bucketCapacity[0] && element.Value < bucketCapacity[1]) ? true : false;
    }

    private void AddSortingElementToBucket(SortingElementBase sortingElement)
    {
        if (ValidateSortingElement(sortingElement))
        {
            currentHolding.Add(sortingElement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Util.SORTING_ELEMENT_TAG)
        {
            SortingElementBase sortingElement = collision.collider.GetComponent<SortingElementBase>();

            if (parent.GetComponent<AlgorithmManagerBase>().IsTutorial)
            {
                if (ValidateSortingElement(sortingElement))
                {
                    // Do animation (color -> green -> color)
                }
                else
                {
                    // Can't be put into this bucket
                }

            }
            else // User test
            {

            }
        }
    }






}
