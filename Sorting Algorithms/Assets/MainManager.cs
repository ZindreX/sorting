﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainManager : MonoBehaviour {

    /* -------------------------------------------- Main Manager ----------------------------------------------------
     * 
     * 
    */

    protected string algorithmName;
    protected bool userStoppedAlgorithm = false, beginnerWait = false, controllerReady = false;
    protected bool userTestReady = false; // debugging

    protected TeachingAlgorithm teachingAlgorithm;

    // A boolean used to wait entering the update cycle while beginner's help (pseudocode) is being written
    public bool BeginnerWait
    {
        get { return beginnerWait; }
        set { beginnerWait = value; }
    }

    public bool ControllerReady
    {
        get { return controllerReady; }
        //set { controllerReady = value; }
    }

    // Finds the number of instructions which the player has to do something to progress  
    protected int FindNumberOfUserAction(Dictionary<int, InstructionBase> instructions)
    {
        int count = 0;
        for (int x = 0; x < instructions.Count; x++)
        {
            if (teachingAlgorithm.SkipDict.ContainsKey(Util.SKIP_NO_ELEMENT) && teachingAlgorithm.SkipDict.ContainsKey(Util.SKIP_NO_DESTINATION))
            {
                if (!teachingAlgorithm.SkipDict[Util.SKIP_NO_ELEMENT].Contains(instructions[x].Instruction) && !teachingAlgorithm.SkipDict[Util.SKIP_NO_DESTINATION].Contains(instructions[x].Instruction))
                    count++;
            }
        }
        return count;
    }


    /* --------------------------------------- Demo ---------------------------------------
     * - Gives a visual presentation of <algorithm>
     * - Player dont need to do anything / Watch & learn
    */
    public abstract void PerformAlgorithmDemo();

    /* --------------------------------------- Step-By-Step ---------------------------------------
     * - Gives a visual presentation of <algorithm>
     * - Player can't directly interact with <algorithm objects>, but can use controllers to progress
     *   one step of a time / or back
    */
    public abstract void PerformAlgorithmStepByStep();

    /* --------------------------------------- User Test ---------------------------------------
     * - Gives a visual presentation of elements used in <algorithm>
     * - Player needs to interact with the <algorithm objects> to progress through the algorithm
    */
    public abstract void PerformAlgorithmUserTest();

    /* Prepares the next instruction based on the algorithm being runned
     * - Sends instruction to the next sorting element the user should move
     * - Beginners (difficulty) will be shown steps on pseudoboard and given some hints
     * - Skips instructions which doesn't contain any elements nor destination
    */
    protected abstract int PrepareNextInstruction(InstructionBase instruction);





    // --------------------------------------- Debugging ---------------------------------------

    private void DebugCheckInstructions(Dictionary<int, InstructionBase> dict)
    {
        for (int x = 0; x < dict.Count; x++)
        {
            Debug.Log(dict[x].DebugInfo());
        }
        Debug.Log("");
    }

    private string DebugCheckGameObjects(GameObject[] list)
    {
        string result = "";
        foreach (GameObject obj in list)
        {
            result += obj.GetComponent<SortingElementBase>().Value + ", ";
        }
        return result;
    }



}
