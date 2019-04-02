using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(HolderManager))]
//[RequireComponent(typeof(ElementManager))]
//[RequireComponent(typeof(UserTestManager))]
//[RequireComponent(typeof(StepByStepManager))]
//[RequireComponent(typeof(SortAlgorithm))]
//[RequireComponent(typeof(DisplayUnitManager))]
public abstract class AlgorithmManagerBase : MonoBehaviour {

    /* -------------------------------------------- Sorting Algorithm Manager Base ----------------------------------------------------
     * 
     * 
    */

    protected SortMain sortMain;

    public void InitSortingManager(SortMain sortMain)
    {
        this.sortMain = sortMain;
        Debug.Log("Algorithm: " + AlgorithmManager);
    }

    // Returns the holder (might change, since insertion sort is the only with some modifications) ***
    public virtual HolderBase GetCorrectHolder(int index)
    {
        return sortMain.HolderManager.GetHolder(index);
    }

    // --------------------------------------- To be implemented in subclasses ---------------------------------------

    public abstract string AlgorithmManager { get; }

    // Moves needed to progress to next instruction
    public abstract int MovesNeeded { get; }

    // Copies the first state of sorting elements into instruction, which can be used when creating instructions for user test
    public abstract InstructionBase[] CopyFirstState(GameObject[] sortingElements);

    /* Prepares the next instruction based on the algorithm being runned
     * - Sends instruction to the next sorting element the user should move
     * - Beginners (difficulty) will be shown steps on pseudoboard and given some hints
     * - Skips instructions which doesn't contain any elements nor destination
    */
    public abstract int PrepareNextInstruction(InstructionBase instruction);
}
