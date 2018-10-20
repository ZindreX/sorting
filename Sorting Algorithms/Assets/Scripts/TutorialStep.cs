﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : UserAlgorithmControl, IBlackboardAble
{

    /* --------------------------------------------  Manager --------------------------------------------
     * 
     * 
    */

    // Tutorial step by step
    private bool playerMove = false, playerIncremented = false, isValidStep = false, firstOrFinal = false;


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
    public bool FirstOrFinal
    {
        get { return firstOrFinal; }
        set { firstOrFinal = value; }
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
        return "Nothing yet";
    }
}
