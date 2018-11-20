using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortElement : SortingElementBase {

    private BubbleSortInstruction bubbleSortInstruction;

    protected override void Awake()
    {
        base.Awake();
        Instruction = new BubbleSortInstruction(sortingElementID, Util.NO_DESTINATION, sortingElementID, Util.NO_DESTINATION, value, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, false, false);
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
            if (bubbleSortInstruction.Instruction == Util.SWITCH_INST)
                nextHolderID = bubbleSortInstruction.SwitchToHolder(sortingElementID);
            else
                nextHolderID = bubbleSortInstruction.GetHolderFor(sortingElementID);
            
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.COMPARE_START_INST: status = "Comparing with " + bubbleSortInstruction.GetValueFor(sortingElementID, true); break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextHolderID;
                    intermediateMove = true;
                    break;
                case Util.EXECUTED_INST: status = Util.EXECUTED_INST; break;
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
                case Util.INIT_INSTRUCTION:
                    return (CurrentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                case Util.COMPARE_START_INST: break;

                case Util.COMPARE_END_INST:
                    //if (CheckPosition())
                    //{
                    //    if (sortingElementID == bubbleSortInstruction.SortingElementID2)
                    //        isSorted = true;
                    //    return Util.CORRECT_HOLDER;
                    //}
                    //return Util.WRONG_HOLDER;
                    break;

                case Util.SWITCH_INST:
                    if (!bubbleSortInstruction.ElementHasBeenExecuted(sortingElementID))
                    {
                        if (IntermediateMove && CurrentStandingOn.HolderID == bubbleSortInstruction.SwitchToHolder(sortingElementID)) // correct move
                        {
                            bubbleSortInstruction.ElementExecuted(sortingElementID);
                            if (bubbleSortInstruction.HasBeenExecuted())
                                Instruction.Instruction = Util.EXECUTED_INST;

                            intermediateMove = false;
                            return Util.CORRECT_HOLDER;
                        }
                        else if (IntermediateMove && CurrentStandingOn.HolderID == bubbleSortInstruction.GetHolderFor(sortingElementID)) // did a mistake, getting back to "start"
                            return Util.CORRECT_HOLDER;
                        return Util.WRONG_HOLDER;
                    }
                    else
                        return (CurrentStandingOn.HolderID == bubbleSortInstruction.SwitchToHolder(sortingElementID)) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
            }
        }
        return Util.CANNOT_VALIDATE_ERROR;
    }

    private bool CheckPosition()
    {
        return sortingElementID == bubbleSortInstruction.SortingElementID1 && CurrentStandingOn.HolderID == bubbleSortInstruction.HolderID1 ||
               sortingElementID == bubbleSortInstruction.SortingElementID2 && CurrentStandingOn.HolderID == bubbleSortInstruction.HolderID2;
    }
}
