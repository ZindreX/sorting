using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SortingElementBase : MonoBehaviour, IChild {

    /* -------------------------------------------- Sorting element (object) --------------------------------------------
     * > Keep an internal status about itself (which the holder checks to determine its color)
     * > Updates the holder it's placed on top of
    */

    public static int SORTING_ELEMENT_NR = 0;

    protected int value, sortingElementID, prevHolderID = Util.INIT_STATE, userMove = 0, validatedUserMove = 0;

    // SortingElement state
    protected bool isSorted = false, isCompare = false;
    // User test
    protected bool moving = false, standingInCorrectHolder = true, intermediateMove = false, nextMove = false;

    protected GameObject parent;
    protected HolderBase currentStandingOn;

    // Debugging
    #region Debugging variables:
    [SerializeField]
    protected string instruction, status;
    [SerializeField]
    protected int hID, nextID;
    #endregion

    [SerializeField]
    private TextMesh[] surfaceTexts;

    protected virtual void Awake()
    {
        value = Random.Range(0, 100);
        sortingElementID = SORTING_ELEMENT_NR++;
        SetSurfaceText(value.ToString());       // element and holder ID (same at start)
    }

    // --------------------------------------- Sorting element info ---------------------------------------

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public string MyRole()
    {
        return Util.SORTING_ELEMENT_TAG + sortingElementID;
    }

    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }

    // --------------------------------------- Getters & setters ---------------------------------------

    public bool IsSorted
    {
        get { return isSorted; }
        set { isSorted = value; }
    }

    public bool IsCompare
    {
        get { return isCompare; }
        set { isCompare = value; }
    }

    public bool StandingInCorrectHolder
    {
        get { return standingInCorrectHolder; }
    }

    public bool NextMove
    {
        set { nextMove = value; }
    }

    public bool Moving
    {
        get { return moving; }
    }

    public bool IntermediateMove
    {
        get { return intermediateMove; }
    }

    public HolderBase CurrentStandingOn
    {
        get { return currentStandingOn; }
    }

    public int SortingElementID
    {
        get { return sortingElementID; }
    }

    private void SetSurfaceText(string text)
    {
        foreach (TextMesh textMesh in surfaceTexts)
        {
            textMesh.text = text;
        }
    }

    public void PlaceManuallySortingElementOn(HolderBase holder)
    {
        currentStandingOn = holder;
    }

    // --------------------------------------- User test validation ---------------------------------------

    public void PerformUserMove(HolderBase holder)
    {
        // Check if the user moved the element to a new holder,         TODO: in case of mistake -> avoid new error when fixing the mistake
        if (holder.HolderID != prevHolderID)
        {
            currentStandingOn = holder;
            userMove++;
            holder.HasPermission = true;

            // Validates the move and notifies whether we're ready to go to next instrution
            if (validatedUserMove < userMove)
            {
                string validation = IsCorrectlyPlaced();
                //Debug.Log("Is correctly placed: " + validation);

                switch (validation)
                {
                    case Util.INIT_OK:
                        standingInCorrectHolder = true;
                        break;
                    case Util.CORRECT_HOLDER:
                        standingInCorrectHolder = true;
                        parent.GetComponent<ScoreManager>().IncrementStreak();
                        break;
                    case Util.INIT_ERROR: case Util.WRONG_HOLDER:
                        standingInCorrectHolder = false;
                        parent.GetComponent<ScoreManager>().Mistake();
                        break;
                    default: Debug.LogError("Add '" + validation + "' case, or ignore"); break;
                }

                // Mark instruction as executed if correct
                if (standingInCorrectHolder)
                {
                    ElementInstruction.Status = Util.EXECUTED_INST;
                    status = Util.EXECUTED_INST; // Debugging


                    // Check if ready for next round
                    if (nextMove)
                    {
                        parent.GetComponent<UserTestManager>().ReadyForNext += 1;
                        nextMove = false;
                    }
                }
                validatedUserMove++;
            }
        }
        else
            currentStandingOn = holder; // Back to the same
                                        //standingInCorrectHolder = true;
    }

    protected bool CanValidate()
    {
        return ElementInstruction != null && currentStandingOn != null;
    }

    // --------------------------------------- Mouse (will be replaced by VR stuff later) ---------------------------------------

    private void OnMouseDown()
    {
        moving = true;
        parent.GetComponent<ElementManager>().NotifyMovingElement(gameObject.GetComponent<SortingElementBase>(), true);
    }

    private void OnMouseUp()
    {
        moving = false;
        parent.GetComponent<ElementManager>().NotifyMovingElement(gameObject.GetComponent<SortingElementBase>(), false);
    }

    //  --------------------------------------- Collision detection ---------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Util.HOLDER_TAG)
        {
            HolderBase holder = collision.collider.GetComponent<HolderBase>();
            if (parent.GetComponent<AlgorithmManagerBase>().IsTutorial)
            {
                currentStandingOn = holder;
            }
            else
            {
                // Update sorting element
                PerformUserMove(holder);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == Util.HOLDER_TAG)
        {
            if (currentStandingOn != null)
                prevHolderID = currentStandingOn.HolderID; // tokidoki null ref exception
            currentStandingOn = null;
            //standingInCorrectHolder = false; // todo
        }
    }

    // --------------------------------------- Implemented in subclass ---------------------------------------

    // Check whether the sorting element is placed in the correct holder, based on instruction given
    protected abstract string IsCorrectlyPlaced();

    // Updating the state of this SortingElement from the instruction that just was given
    protected abstract void UpdateSortingElementState();

    // Get & set instruction
    public abstract InstructionBase ElementInstruction { get; set; } // TODO: Fix

}
