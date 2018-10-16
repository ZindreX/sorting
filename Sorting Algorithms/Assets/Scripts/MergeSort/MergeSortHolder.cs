using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSortHolder : HolderBase {

    protected override void UpdateColorOfHolder() // copied from insertionsort+/-: fix
    {
        if (!currentHolding.StandingInCorrectHolder)
        {
            CurrentColor = Util.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().IncrementError();
            hasPermission = false;
            Debug.Log("Correctly given error?");
        }
        else if (CurrentHolding.IntermediateMove)
            CurrentColor = Util.TEST_COLOR;
        else if (currentHolding.IsCompare)
            CurrentColor = Util.COMPARE_COLOR;
        else if (currentHolding.IsSorted)
            CurrentColor = Util.SORTED_COLOR;
        else
            CurrentColor = prevColor;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Util.SORTING_ELEMENT_TAG)
        {
            // Current holding the sorting element that collided
            currentHolding = collision.collider.GetComponent<MergeSortElement>();

            // Tutorial
            if (parent.GetComponent<AlgorithmManagerBase>().IsTutorial())
            {
                if (currentHolding.IsSorted)
                    CurrentColor = Util.SORTED_COLOR;
            }
            else // User test
            {

            }
        }
    }
}
