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
            return instructions.Count != 0 && currentInstructionNr < instructions.Count;
        return false;
    }

    public bool LastInstruction()
    {
        return currentInstructionNr == (instructions.Count - 1);
    }

    public InstructionBase GetInstruction()
    {
        return instructions[currentInstructionNr];
    }

    public int ErrorCount
    {
        get { return errorCount; }
    }

    // Increasing instruction counter as long there is one
    public void IncrementToNextInstruction()
    {
        if (currentInstructionNr < instructions.Count)
            currentInstructionNr++;
    }

    // Error counting
    public void IncrementError()
    {
        errorCount++;
    }

    /* Checking whether a new instruction can be given
     * > Changed to int counting instead of bool, because of bubblesort instruction changed*
    */
    public int ReadyForNext
    {
        get { return readyForNext; }
        set { readyForNext = value; }
    }

    /* The number of moves needed before handing out a new instruction
     * > Usually 1, but for some there might be more (like bubblesort with 2 element switching per instruction) 
    */
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
