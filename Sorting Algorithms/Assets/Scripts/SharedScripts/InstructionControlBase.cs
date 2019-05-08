using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstructionControlBase : MonoBehaviour {

    /* --------------------------------------------  Instruction Control Base --------------------------------------------
     * - Base of Demo- and User Test managers
     * - Keeps track of all the basic stuff for instructions
    */

    protected int currentInstructionNr = 0, numberOfInstructions;
    protected bool autoProgress, firstInstruction, finalInstruction;

    protected Dictionary<int, InstructionBase> instructions;

    protected MainManager mainManager;
    protected AudioManager audioManager;
    protected ProgressTracker progressTracker;

    private void Awake()
    {
        progressTracker = GetComponentInChildren<ProgressTracker>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public virtual void Init(Dictionary<int, InstructionBase> instructions, int userActionInstructions, bool autoProgress)
    {
        this.instructions = instructions;
        currentInstructionNr = -1; // ?
        numberOfInstructions = instructions.Count - 1;

        this.autoProgress = autoProgress;

        mainManager = GetComponent<MainManager>();

        // Progress tracker (just visualization of progress)
        progressTracker.InitProgressTracker(userActionInstructions, userActionInstructions);
    }

    public bool FirstInstruction { get; set; }
    public bool FinalInstruction { get; set; }

    // Returns false if there are no instructions or all instructions have been dealt out, otherwise true
    public bool HasInstructions()
    {
        if (instructions != null)
            return instructions.Count != 0 && currentInstructionNr < instructions.Count;
        return false;
    }

    // Last instruction check
    public bool LastInstruction()
    {
        return currentInstructionNr == (instructions.Count - 1);
    }


    // Returns correct instruction (within the bounderies - step by step might fall outside?)
    public InstructionBase GetInstruction()
    {
        if (currentInstructionNr < 0)
            return instructions[0];
        else if (currentInstructionNr >= instructions.Count)
            return instructions[instructions.Count - 1];
        return instructions[currentInstructionNr];
    }

    // Get specific instrution
    protected InstructionBase GetInstruction(int index)
    {
        if (instructions.ContainsKey(index))
            return instructions[index];
        return null;
    }

    // Increasing instruction counter as long there is one
    public bool IncrementToNextInstruction()
    {
        if (currentInstructionNr < instructions.Count - 1)
        {
            currentInstructionNr++;
            if (autoProgress)
                progressTracker.Increment();
            return true;
        }
        else
        {
            //Debug.Log("Can't increment, instructionNr: " + currentInstructionNr);
            return false;
        }
    }

    // Decrementing instruction counter as long there is one
    public bool DecrementToPreviousInstruction()
    {
        if (currentInstructionNr >= 0) // ***
        {
            currentInstructionNr--;
            if (autoProgress)
                progressTracker.Decrement();
            return true;
        }
        else
        {
            //Debug.Log("Can't decrement, instructionNr: " + currentInstructionNr);
            return false;
        }
    }

    public int CurrentInstructionNr
    {
        get { return currentInstructionNr; }
        set { currentInstructionNr = value; } // Used by demo for pause correction
    }

    public virtual void ResetState()
    {
        instructions = null;
        currentInstructionNr = -1;
        numberOfInstructions = 0;
        firstInstruction = false;
        finalInstruction = false;

        mainManager = null;
        progressTracker.ResetProgress();
    }

    public abstract string FillInBlackboard();
}
