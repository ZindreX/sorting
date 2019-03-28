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
    private bool currentState;

    [SerializeField]
    private bool enableStepBack;

    private MainManager mainManager;

    protected override void Awake()
    {
        base.Awake();

        throwAble = false;


        section = GetComponentInChildren<Section>();
        section.SectionManager = this;

        section.InitItem(PAUSE, true);

        // Gather settings menu items
        Component[] components = GetComponentsInChildren<SettingsMenuItem>();

        // Create dictionary
        buttons = new Dictionary<string, SettingsMenuItem>();
        foreach (Component component in components)
        {
            SettingsMenuItem item = component.GetComponent<SettingsMenuItem>();
            buttons.Add(item.ItemID, item);
        }

        pauseButton = (ToggleButton)buttons[PAUSE];

        // Hide speed at start
        buttons[REDUCE_SPEED].gameObject.SetActive(false);
        buttons[INCREASE_SPEED].gameObject.SetActive(false);
    }

    private void Start()
    {
        mainManager = FindObjectOfType<MainManager>();


        mainManager.Settings.StepBack = enableStepBack;
        if (!enableStepBack)
            Destroy(buttons[STEP_BACK].gameObject);

    }

    private void Update()
    {
        // Hide buttons based on state (Pause: Step-by-step with (back-)/forward step, Unpaused: Demo with reduce-/increase speed
        bool pauseButtonState = pauseButton.State;
        if (currentState != pauseButtonState)
        {
            buttons[REDUCE_SPEED].gameObject.SetActive(pauseButtonState);
            buttons[INCREASE_SPEED].gameObject.SetActive(pauseButtonState);

            if (enableStepBack)
                buttons[STEP_BACK].gameObject.SetActive(!pauseButtonState);

            buttons[STEP_FORWARD].gameObject.SetActive(!pauseButtonState);

            currentState = pauseButtonState;
        }
    }


    public void UpdateInteraction(string sectionID, string itemID, string itemDescription)
    {
        mainManager.PerformDemoDeviceAction(itemID);
    }

}
