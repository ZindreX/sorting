using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SortAlgorithm : TeachingAlgorithm, IAlgorithm {

    // value1 <- pivot value, value2 <- compare value (usually)
    protected int value1 = UtilSort.INIT_STATE, value2 = UtilSort.INIT_STATE - 1;

    protected List<int> prevHighlight = new List<int>();

    protected virtual void Awake()
    {
        AddSkipAbleInstructions();
    }

    // Instructions which the user don't need to perform any actions to proceed
    public override void AddSkipAbleInstructions()
    {
        skipDict.Add(UtilSort.SKIP_NO_ELEMENT, new List<string>());
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_LOOP_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.END_LOOP_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FINAL_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.INCREMENT_VAR_I);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.SET_VAR_J);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_VAR_J);
    }

    public override void ResetSetup()
    {
        value1 = UtilSort.INIT_STATE;
        value2 = UtilSort.INIT_STATE;
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
    public abstract void ExecuteStepByStepOrder(InstructionBase instruction, bool gotElement, bool increment);

    /* Creates a instruction dictionary
     * - Used in: User test, Step by Step
    */
    public abstract Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
}

