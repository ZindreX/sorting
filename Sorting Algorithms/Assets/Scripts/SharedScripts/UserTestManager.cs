using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : InstructionControlBase {

    /* -------------------------------------------- User Test Manager --------------------------------------------
     * 
     * 
    */

    // User test
    private int totalCorrect, totalErrorCount;

    /* readyForNext: incremented whenever a user does a move (value from 0 to N, where N is decied by the algorithm)
     *  - N: usually 1, so could be changed to a boolean, but for instance BubbleSort use 2
     * 
     * userActionToProceed: N (see above)
     * 
     * userActionInstruction: the number of instructions where the user needs to perform a action
     * 
    */
    private int readyForNext, userActionToProceed, userActionInstructions;

    private Dictionary<string, int> errorLog;

    // Score
    public readonly int SCORE_PER_STREAK = 10, SUBSTRACTION_PER_ERROR = 2;
    public readonly float SCORE_UPDATE = 0.1f;

    private float startTime, endTime, timeSpent;
    private int totalScore, currentStreak, longestStreak, difficultyMultiplier;
    private string difficultyLevel;

    private WaitForSeconds scoreUpdateDuration;

    /* Parameters:
     * - instructions           : a set of instructions which includes all steps through a algorithm
     * - userActionToProceed    : number of user actions needed to proceed to the next instruction
     * - userActionInstructions : total number of user actions required to complete a sorting
    */
    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int userActionToProceed, int userActionInstructions)
    {
        base.Init(instructions, userActionInstructions, false);
        this.userActionToProceed = userActionToProceed;
        this.userActionInstructions = userActionInstructions;
        readyForNext = userActionToProceed;
        totalCorrect = 0;
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();
        scoreUpdateDuration = new WaitForSeconds(SCORE_UPDATE);
    }

    // -------------------------------------------- Getters/Setters --------------------------------------------

    public string DifficultyLevel
    {
        get { return difficultyLevel; }
        //set { difficultyLevel = value; }
    }

    public int DifficultyMultiplier
    {
        get { return difficultyMultiplier; }
        set { difficultyMultiplier = value; }
    }

    /* Checking whether a new instruction can be given
     * ...
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

    public float TimeSpent
    {
        get { return timeSpent; }
    }

    public int TotalErrorCount
    {
        get { return totalErrorCount; }
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
        if (endTime == 0 && mainManager.GetTeachingAlgorithm().IsTaskCompleted)
        {
            endTime = Time.deltaTime;
            timeSpent = endTime - startTime;
        }
    }

    // -------------------------------------------- Report methods --------------------------------------------

    public void IncrementTotalCorrect()
    {
        audioManager.Play("Correct");

        if (mainManager.Settings.Algorithm == Util.BUBBLE_SORT && totalCorrect % 2 == 1) // Bubble sort sends 2x correct
            return;

        // > Increment
        // Streak
        currentStreak++;

        // Number of correct actions
        totalCorrect++;

        // Visual
        progressTracker.Increment();
    }

    public void Mistake()
    {
        audioManager.Play("Mistake");

        // Update score
        totalScore += CalculateIntermediateScore();

        // Update longest streak
        if (currentStreak > longestStreak)
            longestStreak = currentStreak;

        totalErrorCount++;

        // Reset streak counter
        currentStreak = 0;
    }


    // -------------------------------------------- Score stuff --------------------------------------------

    private int StreakScore()
    {
        return SCORE_PER_STREAK * currentStreak;
    }

    public int CalculateScore()
    {
        if (mainManager.GetTeachingAlgorithm().IsTaskCompleted)
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
        return StreakScore() * difficultyMultiplier;
    }

    // Drop this function ?
    private int GetReduction()
    {
        // Do reduction to score
        switch (difficultyLevel)
        {
            //case Util.BEGINNER: return 0; // No punishment
            //case Util.INTERMEDIATE: case Util.EXAMINATION: return (SUBSTRACTION_PER_ERROR * GetComponent<UserTestManager>().TotalErrorCount) * difficultyMultiplier;
            default:
                Debug.LogError("Difficulty level '" + difficultyLevel + "' not implemented.");
                return 0;
        }
    }

    // -------------------------------------------- Feedback stuff --------------------------------------------

    public string GetExaminationResult()
    {
        string result = "Results from User Test:\n";

        // Add errors with some explanation | for now just the instruction ID
        result += "Errors:\n";
        if (errorLog.Count > 0)
        {
            foreach (KeyValuePair<string, int> entry in errorLog)
            {
                result += UtilSort.TranslateInstructionForExamination(entry.Key) + ": " + entry.Value + "\n";
            }
        }
        else
            result += "> None, good job!";
        return result;
    }

    // Error counting
    public void ReportError(int sortingElementID)
    {
        string instructionID = instructions[currentInstructionNr].Instruction;      
        
        //totalErrorCount++;
        if (errorLog.ContainsKey(instructionID))
            errorLog[instructionID] += 1;
        else
            errorLog.Add(instructionID, 1);

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
        currentStreak = 0;
        longestStreak = 0;
        scoreUpdateDuration = null;
    }


    public override string FillInBlackboard()
    {
        return "Inst cleared: " + totalCorrect + "/" + userActionInstructions + "\nInst. nr.: " + CurrentInstructionNr + "\n" + UtilSort.ModifyPluralString("error", totalErrorCount) + ": " + TotalErrorCount; // + "\nDebugging: " + GetInstruction().DebugInfo();
    }

    // Show of at the end of the user test
    public IEnumerator VisualizeScore()
    {
        // TODO: display score on blackboard at end of game
        yield return scoreUpdateDuration;
    }
}
