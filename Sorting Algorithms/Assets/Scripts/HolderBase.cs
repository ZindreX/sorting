using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HolderBase : MonoBehaviour, IChild {

    /* -------------------------------------------- Holder (object) --------------------------------------------
     * 
     * 
    */

    public static int HOLDER_NR = 0;
    protected int holderID, prevElementID;

    protected Color currentColor, prevColor;
    protected bool errorNotified = false, hasPermission = true;

    protected GameObject parent;
    protected SortingElementBase currentHolding;

    void Awake()
    {
        holderID = HOLDER_NR++;
    }

    void Update()
    {
        if (parent.GetComponent<AlgorithmManagerBase>().AlgorithmSettings.IsUserTest())
        {
            // Always checking the status of the sorting element this holder is holding, and changing color thereafter
            if (isValidSortingElement(currentHolding))
                UpdateColorOfHolder();
        }
        else if (parent.GetComponent<AlgorithmManagerBase>().AlgorithmSettings.IsDemo() && currentHolding != null)
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
        return Util.HOLDER_TAG + holderID;
    }

    public GameObject Parent
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
            if (parent.GetComponent<AlgorithmManagerBase>().AlgorithmSettings.Difficulty < Util.EXAMINATION || parent.GetComponent<AlgorithmManagerBase>().Algorithm.IsSortingComplete)
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
        if (collision.collider.tag == Util.SORTING_ELEMENT_TAG)
        {
            // Tutorial
            if (parent.GetComponent<AlgorithmManagerBase>().AlgorithmSettings.IsDemo())
            {

            }
            else // User test
            {
                if (CurrentHolding != null)
                    prevElementID = currentHolding.SortingElementID; // null exception, how?
            }
            CurrentHolding = null;
            CurrentColor = Util.STANDARD_COLOR;
        }
    }



    // --------------------------------------- Implemented in subclass ---------------------------------------

    // Updates the color based on the state of the sorting element
    protected abstract void UpdateColorOfHolder();

    // Performed in subclasses due to different sorting elements
    protected abstract void OnCollisionEnter(Collision collision);

}
