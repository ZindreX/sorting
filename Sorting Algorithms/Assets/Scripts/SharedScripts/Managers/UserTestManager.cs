using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTestManager : InstructionControlBase {

    /* -------------------------------------------- User Test Manager --------------------------------------------
     * > Keeps tracks of the user tests
     *  - Score
     *  - Incorrect moves details
     *  - ...
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

    private float startTime, endTime, timeSpent;
    private int currentScore, totalScore, currentStreak, longestStreak;
    private int difficulty, difficultyMultiplier;

    private WaitForSeconds scoreUpdateDuration;

    /* Parameters:
     * - instructions           : a set of instructions which includes all steps through a algorithm
     * - userActionToProceed    : number of user actions needed to proceed to the next instruction
     * - userActionInstructions : total number of user actions required to complete a sorting
    */
    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int userActionToProceed, int userActionInstructions, int difficulty)
    {
        base.Init(instructions, userActionInstructions, false);
        this.userActionToProceed = userActionToProceed;
        this.userActionInstructions = userActionInstructions;
        readyForNext = userActionToProceed;

        // Score
        currentScore = 0;
        totalCorrect = 0;
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();
        scoreUpdateDuration = new WaitForSeconds(0.1f);

        this.difficulty = difficulty;
        difficultyMultiplier = difficulty;

        // Set start time
        SetStartTime();
    }

    // TODO: display all instructions in progress tracker
    public void InitUserTest(Dictionary<int, InstructionBase> instructions, int userActionToProceed, int difficulty)
    {
        base.Init(instructions, userActionInstructions, false);
        this.userActionToProceed = userActionToProceed;
        readyForNext = userActionToProceed;

        // Score
        currentScore = 0;
        totalCorrect = 0;
        totalErrorCount = 0;
        errorLog = new Dictionary<string, int>();
        scoreUpdateDuration = new WaitForSeconds(0.1f);

        this.difficulty = difficulty;
        difficultyMultiplier = difficulty;

        // Set start time
        SetStartTime();
    }


    // -------------------------------------------- Getters/Setters --------------------------------------------


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
            startTime = Time.time;
    }

    // Sets when User Test ends
    public void SetEndTime()
    {
        if (endTime == 0 && mainManager.GetTeachingAlgorithm().IsTaskCompleted)
        {
            endTime = Time.time;
            timeSpent = endTime - startTime;
        }
    }

    // -------------------------------------------- Report methods --------------------------------------------

    public void IncrementTotalCorrect()
    {
        audioManager.Play("Correct");
        totalCorrect++;

        if (mainManager.Settings.Algorithm == Util.BUBBLE_SORT && totalCorrect % 2 == 1) // Bubble sort sends 2x correct
            return;

        // Visual
        progressTracker.Increment();

        // Score
        currentStreak++;
        UpdateLongestStreak();
        currentScore += StreakScore();
    }

    public void Mistake()
    {
        audioManager.Play("Mistake");
        totalErrorCount++;

        //// Update score
        currentScore -= SUBSTRACTION_PER_ERROR * difficultyMultiplier;

        if (difficulty < Util.ADVANCED && currentScore < 0)
            currentScore = 0;

        // Reset streak counter
        currentStreak = 0;
    }


    // -------------------------------------------- Score stuff --------------------------------------------

    private void UpdateLongestStreak()
    {
        if (currentStreak > longestStreak)
            longestStreak = currentStreak;
    }

    private int StreakScore()
    {
        return SCORE_PER_STREAK * currentStreak;
    }

    public int CalculateScore()
    {
        // Current score
        totalScore = currentScore;

        // Time
        totalScore += (int)TimeReward();

        // Difficulty
        if (difficulty >= Util.INTERMEDIATE)
            totalScore *= difficultyMultiplier;

        return totalScore;
    }

    // Calculate a time reward (needs adjustments)
    private float TimeReward()
    {
        int timeRewardLimit = 0;

        if (difficulty <= Util.PSEUDO_CODE_HIGHTLIGHT_MAX_DIFFICULTY)
            timeRewardLimit = numberOfInstructions * (int)mainManager.Settings.AlgorithmSpeed;

        // Time given: 5 sec/user action instruction + all instructions * inverse difficulty
        timeRewardLimit = userActionInstructions * 5 + instructions.Count * (Util.EXAMINATION - difficulty);

        float reward = timeRewardLimit - timeSpent;
        reward *= 10;

        return (reward >= 0) ? reward : 0;
    }

    private int CalculateIntermediateScore()
    {
        return StreakScore() * difficultyMultiplier;
    }

    // -------------------------------------------- Feedback stuff --------------------------------------------


    public string IncorrectActionDetails()
    {
        // Add errors with some explanation | for now just the instruction ID
        string result = "";

        if (errorLog.Count > 0)
        {
            foreach (KeyValuePair<string, int> entry in errorLog)
            {
                result += "> " + entry.Key + ": " + entry.Value + "\n"; // UtilSort.TranslateInstructionForExamination(entry.Key)
            }
        }
        else
            result += "> Perfect! No mistakes";
        return result;
    }

    public string GetExaminationResult()
    {
        string result = "Difficulty: " + Util.difficultyConverterDict[difficulty] + "    (x" + difficultyMultiplier + ")";

        result += "\nTime: " + (int)TimeSpent + " seconds        (Time reward: " + (int)TimeReward() + ")";

        // Streak
        UpdateLongestStreak();
        result += "\nLongest streak: " + longestStreak;

        // Score
        result += "\nTotal score: " + totalScore;
        result += "\nIncorrect moves: " + TotalErrorCount;
        return result;
    }

    // Error counting
    public void ReportError(int sortingElementID)
    {
        if (instructions != null)
        {
            string instructionID = instructions[currentInstructionNr].Instruction;      
        
            //totalErrorCount++;
            if (errorLog.ContainsKey(instructionID))
                errorLog[instructionID] += 1;
            else
                errorLog.Add(instructionID, 1);
        }
    }

    public void ReportError()
    {
        if (instructions != null)
        {
            string instructionID = instructions[currentInstructionNr].Instruction;

            if (errorLog.ContainsKey(instructionID))
                errorLog[instructionID] += 1;
            else
                errorLog.Add(instructionID, 1);
        }
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

        currentScore = 0;
        totalScore = 0;
        currentStreak = 0;
        longestStreak = 0;
        scoreUpdateDuration = null;
    }


    public override string FillInBlackboard()
    {
        string blackboardFill = "Current streak: " + currentStreak;
        blackboardFill += "\nLongest streak: " + longestStreak;
        blackboardFill += "\nScore: " + currentScore;
        return blackboardFill;
    }
}
