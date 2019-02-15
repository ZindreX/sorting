﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortHolder : HolderBase {

    protected override void UpdateColorOfHolder() {
        if (!currentHolding.StandingInCorrectHolder)
        {
            CurrentColor = UtilSort.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().ReportError(currentHolding.SortingElementID);
            hasPermission = false;
        }
        //else if (CurrentHolding.IntermediateMove)
        //    CurrentColor = Util.STANDARD_COLOR;
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
            currentHolding = collision.collider.GetComponent<BubbleSortElement>();

            // Tutorial
            if (parent.GetComponent<AlgorithmManagerBase>().AlgorithmSettings.IsDemo())
            {
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