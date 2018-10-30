using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortElement : SortingElementBase {

    private InsertionSortInstruction insertionSortInstruction;
    protected bool isPivot = false;

    protected override void Awake()
    {
        base.Awake();
        ElementInstruction = new InsertionSortInstruction(sortingElementID, sortingElementID, Util.INIT_STATE, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, value, false, false, false);
    }

    public bool IsPivot
    {
        get { return isPivot; }
        set { isPivot = value; }
    }

    public override InstructionBase ElementInstruction
    {
        get { return insertionSortInstruction; }
        set { insertionSortInstruction = (InsertionSortInstruction)value; UpdateSortingElementState(); }
    }

    protected override void UpdateSortingElementState()
    {
        if (insertionSortInstruction != null)
        {
            // Debugging
            instruction = insertionSortInstruction.ElementInstruction;
            hID = insertionSortInstruction.HolderID;
            nextHolderID = insertionSortInstruction.NextHolderID;

            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.PIVOT_START_INST: status = "Move to pivot holder"; break;
                case Util.PIVOT_END_INST: status = "Move down from pivot holder"; break;
                case Util.COMPARE_START_INST: status = "Comparing with pivot"; break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextHolderID;
                    intermediateMove = true; // Too easy for the user?
                    break;
                case Util.EXECUTED_INST: status = Util.EXECUTED_INST; break;
                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

            if (insertionSortInstruction.IsPivot)
                isPivot = true;
            else
                isPivot = false;

            if (insertionSortInstruction.IsCompare)
                isCompare = true;
            else
                isCompare = false;

            if (insertionSortInstruction.IsSorted)
                isSorted = true;
            else
                isSorted = false;
        }
    }

    protected override string IsCorrectlyPlaced()
    {
        Debug.Log("IsCorrectlyPlaced()");
        if (CanValidate())
        {
            Debug.Log("Can idd validate");
            if (!insertionSortInstruction.HasBeenExecuted())
            {
                Debug.Log("Not executed");
                switch (insertionSortInstruction.ElementInstruction)
                {
                    case Util.INIT_INSTRUCTION:
                        return (currentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                    case Util.PIVOT_START_INST:
                        return (currentStandingOn.HolderID == insertionSortInstruction.HolderID || ((InsertionSortHolder)currentStandingOn).IsPivotHolder) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.PIVOT_END_INST:
                        return (((InsertionSortHolder)currentStandingOn).IsPivotHolder || currentStandingOn.HolderID == insertionSortInstruction.NextHolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.COMPARE_START_INST:
                        break;

                    case Util.COMPARE_END_INST:
                        return currentStandingOn.HolderID == insertionSortInstruction.HolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.SWITCH_INST:
                        if (intermediateMove && currentStandingOn.HolderID == insertionSortInstruction.HolderID) //insertionSortInstruction.HolderID == currentStandingOn.HolderID && insertionSortInstruction.NextHolderID != Util.NO_INSTRUCTION && IsSorted)
                        {
                            return Util.CORRECT_HOLDER; //Util.MOVE_INTERMEDIATE;
                        }
                        else if (intermediateMove && insertionSortInstruction.NextHolderID == currentStandingOn.HolderID)
                        {
                            intermediateMove = false;
                            return Util.CORRECT_HOLDER;
                        }
                        else
                            return Util.WRONG_HOLDER;

                    default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
                }
            }
            else
            {
                Debug.Log("Testing moving to wrong holder (already executed instruction)");
                switch (insertionSortInstruction.ElementInstruction)
                {
                    case Util.INIT_INSTRUCTION:
                        return (currentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                    case Util.PIVOT_START_INST:
                        return ((InsertionSortHolder)currentStandingOn).IsPivotHolder ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.PIVOT_END_INST:
                        return currentStandingOn.HolderID == insertionSortInstruction.NextHolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.COMPARE_START_INST:
                        break;

                    case Util.COMPARE_END_INST:
                        return currentStandingOn.HolderID == insertionSortInstruction.HolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.SWITCH_INST:
                        return (intermediateMove && insertionSortInstruction.NextHolderID == currentStandingOn.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
                }




                //if (insertionSortInstruction.NextHolderID != Util.NO_DESTINATION)
                //    return (currentStandingOn.HolderID == insertionSortInstruction.NextHolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;
                //return (currentStandingOn.HolderID == insertionSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;
            }
        }
        return Util.CANNOT_VALIDATE_ERROR;
    }
}
