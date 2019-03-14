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
    protected bool pseudoCodeInitilized = false, isTaskCompleted = false, includeLineNr = false; // isReadyForNextMove = false;
    protected PseudoCodeViewer pseudoCodeViewer;
    protected Color useHighlightColor = Util.HIGHLIGHT_COLOR;

    // Demo variables
    protected int i, j, k;
    protected string i_str = "i", j_str = "j", k_str = "k";
    protected string lengthOfList = "len(list)";

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
        set { pseudoCodeViewer = value; pseudoCodeViewer.InitPseudoCodeViewer(this, GetLineSpacing(), GetLineRTDelta()); }
    }

    // Reach to line of code which are not mentioned in instructions
    public void HighllightPseudoLine(int lineNr, Color color)
    {
        pseudoCodeViewer.ChangeColorOfText(lineNr, color);
    }

    // Used in Demo for highlighting pseudo code line for <seconds> then go back to normal color
    protected IEnumerator HighlightPseudoCode(string text, Color color)
    {
        pseudoCodeViewer.SetCodeLine(text, color);
        yield return demoStepDuration;
        pseudoCodeViewer.SetCodeLine(text, Util.BLACKBOARD_TEXT_COLOR);
    }


    // ---------------------------- Maybe overriden in the algorithm class which inherite this base class ----------------------------

    // Instructions which the user don't need to perform any actions to proceed
    public virtual void AddSkipAbleInstructions()
    {
        skipDict.Add(Util.SKIP_NO_ELEMENT, new List<string>());
    }

    public virtual void ResetSetup()
    {
        i = 0;
        j = 0;
        k = 0;
        i_str = "i";
        j_str = "j";
        k_str = "k";
        lengthOfList = "len(Q)";
        isTaskCompleted = false;
        demoStepDuration = null;
        skipDict = null;
        pseudoCodeInitilized = false;
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------

    public abstract string AlgorithmName { get; }


    public abstract MainManager MainManager { get; }

    // Space between lines of code
    protected abstract float GetLineSpacing();

    protected abstract Vector2 GetLineRTDelta();

    /* Collects one line of code at a time and rewrites it into pseudocode
     * - Sort: old system in use (todo: fix)
     * - Graph: uses pseudocode update dict + variables
    */
    public abstract string CollectLine(int lineNr);

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
