using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SortAlgorithm : TeachingAlgorithm, IAlgorithm {

    // value1 <- pivot value, value2 <- compare value (usually)

    protected int minValue, maxValue;
    protected int value1 = UtilSort.INIT_STATE, value2 = UtilSort.INIT_STATE - 1;
    protected string element1Value, element2Value;

    protected List<int> prevHighlight = new List<int>();
    protected SortMain sortMain;

    public void InitSortAlgorithm(SortMain sortMain, float algorithmSpeed)
    {
        this.sortMain = sortMain;
        InitTeachingAlgorithm(algorithmSpeed);

        minValue = sortMain.ElementManager.MinValue;
        maxValue = sortMain.ElementManager.MaxValue;
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_LOOP_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.END_LOOP_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.INCREMENT_VAR_I);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.SET_VAR_J);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_VAR_J);
    }

    public override float LineSpacing
    {
        get { return UtilSort.SPACE_BETWEEN_CODE_LINES; }
    }

    public override float FontSize
    {
        get { return 2f; }
    }

    public override float AdjustYOffset
    {
        get { return 0.25f; }
    }

    public override MainManager MainManager
    {
        get { return sortMain; }
        //set { sortMain = (SortMain)value; }
    }

    protected void PreparePseudocodeValue(int value, int elementNr)
    {
        switch (elementNr)
        {
            case 1: value1 = value; element1Value = value.ToString(); break;
            case 2: value2 = value; element2Value = value.ToString(); break;
        }
    }



    public override void ResetSetup()
    {
        value1 = UtilSort.INIT_STATE;
        value2 = UtilSort.INIT_STATE;
        prevHighlight = new List<int>();
        element1Value = "";
        element2Value = "";
        //sortMain = null;
        base.ResetSetup();
    }


    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------

    // To do stuff important for individual classes
    public abstract void Specials(string method, int number, bool activate); // used?

    /* Tutorial of the chosen sorting algorithm
     * - No interaction from the user (except for settings)
    */
    public abstract IEnumerator Demo(GameObject[] list);

    /* Step by step execution of the chosen sorting algorithm
     * - The user can progress through the algorithm one step at the time
    */
    //public abstract void ExecuteStepByStepOrder(InstructionBase instruction, bool gotElement, bool increment);

    /* Creates a instruction dictionary
     * - Used in: User test, Step by Step
    */
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

