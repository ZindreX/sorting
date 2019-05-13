using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartButton : ToggleButton {

    [SerializeField]
    private Material delayMaterial;

    private bool canToggle;

    private AudioSource delayErrorSound;
    private WaitForSeconds delay = new WaitForSeconds(2f);

    protected override void Awake()
    {
        base.Awake();
        canToggle = true;
        delayErrorSound = GetComponent<AudioSource>();
    }

    public override void Toggle()
    {
        if (canToggle)
        {
            StartCoroutine(DelayNextToggle());
            base.Toggle();
            section.ReportItemClicked(section.SectionID, itemID, interactionDescription);
        }
        else
        {
            ChangeAppearance(delayMaterial);
            delayErrorSound.Play();
        }
    }

    private IEnumerator DelayNextToggle()
    {
        canToggle = false;
        yield return delay;
        canToggle = true;

        if (state)
            ChangeAppearance(onMaterial);
        else
            ChangeAppearance(offMaterial);
    }

    public override string ItemRole()
    {
        return Util.START_BUTTON;
    }
}
