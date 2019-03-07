using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneActiveButton : SettingsMenuButton {

    [SerializeField]
    private Material activeMat, inactiveMat;

    [SerializeField]
    private Section[] subSections;

    private bool active;

    private void Awake()
    {
        SetSubSectionsVisible(false);
    }

    public void ActivateItem()
    {
        active = true;
        ChangeAppearance(itemID, activeMat);
        section.ReportActivation(itemID);
    }

    public void Deactivateitem()
    {
        active = false;
        ChangeAppearance(itemID, inactiveMat);
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

}
