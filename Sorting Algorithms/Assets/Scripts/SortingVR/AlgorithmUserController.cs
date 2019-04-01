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

    [SerializeField]
    private MainManager mainManager;

    [SerializeField]
    private UnityEngine.UI.Text warningMessage;

    [SteamVR_DefaultAction("Increment")]
    public SteamVR_Action_Boolean incrementAction;

    [SteamVR_DefaultAction("Decrement")]
    public SteamVR_Action_Boolean decrementAction;

    [SteamVR_DefaultAction("ToggleStart")]
    public SteamVR_Action_Boolean toggleStartAction;


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Used to start / stop sorting task
        if (SteamVR_Input.__actions_default_in_ToggleStart.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Debug.Log("Toggle");
            mainManager.ToggleVisibleStuff();
        }

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
                mainManager.InstantiateSetup();
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
            switch (mainManager.Settings.TeachingMode)
            {
                case Util.DEMO:
                case Util.STEP_BY_STEP:
                    if (!mainManager.UserPausedTask)
                        return;

                    // Progress to the next instruction
                    if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                        mainManager.PlayerStepByStepInput(true);

                    // Backwards to the previous instruction
                    else if (mainManager.Settings.StepBack && SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                        mainManager.PlayerStepByStepInput(false);
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

    private bool debugNextReady = true;
    private WaitForSeconds buttonClickWait = new WaitForSeconds(0.2f), warningMessageDuration = new WaitForSeconds(3f);
    private IEnumerator DebugWait()
    {
        debugNextReady = false;
        yield return buttonClickWait;
        debugNextReady = true;
    }

    // Test of warning messages in UI (only works on the monitor)
    public IEnumerator CreateWarningMessage(string warningMessage, Color color)
    {
        this.warningMessage.color = color;
        this.warningMessage.text = warningMessage;
        yield return warningMessageDuration;
        this.warningMessage.text = "";
    }
}
