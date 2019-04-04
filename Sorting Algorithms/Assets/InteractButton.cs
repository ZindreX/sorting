using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Animator))]
public class InteractButton : MonoBehaviour {

    private bool activated;

    private Animator animator;
    private AudioManager audioManager;

    private void Awake()
    {
        activated = false;
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public bool Activated
    {
        get { return activated; }
    }

    public void ButtonClicked()
    {
        activated = !activated;
        animator.SetBool("Activated", activated);
        audioManager.Play("ButtonClick");
    }
    

}
