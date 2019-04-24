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
    protected SortingElementBase currentHolding;
    protected List<SortingElementBase> registeredAboveHolder;

    protected virtual void Awake()
    {
        holderID = HOLDER_NR++;
        name = MyRole();
        indexText = GetComponentInChildren<TextMeshPro>();
        indexText.text = holderID.ToString();
        registeredAboveHolder = new List<SortingElementBase>();
    }

    void Update()
    {
        // Always checking the status of the sorting element this holder is holding, and changing color thereafter
        if (isValidSortingElement(currentHolding))
        {
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
            if (parent.SortSettings.Difficulty < Util.EXAMINATION || parent.GetTeachingAlgorithm().IsTaskCompleted)
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
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Entering holder" + holderID + ": " + other.tag);

        if (other.tag == UtilSort.SORTING_ELEMENT_TAG) // .compareTag()
        {
            SortingElementBase element = other.GetComponent<SortingElementBase>();

            if (!registeredAboveHolder.Contains(element))
                registeredAboveHolder.Add(element);
            

            //if (parent.Settings.Difficulty == Util.BEGINNER)
            //    GiveHint();
        }
    }

    // Remove element from this holder (if touched the holder)
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exiting holder" + holderID + ": " + other.tag);

        if (other.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            // Check if other element bumped into this trigger (to avoid removing correct element)
            SortingElementBase otherElement = other.GetComponent<SortingElementBase>();

            if (registeredAboveHolder.Contains(otherElement))
                registeredAboveHolder.Remove(otherElement);

            // Remove from this holder
            if (CurrentHolding != null && CurrentHolding == otherElement)
            {
                prevElementID = currentHolding.SortingElementID;
                CurrentHolding = null;
                CurrentColor = Util.STANDARD_COLOR;

                if (registeredAboveHolder.Count > 0)
                    UtilSort.IndicateElement(registeredAboveHolder[0].gameObject);
            }
        }
    }

    // Set as current holding, and set element as current on top of this holder
    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.tag + " collided with holder " + holderID);

        if (collision.collider.tag == UtilSort.SORTING_ELEMENT_TAG)
        {
            SortingElementBase element = collision.collider.GetComponent<SortingElementBase>();
            element.CurrentStandingOn = this;

            if (parent.Settings.IsUserTest())
                element.PerformUserMove(this);
        }
    }

    protected virtual void GiveHint()
    {
        // First let <..>holder check
        // Base (here) called means user is about to do a wrong move
        parent.AudioManager.Play("HintMistake");
    }




    // --------------------------------------- Implemented in subclass ---------------------------------------

    // Updates the color based on the state of the sorting element
    protected abstract void UpdateColorOfHolder();




}
