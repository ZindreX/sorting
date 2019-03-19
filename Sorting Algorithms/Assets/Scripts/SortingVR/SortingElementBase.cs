using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SortingElementBase : MonoBehaviour, ISortSubElement, IInstructionAble {

    /* -------------------------------------------- Sorting element (object) --------------------------------------------
     * > Keep an internal status about itself (which the holder checks to determine its color)
     * > Updates the holder it's placed on top of
    */

    public static int SORTING_ELEMENT_NR = 0;

    protected int value, sortingElementID, prevHolderID = UtilSort.INIT_STATE, userMove = 0, validatedUserMove = 0;
    public int currentHolderID;

    // SortingElement state
    protected bool isSorted = false, isCompare = false;
    // User test
    protected bool moving = false, standingInCorrectHolder = true, intermediateMove = false, nextMove = false;

    protected SortMain parent;
    protected HolderBase currentStandingOn;
    protected Vector3 placementAboveHolder;

    protected AudioManager audioManager;

    // Debugging
    #region Debugging variables:
    [SerializeField]
    protected string instruction, status;
    [SerializeField]
    protected int hID, nextHolderID;
    #endregion

    protected virtual void Awake()
    {
        sortingElementID = SORTING_ELEMENT_NR++;
        name = MyRole();

        // Audio
        audioManager = FindObjectOfType<AudioManager>();
    }

    // --------------------------------------- Sorting element info ---------------------------------------

    public SortMain SuperElement
    {
        get { return parent; }
        set { parent = value; }
    }

    public string MyRole()
    {
        return UtilSort.SORTING_ELEMENT_TAG + sortingElementID;
    }

    public int Value
    {
        get { return value; }
        set { this.value = value; GetComponent<TextHolder>().SetSurfaceText(value.ToString()); }
    }

    // --------------------------------------- Getters & setters ---------------------------------------

    public bool IsSorted
    {
        get { return isSorted; }
        set { ElementBecomeSorted(value); isSorted = value; }
    }

    private void ElementBecomeSorted(bool sorted)
    {
        if (!isSorted && sorted)
            audioManager.Play("Sorted");
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
        set { nextMove = value; }// Debug.Log(">>> NextMove given to [" + this.value + "]"); }
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
            }
    }

    public int SortingElementID
    {
        get { return sortingElementID; }
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

            if (validatedUserMove < userMove)
            {
                string validation = IsCorrectlyPlaced();
                switch (validation)
                {
                    case UtilSort.INIT_OK:
                        standingInCorrectHolder = true;
                        break;

                    case UtilSort.CORRECT_HOLDER:
                        standingInCorrectHolder = true;
                        parent.GetComponent<UserTestManager>().IncrementTotalCorrect();
                        break;

                    case UtilSort.INIT_ERROR: case UtilSort.WRONG_HOLDER:
                        standingInCorrectHolder = false;
                        parent.GetComponent<UserTestManager>().Mistake();
                        break;

                    default: Debug.Log("Add '" + validation + "' case, or ignore"); break;
                }

                // Mark instruction as executed if correct
                if (standingInCorrectHolder && !IntermediateMove)
                {
                    //Instruction.Status = Util.EXECUTED_INST;
                    status = UtilSort.EXECUTED_INST; // + "***"; // Debugging

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
        if (collision.collider.tag == UtilSort.HOLDER_TAG)
        {
            audioManager.Play("Collision");

            HolderBase holder = collision.collider.GetComponent<HolderBase>();
            if (parent.SortSettings.IsDemo())
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
        if (collision.collider.tag == UtilSort.HOLDER_TAG)
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
