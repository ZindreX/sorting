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
    private GameObject sortingElementPrefab;
    private GameObject[] sortingElements;

    private SortingElementBase currentMoving;
    private bool containsElements = false;


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

    // Creation without rules
    public void CreateObjects(int numberOfElements, Vector3[] positions)
    {
        if (containsElements)
            return;

        sortingElements = new GameObject[numberOfElements]; // Util.CreateObjects(sortingElementPrefab, numberOfElements, positions, gameObject);
        for (int x = 0; x < numberOfElements; x++)
        {
            sortingElements[x] = Instantiate(sortingElementPrefab, positions[x] + Util.ABOVE_HOLDER_VR, Quaternion.identity);
            sortingElements[x].GetComponent<SortingElementBase>().Value = Random.Range(0, Util.MAX_VALUE);
            sortingElements[x].GetComponent<IChild>().Parent = gameObject;
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

        for (int x = 0; x < numberOfElements; x++)
        {
            int newValue = Random.Range(0, Util.MAX_VALUE);
            while (!allowDuplicates && usedValues.Contains(newValue))
            {
                newValue = Random.Range(0, Util.MAX_VALUE);
            }   
            usedValues.Add(newValue);

            sortingElements[x] = Instantiate(sortingElementPrefab, positions[x] + Util.ABOVE_HOLDER_VR, Quaternion.identity);
            sortingElements[x].GetComponent<SortingElementBase>().Value = newValue;
            sortingElements[x].GetComponent<IChild>().Parent = gameObject;
        }

        if (sortingCase != Util.NONE)
            ElementsBasedOnCase(sortingElements, sortingCase);

        // Hotfix (sorting element currentHolding / prevHolding problem)
        for (int x = 0; x < sortingElements.Length; x++)
        {
            sortingElements[x].GetComponent<SortingElementBase>().PlaceManuallySortingElementOn(GetComponent<HolderManager>().Holders[x].GetComponent<HolderBase>());
        }

        containsElements = true;
    }

    // Destroys all elements + reset
    public void DestroyObjects()
    {
        Util.DestroyObjects(sortingElements);
        containsElements = false;
        SortingElementBase.SORTING_ELEMENT_NR = 0;
        usedValues = new HashSet<int>();
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
            case Util.BEST_CASE: values = InsertionSort.InsertionSortFixCase(values, false); break;
            case Util.WORST_CASE: values = InsertionSort.InsertionSortFixCase(values, true); break;
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

}
