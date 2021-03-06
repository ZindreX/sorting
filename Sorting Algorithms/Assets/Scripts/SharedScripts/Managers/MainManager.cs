﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class MainManager : MonoBehaviour {

    /* -------------------------------------------- Main Manager ----------------------------------------------------
     * The main manager of this application
     * - SortMain (Sorting algorithms)
     * - GraphMain (Graph algorithms)
     * 
    */

    protected string algorithmName, prevInstruction;

    // When the user pause (pauses the flow of instructions)
    protected bool userPausedTask;

    // When the user click a button in game to stop the current process (demo/step-by-step, user test)
    protected bool userStoppedTask = false;

    // Used to pause the Update loop while learning assitance is working (difficulty=beginner -> pseudocode highlighting, list visual update, etc..)
    protected int waitForSupportToComplete = 0;

    // Used to check whether the controller is ready for input (see method below)
    protected bool controllerReady = false;

    // Blocks the Update loop until the application is ready and initialized
    protected bool algorithmInitialized = false;

    // 
    protected bool newDemoImplemented, hasFinishedOff;

    private bool requestBacktrack;

    // Check list (startup, shutdown)
    public const string START_UP_CHECK = "Start up check", WAIT_FOR_SUPPORT = "Wait for support", SHUT_DOWN_CHECK = "Shut down check";
    protected bool checkListModeActive;
    protected string activeChecklist;
    protected Dictionary<string, bool> safeStopChecklist;

    protected WaitForSeconds demoSpeedBeforePause;                              // Switching between demo <--> step-by-step
    protected WaitForSeconds stepByStepMode = new WaitForSeconds(0f);           // User set their own pace of step-by-step - no lag
    protected WaitForSeconds loading = new WaitForSeconds(1f);                  // Loading time (spawning/moving objects)
    protected WaitForSeconds warningMessageDuration = new WaitForSeconds(2f);   // Feedback display time
    protected WaitForSeconds finishStepDuration = new WaitForSeconds(0.5f);     // Finish off (after user test)

    protected DemoDevice demoDevice;

    [Header("Main manager")]
    [SerializeField]
    protected TextMeshPro feedbackDisplayText;

    [Space(5)]
    [Header("Debugging")]
    [SerializeField]
    private TextMeshPro debugText;

    public bool useDebugText;
    public int debugLineNumbers = 20;
    private int deleteWhenReached;

    protected DemoManager demoManager;
    protected UserTestManager userTestManager;

    // Audio
    protected AudioManager audioManager;
    public readonly string BUTTON_CLICK_SOUND = "ButtonClick", COLLISION_SOUND = "Collision", CORRECT_SOUND = "Correct", MISTAKE_SOUND = "Mistake";
    public readonly string HINT_CORRECT_SOUND = "HintCorrect", HINT_MISTAKE_SOUND = "HintMistake";

    protected virtual void Awake()
    {
        demoDevice = FindObjectOfType<DemoDevice>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (WaitingForSupportToFinish())
        {
            //Debug.Log("Waiting for support to finish: #" + waitForSupportToComplete);
            return;
        }

        if (checkListModeActive)
            PerformCheckList(activeChecklist);

        // Stop update loop in case algorithm task isnt initialized, or user stopped the task
        if (!algorithmInitialized || userStoppedTask)
            return;


        if (GetTeachingAlgorithm().IsTaskCompleted)
        {
            // When an algorithm task is finished, do some stuff
            if (!hasFinishedOff)
            {
                TaskCompletedFinishOff();
                UpdateCheckList(Settings.TeachingMode, true);
                hasFinishedOff = true;

                if (Settings.IsDemo())
                    demoManager.FinalInstruction = true;
            }
            else if (requestBacktrack)
            {
                GetTeachingAlgorithm().IsTaskCompleted = false;
                requestBacktrack = false;
            }
        }
        else
        {
            // Update based on the active teaching mode
            if (Settings.IsDemo())
                DemoUpdate();
            else if (Settings.IsUserTest())
                    UserTestUpdate();
        }
    }

    // --------------------------------------- Checklist / Safe stop ---------------------------------------
    #region Checklist
    // Check list used for start/stop
    protected abstract void PerformCheckList(string check);

    // Called when user click a stop button in game
    public void SafeStop()
    {
        UpdateCheckList(Settings.TeachingMode, true); // nothing yet to safe stop in new demo/step/user test
            
        userStoppedTask = true;

        activeChecklist = SHUT_DOWN_CHECK;
        checkListModeActive = true;
    }

    protected void ShutdownProcess(bool readyForDestroy)
    {
        if (readyForDestroy)
        {
            Debug.Log("Ready to shut down");
            checkListModeActive = false;
            DestroyAndReset();
        }
    }

    public bool CheckList(string unit)
    {
        if (safeStopChecklist != null && safeStopChecklist.ContainsKey(unit))
        {
            return safeStopChecklist[unit];
        }
        return false;
    }

    public void AddToCheckList(string unit, bool ready)
    {
        if (safeStopChecklist != null && !safeStopChecklist.ContainsKey(unit))
        {
            safeStopChecklist.Add(unit, ready);
        }
    }

    public void UpdateCheckList(string unit, bool ready)
    {
        if (safeStopChecklist != null && safeStopChecklist.ContainsKey(unit))
        {
            safeStopChecklist[unit] = ready;
        }
        else
            Debug.Log(">>>>>>>>>>>>>>>>>> Not containing '" + unit + "'.");
    }
    #endregion
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

    public AudioManager AudioManager
    {
        get { return audioManager; }
    }

    // --------------------------------------- Demo Device ---------------------------------------
    #region Demo device / controller input
    public void PerformDemoDeviceAction(string itemID, bool otherSource)
    {
        switch (itemID)
        {
            case DemoDevice.STEP_BACK:
                if (!WaitingForSupportToFinish())
                {
                    // Report backward step
                    PlayerStepByStepInput(false);

                    // Give feedback incase of invalid step
                    if (!demoManager.IsValidStep)
                        StartCoroutine(SetFeedbackDisplayDuration("First instruction reached.\nCan't decrement."));
                    else
                        StartCoroutine(SetFeedbackDisplayDuration("Backward step"));
                }
                else
                    StartCoroutine(SetFeedbackDisplayDuration("Support working (" + waitForSupportToComplete + "). Wait a second and try again."));
                break;

            case DemoDevice.PAUSE:
                // Toggle pause
                UserPausedTask = !userPausedTask;
                Debug.Log("Pause action: " + userPausedTask);

                if (useDebugText && debugText != null)
                    debugText.text += "\nPause action: " + userPausedTask;

                // Tips display
                string pauseOrUnPauseText = UserPausedText(UserPausedTask);
                SetFeedbackDisplay(pauseOrUnPauseText);

                // Demo device button replacing
                demoDevice.TransitionPause(userPausedTask);

                if (otherSource)
                    demoDevice.OtherSourceClick(DemoDevice.PAUSE);

                if (userPausedTask)
                {
                    // Step back when paused (otherwise it'll skip 1 instruction)
                    demoManager.DecrementToPreviousInstruction(); //demoManager.CurrentInstructionNr--;

                    // Save the current speed used
                    demoSpeedBeforePause = GetTeachingAlgorithm().DemoStepDuration;

                    // Set to step-by-step speed (instant)
                    GetTeachingAlgorithm().DemoStepDuration = stepByStepMode; // If pause -> Step by step (player choose pace themself)
                }
                else
                {
                    GetTeachingAlgorithm().DemoStepDuration = demoSpeedBeforePause; // Use demo speed
                }
                break;

            case DemoDevice.STEP_FORWARD:
                if (!WaitingForSupportToFinish())
                {
                    // Report forward step
                    PlayerStepByStepInput(true);

                    // Give feedback incase of invalid step
                    if (!demoManager.IsValidStep)
                        StartCoroutine(SetFeedbackDisplayDuration("Final instruction reached.\nCan't increment."));
                    else
                        StartCoroutine(SetFeedbackDisplayDuration("Forward step"));
                }
                else
                    StartCoroutine(SetFeedbackDisplayDuration("Support working (" + waitForSupportToComplete + "). Wait a second and try again."));
                break;

            case DemoDevice.REDUCE_SPEED:
                if (Settings.AlgorithmSpeedLevel > 0)
                {
                    Settings.AlgorithmSpeedLevel--;
                    GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(Settings.AlgorithmSpeed);

                    switch (Settings.AlgorithmSpeedLevel)
                    {
                        case 0: demoDevice.ButtonActive(DemoDevice.REDUCE_SPEED, false); break;
                        case 2: demoDevice.ButtonActive(DemoDevice.INCREASE_SPEED, true); break;
                    }
                    StartCoroutine(SetFeedbackDisplayDuration("Speed: " + Util.algorithSpeedConverterDict[Settings.AlgorithmSpeedLevel]));

                }
                else
                    StartCoroutine(SetFeedbackDisplayDuration("Minimum speed already set."));
                break;

            case DemoDevice.INCREASE_SPEED:
                if (Settings.AlgorithmSpeedLevel < 3)
                {
                    Settings.AlgorithmSpeedLevel++;
                    GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(Settings.AlgorithmSpeed);

                    switch (Settings.AlgorithmSpeedLevel)
                    {
                        case 1: demoDevice.ButtonActive(DemoDevice.REDUCE_SPEED, true); break;
                        case 3: demoDevice.ButtonActive(DemoDevice.INCREASE_SPEED, false); break;
                    }
                    StartCoroutine(SetFeedbackDisplayDuration("Speed: " + Util.algorithSpeedConverterDict[Settings.AlgorithmSpeedLevel]));

                }
                else
                    StartCoroutine(SetFeedbackDisplayDuration("Maximum speed already set."));
                break;
        }
    }

    // Input from user during Step-By-Step (increment/decrement)
    public void PlayerStepByStepInput(bool increment)
    {
        if (ControllerReady && !WaitingForSupportToFinish())
            demoManager.NotifyUserInput(increment);

        if (demoManager.FinalInstruction && !increment)
            requestBacktrack = true;
    }

    private string UserPausedText(bool pause)
    {
        if (GetTeachingAlgorithm().CanPerformBackStep)
            return pause ? "Step-by-step\nUse grip buttons to step forward/backward" : "Demo\nUse grip buttons to adjust speed";
        return pause ? "Step-by-step\nClick right grip button to step forward" : "Demo\nUse grip buttons to adjust speed";
    }
    #endregion

    // --------------------------------------- Settings menu / Startpillar / Sorting table ---------------------------------------

    /* --------------------------------------- Instatiate Setup ---------------------------------------
     * > Called from UserController
     * > Creates the holders and the sorting elements
    */
    public virtual void InstantiateSafeStart()
    {
        demoSpeedBeforePause = new WaitForSeconds(Settings.AlgorithmSpeed);

        // Used for shut down process
        safeStopChecklist = new Dictionary<string, bool>();

        activeChecklist = START_UP_CHECK;
        checkListModeActive = true;
        algorithmName = Settings.Algorithm;

        SetFeedbackDisplay("Click start to play");

        // Debugging
        deleteWhenReached = debugLineNumbers;
    }

    public void StartAlgorithm()
    {
        string teachingMode = Settings.TeachingMode;

        // Add to checklist (used for safe shutdown)
        AddToCheckList(teachingMode, false);

        // Start algorithm
        switch (teachingMode)
        {
            case Util.DEMO:
                demoDevice.InitDemoDevice(userPausedTask, GetTeachingAlgorithm().CanPerformBackStep);
                demoDevice.SpawnDeviceInfrontOfPlayer();
                PerformAlgorithmDemo();
                break;

            case Util.USER_TEST:
                PerformAlgorithmUserTest();
                break;
        }

        // Start up process completed
        activeChecklist = "";
        checkListModeActive = false;

        userStoppedTask = false;
        controllerReady = true; // ???
        algorithmInitialized = true;
    }


    /* --------------------------------------- Destroy & Restart ---------------------------------------
     * > Called from UserController
     * > Destroys all the gameobjects
     */
    public virtual void DestroyAndReset()
    {
        SetFeedbackDisplay("");
        algorithmName = "";
        prevInstruction = "";

        activeChecklist = "";
        checkListModeActive = false;
        safeStopChecklist = null;

        algorithmInitialized = false;
        controllerReady = false;
        userStoppedTask = false;
        userPausedTask = false;
        hasFinishedOff = false;
        requestBacktrack = false;

        WaitForSupportToComplete = 0;

        switch (Settings.TeachingMode)
        {
            case Util.DEMO: demoManager.ResetState(); break;
            case Util.USER_TEST: userTestManager.ResetState(); break;
        }
        newDemoImplemented = false;

        // Reset demo device in <..>main
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

    // Text feedback to user (controller input demo device etc.)
    public IEnumerator SetFeedbackDisplayDuration(string text)
    {
        if (feedbackDisplayText!= null)
        {
            feedbackDisplayText.text = text;
            yield return warningMessageDuration;
            feedbackDisplayText.text = "";
        }
    }

    public void SetFeedbackDisplay(string text)
    {
        feedbackDisplayText.text = text;
    }

    public abstract TeachingAlgorithm GetTeachingAlgorithm();
    protected abstract TeachingAlgorithm GrabAlgorithmFromObj();
    public abstract TeachingSettings Settings { get; }

    protected abstract IEnumerator ActivateTaskObjects(bool active);


    /* --------------------------------------- Demo & Step-By-Step ---------------------------------------
     * >>> Demo
     * - Gives a visual presentation of <algorithm>
     * - Player dont need to do anything / Watch & learn
     * 
     * >>> Step-by-step
     * - Gives a visual presentation of <algorithm>
     * - Player can't directly interact with <algorithm objects>, but can use buttons in scene or 
     *   controllers to progress one step of a time / or back
    */
    public abstract void PerformAlgorithmDemo();

    // Fetch instruction
    protected virtual void DemoUpdate()
    {
        if (newDemoImplemented)
        {
            InstructionBase instruction = null;

            // Step by step activated by pausing, and step requested
            if (userPausedTask && demoManager.PlayerMove)
            {
                demoManager.PlayerMove = false;
                instruction = demoManager.GetStep();

                bool increment = demoManager.PlayerIncremented;
                PerformInstruction(instruction, increment);
            }
            else if (!userPausedTask && demoManager.HasInstructions()) // Demo mode
            {
                // First check if user test setup is complete
                if (!WaitingForSupportToFinish())//waitForSupportToComplete == 0)
                {
                    instruction = demoManager.GetInstruction();
                    //Debug.Log("Current: " + demoManager.CurrentInstructionNr + ", Actual: " + instruction.InstructionNr);

                    PerformInstruction(instruction, true);
                    demoManager.IncrementToNextInstruction();
                }
            }

            // Debugging
            if (instruction != null && prevInstruction != instruction.Instruction)
            {
                prevInstruction = instruction.DebugInfo();
                Debug.Log(instruction.DebugInfo());

                if (useDebugText && debugText != null)
                {
                    if (deleteWhenReached <= 0)
                    {
                        debugText.text = instruction.DebugInfo();
                        deleteWhenReached = debugLineNumbers;
                    }
                    else
                        debugText.text += "\n" + instruction.DebugInfo();

                    deleteWhenReached--;
                }
            }
        }
    }

    // Decode and execute instruction
    protected void PerformInstruction(InstructionBase instruction, bool increment)
    {
        if (instruction.Status == Util.EXECUTED_INST && increment)
            return;

        waitForSupportToComplete++;
        StartCoroutine(GetTeachingAlgorithm().ExecuteDemoInstruction(instruction, increment));

        if (increment)
            instruction.Status = Util.EXECUTED_INST;
        else
            instruction.Status = Util.NOT_EXECUTED;
    }

    /* --------------------------------------- User Test ---------------------------------------
     * - Gives a visual presentation of elements used in <algorithm>
     * - Player needs to interact with the <algorithm objects> to progress through the algorithm
    */
    public abstract void PerformAlgorithmUserTest();
    protected abstract void UserTestUpdate();


    /* Prepares the next instruction based on the algorithm being runned
     * - Sends instruction to the next sorting element the user should move
     * - Beginners (difficulty) will be shown steps on pseudoboard and given some hints
     * - Skips instructions which doesn't contain any elements nor destination
    */
    //protected abstract int PrepareNextInstruction(InstructionBase instruction);


    // --------------------------------------- Other ---------------------------------------

    protected virtual IEnumerator FinishUserTest()
    {
        audioManager.Play("Finish");

        yield return finishStepDuration;

        TeachingAlgorithm algorithm = GetTeachingAlgorithm();
        algorithm.IsTaskCompleted = true;
        if (algorithm.PseudoCodeViewer != null)
            algorithm.PseudoCodeViewer.RemoveHightlight();
    }

    protected abstract void TaskCompletedFinishOff(); // Finish off visualization

    public abstract void ToggleVisibleStuff();


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
