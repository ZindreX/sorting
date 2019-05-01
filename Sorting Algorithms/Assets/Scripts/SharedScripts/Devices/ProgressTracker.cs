using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressTracker : MonoBehaviour, IMoveAble {

    [SerializeField]
    private GameObject progressbar;

    private Transform progressbarInitTransform;

    [SerializeField]
    private TextMeshPro percentText, instructionsText;

    private int currentProgress, userActionCount, totaltInstructions;
    private bool otherUseCase; // temp fix

    private Vector3 increaseProgressSizePerStep = new Vector3(0f, 0.01f, 0f);
    private Vector3 moveProgressBarPerStep = new Vector3(0.01f, 0f, 0f);
    private Vector3 startPos;

    private void Awake()
    {
        // Position of the game object (used in graphVR to move it out -> pseudocode visibility)
        startPos = transform.position;

        // For resetting the progressbar
        progressbarInitTransform = progressbar.transform;
    }

    public void InitProgressTracker(int userActionCount, int totaltInstructions)
    {
        currentProgress = 0;
        this.userActionCount = userActionCount;
        this.totaltInstructions = totaltInstructions;

        float progress = 1f / userActionCount;
        increaseProgressSizePerStep = new Vector3(0f, progress, 0f);

        if (otherUseCase)
            moveProgressBarPerStep = new Vector3(0f, 0f, progress); //???
        else
            moveProgressBarPerStep = new Vector3(progress, 0f, 0f);
    }

    public void InitProgressTracker(int total)
    {
        otherUseCase = true;
        InitProgressTracker(total, total);

        if (increaseProgressSizePerStep.y == Mathf.Infinity)
        {
            Debug.Log("Progressbar infinty!");
            increaseProgressSizePerStep = new Vector3(0f, 0.01f, 0f);
        }

        progressbar.transform.localScale += increaseProgressSizePerStep * 5f;
        moveProgressBarPerStep /= 3.35f;

        if (moveProgressBarPerStep.z == Mathf.Infinity)
        {
            Debug.Log("Progressbar infinty!");
            moveProgressBarPerStep = new Vector3(0f, 0f, 0.01f);
        }
        progressbar.transform.position += moveProgressBarPerStep * 2;
    }

    public void Increment()
    {
        currentProgress++;

        if (otherUseCase)
        {
            progressbar.transform.position += moveProgressBarPerStep;
        }
        else
        {
            // Percent
            progressbar.transform.localScale += increaseProgressSizePerStep;
            progressbar.transform.position += moveProgressBarPerStep;

            // Instructions
            instructionsText.text = currentProgress + "/" + userActionCount;
        }
    }

    public void Decrement()
    {
        currentProgress--;

        // Percent
        progressbar.transform.localScale -= increaseProgressSizePerStep;
        progressbar.transform.position -= moveProgressBarPerStep;

        // Instructions
        instructionsText.text = currentProgress + "/" + userActionCount;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Increment();
    //    }
    //}

    public void ResetProgress()
    {
        progressbar.transform.localScale -= increaseProgressSizePerStep * currentProgress;
        progressbar.transform.position -= moveProgressBarPerStep * currentProgress;

        currentProgress = 0;

        instructionsText.text = "";
        percentText.text = "";
    }

    public void MoveOut()
    {
        transform.position += new Vector3(4f, 0f, 0f);
    }

    public void MoveBack()
    {
        transform.position = startPos;
    }
}
