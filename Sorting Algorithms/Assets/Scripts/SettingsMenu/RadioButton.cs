using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButton : SettingsMenuButton {

    [SerializeField]
    private Material activeMat, inactiveMat;

    [SerializeField]
    private Section[] subSections;

    private bool active;

    protected override void Awake()
    {
        base.Awake();

        Deactivateitem();
    }

    // If this button has been chosen
    public void ActivateItem()
    {
        // Make it visible that it has been chosen
        active = true;
        ChangeAppearance(activeMat);

        // Report to section so that other buttons within this section becomes inactive
        section.ReportActivation(itemID);

        // Make sub section(s) of this button visible
        SetSubSectionsVisible(true);
    }

    // If some other button within this section has been chosen
    public void Deactivateitem()
    {
        // Make it visible that it hasn't been chosen
        active = false;
        ChangeAppearance(inactiveMat);

        // Hide sub section(s)
        SetSubSectionsVisible(false);
    }
    
    private void SetSubSectionsVisible(bool visble)
    {
        if (subSections != null)
        {
            foreach (Section section in subSections)
            {
                section.SetSectionVisible(visble);
            }
        }
    }

    public override string ItemRole()
    {
        return Util.RADIO_BUTTON;
    }
}
