using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDevice : InteractionDeviceBase, ISectionManager {

    public const string DEMO_DEVICE = "Demo device";
    public const string PAUSE = "Pause", STEP_BACK = "Step back", STEP_FORWARD = "Step forward";
    public const string REDUCE_SPEED = "Reduce speed", INCREASE_SPEED = "Increase speed";

    private bool demoActive;

    private Dictionary<string, SettingsMenuItem> buttons;

    private Section section;
    private ToggleButton pauseButton;

    private bool algorithmSupportsBackStep;

    private MainManager mainManager;

    protected override void Awake()
    {
        base.Awake();

        // Find section and set this (demo device) as its section manager
        section = GetComponentInChildren<Section>();
        section.SectionManager = this;

        // Gather settings menu items
        Component[] components = GetComponentsInChildren<SettingsMenuItem>();

        // Create dictionary
        buttons = new Dictionary<string, SettingsMenuItem>();
        foreach (Component component in components)
        {
            SettingsMenuItem item = component.GetComponent<SettingsMenuItem>();
            buttons.Add(item.ItemID, item);
        }

        // Find the pause button
        pauseButton = (ToggleButton)buttons[PAUSE];
    }

    protected override bool IsActive()
    {
        return demoActive;
    }

    private void Start()
    {
        // Find main manager
        mainManager = FindObjectOfType<MainManager>();
    }

    public void InitDemoDevice(bool startPaused, bool algorithmSupportsBackStep)
    {
        section.InitItem(PAUSE, startPaused);
        TransitionPause(startPaused);
        demoActive = true;
        this.algorithmSupportsBackStep = algorithmSupportsBackStep;

        // Destroy backward step button if algorithm doesnt support it
        if (!algorithmSupportsBackStep)
            Destroy(buttons[STEP_BACK].gameObject);
    }

    /* ----- Buttons ------
     * > Play   : enable speed adjustment / disable step-by-step
     * > Paused : enable step-by-step / disable speed
    */
    public void TransitionPause(bool paused)
    {
        if (paused)
            SetDemoDeviceTitle(Util.STEP_BY_STEP);
        else
            SetDemoDeviceTitle(Util.DEMO);

        buttons[REDUCE_SPEED].gameObject.SetActive(!paused);
        buttons[INCREASE_SPEED].gameObject.SetActive(!paused);

        if (algorithmSupportsBackStep)
            buttons[STEP_BACK].gameObject.SetActive(paused);

        buttons[STEP_FORWARD].gameObject.SetActive(paused);
    }

    public void OtherSourceClick(string itemID)
    {
        switch (itemID)
        {
            case PAUSE: pauseButton.Toggle(); break;
        }
    }

    public void SetDemoDeviceTitle(string title)
    {
        section.SetSectionTitle(title);
    }

    public void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        mainManager.PerformDemoDeviceAction(itemID, false);
    }

    public void ButtonActive(string buttonID, bool active)
    {
        if (buttons.ContainsKey(buttonID))
            buttons[buttonID].enabled = active; // + change color?
    }

    public override void ResetDevice()
    {
        section.InitItem(PAUSE, false);
        demoActive = false;

        base.ResetDevice();
    }

}
