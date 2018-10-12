using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserDeviceButtonBase : MonoBehaviour {

    [SerializeField]
    private Material[] buttonColors;
   
    private bool active = true, turnedOn = false;

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    protected abstract void ChangeState();


    private void OnMouseDown()           // Change for VR touch stuff
    {
        ChangeState();
    }



}
