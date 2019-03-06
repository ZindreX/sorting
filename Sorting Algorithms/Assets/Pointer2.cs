using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer2 : MonoBehaviour {

    [SerializeField]
    private GameObject laserbeam;

    [SerializeField]
    private Transform pointerEnd;

    [SerializeField]
    private float maxRange = 1f;

    private int layerMask;

    private void Start()
    {
        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update () {

        RaycastHit hit;
        if (true) //Input.GetKey(KeyCode.E))
        {
            if (Physics.Raycast(pointerEnd.position, pointerEnd.TransformDirection(Vector3.forward), out hit)) //, Mathf.Infinity, layerMask))
            {
                Vector3 collidePos = hit.collider.transform.position;

                float distance = hit.distance;
                Debug.Log(distance);
  
                if (distance > maxRange)
                    distance = maxRange;

                // ???
                //Vector3 target = hit.point;
                //laserbeam.transform.position = Vector3.Lerp(pointerEnd.position, target, 0.5f);  //new Vector3(pos.x, pos.y, distance / 2);

                if (laserbeam.transform.localScale.y < distance)
                    laserbeam.transform.localScale += new Vector3(0f, distance, 0f);
                laserbeam.transform.localPosition = new Vector3(0f, 0f, distance);
        
            }

            laserbeam.SetActive(true);
        }
        else
        {
            laserbeam.SetActive(false);
            laserbeam.transform.position = transform.position;
        }

        // Debug
        if (Physics.Raycast(pointerEnd.position, pointerEnd.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(pointerEnd.position, pointerEnd.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(pointerEnd.position, pointerEnd.TransformDirection(Vector3.forward) * 1000, Color.red);
        }

    }
}
