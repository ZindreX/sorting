using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AlgorithmUserController : MonoBehaviour {

    // https://www.youtube.com/watch?v=bn8eMxBcI70

    [SerializeField]
    private GameObject algorithmManagerObj;
    private AlgorithmManagerBase algorithmManager;

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
        // Used to start / stop sorting session
        if (SteamVR_Input.__actions_default_in_ToggleStart.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (!algorithmManager.ControllerReady)
                algorithmManager.InstantiateSetup();
            else
                algorithmManager.DestroyAndReset();
        }

        switch (algorithmManager.TeachingMode)
        {
            case Util.TUTORIAL:
                if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    Debug.Log("Incrementing button clicked - no action implemented for Tutorial");
                }

                if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    Debug.Log("Decrementing button clicked - no action implemented for Tutorial");
                }
                break;

            case Util.STEP_BY_STEP:
                if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    algorithmManager.PlayerStepByStepInput(true);
                    break;
                }

                if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    algorithmManager.PlayerStepByStepInput(false);
                    break;
                }
                break;

            case Util.USER_TEST:
                switch (algorithmManager.Algorithm.AlgorithmName)
                {
                    case Util.INSERTION_SORT:
                        if (SteamVR_Input.__actions_default_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
                            algorithmManager.Algorithm.Specials(Util.INCREMENT, Util.NO_VALUE, true);

                        if (SteamVR_Input.__actions_default_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
                            algorithmManager.Algorithm.Specials(Util.DECREMENT, Util.NO_VALUE, false);

                        break;
                }
                break;
        }
    }


}
