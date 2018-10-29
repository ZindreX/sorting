using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : UserAlgorithmControl, IBlackboardAble {

    /* -------------------------------------------- User Test Manager --------------------------------------------
     * 
     * 
    */

    private int totalErrorCount = 0;
    private int readyForNext, algorithmMovesNeeded;
    private Dictionary<string, int> errorLog;

    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int algorithmMovesNeeded)
    {
        base.Init(instructions);
        this.algorithmMovesNeeded = algorithmMovesNeeded;
        readyForNext = algorithmMovesNeeded;
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();
    }

    public int TotalErrorCount
    {
        get { return totalErrorCount; }
    }

    public string GetExaminationResult()
    {
        string result = "Results from User Test:\n";

        // Add errors with some explanation | for now just the instruction ID
        result += "Errors:\n";
        foreach (KeyValuePair<string, int> entry in errorLog)
        {
            result += entry.Key + ": " + entry.Value + "\n";
        }
        return result;
    }

    // Error counting
    public void ReportError(string instructionID)
    {
        totalErrorCount++;
        if (errorLog.ContainsKey(instructionID))
            errorLog[instructionID] += 1;
        else
            errorLog.Add(instructionID, 1);

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
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();
    }

    public override string FillInBlackboard()
    {
        return "\nInst. nr.: " + CurrentInstructionNr + "\n\n" + Util.ModifyPluralString("error", totalErrorCount) + ": " + TotalErrorCount;
    }

}
