using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressTracker : MonoBehaviour, IMoveAble {

    [SerializeField]
    private GameObject progressbar;

    [SerializeField]
    private TextMeshPro percentText, instructionsText;

    private int currentProgress, userActionCount, totaltInstructions;
    private bool standard;

    private Vector3 increaseProgressSizePerStep = new Vector3(0f, 0.01f, 0f);
    private Vector3 moveProgressBarPerStep = new Vector3(0.01f, 0f, 0f);
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    public void InitProgressTracker(int userActionCount, int totaltInstructions)
    {
        currentProgress = 0;
        this.userActionCount = userActionCount;
        this.totaltInstructions = totaltInstructions;

        float progress = 1f / userActionCount;
        increaseProgressSizePerStep = new Vector3(0f, progress, 0f);
        moveProgressBarPerStep = new Vector3(progress, 0f, 0f);
    }

    public void InitProgressTracker(int total)
    {
        standard = true;
        InitProgressTracker(total, total);
        progressbar.transform.localScale += increaseProgressSizePerStep * 15f;
        moveProgressBarPerStep /= 3.35f;
        progressbar.transform.position += moveProgressBarPerStep * 5;
    }

    public void Increment()
    {
        currentProgress++;

        if (standard)
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
