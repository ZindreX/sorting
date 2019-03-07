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

    private void Awake()
    {
        if (states.Length != materials.Length)
            Debug.LogError("States != materials");
    }

    public void InitMultiStateButton(string state)
    {
        buttonText.text = state;
    }

    public void ToggleNextState()
    {
        state++;
        if (state >= states.Length)
            state = 0;

        ChangeAppearance(states[state], materials[state]);
    }

}
