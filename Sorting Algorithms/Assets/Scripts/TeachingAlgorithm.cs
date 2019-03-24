using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeachingAlgorithm : MonoBehaviour {

    /* -------------------------------------------- Teaching Algorithm ----------------------------------------------------
     * The base class of teaching algoritms.
     * - SortAlgorithm
     * - GraphAlgorithm
     * 
    */

    // Basis variables for all modes
    protected WaitForSeconds demoStepDuration;
    protected bool pseudoCodeInitilized = false, isTaskCompleted = false, includeLineNr = false;
    protected PseudoCodeViewer pseudoCodeViewer;
    
    // Instruction variables
    protected int prevHighlightedLineOfCode;
    protected Color useHighlightColor = Util.HIGHLIGHT_COLOR;

    // Demo variables
    protected int i, j, k;
    //protected string i_str = "i", j_str = "j", k_str = "k";
    protected string lengthOfList = "len(list)";
    protected bool lineCalculation = true;

    // User Test
    protected Dictionary<string, List<string>> skipDict;

    public virtual void InitTeachingAlgorithm(float algorithmSpeed)
    {
        demoStepDuration = new WaitForSeconds(algorithmSpeed);
        
        skipDict = new Dictionary<string, List<string>>();
        AddSkipAbleInstructions();
    }

    // Mark to change values according to demo etc. (before initialized: init pseudocode displays)
    public bool PseudoCodeInitilized
    {
        get { return pseudoCodeInitilized; }
        set { pseudoCodeInitilized = value; }
    }

    public WaitForSeconds DemoStepDuration
    {
        get { return demoStepDuration; }
        set { demoStepDuration = value; }
    }

    public bool IsTaskCompleted
    {
        get { return isTaskCompleted; }
        set { isTaskCompleted = value; }
    }

    public bool IncludeLineNr
    {
        get { return includeLineNr; }
        set { includeLineNr = value; }
    }

    public Dictionary<string, List<string>> SkipDict
    {
        get { return skipDict; }
    }

    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return pseudoCodeViewer; }
        set { pseudoCodeViewer = value; }
    }

    // Reach to line of code which are not mentioned in instructions
    public void HighllightPseudoLine(int lineNr, Color color)
    {
        pseudoCodeViewer.ChangeColorOfText(lineNr, color);
    }

    // Used in Demo for highlighting pseudo code line for <seconds> then go back to normal color
    protected IEnumerator HighlightPseudoCode(string text, Color color)
    {
        string[] lineOfCodeSplit = text.Split(Util.PSEUDO_SPLIT_LINE_ID);
        int index = UtilGraph.ConvertCostToInt(lineOfCodeSplit[0]);
        string pseudoCodeLine = lineOfCodeSplit[1];

        /* If a calculation takes place, then first display formula/pseudocode -> insert values -> result
         * E.g.:
         * Step 1: i = i + 1
         * Step 2: i = 1 + 1
         * Step 3: i = 2
        */
        if (lineCalculation)
        {
            bool valuesNotInserted = true; // First show step 1
            for (int x=0; x < 2; x++)
            {
                string calculation = PseudocodeLineIntoSteps(index, valuesNotInserted);

                if (calculation == "X")
                    break;

                pseudoCodeViewer.SetCodeLine(index, calculation, color);
                yield return demoStepDuration;

                valuesNotInserted = false; // Then show step 2
            }

        }

        // At last show step 3 (result)
        pseudoCodeViewer.SetCodeLine(index, pseudoCodeLine, color);
        yield return demoStepDuration;
        pseudoCodeViewer.SetCodeLine(index, pseudoCodeLine, Util.BLACKBOARD_TEXT_COLOR);


    }


    // ---------------------------- Maybe overriden in the algorithm class which inherite this base class ----------------------------

    // Instructions which the user don't need to perform any actions to proceed
    public virtual void AddSkipAbleInstructions()
    {
        skipDict.Add(Util.SKIP_NO_ELEMENT, new List<string>());
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.FINAL_INSTRUCTION);
    }

    public virtual void ResetSetup()
    {
        i = 0;
        j = 0;
        k = 0;
        lengthOfList = "len(list)";
        isTaskCompleted = false;
        demoStepDuration = null;
        skipDict = null;
        pseudoCodeInitilized = false;
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------

    public abstract string AlgorithmName { get; }


    public abstract MainManager MainManager { get; }

    // Space between lines of code
    public abstract float GetLineSpacing();

    public abstract Vector2 GetLineRTDelta();

    /* Collects one line of code at a time and rewrites it into pseudocode
     * - Sort: old system in use (todo: fix)
     * - Graph: uses pseudocode update dict + variables
    */
    public abstract string CollectLine(int lineNr);

    // Used to display (calculation) pseudocode in more steps
    protected abstract string PseudocodeLineIntoSteps(int lineNr, bool init);

    // first instruction line of code
    public abstract int FirstInstructionCodeLine();
    // final instruction line of code
    public abstract int FinalInstructionCodeLine();


    /* Almost the same method as for the Step by step teaching method
     * - Only does the visualization of the pseudocode
    */
    public abstract IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotElement);


    // Infoplate
    public abstract GameObject InfoPlate { get; }

}
