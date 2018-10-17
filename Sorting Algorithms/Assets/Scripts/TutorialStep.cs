using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : UserAlgorithmControl, IBlackboardAble
{

    /* --------------------------------------------  Manager --------------------------------------------
     * 
     * 
    */

    // Tutorial step by step
    private bool playerMove = false, playerIncremented = false;


    public bool PlayerMove
    {
        get { return playerMove; }
        set { playerMove = value; }
    }

    public bool PlayerIncremented
    {
        get { return playerIncremented; }
    }

    public void NotifyUserInput(bool increment)
    {
        playerIncremented = increment;
        if (playerIncremented)
        {
            if (IncrementToNextInstruction())
                playerMove = true;
        }
        else
        {
            if (DecrementToPreviousInstruction())
                playerMove = true;
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
        return "Nothing yet";
    }
}
