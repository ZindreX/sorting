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

    private HashSet<int> sortingElementValues = new HashSet<int>(); // No duplicates


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
            sortingElements[x].GetComponent<IChild>().Parent = gameObject;
        }

        for (int x = 0; x < sortingElements.Length; x++)
        {
            // Hotfix (sorting element currentHolding / prevHolding problem)
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

    public void InteractionWithSortingElements(bool enable)
    {
        foreach (GameObject obj in SortingElements)
        {
            obj.GetComponent<Interactable>().enabled = enable;
        }
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
