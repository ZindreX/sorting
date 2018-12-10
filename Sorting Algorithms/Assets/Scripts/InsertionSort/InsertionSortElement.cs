using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortElement : SortingElementBase {

    private InsertionSortInstruction insertionSortInstruction;
    protected bool isPivot = false;

    protected override void Awake()
    {
        base.Awake();
        Instruction = new InsertionSortInstruction(Util.INIT_INSTRUCTION, 0, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, sortingElementID, value, false, false, false, sortingElementID, Util.NO_DESTINATION); //new InsertionSortInstruction(sortingElementID, sortingElementID, Util.INIT_STATE, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, value, false, false, false);
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
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.PIVOT_START_INST:
                    status = "Move to pivot holder";
                    intermediateMove = true;
                    break;
                case Util.PIVOT_END_INST:
                    status = "Move down from pivot holder";
                    intermediateMove = true;
                    break;
                case Util.COMPARE_START_INST: status = "Comparing with pivot"; break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextHolderID;
                    intermediateMove = true;
                    break;
                case Util.SET_SORTED_INST:
                    IsSorted = true;
                    Util.IndicateElement(gameObject);
                    break;
                case Util.EXECUTED_INST: status = Util.EXECUTED_INST; break;
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
                    case Util.INIT_INSTRUCTION:
                        return (CurrentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                    case Util.SET_SORTED_INST:
                        return (CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.PIVOT_START_INST:
                        if (IntermediateMove && ((InsertionSortHolder)CurrentStandingOn).IsPivotHolder) // correct move
                        {
                            Instruction.Status = Util.EXECUTED_INST;
                            intermediateMove = false;
                            return Util.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) // did a mistake, getting back to "start"
                            return Util.CORRECT_HOLDER;
                        return Util.WRONG_HOLDER;

                    case Util.PIVOT_END_INST:
                        if (IntermediateMove && CurrentStandingOn.HolderID == insertionSortInstruction.NextHolderID) // correct move
                        {
                            Instruction.Status = Util.EXECUTED_INST;
                            intermediateMove = false;
                            return Util.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && ((InsertionSortHolder)CurrentStandingOn).IsPivotHolder) // did a mistake, getting back to "start"
                            return Util.CORRECT_HOLDER;
                        return Util.WRONG_HOLDER;

                    case Util.COMPARE_START_INST:
                        Debug.Log("What? compare start");
                        break;

                    case Util.COMPARE_END_INST:
                        return CurrentStandingOn.HolderID == insertionSortInstruction.HolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.SWITCH_INST:
                        if (IntermediateMove && insertionSortInstruction.NextHolderID == CurrentStandingOn.HolderID)
                        {
                            Instruction.Status = Util.EXECUTED_INST;
                            intermediateMove = false;
                            return Util.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && CurrentStandingOn.HolderID == insertionSortInstruction.HolderID)
                            return Util.CORRECT_HOLDER;
                        else
                            return Util.WRONG_HOLDER;

                    default: Debug.Log("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
                }
            }
            else
            {
                switch (insertionSortInstruction.Instruction)
                {
                    case Util.INIT_INSTRUCTION:
                        return (CurrentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                    case Util.SET_SORTED_INST:
                        return (CurrentStandingOn.HolderID == insertionSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.PIVOT_START_INST:
                        return ((InsertionSortHolder)CurrentStandingOn).IsPivotHolder ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.PIVOT_END_INST:
                        return CurrentStandingOn.HolderID == insertionSortInstruction.NextHolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.COMPARE_START_INST:
                        Debug.Log("What? compare start");
                        break;

                    case Util.COMPARE_END_INST:
                        return CurrentStandingOn.HolderID == insertionSortInstruction.HolderID ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    case Util.SWITCH_INST:
                        return (insertionSortInstruction.NextHolderID == CurrentStandingOn.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                    default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
                }
            }
        }
        return Util.CANNOT_VALIDATE_ERROR;
    }
}
