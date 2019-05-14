using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class Bucket : MonoBehaviour, ISortSubElement {

    /* ---------------------------------------- Bucket object ----------------------------------------
     * bucket index = value * numberOfBuckets / (maxSize + 1)
     * 
     * 
    */

    public static int BUCKET_NR = 0, BUCKET_SIZE = 10; // change size internal based on how many buckets exists? max - max/2 - max/3 etc.

    // Animations
    public const string ENTER = "Enter", ERROR = "Error", HIGHLIGHT = "Highlight";

    [SerializeField]
    private MeshRenderer innerBucket;

    [SerializeField]
    private TextMeshPro bucketIndexText, bucketCapacityText;

    [SerializeField]
    private Transform dropElementForDisplayPos;

    private SphereCollider enterTrigger;
    private CapsuleCollider onTopOfBucketTrigger;

    private Animator animator;

    private int bucketID;
    private BucketSortInstruction instruction;

    private SortMain parent;

    private int[] bucketCapacity;
    private List<SortingElementBase> currentHolding;
    private WaitForSeconds colorChangeDuration = new WaitForSeconds(UtilSort.COLOR_CHANGE_TIMER);

    void Awake()
    {
        bucketID = BUCKET_NR++;
        currentHolding = new List<SortingElementBase>();

        // Bucket index text
        bucketIndexText.text = bucketID.ToString();

        enterTrigger = GetComponent<SphereCollider>();
        onTopOfBucketTrigger = GetComponent<CapsuleCollider>();
        SetEnterTrigger(true);

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        BUCKET_SIZE = parent.ElementManager.MaxValue / ((BucketSortManager)parent.AlgorithmManagerBase).NumberOfBuckets;
        bucketCapacity = CreateBucketCapacity();

        // Bucket capacity text
        bucketCapacityText.text = "Values:\n" + bucketCapacity[0] + " - " + (bucketCapacity[1] - 1);
    }

    public SortMain SuperElement
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

    public BucketSortInstruction BucketSortInstruction
    {
        get { return instruction; }
        set { instruction = value; }
    }

    public List<SortingElementBase> CurrenHolding
    {
        get { return currentHolding; }
        set { currentHolding = value; }
    }

    // Trigger for entering bucket / on top of check
    public void SetEnterTrigger(bool active)
    {
        enterTrigger.enabled = active;
        onTopOfBucketTrigger.enabled = !active;
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
        if (currentHolding.Contains(sortingElement))
            return;

        Debug.Log("Adding element: " + sortingElement.SortingElementID);
        currentHolding.Add(sortingElement);

        if (sortingElement is BucketSortElement)
        {
            BucketSortElement bucketSortElement = (BucketSortElement)sortingElement;
            bucketSortElement.CurrentInside = this;
            StartCoroutine(Animation(ENTER, 3));

            // Make invisible
            bucketSortElement.transform.position = transform.position;
            bucketSortElement.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public SortingElementBase GetElementForDisplay(int index)
    {
        if (index >= 0 && index < currentHolding.Count)
            return currentHolding[index];
        return null;
    }

    public void Empty()
    {
        currentHolding = new List<SortingElementBase>();
    }

    // Not used
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

    public void RemoveSortingElement(SortingElementBase sortingElement)
    {
        if (currentHolding.Contains(sortingElement))
        {
            currentHolding.Remove(sortingElement);

            if (prevSortingElementID == sortingElement.SortingElementID)
                prevSortingElementID = -1;
        }
    }

    public IEnumerator Animation(string animation, int times)
    {
        for (int i=0; i < times; i++)
        {
            animator.SetBool(animation, true);
            yield return colorChangeDuration;
        }

        animator.SetBool(animation, false);
    }

    private int prevSortingElementID = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            BucketSortElement sortingElement = other.GetComponent<BucketSortElement>();

            // Check for bug(same sorting element got added twice)
            //if (prevSortingElementID == sortingElement.SortingElementID)
            //    return;

            if (enterTrigger.enabled)
            {
                if (parent.SortSettings.IsDemo())
                {
                    if (ValidateSortingElement(sortingElement)) // !displayElements && 
                    {
                        // Do animation (color -> green -> color)
                        AddSortingElementToBucket(sortingElement);
                        prevSortingElementID = sortingElement.SortingElementID;
                    }
                    else
                    {
                        // Can't be put into this bucket
                        StartCoroutine(Animation(ERROR, 2));
                    }

                }
                else // User test
                {
                    if (sortingElement.Instruction is BucketSortInstruction)
                    {
                        BucketSortInstruction inst = (BucketSortInstruction)sortingElement.Instruction;

                        if (instruction == sortingElement.Instruction && instruction.Instruction == UtilSort.MOVE_TO_BUCKET_INST) // inst.Instruction == UtilSort.MOVE_TO_BUCKET_INST && inst.BucketID == bucketID
                        {
                            AddSortingElementToBucket(sortingElement);
                            prevSortingElementID = sortingElement.SortingElementID;

                            // Score
                            parent.GetComponent<UserTestManager>().IncrementTotalCorrect();
                            
                            // Progress user test
                            parent.GetComponent<UserTestManager>().ReadyForNext += 1;
                        }
                        else if (ValidateSortingElement(sortingElement))
                        {

                        }
                        else
                        {
                            // Can't be put into this bucket
                            StartCoroutine(Animation(ERROR, 2));
                            parent.GetComponent<UserTestManager>().Mistake();
                        }
                    }

                }
            }
            else if (onTopOfBucketTrigger.enabled)
            {

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            BucketSortElement sortingElement = other.GetComponent<BucketSortElement>();

            if (onTopOfBucketTrigger.enabled)
            {
                sortingElement.CurrentInside = null;
                RemoveSortingElement(sortingElement);
                sortingElement.RigidBody.constraints = RigidbodyConstraints.None;
                prevSortingElementID = -1;
            }
        }
    }

    public void PutElementForDisplay(BucketSortElement sortingElement)
    {
        Debug.Log("Putting element " + sortingElement.SortingElementID + " for display");

        // Make sure it's still considered inside the bucket
        sortingElement.CurrentInside = this;

        // Fix position and rotation
        sortingElement.transform.position = dropElementForDisplayPos.position;
        sortingElement.transform.rotation = Quaternion.identity;

        // Enable Y-axis movement
        sortingElement.RigidBody.constraints = ~RigidbodyConstraints.FreezePositionY;

        // Disable all constraints after some seconds
        //StartCoroutine(RemoveConstraintsDelay(sortingElement));
    }



    private IEnumerator RemoveConstraintsDelay(BucketSortElement element)
    {
        yield return colorChangeDuration;
        element.RigidBody.constraints = RigidbodyConstraints.None;

    }





}
