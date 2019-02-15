﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScore {

    // Use time to calculate final score?
    void SetStartTime();
    void SetEndTime();
    float TimeSpent { get; }

    int DifficultyMultiplier { get; set; }
    string DifficultyLevel { get; set; }

    // Returns intermediate score / total score (sorting complete)
    int CalculateScore();

    // Visualize total score stuff
    IEnumerator VisualizeScore();

    // Increase score
    // Streak system
    void IncrementStreak();

    // Reduce score (?)
    void Mistake();

    // Reset
    void ResetScore();

}