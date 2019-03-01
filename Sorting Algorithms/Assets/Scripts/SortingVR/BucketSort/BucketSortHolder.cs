using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortHolder : InsertionSortHolder {

    protected override void UpdateColorOfHolder()
    {
        if (!currentHolding.StandingInCorrectHolder)
        {
            CurrentColor = UtilSort.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().ReportError(currentHolding.SortingElementID);
        }
        //else if (CurrentHolding.IntermediateMove)
        //    CurrentColor = Util.TEST_COLOR;
        else if (currentHolding.IsCompare)
            CurrentColor = UtilSort.COMPARE_COLOR;
        else if (currentHolding.IsSorted)
            CurrentColor = UtilSort.SORTED_COLOR;
        else
            CurrentColor = prevColor;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // Current holding the sorting element that collided
            BucketSortElement element = collision.collider.GetComponent<BucketSortElement>();
            currentHolding = element;

            // Tutorial
            if (parent.GetComponent<SortMain>().SortSettings.IsDemo())
            {
                element.CanEnterBucket = true;

                if (currentHolding.IsCompare)
                    CurrentColor = UtilSort.COMPARE_COLOR;
                if (currentHolding.IsSorted)
                    CurrentColor = UtilSort.SORTED_COLOR;
            }
            else // User test
            {

            }
        }
    }

}
