using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Animator))]
public class ButtonGoToScene : MonoBehaviour {

    private bool isActive = false;
    [SerializeField]
    private int sceneBuildIndex;

    private Animator animator;
    [SerializeField]
    private Interactable interactable;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if (interactable.isHovering)
        {
            Interaction();
        }
    }

    public bool IsActive
    {
        get { return isActive; }
    }

    public void Interaction()
    {
        isActive = !isActive;
        animator.Play("ButtonClick");
        StartCoroutine(TeleportPlayer());
    }

    private IEnumerator TeleportPlayer()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneBuildIndex);
    }

}

