using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepByStepManager : InstructionControlBase {

    /* -------------------------------------------- New Demo / Step by step --------------------------------------------
     * 
     * 
    */

    private bool playerMove = false, playerIncremented = false, isValidStep = false, firstInstruction = false, finalInstruction = false;

    public void InitDemo(Dictionary<int, InstructionBase> instructions)
    {
        base.Init(instructions);


    }

    //  -------------------------------------------- Demo --------------------------------------------



    //  -------------------------------------------- Step by step --------------------------------------------
    public bool PlayerMove
    {
        get { return playerMove; }
        set { playerMove = value; }
    }

    public bool PlayerIncremented
    {
        get { return playerIncremented; }
    }

    public bool IsValidStep
    {
        get { return isValidStep; }
    }

    public bool FirstInstruction
    {
        get { return firstInstruction; }
        set { firstInstruction = value; }
    }

    public bool FinalInstruction
    {
        get { return finalInstruction; }
        set { finalInstruction = value; }
    }

    public void NotifyUserInput(bool increment)
    {
        playerIncremented = increment;
        isValidStep = true;
        if (playerIncremented)
        {
            if (IncrementToNextInstruction())
                playerMove = true;
            else
                isValidStep = false;

        }
        else
        {
            if (DecrementToPreviousInstruction())
                playerMove = true;
            else
                isValidStep = false;
        }
    }

    public InstructionBase GetStep()
    {
        if (playerIncremented)
            return GetInstruction(currentInstructionNr);
        return GetInstruction(currentInstructionNr + 1);
    }


    public override string FillInBlackboard()
    {
        return "Fill in blackboard not implemented yet - see StepByStepManager";
    }
}
