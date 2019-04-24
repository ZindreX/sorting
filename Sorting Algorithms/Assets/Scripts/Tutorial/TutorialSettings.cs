using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSettings : SettingsBase {

    // Button room
    public const string ONE_ACTIVE = "One active", CUBE = "Cube", SPHERE = "Sphere", CAPSULE = "Capsule";
    public const string OPTIONAL = "Optional", Rotate = "Rotate", SCALE = "Scale";
    public const string MATERIAL = "Material", SPEED = "Speed";
    private TestObject testObject;

    // Graph room
    public const string TUTORIAL = "Tutorial", RESET = "Reset", LASER = "Laser", LASER_COLOR = "Color";

    private TutorialArea tutorialArea;
    private TutorialPointer pointer;

    private void Start()
    {
        testObject = GetComponentInChildren<TestObject>();

        tutorialArea = GetComponentInParent<TutorialArea>();
        pointer = FindObjectOfType<TutorialPointer>();

        InitButtons();
    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        Debug.Log("Section: " + sectionID + ", Item: " + itemID + ", description: " + itemDescription);
        switch (sectionID)
        {
            // Button
            case ONE_ACTIVE:
                testObject.ChangeAppearance(itemID);
                break;

            case OPTIONAL:
                bool active = ((ToggleButton)settingsSections[sectionID].GetItem(itemID)).State;
                testObject.ChangeAppearance(itemID, active);
                break;

            case MATERIAL:
                Material material = ((MultiStateButton)settingsSections[sectionID].GetItem(MATERIAL)).Material;
                testObject.ChangeAppearance(material);
                break;

            case SPEED:
                testObject.ChangeSpeed(((MultiStateButton)settingsSections[sectionID].GetItem(SPEED)).State);
                break;

            // Graph
            case TUTORIAL:
                switch (itemID)
                {
                    case RESET:
                        TutorialGraphTask[] graphTasks = FindObjectsOfType<TutorialGraphTask>();
                        foreach (TutorialGraphTask graphTask in graphTasks)
                        {
                            graphTask.ResetTask();
                        }
                        break;
                }
                break;

            case LASER:
                pointer.SetLaserBeamMaterial(((MultiStateButton)settingsSections[sectionID].GetItem(LASER_COLOR)).State);
                break;
        }
    }

    public TutorialArea TutorialArea
    {
        get { return tutorialArea; }
    }

    protected override void InitButtons()
    {
        switch (tutorialArea.AreaName)
        {
            case "Button":
                InitButtonState(ONE_ACTIVE, CUBE);
                InitButtonState(OPTIONAL, Rotate, false);
                InitButtonState(OPTIONAL, SCALE, false);
                InitButtonState(MATERIAL, MATERIAL, 0);
                InitButtonState(SPEED, SPEED, 0);
                break;

            case "Graph":
                InitButtonState(LASER, LASER_COLOR, 0);
                break;
        }

    }
}
