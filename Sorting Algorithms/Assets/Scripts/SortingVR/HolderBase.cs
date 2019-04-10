using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class HolderBase : MonoBehaviour, ISortSubElement {

    /* -------------------------------------------- Holder (object) --------------------------------------------
     * 
     * 
    */

    public static int HOLDER_NR = 0;
    protected int holderID, prevElementID;

    protected TextMeshPro indexText;

    protected Color currentColor, prevColor;
    protected bool errorNotified = false, hasPermission = true;

    protected SortMain parent;
    protected SortingElementBase currentHolding, registeredAboveHolder;

    protected virtual void Awake()
    {
        holderID = HOLDER_NR++;
        name = MyRole();
        indexText = GetComponentInChildren<TextMeshPro>();
        indexText.text = holderID.ToString();
    }

    void Update()
    {

        // Always checking the status of the sorting element this holder is holding, and changing color thereafter
        if (isValidSortingElement(currentHolding))
            UpdateColorOfHolder();

        //if (parent.SortSettings.IsUserTest())
        //{
                // old placement
        //}
        //else if (parent.SortSettings.IsDemo() && currentHolding != null)
        //{
        //    if (currentHolding.IsSorted)
        //        UpdateColorOfHolder();
        //}       
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

    // Register elements above
    protected void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering holder" + holderID + ": " + other.tag);

        if (other.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            registeredAboveHolder = other.GetComponent<SortingElementBase>();

            if (parent.Settings.Difficulty == Util.BEGINNER)
            {
                // TODO: hint
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exiting holder" + holderID + ": " + other.tag);

        if (other.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // If element moved outside trigger box
            registeredAboveHolder = null;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Removed from holder " + holderID + ": " + collision.collider.tag);
        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            if (CurrentHolding != null)
                prevElementID = currentHolding.SortingElementID;

            CurrentHolding = null;
            CurrentColor = Util.STANDARD_COLOR;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag + " collided with holder " + holderID);

        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            SortingElementBase element = collision.collider.GetComponent<SortingElementBase>();
            element.CurrentStandingOn = this;

            if (parent.Settings.IsUserTest())
                element.PerformUserMove(this);
        }
    }

    // --------------------------------------- Implemented in subclass ---------------------------------------

    // Updates the color based on the state of the sorting element
    protected abstract void UpdateColorOfHolder();



}
