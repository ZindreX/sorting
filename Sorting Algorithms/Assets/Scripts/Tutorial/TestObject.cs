using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour {

    [SerializeField]
    private Renderer[] figures;
    private Material material;

    [SerializeField]
    private Transform[] goals;
    private int goal = 0;

    private WaitForSeconds delay = new WaitForSeconds(3f);

    private Animator animator;
    private MoveObject moveObject;

    public Vector3 currentGoal; // Debugging

    private void Awake()
    {
        material = figures[0].material;
        ChangeFigure(0);

        animator = GetComponentInChildren<Animator>();
        moveObject = GetComponent<MoveObject>();
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart()
    {
        yield return delay;

        // Set first goal
        currentGoal = goals[goal].position;
        moveObject.SetDestination(currentGoal);
    }

    private void Update()
    {
        if (transform.position == currentGoal)
        {
            goal++;

            if (goal >= goals.Length)
                goal = 0;

            currentGoal = goals[goal].position;
            moveObject.SetDestination(currentGoal);

        }
    }

    public void ChangeAppearance(string figure)
    {
        switch (figure)
        {
            case TutorialSettings.CUBE: ChangeFigure(0); break;
            case TutorialSettings.SPHERE: ChangeFigure(1); break;
            case TutorialSettings.CAPSULE: ChangeFigure(2); break;
        }
    }

    private void ChangeFigure(int figure)
    {
        for (int i=0; i < figures.Length; i++)
        {
            // Enable / disable figures
            bool enable = (i == figure);
            figures[i].enabled = enable;

            // Update material of object
            if (enable)
                figures[i].material = material;
        }
    }

    public void ChangeAppearance(Material material)
    {
        this.material = material;
        GetComponentInChildren<Renderer>().material = material;
    }

    public void ChangeAppearance(string animation, bool active)
    {
        animator.SetBool(animation, active);
    }

    public void ChangeSpeed(int speed)
    {
        if (speed == 0)
            moveObject.Speed = 0f;
        else
            moveObject.Speed = Mathf.Pow(speed, speed * 1.25f);
    }




}
