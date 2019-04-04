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
    private enum AreaCode { InitArea, OpenPath, LoadScene }

    [SerializeField]
    private int loadScene;

    protected Door door;

    private bool playerWithinArea;


    protected virtual void Awake()
    {
        door = GetComponentInChildren<Door>();
    }


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
            case 0: InitArea(); break;
            case 1: OpenPath(); break;
            case 2: SceneManager.LoadScene(loadScene); break;
        }
    }

    public virtual void InitArea()
    {

    }

    public void OpenPath()
    {
        if (door != null)
            door.OpenDoor();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>> area: " + areaName);
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
