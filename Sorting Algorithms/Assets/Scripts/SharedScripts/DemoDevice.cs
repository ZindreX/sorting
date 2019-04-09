using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDevice : InteractionDeviceBase, ISectionManager {

    public const string DEMO_DEVICE = "Demo device";
    public const string PAUSE = "Pause", STEP_BACK = "Step back", STEP_FORWARD = "Step forward";
    public const string REDUCE_SPEED = "Reduce speed", INCREASE_SPEED = "Increase speed";

    private Dictionary<string, SettingsMenuItem> buttons;

    private Section section;
    private ToggleButton pauseButton;

    [SerializeField]
    private bool enableStepBack;

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

    private void Start()
    {
        // Find main manager
        mainManager = FindObjectOfType<MainManager>();

        // Destroy backward step (not finished)
        if (mainManager != null)
            mainManager.Settings.StepBack = enableStepBack;

        if (!enableStepBack)
            Destroy(buttons[STEP_BACK].gameObject);
    }

    //private void Update()
    //{
    //    // Hide buttons based on state (Pause: Step-by-step with (back-)/forward step, Unpaused: Demo with reduce-/increase speed
    //    bool gamePaused = pauseButton.State;
    //    if (currentState != gamePaused)
    //    {
    //        Debug.Log("State changed, paused: " + gamePaused);

    //        buttons[REDUCE_SPEED].gameObject.SetActive(!gamePaused);
    //        buttons[INCREASE_SPEED].gameObject.SetActive(!gamePaused);

    //        if (enableStepBack)
    //            buttons[STEP_BACK].gameObject.SetActive(gamePaused);

    //        buttons[STEP_FORWARD].gameObject.SetActive(gamePaused);

    //        currentState = gamePaused;
    //    }
    //}

    public void InitDemoDevice(bool startPaused)
    {
        section.InitItem(PAUSE, startPaused);
        TransitionPause(startPaused);
    }

    /* ----- Buttons ------
     * > Play   : enable speed adjustment / disable step-by-step
     * > Paused : enable step-by-step / disable speed
    */
    public void TransitionPause(bool paused)
    {
        Debug.Log("State changed, paused: " + paused);
        buttons[REDUCE_SPEED].gameObject.SetActive(!paused);
        buttons[INCREASE_SPEED].gameObject.SetActive(!paused);

        if (enableStepBack)
            buttons[STEP_BACK].gameObject.SetActive(paused);

        buttons[STEP_FORWARD].gameObject.SetActive(paused);
    }

    public void SetDemoDeviceTitle(string title)
    {
        section.SetSectionTitle(title);
    }

    public void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        mainManager.PerformDemoDeviceAction(itemID);
    }

    public void ButtonActive(string buttonID, bool active)
    {
        if (buttons.ContainsKey(buttonID))
            buttons[buttonID].enabled = active; // + change color?
    }

    public override void ResetDevice()
    {
        section.InitItem(PAUSE, false);

        base.ResetDevice();
    }

}
