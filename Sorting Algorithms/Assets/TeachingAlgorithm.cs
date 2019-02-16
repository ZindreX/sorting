using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeachingAlgorithm : MonoBehaviour {

    protected float seconds = 1f;
    protected bool isTaskCompleted = false;// isReadyForNextMove = false;

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

    public Dictionary<string, List<string>> SkipDict
    {
        get { return skipDict; }
    }

    public PseudoCodeViewer PseudoCodeViewer
    {
        get { return pseudoCodeViewer; }
        set { pseudoCodeViewer = value; pseudoCodeViewer.SetAlgorithm(this); }
    }

    // Reach to line of code which are not mentioned in instructions
    public void HighllightPseudoLine(int lineNr, Color color)
    {
        pseudoCodeViewer.ChangeColorOfText(lineNr, color);
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
