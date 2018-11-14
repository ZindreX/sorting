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
    public int currentHolderID;

    // SortingElement state
    protected bool isSorted = false, isCompare = false;
    // User test
    protected bool moving = false, standingInCorrectHolder = true, intermediateMove = false, nextMove = false;

    protected GameObject parent;
    protected HolderBase currentStandingOn;
    protected Vector3 placementAboveHolder;

    // Debugging
    #region Debugging variables:
    [SerializeField]
    protected string instruction, status;
    [SerializeField]
    protected int hID, nextHolderID;
    #endregion

    [SerializeField]
    private TextMesh[] surfaceTexts;

    protected virtual void Awake()
    {
        value = Random.Range(0, 100);
        sortingElementID = SORTING_ELEMENT_NR++;
        SetSurfaceText(value.ToString());
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
        get { return nextMove; }
        set { nextMove = value; }// Debug.Log("NextMove given to [" + this.value + "]"); }
    }

    public bool Moving
    {
        get { return moving; }
        set { moving = value; parent.GetComponent<ElementManager>().NotifyMovingElement(gameObject.GetComponent<SortingElementBase>(), value); }
    }

    public bool IntermediateMove
    {
        get { return intermediateMove; }
    }

    public HolderBase CurrentStandingOn
    {
        get { return currentStandingOn; }
        set { currentStandingOn = value;
            if (value != null)
            {
                currentHolderID = value.HolderID;
                PlacementAboveHolder = value.transform.position;
            }
            } // new stuff
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
        CurrentStandingOn = holder;
    }

    public Vector3 PlacementAboveHolder // new stuff
    {
        get { return placementAboveHolder; }
        set { placementAboveHolder = new Vector3(value.x, value.y + 0.1f, value.z); }
    }

    // --------------------------------------- User test validation ---------------------------------------

    public void PerformUserMove(HolderBase holder)
    {
        // Check if the user moved the element to a new holder,         TODO: in case of mistake -> avoid new error when fixing the mistake
        if (holder.HolderID != prevHolderID)
        {
            CurrentStandingOn = holder;
            userMove++;
            holder.HasPermission = true;

            //Debug.Log("Can validate: " + (validatedUserMove < userMove) + " : " + validatedUserMove + " < " + userMove);
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
                        parent.GetComponent<UserTestManager>().IncrementTotalCorrect(); //parent.GetComponent<ScoreManager>().IncrementStreak();
                        break;

                    case Util.INIT_ERROR: case Util.WRONG_HOLDER:
                        standingInCorrectHolder = false;
                        //parent.GetComponent<ScoreManager>().Mistake();
                        break;

                    default: Debug.LogError("Add '" + validation + "' case, or ignore"); break;
                }

                // Mark instruction as executed if correct
                if (standingInCorrectHolder)
                {
                    Instruction.Status = Util.EXECUTED_INST;
                    status = Util.EXECUTED_INST; // Debugging


                    // Check if ready for next round
                    if (NextMove)
                    {
                        parent.GetComponent<UserTestManager>().ReadyForNext += 1;
                        NextMove = false;
                    }
                }
                validatedUserMove++;
            }
        }
        else
            CurrentStandingOn = holder; // Back to the same
                                        //standingInCorrectHolder = true;
    }

    protected bool CanValidate()
    {
        return Instruction != null && CurrentStandingOn != null;
    }

    //  --------------------------------------- Collision detection ---------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Util.HOLDER_TAG)
        {
            HolderBase holder = collision.collider.GetComponent<HolderBase>();
            if (parent.GetComponent<AlgorithmManagerBase>().IsTutorial())
            {
                CurrentStandingOn = holder;
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
            if (CurrentStandingOn != null)
                prevHolderID = CurrentStandingOn.HolderID; // tokidoki null ref exception
            CurrentStandingOn = null;
            //standingInCorrectHolder = false; // todo
        }
    }

    // --------------------------------------- Implemented in subclass ---------------------------------------

    // Check whether the sorting element is placed in the correct holder, based on instruction given
    protected abstract string IsCorrectlyPlaced();

    // Updating the state of this SortingElement from the instruction that just was given
    protected abstract void UpdateSortingElementState();

    // Get & set instruction
    public abstract InstructionBase Instruction { get; set; }

}
