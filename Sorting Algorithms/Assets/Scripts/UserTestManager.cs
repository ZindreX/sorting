using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : UserAlgorithmControl, IBlackboardAble {

    /* -------------------------------------------- User Test Manager --------------------------------------------
     * 
     * 
    */

    private int errorCount = 0;
    private int readyForNext, algorithmMovesNeeded;

    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int algorithmMovesNeeded)
    {
        base.Init(instructions);
        this.algorithmMovesNeeded = algorithmMovesNeeded;
        readyForNext = algorithmMovesNeeded;
        errorCount = 0;
    }

    public int ErrorCount
    {
        get { return errorCount; }
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

    public override void ResetState()
    {
        base.ResetState();
        errorCount = 0;
        
    }

    public override string FillInBlackboard()
    {
        return "\nInst. nr.: " + CurrentInstructionNr + "\n\n" + Util.ModifyPluralString("error", errorCount) + ": " + ErrorCount;
    }

}
