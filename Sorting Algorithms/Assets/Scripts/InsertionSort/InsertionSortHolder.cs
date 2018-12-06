using System.Collections;
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
        set { isPivotHolder = value; }
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
            CurrentColor = Util.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().ReportError(currentHolding.SortingElementID);
            hasPermission = false;
        }
        else if (CurrentHolding.IntermediateMove)
            CurrentColor = Util.STANDARD_COLOR;
        else if (IsPivotHolder)
            CurrentColor = Util.PIVOT_COLOR;
        else if (((InsertionSortElement)CurrentHolding).IsPivot)
            CurrentColor = Util.PIVOT_COLOR;
        else if (currentHolding.IsCompare)
            CurrentColor = Util.COMPARE_COLOR;
        else if (currentHolding.IsSorted && !IsPivotHolder)
            CurrentColor = Util.SORTED_COLOR;
        else
            CurrentColor = prevColor;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Util.SORTING_ELEMENT_TAG)
        {
            // Current holding the sorting element that collided
            currentHolding = collision.collider.GetComponent<InsertionSortElement>();

            // Tutorial
            if (parent.GetComponent<AlgorithmManagerBase>().algorithmSettings.IsTutorial())
            {
                if (((InsertionSortElement)currentHolding).IsPivot)
                    CurrentColor = Util.PIVOT_COLOR;
                else if (currentHolding.IsCompare)
                    CurrentColor = Util.COMPARE_COLOR;
                else if (currentHolding.IsSorted)
                    CurrentColor = Util.SORTED_COLOR;
            }
            else // User test
            {

            }
        }
    }
}
