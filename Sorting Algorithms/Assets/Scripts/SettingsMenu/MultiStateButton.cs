using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiStateButton : SettingsMenuButton {

    [SerializeField]
    private string[] states;

    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private string[] stateDescription;

    private int state;

    protected override void Awake()
    {
        base.Awake();

        if (states.Length != materials.Length)
            Debug.LogError("States != materials");
    }

    public void InitMultiStateButton(int state)
    {
        if (state >= 0 && state < states.Length)
        {
            this.state = state;
            ChangeAppearance(states[state], materials[state]);
        }
        else
            Debug.LogError("Couldn't initalize state due to index out of range");
    }

    public void ToggleNextState()
    {
        state++;
        if (state >= states.Length)
            state = 0;

        itemID = states[state];
        interactionDescription = stateDescription[state];
        ChangeAppearance(states[state], materials[state]);
    }

    public override string ItemRole()
    {
        return Util.MULTI_STATE_BUTTON;
    }

    public int State
    {
        get { return state; }
    }

    public Material Material
    {
        get { return materials[state]; }
    }
}
