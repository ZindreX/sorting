using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortElement : SortingElementBase {

    private BubbleSortInstruction bubbleSortInstruction;

    protected override void Awake()
    {
        base.Awake();
        ElementInstruction = new BubbleSortInstruction(sortingElementID, Util.NO_INSTRUCTION, sortingElementID, Util.NO_INSTRUCTION, value, Util.NO_INSTRUCTION, Util.INIT_INSTRUCTION, false, false);
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

            hID = bubbleSortInstruction.GetHolderFor(sortingElementID);
            if (bubbleSortInstruction.ElementInstruction == Util.SWITCH_INST)
                nextID = bubbleSortInstruction.SwitchToHolder(sortingElementID);
            else
                nextID = bubbleSortInstruction.GetHolderFor(sortingElementID);
            
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: status = "Init pos"; break;
                case Util.COMPARE_START_INST: status = "Comparing with " + bubbleSortInstruction.GetValueFor(sortingElementID, true); break;
                case Util.COMPARE_END_INST: status = "Comparing stop"; break;
                case Util.SWITCH_INST:
                    status = "Move to " + nextID;
                    //intermediateMove = true;
                    break;
                case Util.EXECUTED_INST: status = "Performed"; break;
                default: Debug.LogError("UpdateSortingElementState(): Add '" + instruction + "' case, or ignore"); break;
            }

            if (bubbleSortInstruction.IsCompare)
                isCompare = true;
            else
                isCompare = false;

            if (IsSortedAchieved())
                isSorted = true;
            else
                isSorted = false;
        }
    }
    
    // Since two elements share instructions, usually the 2nd element is sorted, but in the final instruction both are
    public bool IsSortedAchieved()
    {
        return bubbleSortInstruction.IsSorted && (bubbleSortInstruction.SortingElementID2 == sortingElementID || !parent.GetComponent<UserTestManager>().HasInstructions());
    }

    protected override string IsCorrectlyPlaced()
    {
        if (CanValidate())
        {
            switch (instruction)
            {
                case Util.INIT_INSTRUCTION: return (currentStandingOn.HolderID == sortingElementID) ? Util.INIT_OK : Util.INIT_ERROR;
                case Util.COMPARE_START_INST: break;
                case Util.COMPARE_END_INST:
                    // Check if this sorting element's value is larger than the other, in that case they should have switched    ::::: TODO: switch larger than sign``````????
                    if (bubbleSortInstruction.GetValueFor(sortingElementID, false) > bubbleSortInstruction.GetValueFor(sortingElementID, true))
                        return (currentStandingOn.HolderID == bubbleSortInstruction.SwitchToHolder(sortingElementID)) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;
                    // else they should stay where they stood
                    return (currentStandingOn.HolderID == bubbleSortInstruction.GetHolderFor(sortingElementID)) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.SWITCH_INST:
                    return (currentStandingOn.HolderID == bubbleSortInstruction.GetHolderFor(sortingElementID)
                           || bubbleSortInstruction.SwitchToHolder(sortingElementID) == currentStandingOn.HolderID)
                           ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                case Util.EXECUTED_INST:
                    return (bubbleSortInstruction.SwitchToHolder(sortingElementID) == currentStandingOn.HolderID) ? Util.CORRECT_HOLDER : Util.WRONG_HOLDER;

                default: Debug.LogError("IsCorrectlyPlaced(): Add '" + instruction + "' case, or ignore"); break;
            }
        }
        return Util.CANNOT_VALIDATE_ERROR;
    }

}
