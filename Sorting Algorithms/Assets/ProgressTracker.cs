﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressTracker : MonoBehaviour {

    [SerializeField]
    private GameObject progressbar;

    [SerializeField]
    private TextMeshPro percentText, instructionsText;

    private int currentProgress, userActionCount, totaltInstructions;
    private Vector3 increaseProgressSize = new Vector3(0f, 0.01f, 0f);
    private Vector3 moveProgressBar = new Vector3(0.01f, 0f, 0f);

    public void InitProgressTracker(int userActionCount, int totaltInstructions)
    {
        this.userActionCount = userActionCount;
        this.totaltInstructions = totaltInstructions;
    }

    public void Increment()
    {
        currentProgress++;

        // Percent
        progressbar.transform.localScale += increaseProgressSize;
        progressbar.transform.position += moveProgressBar;

        // Instructions
        instructionsText.text = currentProgress + "/" + totaltInstructions;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Increment();
        }
    }


}
