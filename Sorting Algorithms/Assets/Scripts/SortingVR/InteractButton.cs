using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InteractButton : MonoBehaviour {

    private bool isActive = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsActive
    {
        get { return isActive; }
    }

    public void OnMouseEnter()
    {
        //Debug.Log("Enter");
    }

    public void OnMouseExit()
    {
        //Debug.Log("Exit");
    }

    public void OnMouseDown()
    {
        //Debug.Log("Up");

        if (isActive)
            ButtonStart();
        else
            ButtonStop();
        isActive = !isActive;
    }

    public void ButtonStart()
    {
        Debug.Log("Starting");
        animator.Play("ButtonStartProcess");
    }

    public void ButtonStop()
    {
        Debug.Log("Stopping");
        animator.Play("ButtonStopProcess");
    }
}
