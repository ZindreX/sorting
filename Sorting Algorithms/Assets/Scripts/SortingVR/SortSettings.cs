using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSettings : MonoBehaviour, ISettings {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */


    // ************** DEBUGGING ****************
    [Header("Overall settings")]
    [SerializeField]
    private AlgorithmEditor algorithmEditor;
    private enum AlgorithmEditor { BubbleSort, InsertionSort, BucketSort }

    [SerializeField]
    private TeachingModeEditor teachingModeEditor;
    private enum TeachingModeEditor { demo, stepByStep, userTest }

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

    [SerializeField]
    private TutorialSpeedEditor tutorialSpeed;
    private enum TutorialSpeedEditor { slow, normal, fast }

    // Algorithm settings
    private int numberOfElements = 8, difficulty = Util.BEGINNER;
    private string algorithm = Util.BUBBLE_SORT, teachingMode = Util.DEMO, sortingCase = UtilSort.NONE;
    private float demoStepDuration;
    private bool allowDuplicates = true;

    [Space(2)]
    [Header("Objects")]
    [SerializeField]
    private SortMain sortMain;

    public void PrepareSettings()
    {
        switch ((int)algorithmEditor)
        {
            case 0: algorithm = Util.BUBBLE_SORT; break;
            case 1: algorithm = Util.INSERTION_SORT; break;
            case 2: algorithm = Util.BUCKET_SORT; break;
        }

        switch ((int)teachingModeEditor)
        {
            case 0: teachingMode = Util.DEMO; break;
            case 1: teachingMode = Util.STEP_BY_STEP; break;
            case 2: teachingMode = Util.USER_TEST; break;
        }

        Debug.Log(">>>>> If any error(s) <--- check here");
        difficulty = (int)difficultyEditor + 1;

        switch ((int)sortingCaseEditor)
        {
            case 0: sortingCase = UtilSort.NONE; break;
            case 1: sortingCase = UtilSort.BEST_CASE; break;
            case 2: sortingCase = UtilSort.WORST_CASE; break;
        }

        numberOfElements = ((int)numberofElementsEditor + 1) * 2;

        switch ((int)tutorialSpeed)
        {
            case 0: demoStepDuration = 2f; break;
            case 1: demoStepDuration = 1f; break;
            case 2: demoStepDuration = 0.5f; break;
        }

        allowDuplicates = allowDupEditor;

        Debug.Log("Teachingmode: " + teachingMode + ", difficulty: " + difficulty + ", case: " + sortingCase + ", #: " + numberOfElements + ", allowdup: " + allowDuplicates);
    }
    // ********** DEBUGGING **************


    [Header("New setup")]
    [SerializeField]
    public GameObject sortAlgorithmsObj, displayUnitManagerObj, settingsObj;

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
        get { return demoStepDuration; }
        set { demoStepDuration = value; blackboard.ChangeText(blackboard.TextIndex, "Demo speed: " + value + " seconds"); }
    }

    public void StartSorting()
    {
        sortMain.InstantiateSetup();
    }

    public void StopSorting()
    {
        sortMain.DestroyAndReset();
    }

    public void SetSettingsActive(bool active)
    {
        settingsObj.SetActive(active);
    }

    // Check if it's a Tutorial (including stepbystep for simplicity, might fix this later)
    public bool IsDemo()
    {
        return teachingMode == Util.DEMO || teachingMode == Util.STEP_BY_STEP;
    }

    // Check if it's StepByStep (not used?)
    public bool IsStepByStep()
    {
        return teachingMode == Util.STEP_BY_STEP;
    }

    // Check if it's UserTest
    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

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

    public TeachingAlgorithm GetAlgorithm()
    {
        switch (algorithm)
        {
            case Util.BUBBLE_SORT: return sortAlgorithmsObj.GetComponent<BubbleSort>();
            case Util.INSERTION_SORT: return sortAlgorithmsObj.GetComponent<InsertionSort>();
            case Util.BUCKET_SORT: return sortAlgorithmsObj.GetComponent<BucketSort>();
            default: Debug.LogError("'" + algorithm + "' not valid"); return null;
        }
    }

}
