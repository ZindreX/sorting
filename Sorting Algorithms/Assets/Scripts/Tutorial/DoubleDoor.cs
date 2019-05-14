using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : Door {

    [Header("Double door settings")]
    [Space(10)]

    [Header("Doors")]
    [SerializeField]
    private GameObject leftDoor;

    [SerializeField]
    private GameObject rightDoor;

    [Header("Door movement")]
    [Space(5)]
    [SerializeField]
    private Vector3 openDoorPosition;

    [Space(5)]
    [SerializeField]
    private Vector3 openDoorRotation;

    private MoveObject leftDoorMoveObject, rightDoorMoveObject;

    protected override void Awake()
    {
        // Get move components of doors
        leftDoorMoveObject = leftDoor.GetComponent<MoveObject>();
        rightDoorMoveObject = rightDoor.GetComponent<MoveObject>();
        base.Awake();
    }


    public override void OpenDoor()
    {
        leftDoorMoveObject.AddVector3(-openDoorPosition);
        rightDoorMoveObject.AddVector3(openDoorPosition);
        doorOpen = true;
    }

    public override void CloseDoor()
    {
        leftDoorMoveObject.AddVector3(openDoorPosition);
        rightDoorMoveObject.AddVector3(-openDoorPosition);
        doorOpen = false;
    }

}
