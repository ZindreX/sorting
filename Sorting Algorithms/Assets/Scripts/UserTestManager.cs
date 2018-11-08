using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : InstructionControlBase, IBlackboardAble {

    /* -------------------------------------------- User Test Manager --------------------------------------------
     * 
     * 
    */

    // User test
    private int totalCorrect, totalErrorCount;
    private int readyForNext, userActionToProceed, userActionInstructions;
    private Dictionary<string, int> errorLog;

    // Score
    public readonly int SCORE_PER_STREAK = 10, SUBSTRACTION_PER_ERROR = 2;
    public readonly float SCORE_UPDATE = 0.1f;

    private float startTime, endTime, timeSpent;
    private int streakScore, totalScore, currentStreak, longestStreak, difficultyMultiplier;
    private string difficultyLevel;

    /* Parameters:
     * - instructions           : a set of instructions which includes all steps through a algorithm
     * - userActionToProceed    : number of user actions needed to proceed to the next instruction
     * - userActionInstructions : total number of user actions required to complete a sorting
    */
    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int userActionToProceed, int userActionInstructions)
    {
        base.Init(instructions);
        this.userActionToProceed = userActionToProceed;
        this.userActionInstructions = userActionInstructions;
        readyForNext = userActionToProceed;
        totalCorrect = 0;
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();
    }

    // Added methods from scoremanager

    public void IncrementTotalCorrect()
    {
        totalCorrect++;
    }

    public string DifficultyLevel
    {
        get { return difficultyLevel; }
        set { difficultyLevel = value; }
    }

    public int DifficultyMultiplier
    {
        get { return difficultyMultiplier; }
        set { difficultyMultiplier = value; }
    }

    public int CalculateScore()
    {
        if (GetComponent<Algorithm>().IsSortingComplete)
            return CalculateTotalScore();
        return CalculateIntermediateScore();
    }

    private int CalculateTotalScore()
    {
        totalScore += CalculateIntermediateScore() + (int)TimeSpent;
        return totalScore;
    }

    private int CalculateIntermediateScore()
    {
        return streakScore * difficultyMultiplier;
    }

    // Show of at the end of the user test
    public IEnumerator VisualizeScore()
    {
        // TODO: display score on blackboard at end of game
        yield return new WaitForSeconds(SCORE_UPDATE);
    }

    // Sets when User Test begins
    public void SetStartTime()
    {
        if (startTime == 0)
            startTime = Time.deltaTime;
    }

    // Sets when User Test ends
    public void SetEndTime()
    {
        if (endTime == 0 && GetComponent<Algorithm>().IsSortingComplete)
        {
            endTime = Time.deltaTime;
            timeSpent = endTime - startTime;
        }
    }

    public float TimeSpent
    {
        get { return timeSpent; }
    }

    public void IncrementStreak()
    {
        currentStreak++;
        streakScore = SCORE_PER_STREAK * currentStreak;
    }

    public void Mistake()
    {
        // Update score
        totalScore += CalculateIntermediateScore();

        // Update longest streak
        if (currentStreak > longestStreak)
            longestStreak = currentStreak;

        // Reset streak counter
        currentStreak = 0;
        streakScore = 0;
    }

    // Drop this function ?
    private int GetReduction()
    {
        // Do reduction to score
        switch (difficultyLevel)
        {
            case Util.BEGINNER: return 0; // No punishment
            case Util.INTERMEDIATE: case Util.EXAMINATION: return (SUBSTRACTION_PER_ERROR * GetComponent<UserTestManager>().TotalErrorCount) * difficultyMultiplier;
            default:
                Debug.LogError("Difficulty level '" + difficultyLevel + "' not implemented.");
                return 0;
        }
    }

    //


    public int TotalErrorCount
    {
        get { return totalErrorCount; }
    }

    public string GetExaminationResult()
    {
        string result = "Results from User Test:\n";

        // Add errors with some explanation | for now just the instruction ID
        result += "Errors:\n";
        if (errorLog.Count > 0)
        {
            foreach (KeyValuePair<string, int> entry in errorLog)
            {
                result += Util.TranslateInstructionForExamination(entry.Key) + ": " + entry.Value + "\n";
            }
        }
        else
            result += "> None, good job!";
        return result;
    }

    // Error counting
    public void ReportError(int sortingElementID)
    {
        string instructionID = instructions[currentInstructionNr].Instruction; //GetComponent<ElementManager>().GetSortingElement(sortingElementID).GetComponent<SortingElementBase>().Instruction.Instruction;
        //Debug.LogError("What went wrong: " + Util.TranslateInstructionForExamination(instructionID));
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
    public int UserActionToProceed
    {
        get { return userActionToProceed; }
        set { userActionToProceed = value; }
    }

    public override void ResetState()
    {
        base.ResetState();
        totalCorrect = 0;
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();

        // from score
        startTime = 0;
        endTime = 0;
        timeSpent = 0;
        totalScore = 0;
        streakScore = 0;
        currentStreak = 0;
        longestStreak = 0;
    }



    public override string FillInBlackboard()
    {
        return "Inst cleared: " + totalCorrect + "/" + userActionInstructions + "\nInst. nr.: " + CurrentInstructionNr + "\n" + Util.ModifyPluralString("error", totalErrorCount) + ": " + TotalErrorCount
            + "\nDebugging: " + GetInstruction().DebugInfo();
    }

}
