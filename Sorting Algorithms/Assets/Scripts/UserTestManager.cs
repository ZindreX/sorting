using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : MonoBehaviour, IBlackboardAble {

    /* -------------------------------------------- User Test --------------------------------------------
     * 
     * 
    */

    private int currentInstructionNr = 0, errorCount = 0;
    private bool readyForNext;
    private Dictionary<int, InstructionBase> instructions;

    public void InitUserTest(Dictionary<int, InstructionBase> instructions)
    {
        this.instructions = instructions;
        readyForNext = true;
        currentInstructionNr = 0;
        errorCount = 0;
    }

    public bool HasInstructions()
    {
        if (instructions != null)
            return (instructions.Count != 0);
        return false;
    }

    public InstructionBase GetInstruction(int index)
    {
        return instructions[index];
    }

    public int ErrorCount
    {
        get { return errorCount; }
    }

    public void IncrementToNextMove()
    {
        if (currentInstructionNr < instructions.Count)
            currentInstructionNr++;
    }

    public void IncrementError()
    {
        errorCount++;
    }

    public bool ReadyForNext
    {
        get { return readyForNext; }
        set { readyForNext = value; }
    }

    public int CurrentInstructionNr
    {
        get { return currentInstructionNr; }
    }

    public string FillInBlackboard()
    {
        return "\nInst. nr.: " + CurrentInstructionNr + "\n\n" + Util.ModifyPluralString("error", errorCount) + ": " + ErrorCount;
    }

}
