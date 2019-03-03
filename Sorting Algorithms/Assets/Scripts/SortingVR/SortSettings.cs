using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSettings : SettingsBase {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */


    // ************** DEBUGGING ****************

    [SerializeField]
    private SortMain sortmain;

    [Space(5)]
    [Header("Sorting settings")]
    [SerializeField]
    private AlgorithmEditor algorithmEditor;
    private enum AlgorithmEditor { BubbleSort, InsertionSort, BucketSort, MergeSort }

    [SerializeField]
    private DifficultyEditor difficultyEditor;
    private enum DifficultyEditor { beginner, intermediate, advanced, examination }

    [Space(2)]
    [Header("Rules / extra")]
    [SerializeField]
    private NumberofElementsEditor numberofElementsEditor;
    private enum NumberofElementsEditor { two, four, six, eight }

    [SerializeField]
    private bool allowDupEditor = false;

    [SerializeField]
    private CaseEditor sortingCaseEditor;
    private enum CaseEditor { none, best, worst }

    // Algorithm settings
    private int numberOfElements = 8, difficulty = Util.BEGINNER;
    //private string algorithm = Util.BUBBLE_SORT, teachingMode = Util.DEMO;
    private string sortingCase = UtilSort.NONE;
    private bool allowDuplicates = true;

    [Space(2)]
    [Header("Objects")]
    [SerializeField]
    private SortMain sortMain;

    public override void PrepareSettings()
    {
        switch ((int)algorithmEditor)
        {
            case 0: algorithm = Util.BUBBLE_SORT; break;
            case 1: algorithm = Util.INSERTION_SORT; break;
            case 2: algorithm = Util.BUCKET_SORT; break;
            case 3: algorithm = Util.MERGE_SORT; break;
        }

        switch ((int)teachingModeEditor)
        {
            case 0: teachingMode = Util.DEMO; break;
            case 1: teachingMode = Util.STEP_BY_STEP; break;
            case 2: teachingMode = Util.USER_TEST; break;
        }

        difficulty = (int)difficultyEditor + 1;

        switch ((int)sortingCaseEditor)
        {
            case 0: sortingCase = UtilSort.NONE; break;
            case 1: sortingCase = UtilSort.BEST_CASE; break;
            case 2: sortingCase = UtilSort.WORST_CASE; break;
        }

        numberOfElements = ((int)numberofElementsEditor + 1) * 2;

        switch ((int)algorithmSpeed)
        {
            case 0: algorithmSpeed = 2f; break;
            case 1: algorithmSpeed = 1f; break;
            case 2: algorithmSpeed = 0.5f; break;
        }

        allowDuplicates = allowDupEditor;

        Debug.Log("Teachingmode: " + teachingMode + ", difficulty: " + difficulty + ", case: " + sortingCase + ", #: " + numberOfElements + ", allowdup: " + allowDuplicates);
    }
    // ********** DEBUGGING **************


    [Header("New setup")]
    [SerializeField]
    public GameObject settingsObj;

    [Space(4)]
    [Header("Sub settings")]
    [SerializeField]
    private Blackboard blackboard;

    [SerializeField]
    private GameObject subSettingsTitle;

    [SerializeField]
    private GameObject[] difficultyButtons, demoSpeedButtons, numberOfElementsButtons, caseButtons, duplicateButtons;

    private void Start()
    {
        // Debugging
        //SettingsFromEditor();
        ChangeSubSettingsDisplay(TeachingMode);
    }

    protected override MainManager MainManager
    {
        get { return sortmain; }
    }

    //public string Algorithm
    //{
    //    get { return algorithm; }
    //    set { algorithm = value; blackboard.ChangeText(blackboard.TextIndex, "Algorithm: " + value); }
    //}

    // Tutorial, Step-By-Step, or User Test
    //public string TeachingMode
    //{
    //    get { return teachingMode; }
    //    set { teachingMode = value; ChangeSubSettingsDisplay(value); blackboard.ChangeText(blackboard.TextIndex, "Teaching mode: " + value); }
    //}

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

    //public float DemoSpeed
    //{
    //    get { return demoStepDuration; }
    //    set { demoStepDuration = value; blackboard.ChangeText(blackboard.TextIndex, "Demo speed: " + value + " seconds"); }
    //}

    //public void StartTask()
    //{
    //    sortMain.InstantiateSetup();
    //}

    //public void StopTask()
    //{
    //    sortMain.DestroyAndReset();
    //}

    public void SetSettingsActive(bool active)
    {
        settingsObj.SetActive(active);
    }

    // Check if it's a Tutorial (including stepbystep for simplicity, might fix this later)
    //public bool IsDemo()
    //{
    //    return teachingMode == Util.DEMO || teachingMode == Util.STEP_BY_STEP;
    //}

    //// Check if it's StepByStep (not used?)
    //public bool IsStepByStep()
    //{
    //    return teachingMode == Util.STEP_BY_STEP;
    //}

    //// Check if it's UserTest
    //public bool IsUserTest()
    //{
    //    return teachingMode == Util.USER_TEST;
    //}

    // Changes the sub settings (middle wall w/buttons) based on the chosen teaching mode
    private void ChangeSubSettingsDisplay(string teachingMode)
    {
        switch (teachingMode)
        {
            case Util.DEMO:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "Demo speed";
                ActiveButtons(false, true);
                break;
            case Util.STEP_BY_STEP:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "";
                ActiveButtons(false, false);
                sortMain.GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(0.5f);
                break;
            case Util.USER_TEST:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "Difficulty";
                ActiveButtons(true, false);
                sortMain.GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(0.5f);
                break;
        }
    }

    /* Activate / deactivate sub settings buttons based on user input
     * - Display demo speed buttons if demo
     * - Display difficulty buttons if user test
    */
    private void ActiveButtons(bool diffActive, bool speedActive)
    {
        foreach (GameObject obj in difficultyButtons)
        {
            obj.SetActive(diffActive);
            //obj.active = diffActive;
        }
        foreach (GameObject obj in demoSpeedButtons)
        {
            obj.SetActive(speedActive);
            //obj.active = speedActive;
        }
    }

}
