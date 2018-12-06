using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlgorithmManager {
    // NOT USED

    void InitAlgorithm();
    void InitRules();
    void PerformAlgorithmTutorial();
    void PerformAlgorithmUserTest();

    bool PrepareNextInstruction();
    HolderBase GetCorrectHolder();
    bool IsValidSetup();
}
