using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AlgorithmUserController : MonoBehaviour {


    [SteamVR_DefaultActionSet("default")]
    public SteamVR_ActionSet actionSet;

    [SteamVR_DefaultAction("Grip", "default")]
    public SteamVR_Action_Boolean a_grip;

    private Interactable interactable;


    private string teachingMode;

	// Use this for initialization
	void Start () {

        interactable = GetComponent<Interactable>();
        interactable.activateActionSetOnAttach = actionSet;
    }
	
	// Update is called once per frame
	void Update () {

        if (interactable.attachedToHand)
        {

            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;

            Debug.Log("Gripping: " + a_grip.GetState(hand));

            if (teachingMode == Util.USER_TEST)
            {

            }

        }
	}
}
