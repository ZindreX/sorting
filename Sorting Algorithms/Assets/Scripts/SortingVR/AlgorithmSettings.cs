using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmSettings : MonoBehaviour {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */

    // ************** DEBUGGING ****************
    [SerializeField]
    private TeachingModeEditor teachingModeEditor;
    private enum TeachingModeEditor { demo, stepByStep, userTest }

    [SerializeField]
    private DifficultyEditor difficultyEditor;
    private enum DifficultyEditor { beginner, intermediate, advanced, examination }

    [SerializeField]
    private NumberofElementsEditor numberofElementsEditor;
    private enum NumberofElementsEditor { two, four, six, eight }

    [SerializeField]
    private bool allowDupEditor = false;

    [SerializeField]
    private CaseEditor sortingCaseEditor;
    private enum CaseEditor { none, best, worst }

    [SerializeField]
    private TutorialSpeedEditor tutorialSpeed;
    private enum TutorialSpeedEditor { slow, normal, fast }

    private void SettingsFromEditor()
    {
        switch ((int)teachingModeEditor)
        {
            case 0: teachingMode = UtilSort.DEMO; break;
            case 1: teachingMode = UtilSort.STEP_BY_STEP; break;
            case 2: teachingMode = UtilSort.USER_TEST; break;
        }

        switch ((int)difficultyEditor)
        {
            case 0: difficulty = UtilSort.BEGINNER; break;
            case 1: difficulty = UtilSort.INTERMEDIATE; break;
            case 2: difficulty = UtilSort.ADVANCED; break;
            case 3: difficulty = UtilSort.EXAMINATION; break;
        }

        switch ((int)sortingCaseEditor)
        {
            case 0: sortingCase = UtilSort.NONE; break;
            case 1: sortingCase = UtilSort.BEST_CASE; break;
            case 2: sortingCase = UtilSort.WORST_CASE; break;
        }

        switch ((int)numberofElementsEditor)
        {
            case 0: numberOfElements = 2; break;
            case 1: numberOfElements = 4; break;
            case 2: numberOfElements = 6; break;
            case 3: numberOfElements = 8; break;
        }

        switch ((int)tutorialSpeed)
        {
            case 0: algorithmManager.Algorithm.Seconds = 2f; break;
            case 1: algorithmManager.Algorithm.Seconds = 1f; break;
            case 2: algorithmManager.Algorithm.Seconds = 0.5f; break;
        }

        allowDuplicates = allowDupEditor;

        Debug.Log("Teachingmode: " + teachingMode + ", difficulty: " + difficulty + ", case: " + sortingCase + ", #: " + numberOfElements + ", allowdup: " + allowDuplicates);
    }
    // ********** DEBUGGING **************

    // Algorithm settings
    private int numberOfElements = 8, difficulty = UtilSort.BEGINNER;
    private string algorithm = UtilSort.BUBBLE_SORT, teachingMode = UtilSort.DEMO, sortingCase = UtilSort.NONE;
    private bool allowDuplicates = true;

    [SerializeField]
    private AlgorithmManagerBase algorithmManager;

    [SerializeField]
    private Blackboard blackboard;

    [SerializeField]
    private GameObject subSettingsTitle;

    [SerializeField]
    private GameObject[] difficultyButtons, tutorialSpeedButtons, numberOfElementsButtons, caseButtons, duplicateButtons;

    private void Start()
    {
        // Debugging
        SettingsFromEditor();
        ChangeSubSettingsDisplay(TeachingMode);
    }

    private void Update()
    {
        
    }

    public string Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; blackboard.ChangeText(blackboard.TextIndex, "Algorithm: " + value); }
    }

    // Tutorial, Step-By-Step, or User Test
    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; ChangeSubSettingsDisplay(value); blackboard.ChangeText(blackboard.TextIndex, "Teaching mode: " + value); }
    }

    // Beginner, Intermediate, or Examination
    public int Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; blackboard.ChangeText(blackboard.TextIndex, "Difficulty: " + value); }
    }

    // Number of elements used
    public int NumberOfElements
    {
        get { return numberOfElements; }
        set { numberOfElements = value; blackboard.ChangeText(blackboard.TextIndex, "Number of elements: " + value); }
    }

    // Duplicates can occour in the problem sets (not implemented yet)
    public bool Duplicates
    {
        get { return allowDuplicates; }
        set { allowDuplicates = value; blackboard.ChangeText(blackboard.TextIndex, "Duplicates: " + UtilSort.EnabledToString(value)); }
    }

    // None, Best-case, Worst-case (not implemented yet)
    public string SortingCase
    {
        get { return sortingCase; }
        set { sortingCase = value; blackboard.ChangeText(blackboard.TextIndex, "Case activated: " + value); }
    }

    public float DemoSpeed
    {
        get { return algorithmManager.Algorithm.Seconds; }
        set { algorithmManager.Algorithm.Seconds = value; blackboard.ChangeText(blackboard.TextIndex, "Demo speed: " + value + " seconds"); }
    }


    public void StartSorting()
    {
        algorithmManager.InstantiateSetup();
    }

    public void StopSorting()
    {
        algorithmManager.DestroyAndReset();
    }

    // Check if it's a Tutorial (including stepbystep for simplicity, might fix this later)
    public bool IsDemo()
    {
        return teachingMode == UtilSort.DEMO || teachingMode == UtilSort.STEP_BY_STEP;
    }

    // Check if it's StepByStep (not used?)
    public bool IsTutorialStep()
    {
        return teachingMode == UtilSort.STEP_BY_STEP;
    }

    // Check if it's UserTest
    public bool IsUserTest()
    {
        return teachingMode == UtilSort.USER_TEST;
    }

    // Changes the sub settings (middle wall w/buttons) based on the chosen teaching mode
    private void ChangeSubSettingsDisplay(string teachingMode)
    {
        switch (teachingMode)
        {
            case UtilSort.DEMO:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "Demo speed";
                ActiveButtons(false, true);
                break;
            case UtilSort.STEP_BY_STEP:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "";
                ActiveButtons(false, false);
                algorithmManager.Algorithm.Seconds = 0.5f;
                break;
            case UtilSort.USER_TEST:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "Difficulty";
                ActiveButtons(true, false);
                algorithmManager.Algorithm.Seconds = 0.5f;
                break;
        }
    }

    private void ActiveButtons(bool diffActive, bool speedActive)
    {
        foreach (GameObject obj in difficultyButtons)
        {
            obj.SetActive(diffActive);
            //obj.active = diffActive;
        }
        foreach (GameObject obj in tutorialSpeedButtons)
        {
            obj.SetActive(speedActive);
            //obj.active = speedActive;
        }
    }

}
