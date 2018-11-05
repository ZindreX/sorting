using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AlgorithmUserController : MonoBehaviour {

    // https://www.youtube.com/watch?v=bn8eMxBcI70

    [SteamVR_DefaultAction("Increment")]
    public SteamVR_Action_Boolean incrementAction;

    [SteamVR_DefaultAction("Decrement")]
    public SteamVR_Action_Boolean decrementAction;

    [SteamVR_DefaultAction("InteractUI")]
    public SteamVR_Action_Boolean interactUIAction;

    private string teachingMode; // create action sets instead

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (SteamVR_Input.__actions_sortingActions_in_Increment.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("Incrementing");
        }

        if (SteamVR_Input.__actions_sortingActions_in_Decrement.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("Decrementing");
        }

        if (SteamVR_Input.__actions_sortingActions_in_InteractUI.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Debug.Log("Opening menu");
        }


    }
}
