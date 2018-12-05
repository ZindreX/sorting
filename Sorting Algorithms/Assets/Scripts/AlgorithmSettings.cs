using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmSettings : MonoBehaviour {

    /* -------------------------------------------- Algorithm settings ----------------------------------------------------
     * Takes all input from the settings walls in the VR environment and forwards it to the main unit
     * 
    */

    [SerializeField]
    private AlgorithmManagerBase algorithmManager;

    [SerializeField]
    private GameObject subSettingsTitle;

    [SerializeField]
    private GameObject[] difficultyButtons, tutorialSpeedButtons;

    private void Start()
    {
        ChangeSubSettingsDisplay(algorithmManager.TeachingMode);
    }

    public string TeachingMode
    {
        set { algorithmManager.TeachingMode = value; ChangeSubSettingsDisplay(value); }
    }

    public int Difficulty
    {
        set { algorithmManager.Difficulty = value; }
    }

    public int NumberOfElements
    {
        set { algorithmManager.NumberOfElements = value; }
    }

    public bool Duplicates
    {
        set { algorithmManager.Duplicates = value; }
    }

    public string SortingCase
    {
        set { algorithmManager.SortingCase = value; }
    }

    public float TutorialSpeed
    {
        set { algorithmManager.TutorialSpeed = value; }
    }

    public void StartSorting()
    {
        algorithmManager.InstantiateSetup();
    }

    public void StopSorting()
    {
        algorithmManager.DestroyAndReset();
    }

    // Changes the sub settings (middle wall w/buttons) based on the chosen teaching mode
    private void ChangeSubSettingsDisplay(string teachingMode)
    {
        switch (teachingMode)
        {
            case Util.TUTORIAL:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "Tutorial speed";
                ActiveButtons(false, true);
                break;
            case Util.STEP_BY_STEP:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "";
                ActiveButtons(false, false);
                break;
            case Util.USER_TEST:
                subSettingsTitle.GetComponent<UnityEngine.UI.Text>().text = "Difficulty";
                ActiveButtons(true, false);
                break;
        }
    }

    private void ActiveButtons(bool diffActive, bool speedActive)
    {
        foreach (GameObject obj in difficultyButtons)
        {
            obj.SetActive(diffActive);
            //obj.active = diffActive;
        }
        foreach (GameObject obj in tutorialSpeedButtons)
        {
            obj.SetActive(speedActive);
            //obj.active = speedActive;
        }
    }

}
