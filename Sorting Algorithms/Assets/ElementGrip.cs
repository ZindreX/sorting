﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
public class ElementGrip : MonoBehaviour {

    [SteamVR_DefaultActionSet("default")]
    public SteamVR_ActionSet actionSet;

    [SteamVR_DefaultAction("Grabgrip", "default")]
    public SteamVR_Action_Boolean a_grab;

    private Interactable interactable;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        interactable = GetComponent<Interactable>();
        interactable.activateActionSetOnAttach = actionSet;
	}
	
	// Update is called once per frame
	void Update () {
		if (interactable.attachedToHand)
        {
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;

            if (a_grab.GetStateDown(hand))
            {
                Debug.Log("Grabbing");
                rb.isKinematic = false;
            }
            else
            {
                rb.isKinematic = true;
            }
        }
	}

}
