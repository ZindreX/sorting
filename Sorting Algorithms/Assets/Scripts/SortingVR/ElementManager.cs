using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ElementManager : MonoBehaviour, IManager {

    /* -------------------------------------------- Manage all sorting elements --------------------------------------------
     * 
     * 
    */

    [SerializeField]
    private GameObject sortingElementPrefab, sortingTableElementsObj;
    private GameObject[] sortingElements;

    private int minValue, maxValue;

    private SortingElementBase currentMoving;
    private bool containsElements = false;
    private SortMain superElement;

    public void InitManager()
    {
        superElement = GetComponent<SortMain>();
    }

    public void SetMinMax(int min, int max)
    {
        minValue = min;
        maxValue = max;
    }

    public int MinValue
    {
        get { return minValue; }
    }

    public int MaxValue
    {
        get { return maxValue; }
    }

    public GameObject[] SortingElements
    {
        get { return sortingElements; }
    }

    public GameObject GetSortingElement(int index)
    {
        if (index >= 0 && index < sortingElements.Length)
            return sortingElements[index];
        return null;
    }

    public int GenerateValue()
    {
        return Random.Range(minValue, maxValue);
    }

    // Creation without rules
    public void CreateObjects(int numberOfElements, Vector3[] positions) // NOT USED?
    {
        if (containsElements)
            return;

        Debug.Log("OBS!!!!! Fix back to random here <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        int val = 0;
        sortingElements = new GameObject[numberOfElements]; // Util.CreateObjects(sortingElementPrefab, numberOfElements, positions, gameObject);
        for (int x = 0; x < numberOfElements; x++)
        {
            sortingElements[x] = Instantiate(sortingElementPrefab, positions[x] + UtilSort.ABOVE_HOLDER_VR, Quaternion.identity);
            switch (superElement.GetTeachingAlgorithm().AlgorithmName)
            {
                case Util.BUBBLE_SORT: sortingElements[x].AddComponent<BubbleSortElement>(); break;
                case Util.INSERTION_SORT: sortingElements[x].AddComponent<InsertionSortElement>(); break;
                case Util.BUCKET_SORT: sortingElements[x].AddComponent<BucketSortElement>(); break;
                case Util.MERGE_SORT: sortingElements[x].AddComponent<MergeSortElement>(); break;
                default: Debug.LogError("Add subclass for sorting element!"); break;
            }
            sortingElements[x].GetComponent<SortingElementBase>().Value = val++;//Random.Range(0, UtilSort.MAX_VALUE);
            sortingElements[x].GetComponent<ISortSubElement>().SuperElement = superElement;
            sortingElements[x].transform.parent = sortingTableElementsObj.transform;

        }

        for (int x = 0; x < sortingElements.Length; x++)
        {
            // Hotfix (sorting element currentHolding / prevHolding problem)
            sortingElements[x].GetComponent<SortingElementBase>().PlaceManuallySortingElementOn(GetComponent<HolderManager>().Holders[x].GetComponent<HolderBase>());
        }

        containsElements = true;
    }

    // Creation with rules
    private HashSet<int> usedValues = new HashSet<int>();
    public void CreateObjects(int numberOfElements, Vector3[] positions, bool allowDuplicates, string sortingCase)
    {
        if (containsElements)
            return;

        sortingElements = new GameObject[numberOfElements];

        //Debug.Log("OBS!!!!! Fix back to random here <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        //int val = 8;

        for (int x = 0; x < numberOfElements; x++)
        {
            int newValue = GenerateValue();
            while (!allowDuplicates && usedValues.Contains(newValue))
            {
                newValue = GenerateValue();
            }   
            usedValues.Add(newValue);

            sortingElements[x] = Instantiate(sortingElementPrefab, positions[x] + UtilSort.ABOVE_HOLDER_VR, Quaternion.identity);
            switch (superElement.GetTeachingAlgorithm().AlgorithmName)
            {
                case Util.BUBBLE_SORT: sortingElements[x].AddComponent<BubbleSortElement>(); break;
                case Util.INSERTION_SORT: sortingElements[x].AddComponent<InsertionSortElement>(); break;
                case Util.BUCKET_SORT: sortingElements[x].AddComponent<BucketSortElement>(); break;
                case Util.MERGE_SORT: sortingElements[x].AddComponent<MergeSortElement>(); break;
                default: Debug.LogError("Add subclass for sorting element!"); break;
            }
            sortingElements[x].GetComponent<SortingElementBase>().Value = newValue;
            sortingElements[x].GetComponent<ISortSubElement>().SuperElement = superElement;
            sortingElements[x].transform.parent = sortingTableElementsObj.transform;
        }

        if (sortingCase != UtilSort.NONE)
            ElementsBasedOnCase(sortingElements, sortingCase);

        // Hotfix (sorting element currentHolding / prevHolding problem)
        for (int x = 0; x < sortingElements.Length; x++)
        {
            HolderBase holder = GetComponent<HolderManager>().Holders[x].GetComponent<HolderBase>();
            sortingElements[x].GetComponent<SortingElementBase>().PlaceManuallySortingElementOn(holder);
        }

        containsElements = true;
    }

    // Destroys all elements + reset
    public void DestroyAndReset()
    {
        UtilSort.DestroyObjects(sortingElements);
        containsElements = false;
        SortingElementBase.SORTING_ELEMENT_NR = 0;
        usedValues = new HashSet<int>();
        currentMoving = null;
        superElement = null;
    }

    public SortingElementBase CurrentMoving
    {
        get { return currentMoving; }
    }

    public void NotifyMovingElement(SortingElementBase currentMoving, bool moving)
    {
        if (moving)
            this.currentMoving = currentMoving;
        else
            this.currentMoving = null;
    }

    // Check if all the soring elements have been sorted
    public bool AllSorted()
    {
        foreach (GameObject element in sortingElements)
        {
            if (!element.GetComponent<SortingElementBase>().IsSorted)
                return false;
        }
        return true;
    }

    /* Creates a specific sorting case as an exercise for the user
     * - Since the elements have already been instantiated -> sorting values and hands them out again
    */
    public void ElementsBasedOnCase(GameObject[] sortingElements, string sortingCase)
    {
        // First gathers the values of the elements (which has been instantiated)
        int[] values = new int[sortingElements.Length];
        for (int x=0; x < sortingElements.Length; x++)
        {
            values[x] = sortingElements[x].GetComponent<SortingElementBase>().Value;
        }

        // Sort these values (using insertion sort)
        switch (sortingCase)
        {
            case UtilSort.BEST_CASE: values = InsertionSort.InsertionSortFixCase(values, false); break;
            case UtilSort.WORST_CASE: values = InsertionSort.InsertionSortFixCase(values, true); break;
            default: Debug.LogError("Case '" + sortingCase + "' not added."); break;
        }

        // Change the values of the instantiated elements
        for (int x=0; x < sortingElements.Length; x++)
        {
            sortingElements[x].GetComponent<SortingElementBase>().Value = values[x];
        }
    }

    // Change whether the user can interact with the sorting elements
    public void InteractionWithSortingElements(bool enable)
    {
        foreach (GameObject obj in SortingElements)
        {
            //obj.GetComponent<Interactable>().enabled = enable;
            //obj.GetComponent<Throwable>().enabled = enable;
            //obj.GetComponent<VelocityEstimator>().enabled = enable;
        }
    }

    // Returns a string with the initial list values, e.g.: [13, 4, 2, ..., 43]
    public string InitList()
    {
        string result = "[";

        foreach (GameObject obj in sortingElements)
        {
            SortingElementBase element = obj.GetComponent<SortingElementBase>();
            result += element.Value + ", ";
        }
        return result.Substring(0, result.Length - 2) + "]";
    }

}
