using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffButton : UserDeviceButtonBase {

    private bool state;

    private void Awake()
    {
        state = false;
    }


    protected override void ChangeState()
    {
        state = !state;
    }
}
