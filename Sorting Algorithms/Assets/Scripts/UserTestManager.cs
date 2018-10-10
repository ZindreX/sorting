using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : MonoBehaviour, IBlackboardAble {

    /* -------------------------------------------- User Test Manager --------------------------------------------
     * 
     * 
    */

    private int currentInstructionNr = 0, errorCount = 0;
    private int readyForNext, algorithmMovesNeeded; // bool readyForNext;
    private Dictionary<int, InstructionBase> instructions;

    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int algorithmMovesNeeded)
    {
        this.instructions = instructions;
        this.algorithmMovesNeeded = algorithmMovesNeeded;
        readyForNext = algorithmMovesNeeded; // true
        currentInstructionNr = -1;
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

    public int ReadyForNext
    {
        get { return readyForNext; }
        set { readyForNext = value; }
    }

    public int AlgorithmMovesNeeded
    {
        get { return algorithmMovesNeeded; }
        set { algorithmMovesNeeded = value; }
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
