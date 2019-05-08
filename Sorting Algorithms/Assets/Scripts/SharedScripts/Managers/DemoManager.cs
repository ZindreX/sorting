using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : InstructionControlBase {

    /* -------------------------------------------- New Demo / Step by step --------------------------------------------
     * 
     * 
    */

    private bool playerMove = false, playerIncremented = false, isValidStep = false;

    public void InitDemo(Dictionary<int, InstructionBase> instructions)//, int userActionInstructions)
    {
        base.Init(instructions, instructions.Count, true);
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

    public void NotifyUserInput(bool increment)
    {
        playerIncremented = increment;
        isValidStep = true;
        if (playerIncremented)
        {
            if (IncrementToNextInstruction())
                playerMove = true;
            else
            {
                isValidStep = false;
                finalInstruction = true;
            }

        }
        else
        {
            if (DecrementToPreviousInstruction())
                playerMove = true;
            else
            {
                isValidStep = false;
                firstInstruction = true;
            }
        }
    }

    public InstructionBase GetStep()
    {
        if (playerIncremented)
            return GetInstruction(currentInstructionNr);
        return GetInstruction(currentInstructionNr + 1);
    }

    public override void ResetState()
    {
        base.ResetState();
        playerMove = false;
        playerIncremented = false;
        isValidStep = false;
        firstInstruction = false;
        finalInstruction = false;
    }


    public override string FillInBlackboard()
    {
        return "Fill in blackboard not implemented yet - see StepByStepManager";
    }
}
