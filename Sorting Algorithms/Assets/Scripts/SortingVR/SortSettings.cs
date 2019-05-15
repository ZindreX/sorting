using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortSettings : TeachingSettings {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */

    // Algorithm settings
    [Space(5)]
    [Header("Sorting settings")]

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
    private bool allowDuplicates;

    [SerializeField]
    [Range(-100, 99)]
    private int elementMinValue;

    [SerializeField]
    [Range(-99, 100)]
    private int elementMaxValue;

    [SerializeField]
    [Range(2, 8)]
    private int numberOfElements;

    [SerializeField]
    [Range(2, 10)]
    private int numberOfBuckets;

    [SerializeField]
    private CaseEditor sortingCaseEditor;
    private enum CaseEditor { none, best, worst }
    private string sortingCase;


    [Space(10)]
    [SerializeField]
    private TextMeshPro numberOfElementsText;
    
    [SerializeField]
    private TextMeshPro numberOfBucketsText;

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
            numberOfElementsText.text = numberOfElements.ToString();

            SortingCase = UtilSort.NONE;
            Duplicates = true;

            NumberOfBuckets = 10;
            numberOfBucketsText.text = numberOfBuckets.ToString();
        }
        tooltips.text = "";

        InitButtons();
    }

    protected override void GetSettingsFromEditor()
    {
        base.GetSettingsFromEditor(); // remove?

        numberOfElementsText.text = numberOfElements.ToString();
        numberOfBucketsText.text = numberOfBuckets.ToString();

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
        InitButtonState(UtilSort.SORTING_CASE, sortingCase);
        InitButtonState(UtilSort.DUPLICATES, UtilSort.DUPLICATES, allowDuplicates);

    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        // Fill information on the "display" on the settings menu about the button just clicked
        FillTooltips(itemDescription, false);
        Debug.Log("Section: " + sectionID + ", item: " + itemID + ", description: " + itemDescription);

        switch (sectionID)
        {
            case Util.ALGORITHM: Algorithm = itemID; break;
            case UtilSort.SORTING_CASE: SortingCase = itemID; break;
            case UtilSort.NUMBER_OF_ELEMENTS: ChangeNumberOfElements(itemID); break;
            case UtilSort.DUPLICATES: Duplicates = Util.ConvertStringToBool(itemDescription); break;
            case UtilSort.NUMBER_OF_BUCKETS: ChangeNumberOfBuckets(itemID); break;
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

    public int ElementMinValue
    {
        get { return elementMinValue; }
    }

    public int ElementMaxValue
    {
        get { return elementMaxValue; }
    }

    // Number of elements used
    public int NumberOfElements
    {
        get { return numberOfElements; }
        set { numberOfElements = value; }
    }

    public int NumberOfBuckets
    {
        get { return numberOfBuckets; }
        set { numberOfBuckets = value; }
    }

    public void ChangeNumberOfElements(string buttonID)
    {
        bool increaseNumberOfElements = buttonID == Util.PLUS;
        if (increaseNumberOfElements)
        {
            if (numberOfElements < UtilSort.MAX_NUMBER_OF_ELEMENTS)
            {
                numberOfElements += 1;
                numberOfElementsText.text = numberOfElements.ToString();
            }
            else
            {
                FillTooltips("Can't add more elements!", false);
            }
        }
        else
        {
            if (numberOfElements > 2)
            {
                numberOfElements -= 1;
                numberOfElementsText.text = numberOfElements.ToString();
            }
            else
            {
                FillTooltips("Minimum 2 elements.", false);
            }
        }
    }

    public void ChangeNumberOfBuckets(string buttonID)
    {
        bool increaseNumberOfBuckets = buttonID == Util.PLUS;
        if (increaseNumberOfBuckets)
        {
            if (numberOfBuckets < UtilSort.MAX_NUMBER_OF_BUCKETS)
            {
                numberOfBuckets++;
                while (elementMaxValue % numberOfBuckets != 0)
                    numberOfBuckets++;

                numberOfBucketsText.text = numberOfBuckets.ToString();
            }
            else
            {
                FillTooltips("Maximum 10 buckets.", false);
            }
        }
        else
        {
            if (numberOfBuckets > 2)
            {
                numberOfBuckets--;
                while (elementMaxValue % numberOfBuckets != 0)
                    numberOfBuckets--;

                numberOfBucketsText.text = numberOfBuckets.ToString();
            }
            else
            {
                FillTooltips("Minimum 2 buckets.", false);
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
