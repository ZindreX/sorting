using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeachingAlgorithm : MonoBehaviour {

    // Basis variables for all modes
    protected float seconds = 1f;
    protected bool isTaskCompleted = false, includeLineNr = false;  // isReadyForNextMove = false;
    protected PseudoCodeViewer pseudoCodeViewer;

    // Demo variables
    protected int i, j, k;
    protected string lengthOfList = "len(Q)";

    // User Test
    protected Dictionary<string, List<string>> skipDict = new Dictionary<string, List<string>>();


    public float Seconds
    {
        get { return seconds; }
        set { seconds = value; }
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
        set { pseudoCodeViewer = value; pseudoCodeViewer.InitPseudoCodeViewer(this, GetLineSpacing()); }
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
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(text, Util.BLACKBOARD_TEXT_COLOR);
    }


    // ---------------------------- Maybe overriden in the algorithm class which inherite this base class ----------------------------

    public virtual void AddSkipAbleInstructions()
    {
        // Insert base skip instructions
    }

    public virtual void ResetSetup()
    {
        i = 0;
        j = 0;
        k = 0;
        lengthOfList = "len(Q)";
    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------

    public abstract string AlgorithmName { get; }

    // Space between lines of code
    protected abstract float GetLineSpacing();

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


}
