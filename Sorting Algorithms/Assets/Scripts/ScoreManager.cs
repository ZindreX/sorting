using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScore {

    /* -------------------------------------------- Managing the score --------------------------------------------
     * > Increase streak for each correct move by the user
     * > Reset streak when the user does a mistake
     * 
    */

    public readonly int SCORE_PER_STREAK = 10, SUBSTRACTION_PER_ERROR = 2;
    public readonly float SCORE_UPDATE = 0.1f;

    private float startTime, endTime, timeSpent;
    private int streakScore, totalScore, currentStreak, longestStreak, difficultyMultiplier;
    private string difficultyLevel;

    void Awake()
    {
        ResetScore();
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

    public string FillInBlackboard()
    {
        //return "Streak: " + streakScore + "   (x" + difficultyMultiplier + ")\n\nTotal Score: " + totalScore;
        return "Streak: " + currentStreak + " (" + longestStreak + ")\nTotal Score: " + totalScore;
    }

    public void ResetScore()
    {
        startTime = 0;
        endTime = 0;
        timeSpent = 0;
        totalScore = 0;
        streakScore = 0;
        currentStreak = 0;
        longestStreak = 0;
    }
}
