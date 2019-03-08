using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSettings : SettingsBase {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */

    // Algorithm settings
    [Space(5)]
    [Header("Sorting settings")]
    [SerializeField]
    private bool allowDuplicates;

    private int numberOfElements;
    private string sortingCase;

    [Space(2)]
    [Header("Objects")]
    [SerializeField]
    private SortMain sortMain;

    // ************** DEBUGGING ****************

    [Space(2)]
    [Header("Debugging")]

    [SerializeField]
    private AlgorithmEditor algorithmEditor;
    private enum AlgorithmEditor { BubbleSort, InsertionSort, BucketSort, MergeSort }

    [Space(2)]
    [Header("Rules / extra")]
    [SerializeField]
    private NumberofElementsEditor numberofElementsEditor;
    private enum NumberofElementsEditor { two, four, six, eight }

    [SerializeField]
    private CaseEditor sortingCaseEditor;
    private enum CaseEditor { none, best, worst }

    private void Start()
    {
        // Debugging editor (fast edit settings)
        if (useDebuggingSettings)
            PrepareSettings();
        else
        {
            // Init settings
            Algorithm = Util.BUBBLE_SORT;
            TeachingMode = Util.DEMO;
            NumberOfElements = 8;
            SortingCase = UtilSort.NONE;
            AlgorithmSpeedLevel = 1;
            Difficulty = 1;
            Duplicates = true; //SetDuplicates();
        }
        tooltips.text = "";

        // Hide inactive subsection buttons
        HideSubSections();
    }

    public override void PrepareSettings()
    {
        base.PrepareSettings();

        switch ((int)algorithmEditor)
        {
            case 0: Algorithm = Util.BUBBLE_SORT; break;
            case 1: Algorithm = Util.INSERTION_SORT; break;
            case 2: Algorithm = Util.BUCKET_SORT; break;
            case 3: Algorithm = Util.MERGE_SORT; break;
        }

        switch ((int)sortingCaseEditor)
        {
            case 0: SortingCase = UtilSort.NONE; break;
            case 1: SortingCase = UtilSort.BEST_CASE; break;
            case 2: SortingCase = UtilSort.WORST_CASE; break;
        }

        NumberOfElements = ((int)numberofElementsEditor + 1) * 2;

        if (allowDuplicates)
            Duplicates = allowDuplicates; // Toggle false -> true
        else
        {
            // No color/text if not toggle 2x (TODO: improve)
            Duplicates = allowDuplicates; // Toggle false -> true
            Duplicates = allowDuplicates; // Toggle true -> false (correct color/text)
        }


        Debug.Log("Algorithm: " + algorithm + ", teachingmode: " + teachingMode + ", difficulty: " + difficulty + ", case: " + sortingCase + ", #: " + numberOfElements + ", allowdup: " + allowDuplicates);
    }
    // ********** DEBUGGING **************


    [Header("New setup")]


    [Space(4)]
    [Header("Sub settings")]

    [SerializeField]
    private GameObject subSettingsTitle;

    [SerializeField]
    private GameObject[] difficultyButtons, demoSpeedButtons, numberOfElementsButtons, caseButtons, duplicateButtons;


    protected override MainManager MainManager
    {
        get { return sortMain; }
        set { sortMain = (SortMain)value; }
    }

    public override bool IsDemo()
    {
        return teachingMode == Util.DEMO || teachingMode == Util.STEP_BY_STEP;
    }

    // Number of elements used
    public int NumberOfElements
    {
        get { return numberOfElements; }
        set { numberOfElements = value; FillTooltips("Number of elements:\n" + value); } // remove
    }

    public void IncreaseNumberOfElements()
    {
        if (numberOfElements < UtilSort.MAX_NUMBER_OF_ELEMENTS)
        {
            numberOfElements += 1;
            FillTooltips("#Elements:\n" + numberOfElements);
            //buttons[] >>> fill in text #elements
        }
        else
        {
            FillTooltips("Can't add more elements!");
        }
    }

    public void ReduceNumberOfElements()
    {
        if (numberOfElements > 2)
        {
            numberOfElements -= 1;
            FillTooltips("#Elements: " + numberOfElements);
        }
        else
        {
            FillTooltips("Minimum 2 elements.");
        }
    }

    // Duplicates can occour in the problem sets (used by the in game button)
    public void SetDuplicates()
    {
        allowDuplicates = !allowDuplicates;
        FillTooltips("Allow duplicates:\n" + allowDuplicates);
    }

    // Init setup duplicates
    public bool Duplicates
    {
        get { return allowDuplicates; }
        set { allowDuplicates = value; FillTooltips("Allow duplicates:\n" + Util.EnabledToString(value)); SetActiveButton(UtilSort.DUPLICATES); }
    }
    

    // None, Best-case, Worst-case (not implemented yet)
    public string SortingCase
    {
        get { return sortingCase; }
        set { sortingCase = value; FillTooltips("Sorting case:\n" + value); SetActiveButton(value); }
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

}
