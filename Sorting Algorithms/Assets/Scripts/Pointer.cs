using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class Pointer : MonoBehaviour {

    [SerializeField]
    private float laserRange = 50f;
    private int layerMask;

    // 
    private string currentTask;
    private int numberOfNodesToChoose;

    [SerializeField]
    private GraphMain graphMain;

    [SerializeField]
    private PositionManager positionManager;

    [SerializeField]
    private Transform pointerEnd, hand;

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
        // Set position & rotation to hand
        transform.position = hand.position;
        transform.rotation = hand.rotation;

        // Debugging without VR
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentTask == "Start nodes")
            {
                // Check if player wants to select start/end nodes
                if (graphMain.ChosenNodes < numberOfNodesToChoose)
                {
                    // Currently choosing start node
                    if (graphMain.ChosenNodes == 0)
                    {
                        graphMain.GraphManager.StartNode = prevNodeShot;
                        graphMain.ChosenNodes++;
                    }
                    else if (prevNodeShot != graphMain.GraphManager.StartNode)
                    {
                        graphMain.GraphManager.EndNode = prevNodeShot;
                        graphMain.ChosenNodes++;
                    }
                }
            }
        }

        if (currentTask != "")
        {
            if (SteamVR_Input.__actions_default_in_ToggleStart.GetState(SteamVR_Input_Sources.RightHand))
            {
                laserBeam.enabled = true;

                // Declare a raycast hit to store information about what our raycast hit
                RaycastHit hit;

                // Set the start position of our visual effect for our laser to the position of the pointerEnd
                laserBeam.SetPosition(0, pointerEnd.position);

                // Check if our raycast has hit anything
                if (Physics.Raycast(pointerEnd.position, pointerEnd.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                    //Physics.Raycast(rayOrigin, vrCamera.transform.forward, out hit, laserRange))
                {
                    // Set the end position for our laser line
                    laserBeam.SetPosition(1, hit.point);

                    //Debug.Log("Gameobject: " + hit.collider.gameObject);
                    hitObject = hit.collider.gameObject;

                    // Get a reference to the node the raycast collided with
                    Node node = hit.collider.GetComponent<Node>();

                    // If there was a node script attached
                    if (node != null)
                    {
                        if (currentTask == "Start nodes")
                        {
                            // Check if player wants to select start/end nodes
                            if (graphMain.ChosenNodes < numberOfNodesToChoose)
                            {
                                // Currently choosing start node
                                if (graphMain.ChosenNodes == 0)
                                {
                                    graphMain.GraphManager.StartNode = node;
                                    graphMain.ChosenNodes++;
                                }
                                else if (node != graphMain.GraphManager.StartNode)
                                {
                                    graphMain.GraphManager.EndNode = prevNodeShot;
                                    graphMain.ChosenNodes++;
                                }
                                prevNodeShot = null;
                            }
                        }
                        else if (currentTask == Util.USER_TEST)
                        {
                            if (prevNodeShot != node)
                            {
                                Debug.Log("Node " + node.NodeAlphaID + " shot, performing user move on it");
                                node.PerformUserMove(UtilGraph.NODE_VISITED);
                                prevNodeShot = node;
                            }
                        }
                    }

                }
                else
                {
                    // If we didn't hit anything, set the end of the line to a position directly in front of the camera at the distance of laserRange
                    //laserBeam.SetPosition(1, pointerEnd.transform.position * laserRange);
                    //laserBeam.SetPosition(1, rayOrigin + (vrCamera.transform.position * laserRange));
                }
            }
            else
            {
                laserBeam.enabled = false;
            }
        }
    }

    public string CurrentTask
    {
        get { return currentTask; }
        set { currentTask = value; }
    }

}
