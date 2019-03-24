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

    // When the user pause
    protected bool userPausedTask;

    // When the user click a button in game to stop the current process (demo, step-by-step, user test)
    protected bool userStoppedTask = false;

    // Used to pause the Update loop while learning assitance is working (difficulty=beginner -> pseudocode highlighting)
    protected int waitForSupportToComplete = 0;

    // Used to check whether the controller is ready for input (see method below)
    protected bool controllerReady = false;

    // Blocks the Update loop until the application is ready and initialized
    protected bool algorithmInitialized = false;

    
    // Check list (startup, shutdown)
    public const string START_UP_CHECK = "Start up check", WAIT_FOR_SUPPORT = "Wait for support", SHUT_DOWN_CHECK = "Shut down check";
    protected bool checkListModeActive;
    protected string activeChecklist;
    protected Dictionary<string, bool> safeStopChecklist;

    // Audio
    protected AudioManager audioManager;

    protected WaitForSeconds loading = new WaitForSeconds(1f);

    protected StepByStepManager stepByStepManager;
    protected UserTestManager userTestManager;

    protected virtual void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (checkListModeActive)
            PerformCheckList(activeChecklist);

        // Stop update loop in case algorithm task isnt initialized, or user stopped the task
        if (!algorithmInitialized || userStoppedTask)
            return;

        if (WaitingForSupportToFinish())
        {
            //Debug.Log("Waiting for support to finish: #" + waitForSupportToComplete);
            return;
        }

        if (GetTeachingAlgorithm().IsTaskCompleted)
        {
            // When an algorithm task is finished, do some stuff
            TaskCompletedFinishOff();
            UpdateCheckList(Settings.TeachingMode, true);
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

    // --------------------------------------- Checklist / Safe stop ---------------------------------------

    // Check list used for start/stop
    protected abstract void PerformCheckList(string check);

    // Called when user click a stop button in game
    public void SafeStop()
    {
        if (Settings.TeachingMode != Util.DEMO)
            UpdateCheckList(Settings.TeachingMode, true); // nothing yet to safe stop in step/user test

        userStoppedTask = true;

        activeChecklist = SHUT_DOWN_CHECK;
        checkListModeActive = true;
    }

    public bool CheckList(string unit)
    {
        if (safeStopChecklist.ContainsKey(unit))
        {
            return safeStopChecklist[unit];
        }
        return false;
    }

    public void AddToCheckList(string unit, bool ready)
    {
        if (!safeStopChecklist.ContainsKey(unit))
        {
            safeStopChecklist.Add(unit, ready);
        }
    }

    public void UpdateCheckList(string unit, bool ready)
    {
        if (safeStopChecklist.ContainsKey(unit))
        {
            safeStopChecklist[unit] = ready;
        }
    }

    // --------------------------------------- Getters / Setters ---------------------------------------

    // Algorithm is initialized and ready to go
    public bool AlgorithmInitialized
    {
        get { return algorithmInitialized; }
    }

    // A variable used to wait entering the update cycle while beginner's help (pseudocode) is being written (used to be boolean, now int to support multiple supports going on)
    // Value range: 0, n | 0: Ready, else not ready
    public int WaitForSupportToComplete
    {
        get { return waitForSupportToComplete; }
        set { if (value >= 0) waitForSupportToComplete = value; }
    }

    public bool WaitingForSupportToFinish()
    {
        return waitForSupportToComplete > 0;
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

    public bool UserPausedTask
    {
        get { return userPausedTask; }
        set { userPausedTask = value; }
    }

    // --------------------------------------- Demo Device ---------------------------------------

    public void DemoDevice(string itemID)
    {
        switch (itemID)
        {
            case "Step back":
                if (!WaitingForSupportToFinish())
                    PlayerStepByStepInput(false);
                else
                    Debug.Log("Can't perform step yet. Wait for support to finish");
                break;

            case "Pause":
                userPausedTask = !userPausedTask;

                if (userPausedTask)
                    GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(0f); // If pause -> Step by step (player choose pace themself)
                else
                    GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(Settings.AlgorithmSpeed); // Use demo speed
                break;

            case "Step forward":
                if (!WaitingForSupportToFinish())
                    PlayerStepByStepInput(true);
                else
                    Debug.Log("Can't perform step yet. Wait for support to finish");
                break;
        }
    }

    // Input from user during Step-By-Step (increment/decrement)
    public void PlayerStepByStepInput(bool increment)
    {
        if (ControllerReady)
            stepByStepManager.NotifyUserInput(increment);
    }


    // --------------------------------------- Settings menu / Start pillar ---------------------------------------

    public void StartAlgorithm()
    {
        string teachingMode = Settings.TeachingMode;

        // Add to checklist (used for safe shutdown)
        AddToCheckList(teachingMode, false);

        // Start algorithm
        switch (teachingMode)
        {
            case Util.DEMO: PerformAlgorithmDemo(); break;
            case Util.STEP_BY_STEP: PerformAlgorithmStepByStep(); break;
            case Util.USER_TEST: PerformAlgorithmUserTest(); break;
        }

        userStoppedTask = false;
        controllerReady = true; // ???
        algorithmInitialized = true;
    }


    // --------------------------------------- Extra methods ---------------------------------------

    // Finds the number of instructions which the player has to do something to progress  
    protected int FindNumberOfUserAction(Dictionary<int, InstructionBase> instructions)
    {
        Dictionary<string, List<string>> skipDict = GetTeachingAlgorithm().SkipDict;

        int count = 0;
        for (int x = 0; x < instructions.Count; x++)
        {
            InstructionBase inst = instructions[x];

            if (inst is ListVisualInstruction)
                continue;

            if (skipDict.ContainsKey(Util.SKIP_NO_ELEMENT) && skipDict.ContainsKey(Util.SKIP_NO_DESTINATION))
            {
                string instruction = instructions[x].Instruction;

                if (!skipDict[Util.SKIP_NO_ELEMENT].Contains(instruction) && !skipDict[Util.SKIP_NO_DESTINATION].Contains(instruction))
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
    public virtual void InstantiateSetup()
    {
        // Used for shut down process
        safeStopChecklist = new Dictionary<string, bool>();
        algorithmName = Settings.Algorithm;

    }

    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public virtual void DestroyAndReset()
    {
        algorithmName = "";

        activeChecklist = "";
        checkListModeActive = false;
        safeStopChecklist = null;

        algorithmInitialized = false;
        controllerReady = false;
        userStoppedTask = false;

        WaitForSupportToComplete = 0;
    }



    // --------------------------------------- Teaching mode updates ---------------------------------------
    protected abstract void DemoUpdate();
    protected abstract void StepByStepUpdate();
    protected abstract void UserTestUpdate();
    protected abstract void TaskCompletedFinishOff(); // Finish off visualization

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
