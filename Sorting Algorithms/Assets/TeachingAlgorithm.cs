using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeachingAlgorithm : MonoBehaviour {

    protected float seconds = 1f;
    protected bool isTaskCompleted = false, includeLineNr = false;  // isReadyForNextMove = false;

    protected Dictionary<string, List<string>> skipDict = new Dictionary<string, List<string>>();

    protected PseudoCodeViewer pseudoCodeViewer;

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

    }

    // ---------------------------- Overriden in the algorithm class which inherite this base class ----------------------------

    public abstract string AlgorithmName { get; }

    // Space between lines of code
    protected abstract float GetLineSpacing();

    /* Collects one line of code at a time and rewrites it into pseudocode
     * > a method called PseudoCode(...) must be created aswell (this is where the pseudocode will be placed
     * 
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
