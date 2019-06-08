using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class Pointer : MonoBehaviour {

    /* ---------------------------------- Pointer ----------------------------------
     * - Used for adding a node to a queue/stack
     * - or relaxing a node (adding or updating a node in a priortiy list)
     * 
    */

    [SteamVR_DefaultAction("PointerShoot")]
    public SteamVR_Action_Boolean pointerShootAction;

    [SerializeField]
    private float laserRange = 50f;
    private int layerMask;
    private bool allowShooting;

    private string currentTask;
    private int numberOfNodesToChoose;

    [SerializeField]
    private GraphMain graphMain;

    [SerializeField]
    private PositionManager positionManager;

    [SerializeField]
    private Transform leftHand, rightHand;

    [SerializeField]
    private Material laserMaterial;
    private LineRenderer laserBeam;

    public GameObject hitObject;
    public Node prevNodeShot;

    public void InitPointer(string startTask, int numberOfNodesToChoose)
    {
        currentTask = startTask;
        this.numberOfNodesToChoose = numberOfNodesToChoose;
    }

    public void ResetPointer()
    {
        currentTask = "";
        numberOfNodesToChoose = 0;
        hitObject = null;
        prevNodeShot = null;
        allowShooting = false;
    }

    private void Awake()
    {
        currentTask = "";
    }

    // Use this for initialization
    void Start () {
        laserBeam = GetComponent<LineRenderer>();
        laserBeam.material = laserMaterial;

        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

    }
	
	// Update is called once per frame
	void Update () {
        // Right hand only
        // Position
        transform.position = rightHand.position;
        // Rotation
        transform.rotation = rightHand.rotation;

        if (!allowShooting)
        {
            ResetLaserBeam();
            return;
        }

        if (currentTask != UtilGraph.NO_TASK)
        {
            if (currentTask == Util.DEMO || currentTask == Util.STEP_BY_STEP)
                return;

            //if (SteamVR_Input.__actions_default_in_PointerShoot.GetState(SteamVR_Input_Sources.Any))
            //{
            //    if (SteamVR_Input.__actions_default_in_PointerShoot.GetLastState(SteamVR_Input_Sources.LeftHand))
            //    {
            //        transform.position = leftHand.position;
            //        transform.rotation = leftHand.rotation;
            //    }
            //    else if (SteamVR_Input.__actions_default_in_PointerShoot.GetLastState(SteamVR_Input_Sources.RightHand))
            //    {
            //        transform.position = rightHand.position;
            //        transform.rotation = rightHand.rotation;
            //    }
            //}

            //if (SteamVR_Input.__actions_default_in_ToggleStart.GetState(SteamVR_Input_Sources.Any))
            if (SteamVR_Input.__actions_default_in_PointerShoot.GetState(SteamVR_Input_Sources.RightHand))
            {
                // Declare a raycast hit to store information about what our raycast hit
                RaycastHit hit;

                // Set the start position of our visual effect for our laser to the position of the pointerEnd
                laserBeam.SetPosition(0, transform.position);

                // Check if our raycast has hit anything
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, laserRange)) // Mathf.Infinity, layerMask))
                //Physics.Raycast(rayOrigin, vrCamera.transform.forward, out hit, laserRange))
                {
                    // Set the end position for our laser line
                    laserBeam.SetPosition(1, hit.point);
                    laserBeam.enabled = true;

                    // Debugging
                    hitObject = hit.collider.gameObject;

                    // Get a reference to the node the raycast collided with
                    Node node = hit.collider.GetComponent<Node>();

                    // If there was a node script attached
                    if (node != null)
                    {
                        if (currentTask == UtilGraph.SELECT_NODE)
                        {
                            // Check if player wants to select start/end nodes
                            if (graphMain.ChosenNodes < numberOfNodesToChoose)
                            {
                                // Currently choosing start node
                                if (graphMain.ChosenNodes == 0)
                                {
                                    graphMain.GraphManager.StartNode = node;
                                    graphMain.ChosenNodes++;
                                    if (graphMain.ChosenNodes == graphMain.NumberOfNodesToChoose)
                                        currentTask = UtilGraph.NO_TASK;
                                }
                                else if (node != graphMain.GraphManager.StartNode)
                                {
                                    graphMain.GraphManager.EndNode = node;
                                    graphMain.ChosenNodes++;
                                    currentTask = UtilGraph.NO_TASK;
                                }
                                prevNodeShot = null;
                            }
                        }
                        else if (currentTask == Util.USER_TEST)
                        {
                            if (prevNodeShot != node)
                            {
                                node.PerformUserMove(UtilGraph.NODE_VISITED);
                                prevNodeShot = node;
                            }
                        }
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
        else
            ResetLaserBeam();
    }


    public string CurrentTask
    {
        get { return currentTask; }
        set { currentTask = value; }
    }

    public bool AllowShooting
    {
        get { return allowShooting; }
        set { allowShooting = value; }
    }

    private void ResetLaserBeam()
    {
        laserBeam.SetPosition(1, transform.position);
        laserBeam.enabled = false;
    }



    private void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserBeam.SetPosition(0, targetPosition);
        laserBeam.SetPosition(1, endPosition);
    }

    // Debugging without VR
    //if (Input.GetKeyDown(KeyCode.G))
    //{
    //    if (currentTask == "Start nodes")
    //    {
    //        // Check if player wants to select start/end nodes
    //        if (graphMain.ChosenNodes < numberOfNodesToChoose)
    //        {
    //            // Currently choosing start node
    //            if (graphMain.ChosenNodes == 0)
    //            {
    //                graphMain.GraphManager.StartNode = prevNodeShot;
    //                graphMain.ChosenNodes++;
    //            }
    //            else if (prevNodeShot != graphMain.GraphManager.StartNode)
    //            {
    //                graphMain.GraphManager.EndNode = prevNodeShot;
    //                graphMain.ChosenNodes++;
    //            }
    //        }
    //    }
    //}
}
