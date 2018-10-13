using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortElement : SortingElementBase {

    private BubbleSortInstruction bubbleSortInstruction;

    protected override void Awake()
    {
        base.Awake();
        ElementInstruction = new BubbleSortInstruction(sortingElementID, sortingElementID, Util.NO_DESTINATION, value, Util.INIT_INSTRUCTION, false, false);
    }

    public override InstructionBase ElementInstruction
    {
        get { return bubbleSortInstruction; }
        set { bubbleSortInstruction = (BubbleSortInstruction)value; UpdateSortingElementState(); }
    }

    protected override void UpdateSortingElementState()
    {
        if (bubbleSortInstruction != null)
        {
            // Debugging
            instruction = bubbleSortInstruction.ElementInstruction;

            hID = bubbleSortInstruction.HolderID;
            nextID = bubbleSortInstruction.NextHolderID;
            
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.COMPARE_START_INST: status = "Comparing"; break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextID;
                    intermediateMove = true;
                    break;
                case Util.EXECUTED_INST: status = "Performed"; break;
                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

            if (bubbleSortInstruction.IsCompare)
                isCompare = true;
            else
                isCompare = false;

            if (bubbleSortInstruction.IsSorted)
                isSorted = true;
            else
                isSorted = false;
        }
    }

    protected override string IsCorrectlyPlaced()
    {
        if (CanValidate())
        {
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION:
                    return (currentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;

                case Util.COMPARE_START_INST: break;

                case Util.COMPARE_END_INST:
                    intermediateMove = false;
                    return (currentStandingOn.HolderID == bubbleSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.SWITCH_INST:
                    if (intermediateMove && currentStandingOn.HolderID == bubbleSortInstruction.HolderID)
                    {
                        return Util.CORRECT_HOLDER;
                    }
                    else if (intermediateMove && bubbleSortInstruction.NextHolderID == currentStandingOn.HolderID)
                    {
                        intermediateMove = false;
                        return Util.CORRECT_HOLDER;
                    }
                    else
                        return Util.WRONG_HOLDER;

                case Util.EXECUTED_INST:
                    if (bubbleSortInstruction.NextHolderID != Util.NO_DESTINATION)
                        return (currentStandingOn.HolderID == bubbleSortInstruction.NextHolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;
                    return (currentStandingOn.HolderID == bubbleSortInstruction.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
            }
        }
        return Util.CANNOT_VALIDATE_ERROR;
    }
}
