using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDeviceButton : MonoBehaviour {

    [SerializeField]
    private Material[] buttonColors;
   

    private bool active = true, turnedOn = false;


    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    private void ChangeState()
    {
        if (active)
        {
            //switch ()
            //{
            //    case 0: break;
            //}
        }
    }


    private void OnMouseDown()
    {
        ChangeState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // VR touch stuff
    }



}
