using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeStatesButton : UserDeviceButtonBase {
    
    private int currentState, maxState = 3;
    private Dictionary<int, string> states;
    private Material[] materials;

    private void Awake()
    {
        currentState = 0;
        CreateStates();
    }

    private void CreateStates()
    {
        states.Add(0, "Off");
        states.Add(1, "State1");
        states.Add(2, "State2");
    }

    protected override void ChangeState()
    {
        if (currentState < maxState)
            currentState++;
        else
            currentState = 0;

        switch (currentState)
        {
            case 0: break;
            case 1: break;
            case 2: break;
        }
    }
}
