using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class Pointer : MonoBehaviour {

    [SerializeField]
    private float laserWidth = 100f, laserRange = 50f;
    private int layerMask;

    [SerializeField]
    private Transform pointerEnd;

    [SerializeField]
    private Camera vrCamera;

    private LineRenderer laserBeam;

	// Use this for initialization
	void Start () {
        laserBeam = GetComponent<LineRenderer>();

        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

    }
	
	// Update is called once per frame
	void Update () {

        if (false) // input steamvr
        {
            laserBeam.enabled = true;

            // Create a vector at the center of our camera's viewport
            //Vector3 rayOrigin = vrCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

            // Declare a raycast hit to store information about what our raycast hit
            RaycastHit hit;

            // Set the start position of our visual effect for our laser to the position of the pointerEnd
            laserBeam.SetPosition(0, pointerEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(pointerEnd.position, pointerEnd.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))//Physics.Raycast(rayOrigin, vrCamera.transform.forward, out hit, laserRange))
            {
                // Set the end position for our laser line
                laserBeam.SetPosition(1, hit.point);

                // Get a reference to the node the raycast collided with
                Node node = hit.collider.GetComponent<Node>();

                // If there was a node script attached
                if (node != null)
                {
                    node.Visited = !node.Visited;
                }
            }
            else
            {
                // If we didn't hit anything, set the end of the line to a position directly in front of the camera at the distance of laserRange
                //laserBeam.SetPosition(1, rayOrigin + (vrCamera.transform.position * laserRange));
            }

        }
        else
        {
            laserBeam.enabled = false;
        }
    }



    private void Test()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
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
