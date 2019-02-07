using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortElement : SortingElementBase {

    private BubbleSortInstruction bubbleSortInstruction;

    protected override void Awake()
    {
        base.Awake();
        Instruction = new BubbleSortInstruction(UtilSort.INIT_INSTRUCTION, 0, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, sortingElementID, sortingElementID, UtilSort.NO_VALUE, UtilSort.NO_VALUE, value, UtilSort.NO_VALUE, false, false); //new BubbleSortInstruction(sortingElementID, Util.NO_DESTINATION, sortingElementID, Util.NO_DESTINATION, value, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, 0, false, false);
    }

    public override InstructionBase Instruction
    {
        get { return bubbleSortInstruction; }
        set { bubbleSortInstruction = (BubbleSortInstruction)value; UpdateSortingElementState(); }
    }

    protected override void UpdateSortingElementState()
    {
        if (bubbleSortInstruction != null)
        {
            // Debugging
            instruction = bubbleSortInstruction.Instruction;

            hID = bubbleSortInstruction.GetHolderFor(sortingElementID);
            if (bubbleSortInstruction.Instruction == UtilSort.SWITCH_INST)
                nextHolderID = bubbleSortInstruction.SwitchToHolder(sortingElementID);
            else
                nextHolderID = bubbleSortInstruction.GetHolderFor(sortingElementID);
            
            switch (instruction)
            {
                case UtilSort.INIT_INSTRUCTION: status = "Init pos"; break;
                case UtilSort.COMPARE_START_INST: status = "Comparing with " + bubbleSortInstruction.GetValueFor(sortingElementID, true); break;
                case UtilSort.COMPARE_END_INST: status = "Comparing stop"; break;
                case UtilSort.SWITCH_INST:
                    status = "Move to " + nextHolderID;
                    intermediateMove = true;
                    break;
                case UtilSort.EXECUTED_INST: status = UtilSort.EXECUTED_INST; break;
                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

            if (bubbleSortInstruction.IsCompare)
                IsCompare = true;
            else
                IsCompare = false;

            if (bubbleSortInstruction.IsElementSorted(sortingElementID))
                IsSorted = true;
            else
                IsSorted = false;
        }
    }

    protected override string IsCorrectlyPlaced()
    {
        if (CanValidate())
        {
            switch (bubbleSortInstruction.Instruction)
            {
                case UtilSort.INIT_INSTRUCTION:
                    return (CurrentStandingOn.HolderID == sortingElementID) ? UtilSort.INIT_OK : UtilSort.INIT_ERROR;

                case UtilSort.COMPARE_START_INST: break;

                case UtilSort.COMPARE_END_INST:
                    return CheckPosition() ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                case UtilSort.SWITCH_INST:
                    if (!bubbleSortInstruction.ElementHasBeenExecuted(sortingElementID))
                    {
                        if (IntermediateMove && CurrentStandingOn.HolderID == bubbleSortInstruction.SwitchToHolder(sortingElementID)) // correct move
                        {
                            bubbleSortInstruction.ElementExecuted(sortingElementID);
                            if (bubbleSortInstruction.HasBeenExecuted())
                                Instruction.Instruction = UtilSort.EXECUTED_INST;

                            intermediateMove = false;
                            return UtilSort.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && CurrentStandingOn.HolderID == bubbleSortInstruction.GetHolderFor(sortingElementID)) // did a mistake, getting back to "start"
                            return UtilSort.CORRECT_HOLDER;
                        return UtilSort.WRONG_HOLDER;
                    }
                    else
                        return (CurrentStandingOn.HolderID == bubbleSortInstruction.SwitchToHolder(sortingElementID)) ? UtilSort.CORRECT_HOLDER : UtilSort.WRONG_HOLDER;

                default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
            }
        }
        return UtilSort.CANNOT_VALIDATE_ERROR;
    }

    private bool CheckPosition()
    {
        return sortingElementID == bubbleSortInstruction.SortingElementID1 && CurrentStandingOn.HolderID == bubbleSortInstruction.HolderID1 ||
               sortingElementID == bubbleSortInstruction.SortingElementID2 && CurrentStandingOn.HolderID == bubbleSortInstruction.HolderID2;
    }
}
