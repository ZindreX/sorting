using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HolderBase : MonoBehaviour, ISortSubElement {

    /* -------------------------------------------- Holder (object) --------------------------------------------
     * 
     * 
    */

    public static int HOLDER_NR = 0;
    protected int holderID, prevElementID;

    protected Color currentColor, prevColor;
    protected bool errorNotified = false, hasPermission = true;

    protected SortMain parent;
    protected SortingElementBase currentHolding;

    void Awake()
    {
        holderID = HOLDER_NR++;
    }

    void Update()
    {
        if (parent.SortSettings.IsUserTest())
        {
            // Always checking the status of the sorting element this holder is holding, and changing color thereafter
            if (isValidSortingElement(currentHolding))
                UpdateColorOfHolder();
        }
        else if (parent.SortSettings.IsDemo() && currentHolding != null)
        {
            if (currentHolding.IsSorted)
                UpdateColorOfHolder();
        }       
    }

    private bool isValidSortingElement(SortingElementBase element)
    {
        return element != null && element.Instruction != null;
    }

    public virtual string MyRole()
    {
        return UtilSort.HOLDER_TAG + holderID;
    }

    public SortMain SuperElement
    {
        get { return parent; }
        set { parent = value; }
    }

    public int HolderID
    {
        get { return holderID; }
    }

    public Color CurrentColor
    {
        get { return GetComponentInChildren<Renderer>().material.color; }
        set {
            if (parent.SortSettings.Difficulty < UtilSort.EXAMINATION || parent.GetTeachingAlgorithm().IsTaskCompleted)
            {
                prevColor = currentColor;
                GetComponentInChildren<Renderer>().material.color = value;
            }}
    }

    public Color PrevColor
    {
        get { return prevColor; }
    }

    public SortingElementBase CurrentHolding
    {
        get { return currentHolding; }
        set { currentHolding = value; }
    }

    public bool HasPermission
    {
        set { hasPermission = value; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // Tutorial
            if (parent.SortSettings.IsDemo())
            {

            }
            else // User test
            {
                if (CurrentHolding != null)
                    prevElementID = currentHolding.SortingElementID; // null exception, how?
            }
            CurrentHolding = null;
            CurrentColor = UtilSort.STANDARD_COLOR;
        }
    }



    // --------------------------------------- Implemented in subclass ---------------------------------------

    // Updates the color based on the state of the sorting element
    protected abstract void UpdateColorOfHolder();

    // Performed in subclasses due to different sorting elements
    protected abstract void OnCollisionEnter(Collision collision);

}
