using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSortHolder : HolderBase {

    private int holdingNumberOfElements;
    private bool isSplitHolder;

    private void Awake()
    {
        holdingNumberOfElements = 0;
    }

    private void Update()
    {
        // raycast to check how many elements?

    }

    public bool IsSplitHolder
    {
        get { return isSplitHolder; }
        set { isSplitHolder = value; }
    }

    public int HoldingNumberOfElements
    {
        get { return holdingNumberOfElements; }
        set { holdingNumberOfElements = value; }
    }

    public Vector3 NextElementPos()
    {
        float yPos = holdingNumberOfElements * 0.2f;
        return new Vector3(0f, yPos, 0f);
    }


    protected override void UpdateColorOfHolder() // copied from insertionsort+/-: fix
    {
        if (!currentHolding.StandingInCorrectHolder)
        {
            CurrentColor = UtilSort.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().ReportError(currentHolding.SortingElementID);
            hasPermission = false;
            Debug.Log("Correctly given error?");
        }
        else if (CurrentHolding.IntermediateMove)
            CurrentColor = UtilSort.TEST_COLOR;
        else if (currentHolding.IsCompare)
            CurrentColor = UtilSort.COMPARE_COLOR;
        else if (currentHolding.IsSorted)
            CurrentColor = UtilSort.SORTED_COLOR;
        else
            CurrentColor = prevColor;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // Current holding the sorting element that collided
            currentHolding = collision.collider.GetComponent<MergeSortElement>();

            // Tutorial
            if (parent.GetComponent<SortMain>().SortSettings.IsDemo())
            {
                if (currentHolding.IsSorted)
                    CurrentColor = UtilSort.SORTED_COLOR;
            }
        }
    }

    //protected override void GiveHint()
    //{
    //    if (registeredAboveHolder != null && registeredAboveHolder.Instruction != null)
    //    {
    //        MergeSortInstruction insertInst = (MergeSortInstruction)registeredAboveHolder.Instruction;
    //        int registeredID = registeredAboveHolder.SortingElementID;
    //        int nextHolder = insertInst.NextHolderID;

    //        if (nextHolder == holderID)
    //            parent.AudioManager.Play("Hint");
    //    }
    //}
}
