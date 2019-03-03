using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsBase : MonoBehaviour {

    protected float algorithmSpeed;
    protected string algorithm, teachingMode;

    [Header("Settings base")]
    //[SerializeField]
    //public MainManager mainManager;

    [SerializeField]
    protected Material activeButtonMaterial, inactiveButtonMaterial;

    [SerializeField]
    protected TextMeshPro title, tooltips;

    [Header("Overall settings")]
    [SerializeField]
    protected TeachingModeEditor teachingModeEditor;
    protected enum TeachingModeEditor { Demo, StepByStep, UserTest }

    [SerializeField]
    protected AlgorithmSpeedEditor algorithmSpeedEditor;
    protected enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }

    //
    public abstract void PrepareSettings();
    protected abstract MainManager MainManager { get; }

    public string Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; FillTooltips("Algorithm: " + value); }
    }

    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; FillTooltips("Teaching mode: " + value); }
    }

    public bool IsDemo()
    {
        return teachingMode == Util.DEMO || teachingMode == Util.STEP_BY_STEP; // fix??
    }

    public bool IsStepByStep()
    {
        return teachingMode == Util.STEP_BY_STEP;
    }

    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    public float AlgorithmSpeed
    {
        get { return algorithmSpeed; }
        set { algorithmSpeed = value; FillTooltips("Demo speed: " + value + "s"); }
    }

    public void ActiveButtonInSection(GameObject[] section, GameObject activeButton)
    {
        foreach (GameObject button in section)
        {
            if (button == activeButton)
                button.GetComponentInChildren<Renderer>().material = activeButtonMaterial;
            else
                button.GetComponentInChildren<Renderer>().material = inactiveButtonMaterial;
        }
    }

    public void StartTask()
    {
        MainManager.InstantiateSetup();
    }

    public void StopTask()
    {
        MainManager.DestroyAndReset();
    }

    public void FillTitle(string title)
    {
        this.title.text = title;
        tooltips.text = "";
    }

    public void FillTooltips(string text)
    {
        title.text = "";
        tooltips.text = text;
    }

}
