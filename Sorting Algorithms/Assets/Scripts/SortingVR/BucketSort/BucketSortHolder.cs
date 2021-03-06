﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketSortHolder : HolderBase {

    protected override void UpdateColorOfHolder()
    {
        if (!currentHolding.StandingInCorrectHolder)
        {
            CurrentColor = UtilSort.ERROR_COLOR;
            if (hasPermission)
                parent.GetComponent<UserTestManager>().ReportError(currentHolding.SortingElementID);
            hasPermission = false;
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
        base.OnCollisionEnter(collision);

        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // Current holding the sorting element that collided
            BucketSortElement element = collision.collider.GetComponent<BucketSortElement>();
            CurrentHolding = element;

            // Demo
            if (parent.SortSettings.IsDemo())
            {
                if (currentHolding.IsCompare)
                    CurrentColor = UtilSort.COMPARE_COLOR;
                if (currentHolding.IsSorted)
                    CurrentColor = UtilSort.SORTED_COLOR;
            }
        }
    }

    //protected override void GiveHint()
    //{
    //    if (registeredAboveHolder != null && registeredAboveHolder.Instruction != null)
    //    {
    //        BucketSortInstruction bucketInst = (BucketSortInstruction)registeredAboveHolder.Instruction;
    //        int registeredID = registeredAboveHolder.SortingElementID;
    //        int nextHolder = bucketInst.NextHolderID;

    //        if (nextHolder == holderID)
    //            parent.AudioManager.Play(parent.HINT_CORRECT_SOUND);
    //        else if (bucketInst.Instruction == Util.INIT_INSTRUCTION)
    //        {

    //        }
    //        else
    //            base.GiveHint();
    //    }
    //}

}
