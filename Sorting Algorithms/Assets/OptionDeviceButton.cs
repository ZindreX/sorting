using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionDeviceButton : MonoBehaviour {

    [SerializeField]
    private Material[] buttonColors;

    private bool active = true;


    public bool Active
    {
        get { return active; }
        set { active = value; }
    }



}
