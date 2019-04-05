using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TutorialPointer : MonoBehaviour {

    [SteamVR_DefaultAction("PointerShoot")]
    public SteamVR_Action_Boolean pointerShootAction;

    [SerializeField]
    private float laserRange = 50f;
    private int layerMask;
    private bool allowShooting;

    [SerializeField]
    private Transform leftHand, rightHand;

    [SerializeField]
    private Material[] laserMaterials;
    private LineRenderer laserBeam;


    public TutorialNode prevNodeShot;

    // Use this for initialization
    void Start()
    {
        prevNodeShot = null;

        laserBeam = GetComponent<LineRenderer>();
        laserBeam.material = laserMaterials[0];

        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowShooting)
        {
            ResetLaserBeam();
            return;
        }

        transform.position = rightHand.position;
        transform.rotation = rightHand.rotation;

        //if (SteamVR_Input.__actions_default_in_ToggleStart.GetState(SteamVR_Input_Sources.Any))
        if (SteamVR_Input.__actions_default_in_PointerShoot.GetState(SteamVR_Input_Sources.RightHand))
        {
            // Declare a raycast hit to store information about what our raycast hit
            RaycastHit hit;

            // Set the start position of our visual effect for our laser to the position of the pointerEnd
            laserBeam.SetPosition(0, transform.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, laserRange))
            {
                // Set the end position for our laser line
                laserBeam.SetPosition(1, hit.point);
                laserBeam.enabled = true;

                // Get a reference to the node the raycast collided with
                TutorialNode node = hit.collider.GetComponent<TutorialNode>();

                // If there was a node script attached
                if (node != null && prevNodeShot != node)
                {
                    node.PerformUserAction(UtilGraph.NODE_VISITED);
                    prevNodeShot = node;

                    //// First check if it's the first node the player shot
                    //if (edgeLeadingToNode == null)
                    //{
                    //    node.Visited = true;
                    //}
                    //else
                    //{
                    //    bool isDirectedEdge = edgeLeadingToNode.IsDirectedEdge;

                    //    if (isDirectedEdge)
                    //    {
                    //        bool correctDirection = edgeLeadingToNode.OtherNode(node) != null;

                    //        if (correctDirection && !node.Visited)
                    //        {
                    //            node.PrevEdge = edgeLeadingToNode;
                    //            node.Visited = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (!node.Visited)
                    //        {
                    //            node.PrevEdge = edgeLeadingToNode;
                    //            node.Visited = true;
                    //        }
                    //    }
                    //}

                }
            }
            else
            {
                // If we didn't hit anything, set the end of the line to a position directly in front of the camera at the distance of laserRange
                laserBeam.SetPosition(1, transform.position + (laserRange * transform.TransformDirection(Vector3.forward)));
            }
        }
        else
            ResetLaserBeam();
    }

    public bool AllowShooting
    {
        set { allowShooting = value; }
    }

    public void SetLaserBeamMaterial(int material)
    {
        laserBeam.material = laserMaterials[material];
    }


    private void ResetLaserBeam()
    {
        laserBeam.SetPosition(1, transform.position);
        laserBeam.enabled = false;
    }

    public void ResetPointer()
    {
        prevNodeShot = null;
        allowShooting = false;
    }
}
