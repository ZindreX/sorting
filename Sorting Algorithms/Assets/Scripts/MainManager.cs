using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainManager : MonoBehaviour {

    /* -------------------------------------------- Main Manager ----------------------------------------------------
     * The main manager of this application
     * - AlgorithmManagerBase (sorting) <- change name?
     * - GraphMain
     * 
    */

    protected string algorithmName;

    // When the user click a button in game to stop the current process (demo, step-by-step, user test)
    protected bool userStoppedTask = false;

    // Used to pause the Update loop while learning assitance is working (difficulty=beginner -> pseudocode highlighting)
    protected bool waitForSupportToComplete = false;

    // Used to check whether the controller is ready for input (see method below)
    protected bool controllerReady = false;

    // Blocks the Update loop until the application is ready and initialized
    protected bool algorithmInitialized = false;

    // debugging, remove now?
    protected bool userTestInitialized = false; 
    
    // Check list (startup, shutdown)
    public const string START_UP_CHECK = "Start up check", SHUT_DOWN_CHECK = "Shut down check";
    protected bool checkListModeActive;
    protected string activeChecklist;
    protected Dictionary<string, bool> subUnitChecks;



    protected WaitForSeconds loading = new WaitForSeconds(1f);

    void Update()
    {
        if (checkListModeActive)
            PerformCheckList(activeChecklist);

        // Stop update loop in case algorithm task isnt initialized, or user stopped the task
        if (!algorithmInitialized || userStoppedTask)
            return;

        if (GetTeachingAlgorithm().IsTaskCompleted)
        {
            // When an algorithm task is finished, do some stuff
            TaskCompletedFinishOff();
        }
        else
        {
            // Update based on the active teaching mode
            if (Settings.IsStepByStep())
                StepByStepUpdate();
            else if (Settings.IsDemo())
                DemoUpdate();
            else if (Settings.IsUserTest())
                UserTestUpdate();
        }
    }

    // Check list used for start/stop
    protected abstract void PerformCheckList(string check);

    // Called when user click a stop button in game
    public void SafeStop()
    {
        userStoppedTask = true;

        activeChecklist = SHUT_DOWN_CHECK;
        checkListModeActive = true;
    }

    // Teaching mode updates
    protected abstract void DemoUpdate();
    protected abstract void StepByStepUpdate();
    protected abstract void UserTestUpdate();

    // Finish off visualization
    protected abstract void TaskCompletedFinishOff();

    // Algorithm is initialized and ready to go
    public bool AlgorithmInitialized
    {
        get { return algorithmInitialized; }
    }

    // A boolean used to wait entering the update cycle while beginner's help (pseudocode) is being written
    public bool WaitForSupportToComplete
    {
        get { return waitForSupportToComplete; }
        set { waitForSupportToComplete = value; }
    }

    public bool UserStoppedTask
    {
        get { return userStoppedTask; }
        set { userStoppedTask = value; }
    }

    // Checks whether the controller is ready (old: menu button start/stop, insertion sort: move pivot holder, step-by-step: instruction swapping)
    public bool ControllerReady
    {
        get { return controllerReady; }
    }

    public bool CheckListModeActive
    {
        get { return checkListModeActive; }
        set { checkListModeActive = value; }
    }

    public void StartAlgorithm()
    {
        // Start algorithm
        switch (Settings.TeachingMode)
        {
            case Util.DEMO: PerformAlgorithmDemo(); break;
            case Util.STEP_BY_STEP: PerformAlgorithmStepByStep(); break;
            case Util.USER_TEST: PerformAlgorithmUserTest(); break;
        }
        userStoppedTask = false;
        controllerReady = true; // ???
        algorithmInitialized = true;
    }


    // Finds the number of instructions which the player has to do something to progress  
    protected int FindNumberOfUserAction(Dictionary<int, InstructionBase> instructions)
    {
        int count = 0;
        for (int x = 0; x < instructions.Count; x++)
        {
            if (GetTeachingAlgorithm().SkipDict.ContainsKey(Util.SKIP_NO_ELEMENT) && GetTeachingAlgorithm().SkipDict.ContainsKey(Util.SKIP_NO_DESTINATION))
            {
                if (!GetTeachingAlgorithm().SkipDict[Util.SKIP_NO_ELEMENT].Contains(instructions[x].Instruction) && !GetTeachingAlgorithm().SkipDict[Util.SKIP_NO_DESTINATION].Contains(instructions[x].Instruction))
                    count++;
            }
        }
        return count;
    }



    public abstract TeachingAlgorithm GetTeachingAlgorithm();
    protected abstract TeachingAlgorithm GrabAlgorithmFromObj();
    public abstract SettingsBase Settings { get; }

    protected abstract IEnumerator ActivateTaskObjects(bool active);

    /* --------------------------------------- Instatiate Setup ---------------------------------------
     * > Called from UserController
     * > Creates the holders and the sorting elements
    */
    public abstract void InstantiateSetup();
    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public abstract void DestroyAndReset();

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
    //protected abstract int PrepareNextInstruction(InstructionBase instruction);


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
