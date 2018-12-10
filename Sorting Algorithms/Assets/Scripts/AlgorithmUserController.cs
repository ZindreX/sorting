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
    private GameObject algorithmManagerObj;
    private AlgorithmManagerBase algorithmManager;

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
        algorithmManager = algorithmManagerObj.GetComponent<AlgorithmManagerBase>();
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        // Used to start / stop sorting task
        if (SteamVR_Input.__actions_default_in_ToggleStart.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (!algorithmManager.ControllerReady)
                algorithmManager.InstantiateSetup();
            else
                algorithmManager.DestroyAndReset();

        }

        // ************* DEBUGGING *************
        if (debugNextReady)
        {
            if (Input.GetKey(KeyCode.E))
            {
                algorithmManager.PlayerStepByStepInput(true);
                StartCoroutine(DebugWait());
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                algorithmManager.PlayerStepByStepInput(false);
                StartCoroutine(DebugWait());
            }
            else if (Input.GetKey(KeyCode.I))
            {
                algorithmManager.InstantiateSetup();
                algorithmManager.PerformAlgorithmTutorialStep();
                StartCoroutine(DebugWait());
            }
            else if (Input.GetKey(KeyCode.O))
            {
                algorithmManager.DestroyAndReset();
            }
        }
        // *************

        switch (algorithmManager.AlgorithmSettings.TeachingMode)
        {
            case Util.DEMO:
                if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    Debug.Log("Incrementing button clicked - no action implemented for Tutorial");
                }
                else if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    Debug.Log("Decrementing button clicked - no action implemented for Tutorial");
                }
                break;

            case Util.STEP_BY_STEP:
                // Progress to the next instruction
                if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                    algorithmManager.PlayerStepByStepInput(true);

                // Backwards to the previous instruction
                else if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    algorithmManager.PlayerStepByStepInput(false);

                break;

            case Util.USER_TEST:
                switch (algorithmManager.Algorithm.AlgorithmName)
                {
                    case Util.INSERTION_SORT:
                        // Moves the pivot holder
                        if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                            algorithmManager.Algorithm.Specials(Util.INCREMENT, Util.NO_VALUE, true);

                        else if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                            algorithmManager.Algorithm.Specials(Util.DECREMENT, Util.NO_VALUE, false);

                        break;
                }
                break;
        }
    }

    private bool debugNextReady = true;
    private IEnumerator DebugWait()
    {
        debugNextReady = false;
        yield return new WaitForSeconds(0.2f);
        debugNextReady = true;
    }

    public IEnumerator CreateWarningMessage(string warningMessage, Color color)
    {
        this.warningMessage.color = color;
        this.warningMessage.text = warningMessage;
        yield return new WaitForSeconds(3.0f);
        this.warningMessage.text = "";
    }
}
