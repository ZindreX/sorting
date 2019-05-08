using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AlgorithmUserController : MonoBehaviour {

    /* -------------------------------------------- Algorithm User Controller ----------------------------------------------------
     * Changes some of the controller bindings depending on the algorithm / teaching mode
     * 
     *  https://www.youtube.com/watch?v=bn8eMxBcI70
    */

    private MainManager mainManager;

    [SteamVR_DefaultAction("Increment")]
    public SteamVR_Action_Boolean incrementAction;

    [SteamVR_DefaultAction("Decrement")]
    public SteamVR_Action_Boolean decrementAction;

    [SteamVR_DefaultAction("ToggleStart")]
    public SteamVR_Action_Boolean toggleStartAction;

    private bool debugNextReady = true;


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // ************* DEBUGGING *************
        if (debugNextReady)
        {
            if (Input.GetKey(KeyCode.E))
            {
                mainManager.PlayerStepByStepInput(true);
                StartCoroutine(DebugWait());
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                mainManager.PlayerStepByStepInput(false);
                StartCoroutine(DebugWait());
            }
            else if (Input.GetKey(KeyCode.I))
            {
                mainManager.InstantiateSafeStart();
                StartCoroutine(DebugWait());
            }
            else if (Input.GetKey(KeyCode.U))
            {
                mainManager.StartAlgorithm();
            }
            else if (Input.GetKey(KeyCode.O))
            {
                mainManager.DestroyAndReset();
            }
        }
        // *************

        // Do actions according to what teaching mode the player has activated
        if (mainManager.AlgorithmInitialized)
        {
            // Used to start / stop sorting task
            if (SteamVR_Input.__actions_default_in_ToggleStart.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                mainManager.ToggleVisibleStuff();
            }


            switch (mainManager.Settings.TeachingMode)
            {
                case Util.DEMO:
                case Util.STEP_BY_STEP:

                    if (SteamVR_Input.__actions_default_in_ToggleStart.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        mainManager.PerformDemoDeviceAction(DemoDevice.PAUSE, true);
                    }

                    if (mainManager.UserPausedTask) // Demo paused --> Step-by-step
                    {
                        // Progress to the next instruction
                        if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                            mainManager.PerformDemoDeviceAction(DemoDevice.STEP_FORWARD, true);

                        // Backwards to the previous instruction
                        else if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                        {
                            if (mainManager.Settings.StepBack)
                                mainManager.PerformDemoDeviceAction(DemoDevice.STEP_BACK, true);
                            else
                                StartCoroutine(mainManager.SetFeedbackDisplay("Backward step not available for this algorithm."));
                        }
                    }
                    else // Demo play --> automatically update (speed adjustable)
                    {
                        // Increase algorithm speed
                        if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                            mainManager.PerformDemoDeviceAction(DemoDevice.INCREASE_SPEED, true);

                        // Reduce algorithm speed
                        else if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                            mainManager.PerformDemoDeviceAction(DemoDevice.REDUCE_SPEED, true);
                    }

                    break;

                case Util.USER_TEST:
                    switch (mainManager.GetTeachingAlgorithm().AlgorithmName)
                    {
                        case Util.INSERTION_SORT:
                            // Moves the pivot holder
                            if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                                ((SortAlgorithm)mainManager.GetTeachingAlgorithm()).Specials(UtilSort.INCREMENT, UtilSort.NO_VALUE, true);

                            else if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                                ((SortAlgorithm)mainManager.GetTeachingAlgorithm()).Specials(UtilSort.DECREMENT, UtilSort.NO_VALUE, false);

                            break;
                    }
                    break;
                default: Debug.Log("No teaching mode with name " + mainManager.Settings.TeachingMode); break;
            }
        }
    }


    private WaitForSeconds buttonClickWait = new WaitForSeconds(0.2f), warningMessageDuration = new WaitForSeconds(3f);
    private IEnumerator DebugWait()
    {
        debugNextReady = false;
        yield return buttonClickWait;
        debugNextReady = true;
    }
}
