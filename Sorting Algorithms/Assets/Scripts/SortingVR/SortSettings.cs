using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSettings : TeachingSettings {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */

    // Algorithm settings
    [Space(5)]
    [Header("Sorting settings")]
    [SerializeField]
    private bool allowDuplicates;

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
    [Range(2, 8)]
    private int numberOfElements;

    [SerializeField]
    private CaseEditor sortingCaseEditor;
    private enum CaseEditor { none, best, worst }

    protected override void Start()
    {
        // Debugging editor (fast edit settings)
        if (useDebuggingSettings)
            GetSettingsFromEditor();
        else
        {
            base.Start();

            // Init settings
            Algorithm = Util.BUBBLE_SORT;
            NumberOfElements = 8;
            SortingCase = UtilSort.NONE;
            Duplicates = true;
        }
        tooltips.text = "";

        InitButtons();
    }

    protected override void GetSettingsFromEditor()
    {
        base.GetSettingsFromEditor(); // remove?

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
        Duplicates = allowDuplicates;


        Debug.Log("Algorithm: " + algorithm + ", teachingmode: " + teachingMode + ", difficulty: " + difficulty + ", case: " + sortingCase + ", #: " + numberOfElements + ", allowdup: " + allowDuplicates);
    }
    // ********** DEBUGGING **************



    protected override void InitButtons()
    {
        base.InitButtons();

        InitButtonState(Util.ALGORITHM, algorithm);
        InitButtonState(Util.TEACHING_MODE, teachingMode);
        InitButtonState(Util.DIFFICULTY, Util.DIFFICULTY, difficulty);
        InitButtonState(Util.DEMO_SPEED, Util.DEMO_SPEED, algSpeed);
        InitButtonState(UtilSort.SORTING_CASE, sortingCase);
        InitButtonState(UtilSort.DUPLICATES, UtilSort.DUPLICATES, allowDuplicates);

    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        // Fill information on the "display" on the settings menu about the button just clicked
        FillTooltips(itemDescription);

        switch (sectionID)
        {
            case Util.ALGORITHM: Algorithm = itemID; break;
            case UtilSort.SORTING_CASE: SortingCase = itemID; break;
            case UtilSort.NUMBER_OF_ELEMENTS: ChangeNumberOfElements(itemID); break;
            case UtilSort.DUPLICATES: Duplicates = Util.ConvertStringToBool(itemDescription); break;
            default: base.UpdateInteraction(sectionID, itemID, itemDescription); break;
        }
    }

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
        set { numberOfElements = value; }
    }

    public void ChangeNumberOfElements(string buttonID)
    {
        bool increaseNumberOfElements = buttonID == Util.PLUS;
        if (increaseNumberOfElements)
        {
            if (numberOfElements < UtilSort.MAX_NUMBER_OF_ELEMENTS)
            {
                numberOfElements += 1;
                FillTooltips("#Elements:" + numberOfElements);
                //buttons[] >>> fill in text #elements
            }
            else
            {
                FillTooltips("Can't add more elements!");
            }
        }
        else
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
    }

    // Duplicates can occour in the problem sets (used by the in game button)
    public void SetDuplicates()
    {
        allowDuplicates = !allowDuplicates;
    }

    // Init setup duplicates
    public bool Duplicates
    {
        get { return allowDuplicates; }
        set { allowDuplicates = value; }
    }
    

    // None, Best-case, Worst-case (not implemented yet)
    public string SortingCase
    {
        get { return sortingCase; }
        set { sortingCase = value; }
    }


}
