using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Door : MonoBehaviour {

    [Header("Door settings")]
    [Space(5)]
    [SerializeField]
    protected bool startPosition;

    protected bool doorOpen;

    protected virtual void Awake()
    {
        doorOpen = startPosition;

        if (startPosition)
            OpenDoor();
        else
            CloseDoor();
    }

    public bool DoorOpen
    {
        get { return doorOpen; }
    }

    public abstract void OpenDoor();
    public abstract void CloseDoor();

}
