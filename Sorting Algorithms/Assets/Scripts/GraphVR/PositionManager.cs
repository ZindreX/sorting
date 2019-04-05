using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public class PositionManager : MonoBehaviour, IMoveAble {

    [Header("Goal(s)")]
    [SerializeField]
    private Node currentGoal, reportedNode, previousNode;
    private Node[] allNodes;

    [Header("Other areas")]
    [SerializeField]
    private BoxCollider[] otherAreas;

    private bool showDistance, playerWithinGoalPosition;
    private TextMeshPro playerPositionText, nodeDistText;

    private PseudoCodeViewer pseudoCodeViewer;

    // For start area check
    private Camera playerCamera;
    private bool otherAreaPositionUpdated;

    private Vector3 startPos;

    public void InitPositionManager(bool showDistance)
    {
        this.showDistance = showDistance;
        SetPlayerPositionText("\nStart area");
        playerWithinGoalPosition = false;

        if (!showDistance)
            nodeDistText.text = "";

        allNodes = FindObjectsOfType<Node>();
    }

    private void Awake()
    {
        otherAreaPositionUpdated = true;

        Component[] components = GetComponentsInChildren<TextMeshPro>();
        playerPositionText = components[0].GetComponent<TextMeshPro>();
        nodeDistText = components[1].GetComponent<TextMeshPro>();

        pseudoCodeViewer = FindObjectOfType<PseudoCodeViewer>();
        playerCamera = FindObjectOfType<Player>().GetComponentInChildren<Camera>();
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentGoal != null && reportedNode != null)
        {
            if (currentGoal == reportedNode)
            {
                PlayerWithinGoalPosition = true;
                currentGoal = null;
            }
            else
                PlayerWithinGoalPosition = false;
        }

        if (reportedNode != null && previousNode != null)
        {
            if (reportedNode != previousNode)
                previousNode.DisplayNodeInfo();
        }

        // start area
        if (!otherAreaPositionUpdated)
        {
            Vector3 playerPos = playerCamera.transform.position;
            if (playerPos.z <= -2f)
            {
                SetPlayerPositionText("\nStart area");
                pseudoCodeViewer.ChangeSizeOfPseudocode(playerPos);

                RotateNodesTowards(playerPos);

                otherAreaPositionUpdated = true;
            }
        }
    }

    public void ReportPlayerOnNode(Node node)
    {
        // Set previous node
        if (node != reportedNode)
            previousNode = reportedNode;

        // Set reported node
        reportedNode = node;

        // Fix pseudocode size
        pseudoCodeViewer.ChangeSizeOfPseudocode(node.transform.position);

        // Rotate all node text toward player
        RotateNodesTowards(node.transform.position);

        // Display information about the node the player currently is standing on
        SetPlayerPositionText(node.NodeAlphaID.ToString());
        if (showDistance)
            nodeDistText.text = "Node dist: " + UtilGraph.ConvertDist(node.Dist);

        otherAreaPositionUpdated = false;
    }

    public Node CurrentGoal
    {
        get { return currentGoal; }
        set { currentGoal = value; UpdateNewGoal(value); }
    }

    private void UpdateNewGoal(Node node)
    {
        playerWithinGoalPosition = false;

        if (currentGoal == previousNode)
            previousNode = null;
    }

    public Node ReportedNode
    {
        get { return reportedNode; }
        set { reportedNode = value; }
    }

    public Node PreviousNode
    {
        get { return previousNode; }
        set { previousNode = value; }
    }

    public bool PlayerWithinGoalPosition
    {
        get { return playerWithinGoalPosition; }
        set { playerWithinGoalPosition = value; }
    }

    private void SetPlayerPositionText(string position)
    {
        playerPositionText.text = "Player position: " + position;
    }

    public void RotateNodesTowards(Vector3 pos)
    {
        foreach (Node node in allNodes)
        {
            node.RotateTowardsPoint(pos);
        }
    }






    public void MoveOut()
    {
        transform.position += new Vector3(4f, 0f, 0f);
    }

    public void MoveBack()
    {
        transform.position = startPos;
    }
}
