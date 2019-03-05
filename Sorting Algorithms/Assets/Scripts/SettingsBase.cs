using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class SettingsBase : MonoBehaviour {

    protected float algorithmSpeed;
    protected string algorithm, teachingMode;
    protected int difficulty;

    [Header("Settings base")]
    //[SerializeField]
    //public MainManager mainManager;

    [SerializeField]
    protected TextMeshPro title, tooltips;

    [Space(2)]
    [Header("Overall settings")]
    [SerializeField]
    protected bool useDebuggingSettings;
    [SerializeField]
    protected TeachingModeEditor teachingModeEditor;
    protected enum TeachingModeEditor { Demo, StepByStep, UserTest }

    [SerializeField]
    protected AlgorithmSpeedEditor algorithmSpeedEditor;
    protected enum AlgorithmSpeedEditor { Slow, Normal, Fast, Test }

    protected Dictionary<string, OnOffButton> buttons;

    //
    public abstract void PrepareSettings();
    protected abstract MainManager MainManager { get; set; }

    public void SetSettingsActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public string Algorithm
    {
        get { return algorithm; }
        set { algorithm = value; FillTooltips("Algorithm:\n" + value); SetActiveButton(algorithm); }
    }

    public string TeachingMode
    {
        get { return teachingMode; }
        set { teachingMode = value; FillTooltips("Teaching mode:\n" + value); SetActiveButton(teachingMode); }
    }

    public virtual bool IsDemo()
    {
        return teachingMode == Util.DEMO;
    }

    public bool IsStepByStep()
    {
        return teachingMode == Util.STEP_BY_STEP;
    }

    public bool IsUserTest()
    {
        return teachingMode == Util.USER_TEST;
    }

    // Beginner, Intermediate, or Examination
    public int Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; FillTooltips("Difficulty:\n" + value); SetActiveButton(Util.ConvertDifficulty(value)); }
    }

    public float AlgorithmSpeed
    {
        get { return algorithmSpeed; }
        set { algorithmSpeed = value; FillTooltips("Demo speed:\n" + value + " sec per step"); SetActiveButton(Util.ConvertAlgorithmSpeed(value)); }
    }

    public void InstantiateTask()
    {
        MainManager.InstantiateSetup();
    }

    public void StartStopTask()
    {
        if (!MainManager.AlgorithmStarted)
            MainManager.StartAlgorithm();
        else
        {
            MainManager.UserStoppedAlgorithm = true; //
            //MainManager.GetTeachingAlgorithm().DemoStepDuration = new WaitForSeconds(0f);
            MainManager.DestroyAndReset();
        }
    }

    public void StartTask()
    {
        MainManager.StartAlgorithm();
    }

    public void StopTask()
    {
        MainManager.DestroyAndReset();
    }

    public void FillTitle(string title)
    {
        if (title != null && tooltips != null)
        {
            this.title.text = title;
            tooltips.text = "";
        }
    }

    public void FillTooltips(string text)
    {
        if (title != null && tooltips != null)
        {
            title.text = "";
            tooltips.text = text;
        }
    }

    protected void SetActiveButton(string buttonID)
    {
        if (buttons.ContainsKey(buttonID))
        {
            OnOffButton button = buttons[buttonID];

            if (button.IsOnOffButton)
                button.ToggleState();
            else
                button.ChangeState();
        }
    }


    //public void ActiveButtonInSection(GameObject[] section, GameObject activeButton)
    //{
    //    foreach (GameObject button in section)
    //    {
    //        if (button == activeButton)
    //            button.GetComponentInChildren<Renderer>().material = activeButtonMaterial;
    //        else
    //            button.GetComponentInChildren<Renderer>().material = inactiveButtonMaterial;
    //    }
    //}
    //public void UpdateSection(GameObject section, int active)
    //{
    //    Component[] parts = section.GetComponentsInChildren(typeof(MeshRenderer));
    //    for (int i=0; i < parts.Length; i++)
    //    {
    //        parts[i].GetComponent<MeshRenderer>()
    //    }
    //}

}
