using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortElement : SortingElementBase {

    private BubbleSortInstruction bubbleSortInstruction;

    protected override void Awake()
    {
        base.Awake();
        ElementInstruction = new BubbleSortInstruction(sortingElementID, Util.NO_DESTINATION, sortingElementID, Util.NO_DESTINATION, value, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, Util.INIT_INSTRUCTION, false, false);
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
        return bubbleSortInstruction.IsSorted && (bubbleSortInstruction.SortingElementID2 == sortingElementID || parent.GetComponent<UserTestManager>().LastInstruction());
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
                    if (CheckPosition())
                    {
                        if (sortingElementID == bubbleSortInstruction.SortingElementID2)
                            isSorted = true;
                        return Util.CORRECT_HOLDER;
                    }
                    return Util.WRONG_HOLDER;

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

    private bool CheckPosition()
    {
        return sortingElementID == bubbleSortInstruction.SortingElementID1 && currentStandingOn.HolderID == bubbleSortInstruction.HolderID1 ||
               sortingElementID == bubbleSortInstruction.SortingElementID2 && currentStandingOn.HolderID == bubbleSortInstruction.HolderID2;
    }
}
