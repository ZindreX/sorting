﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortHolder : HolderBase
{
    private bool isPivotHolder = false;
    private int positionIndex = 1;

    public override string MyRole()
    {
        if (isPivotHolder)
            return "Pivot holder";
        return base.MyRole();
    }

    public bool IsPivotHolder
    {
        get { return isPivotHolder; }
        set { isPivotHolder = value; indexText.text = "Pivot"; }
    }

    public int PositionIndex
    {
        get { return positionIndex; }
        set { positionIndex = value; }
    }

    protected override void UpdateColorOfHolder()
    {
        if (!currentHolding.StandingInCorrectHolder)
        {
            CurrentColor = UtilSort.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().ReportError(currentHolding.SortingElementID);
            hasPermission = false;
        }
        else if (CurrentHolding.IntermediateMove)
            CurrentColor = UtilSort.STANDARD_COLOR;
        else if (IsPivotHolder)
            CurrentColor = UtilSort.PIVOT_COLOR;
        else if (((InsertionSortElement)CurrentHolding).IsPivot)
            CurrentColor = UtilSort.PIVOT_COLOR;
        else if (currentHolding.IsCompare)
            CurrentColor = UtilSort.COMPARE_COLOR;
        else if (currentHolding.IsSorted && !IsPivotHolder)
            CurrentColor = UtilSort.SORTED_COLOR;
        else
            CurrentColor = prevColor;
    }

    // New holder
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // Current holding the sorting element that collided
            currentHolding = other.GetComponent<InsertionSortElement>();

            // Tutorial
            if (parent.GetComponent<SortMain>().SortSettings.IsDemo())
            {
                if (((InsertionSortElement)currentHolding).IsPivot)
                    CurrentColor = UtilSort.PIVOT_COLOR;
                else if (currentHolding.IsCompare)
                    CurrentColor = UtilSort.COMPARE_COLOR;
                else if (currentHolding.IsSorted)
                    CurrentColor = UtilSort.SORTED_COLOR;
            }
            else // User test
            {

            }
        }
    }

    //protected override void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
    //    {
    //        // Current holding the sorting element that collided
    //        currentHolding = collision.collider.GetComponent<InsertionSortElement>();

    //        // Tutorial
    //        if (parent.GetComponent<SortMain>().SortSettings.IsDemo())
    //        {
    //            if (((InsertionSortElement)currentHolding).IsPivot)
    //                CurrentColor = UtilSort.PIVOT_COLOR;
    //            else if (currentHolding.IsCompare)
    //                CurrentColor = UtilSort.COMPARE_COLOR;
    //            else if (currentHolding.IsSorted)
    //                CurrentColor = UtilSort.SORTED_COLOR;
    //        }
    //        else // User test
    //        {

    //        }
    //    }
    //}
}
