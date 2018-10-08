using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private HashSet<int> sortingElementValues = new HashSet<int>(); // No duplicates


    public GameObject[] SortingElements
    {
        get { return sortingElements; }
    }

    public GameObject GetSortingElement(int index)
    {
        //if (sortingElements[index] == null)
        //    return GetComponent<InsertionSort>().PivotHolder.GetComponent<HolderBase>().CurrentHolding; // Pivot element:--> test and remove
        return sortingElements[index];
    }

    // Creation without rules
    public void CreateObjects(int numberOfElements, Vector3[] positions)
    {
        if (containsElements)
            return;

        sortingElements = new GameObject[numberOfElements]; // Util.CreateObjects(sortingElementPrefab, numberOfElements, positions, gameObject);
        for (int x = 0; x < numberOfElements; x++)
        {
            sortingElements[x] = Instantiate(sortingElementPrefab, positions[x] + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
            sortingElements[x].GetComponent<IChild>().Parent = gameObject;
        }

        // Hotfix (sorting element currentHolding / prevHolding problem)
        for (int x = 0; x < sortingElements.Length; x++)
        {
            sortingElements[x].GetComponent<SortingElementBase>().PlaceManuallySortingElementOn(GetComponent<HolderManager>().Holders[x].GetComponent<HolderBase>());
        }
    }

    // Destroys all elements + reset
    public void DestroyObjects()
    {
        Util.DestroyObjects(sortingElements);
        containsElements = false;
        SortingElementBase.SORTING_ELEMENT_NR = 0;
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

    public bool AllSorted()
    {
        foreach (GameObject element in sortingElements)
        {
            if (!element.GetComponent<SortingElementBase>().IsSorted)
                return false;
        }
        return true;
    }


    // OUTDATED -- to be fixed

    /* Creation with rules
     * rules[0]: duplicates etc.
     * rules[1]: worst-/bestcase
    */
    //public void CreateSortingElementsWithRules(int numberOfElements, string[] rules, Vector3[] positions)
    //{
    //    for (int x = 0; x < numberOfElements; x++)
    //    {
    //        if (rules[0] != null)
    //            SortingElements[x] = CreateSortingElementBasedOn(rules[0], positions[x]);
    //    }
    //    if (rules[1] != null)
    //        FixCase(rules[1]);
    //}

    //// No duplicates, etc..
    //private SortingElementBase CreateSortingElementBasedOn(string rule, Vector3 pos)
    //{
    //    GameObject element = Instantiate(sortingElementPrefab, pos, Quaternion.identity);
    //    if (rule.Equals(Util.NO_DUPLICATES))
    //    {
    //        while (sortingElementValues.Contains(element.GetComponent<SortingElementBase>().Value))
    //        {
    //            element.GetComponent<SortingElementBase>().Value = Random.Range(0, Util.MAX_VALUE);
    //        }
    //    }
    //    else if (rule.Equals(Util.ALL_SAME))
    //        element.GetComponent<SortingElementBase>().Value = 1;
    //    return element.GetComponent<SortingElementBase>();
    //}

    //// Worst-/best case...
    //private void FixCase(string rule)
    //{
    //    switch (rule)
    //    {
    //        case Util.WORST_CASE: InsertionSort.InsertionSortStandard(sortingElements, true); break;
    //        case Util.BEST_CASE: InsertionSort.InsertionSortStandard(sortingElements, false); break;
    //        default: Debug.LogError("Case rule '" + rule + "' doesn't exist."); break;
    //    }
    //}

}
