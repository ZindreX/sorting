using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Pointer2 : MonoBehaviour {

    [SteamVR_DefaultAction("PointerShoot")]
    public SteamVR_Action_Boolean pointerShootAction;

    [SerializeField]
    private GameObject laserbeam;

    [SerializeField]
    private Transform rightHand;

    [SerializeField]
    private float maxRange = 1f;

    private int layerMask;

    private void Start()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
    }


    // Update is called once per frame
    void Update () {
        transform.position = rightHand.position;
        transform.rotation = rightHand.rotation;

        RaycastHit hit;
        if (SteamVR_Input.__actions_default_in_PointerShoot.GetState(SteamVR_Input_Sources.RightHand)) 
        {
            if (Physics.Raycast(rightHand.position, rightHand.TransformDirection(Vector3.forward), out hit)) //, Mathf.Infinity, layerMask))
            {
                Vector3 collidePos = hit.collider.transform.position;

                float distance = hit.distance;

                //if (distance > maxRange)
                //    distance = maxRange;

                // ???
                //Vector3 target = hit.point;
                //laserbeam.transform.position = Vector3.Lerp(pointerEnd.position, target, 0.5f);  //new Vector3(pos.x, pos.y, distance / 2);

                //if (laserbeam.transform.localScale.y < distance)
                //    laserbeam.transform.localScale += new Vector3(0f, distance, 0f);
                //else
                //    laserbeam.transform.localScale -= new Vector3(0f, prevDistance - distance, 0f);

                //Vector3 temp = laserbeam.transform.localScale;
                //temp.y += distance;

                Vector3 center = transform.position - collidePos;
                laserbeam.transform.localScale += new Vector3(0f, distance, 0f);
                laserbeam.transform.position = center;

            }
            laserbeam.SetActive(true);
        }
        else
        {
            laserbeam.SetActive(false);
            laserbeam.transform.position = rightHand.position;
        }

        // Debug
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
        //}

    }
}
