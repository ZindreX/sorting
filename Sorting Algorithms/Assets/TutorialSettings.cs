using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSettings : SettingsBase {

    private MultiStateButton laserButton;

    private TutorialPointer pointer;

    private void Start()
    {
        laserButton = (MultiStateButton)settingsSections["Laser"].GetItem("Color");
        pointer = FindObjectOfType<TutorialPointer>();
    }

    // Fix and remove?
    protected override MainManager MainManager
    {
        get { return null; }
        set { }
    }

    public override void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        Debug.Log("Section: " + sectionID + ", Item: " + itemID + ", description: " + itemDescription);
        switch (sectionID)
        {
            case "Reset": break;
            case "Laser":
                pointer.SetLaserBeamMaterial(laserButton.State);
                break;
            default:  base.UpdateInteraction(sectionID, itemID, itemDescription); break;
        }
    }

    protected override void InitButtons()
    {

    }
}
