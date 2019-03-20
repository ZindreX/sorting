using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstructionControlBase : MonoBehaviour {

    /* --------------------------------------------  Manager --------------------------------------------
     * 
     * 
    */

    protected int currentInstructionNr = 0;
    protected Dictionary<int, InstructionBase> instructions;

    protected MainManager mainManager;

    protected AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public virtual void Init(Dictionary<int, InstructionBase> instructions)
    {
        this.instructions = instructions;
        currentInstructionNr = -1;
        mainManager = GetComponent<MainManager>();
    }

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
            return true;
        }
        else
        {
            Debug.Log("Can't increment, instructionNr: " + currentInstructionNr);
            return false;
        }
    }

    // Decrementing instruction counter as long there is one
    public bool DecrementToPreviousInstruction()
    {
        if (currentInstructionNr >= 0) // ***
        {
            currentInstructionNr--;
            return true;
        }
        else
        {
            Debug.Log("Can't decrement, instructionNr: " + currentInstructionNr);
            return false;
        }
    }

    public int CurrentInstructionNr
    {
        get { return currentInstructionNr; }
    }

    public virtual void ResetState()
    {
        instructions = null;
        currentInstructionNr = -1;

        mainManager = null;
        audioManager = null;
    }

    public abstract string FillInBlackboard();
}
