using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(Interactable))]
public abstract class InteractionDeviceBase : MonoBehaviour {

    [SerializeField]
    protected bool integrated;

    protected bool playerHoldingDevice, throwAble;

    protected Vector3 startPos;

    private Camera playerCamera;
    private Rigidbody rb;


    protected virtual void Awake()
    {
        playerCamera = FindObjectOfType<Player>().GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        throwAble = false;
    }

    private void Update()
    {
        if (integrated)
            return;

        if (throwAble && transform.position.y < 0.1f)
            SpawnDeviceInfrontOfPlayer();
    }

    public void SpawnDeviceInfrontOfPlayer()
    {
        if (integrated)
            return;

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // Place infront of player
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * 0.4f;

        // Rotate towards the player
        transform.LookAt(2 * transform.position - playerCamera.transform.position);
    }

    public void PlayerHoldingDevice(bool holding)
    {
        playerHoldingDevice = holding;

        if (holding)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            if (!throwAble)
                rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void ResetDevicePosition()
    {
        rb.useGravity = true;
        transform.position = startPos;
    }

}
