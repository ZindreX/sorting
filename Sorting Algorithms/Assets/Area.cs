using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(IgnoreTeleportTrace))]
[RequireComponent(typeof(BoxCollider))]
public class Area : MonoBehaviour {

    [SerializeField]
    private string areaName;

    [SerializeField]
    private AreaCode areaCode;
    private enum AreaCode { InitArea, LoadScene }

    [SerializeField]
    private int loadScene;

    private bool playerWithinArea;


    public string AreaName
    {
        get { return areaName; }
    }

    public bool PlayerWithinArea
    {
        get { return playerWithinArea; }
    }

    public void PerformAreaCode()
    {
        switch ((int)areaCode)
        {
            case 0: DoAreaStuff(); break;
            case 1: SceneManager.LoadScene(loadScene); break;
        }
    }

    public void DoAreaStuff()
    {
        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>> area: " + areaName);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            PerformAreaCode();
            playerWithinArea = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            playerWithinArea = false;
        }
    }

}
