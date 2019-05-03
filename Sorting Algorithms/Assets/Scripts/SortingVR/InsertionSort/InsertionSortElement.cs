using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortElement : SortingElementBase {

    private InsertionSortInstruction insertionSortInstruction;
    protected bool isPivot = false;

    protected override void Awake()
    {
        base.Awake();
        Instruction = new InsertionSortInstruction(UtilSort.INIT_INSTRUCTION, 0, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, sortingElementID, value, false, false, false, sortingElementID, UtilSort.NO_DESTINATION); //new InsertionSortInstruction(sortingElementID, sortingElementID, Util.INIT_STATE, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, false, false, false);
    }

    public bool IsPivot
    {
        get { return isPivot; }
        set { isPivot = value; }
    }

    public override InstructionBase Instruction
    {
        get { return insertionSortInstruction; }
        set { insertionSortInstruction = (InsertionSortInstruction)value; UpdateSortingElementState(); }
    }

    protected override void UpdateSortingElementState()
    {
        if (insertionSortInstruction != null)
        {
            // Debugging
            instruction = insertionSortInstruction.Instruction;
            hID = insertionSortInstruction.HolderID;
            nextHolderID = insertionSortInstruction.NextHolderID;

            switch (insertionSortInstruction.Instruction)
            {
                case UtilSort.INIT_INSTRUCTION: status = "Init pos"; break;
                case UtilSort.PIVOT_START_INST:
                    status = "Move to pivot holder";
                    intermediateMove = true;
                    break;

                case UtilSort.PIVOT_END_INST:
                    status = "Move down from pivot holder";
                    intermediateMove = true;
                    break;

                case UtilSort.COMPARE_START_INST: status = "Comparing with pivot"; break;
                case UtilSort.COMPARE_END_INST: status = "Comparing stop"; break;

                case UtilSort.SWITCH_INST:
                    status = "Move to " + nextHolderID;
                    intermediateMove = true;
                    break;

                case UtilSort.SET_SORTED_INST:
                    IsSorted = true;
                    UtilSort.IndicateElement(gameObject);
                    break;

                case UtilSort.EXECUTED_INST: status = UtilSort.EXECUTED_INST; break;
                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

            if (insertionSortInstruction.IsPivot)
                IsPivot = true;
            else
                IsPivot = false;

            if (insertionSortInstruction.IsCompare)
                IsCompare = true;
            else
                IsCompare = false;

            if (insertionSortInstruction.IsSorted)
                IsSorted = true;
            else
                IsSorted = false;
        }
    }

    protected override string IsCorrectlyPlaced()
    {
        if (CanValidate())
        {
            if (!insertionSortInstruction.HasBeenExecuted())
            {
                switch (insertionSortInstruction.Instruction)
                {
                    case UtilSort.INIT_INSTRUCTION:
                        return (CurrentStandingOn.HolderID == sortingElementID) ? UtilSort.INIT_OK : UtilSort.INIT_ERROR;

                    case UtilSort.SET_SORTED_INST:
                        return (CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    case UtilSort.PIVOT_START_INST:
                        if (IntermediateMove && ((InsertionSortHolder)CurrentStandingOn).IsPivotHolder) // correct move
                        {
                            Instruction.Status = Util.EXECUTED_INST;
                            intermediateMove = false;
                            return UtilSort.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) // did a mistake, getting back to "start"
                            return UtilSort.CORRECT_HOLDER;
                        return UtilSort.WRONG_HOLDER;

                    case UtilSort.PIVOT_END_INST:
                        if (IntermediateMove && CurrentStandingOn.HolderID == insertionSortInstruction.NextHolderID) // correct move
                        {
                            Instruction.Status = Util.EXECUTED_INST;
                            intermediateMove = false;
                            return UtilSort.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && ((InsertionSortHolder)CurrentStandingOn).IsPivotHolder) // did a mistake, getting back to "start"
                            return UtilSort.CORRECT_HOLDER;
                        return UtilSort.WRONG_HOLDER;

                    case UtilSort.COMPARE_START_INST:
                        return elementInteraction.PickedUp && (CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER; // break;

                    case UtilSort.COMPARE_END_INST:
                        return CurrentStandingOn.HolderID == insertionSortInstruction.HolderID ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    case UtilSort.SWITCH_INST:
                        if (IntermediateMove && insertionSortInstruction.NextHolderID == CurrentStandingOn.HolderID)
                        {
                            Instruction.Status = UtilSort.EXECUTED_INST;
                            intermediateMove = false;
                            return UtilSort.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && CurrentStandingOn.HolderID == insertionSortInstruction.HolderID)
                            return UtilSort.CORRECT_HOLDER;
                        else
                            return UtilSort.WRONG_HOLDER;

                    default: Debug.Log("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
                }
            }
            else
            {
                switch (insertionSortInstruction.Instruction)
                {
                    case UtilSort.INIT_INSTRUCTION:
                        return (CurrentStandingOn.HolderID == sortingElementID) ? UtilSort.INIT_OK : UtilSort.INIT_ERROR;

                    case UtilSort.SET_SORTED_INST:
                        return (CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    case UtilSort.PIVOT_START_INST:
                        return ((InsertionSortHolder)CurrentStandingOn).IsPivotHolder ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    case UtilSort.PIVOT_END_INST:
                        return CurrentStandingOn.HolderID == insertionSortInstruction.NextHolderID ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    case UtilSort.COMPARE_START_INST:
                        return elementInteraction.PickedUp && (CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER; // break;

                    case UtilSort.COMPARE_END_INST:
                        return CurrentStandingOn.HolderID == insertionSortInstruction.HolderID ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    case UtilSort.SWITCH_INST:
                        return (insertionSortInstruction.NextHolderID == CurrentStandingOn.HolderID) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                    default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
                }
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }
}
