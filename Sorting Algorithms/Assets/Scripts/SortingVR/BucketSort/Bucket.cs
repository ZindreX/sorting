using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour, IChild {

    /* ---------------------------------------- Bucket object ----------------------------------------
     * bucket index = value * numberOfBuckets / (maxSize + 1)
     * 
     * 
    */

    public static int BUCKET_NR = 0, BUCKET_SIZE = 10; // change size internal based on how many buckets exists? max - max/2 - max/3 etc.

    [SerializeField]
    private MeshRenderer innerBucket;

    [SerializeField]
    private TextMesh text;

    private int bucketID;
    private bool displayElements = false;
    private GameObject parent;

    private int[] bucketCapacity;
    private List<SortingElementBase> currentHolding;

    void Awake()
    {
        bucketID = BUCKET_NR++;
        bucketCapacity = CreateBucketCapacity();
        currentHolding = new List<SortingElementBase>();
        text.text = bucketID.ToString();
        displayElements = false;
    }

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public int BucketID
    {
        get { return bucketID; }
    }

    public string MyRole()
    {
        return UtilSort.BUCKET_TAG + BUCKET_NR;
    }

    public List<SortingElementBase> CurrenHolding
    {
        get { return currentHolding; }
        set { currentHolding = value; }
    }

    public bool DisplayElements
    {
        set { displayElements = value; }
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

    public void AddSortingElementToBucket(SortingElementBase sortingElement)
    {
        currentHolding.Add(sortingElement);
        StartCoroutine(ChangeColor(UtilSort.SORTED_COLOR));

        // Make invisible
        sortingElement.gameObject.SetActive(false);
    }

    public SortingElementBase GetElementForDisplay(int index)
    {
        if (index < currentHolding.Count)
        {
            return currentHolding[index];
        }
        return null;
    }

    public SortingElementBase RemoveSoringElement()
    {
        if (currentHolding.Count > 0)
        {
            SortingElementBase element = currentHolding[0];
            currentHolding.RemoveAt(0);
            return element;
        }
        return null;
    }

    public IEnumerator ChangeColor(Color color)
    {
        Color prevColor = innerBucket.material.color;
        innerBucket.material.color = color;
        yield return new WaitForSeconds(UtilSort.COLOR_CHANGE_TIMER);
        innerBucket.material.color = prevColor;
        yield return new WaitForSeconds(UtilSort.COLOR_CHANGE_TIMER);
        innerBucket.material.color = color;
        yield return new WaitForSeconds(UtilSort.COLOR_CHANGE_TIMER);
        innerBucket.material.color = prevColor;
    }

    private int prevSortingElementID = -1;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            BucketSortElement sortingElement = collision.collider.GetComponent<BucketSortElement>();

            // Check for bug (same sorting element got added twice)
            if (prevSortingElementID == sortingElement.SortingElementID)
                return;

            if (!sortingElement.CanEnterBucket)
            {
                Debug.Log(sortingElement.CanEnterBucket);
                return;
            }

            if (parent.GetComponent<AlgorithmManagerBase>().AlgorithmSettings.IsDemo())
            {
                if (!displayElements && ValidateSortingElement(sortingElement))
                {
                    // Do animation (color -> green -> color)
                    AddSortingElementToBucket(sortingElement);
                    prevSortingElementID = sortingElement.SortingElementID;
                }
                else
                {
                    // Can't be put into this bucket
                    ChangeColor(UtilSort.ERROR_COLOR);
                }

            }
            else // User test
            {

            }
        }
    }






}
