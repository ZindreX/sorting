using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionManager : MonoBehaviour {

    [Header("Goal(s)")]
    [SerializeField]
    private Node currentGoal, reportedNode, previousNode;

    private bool showDistance, playerWithinGoalPosition;
    private TextMeshPro playerPositionText, nodeDistText;

    public void InitPositionManager(bool showDistance)
    {
        this.showDistance = showDistance;
        playerPositionText.text = "Player position: start area";
        playerWithinGoalPosition = false;
    }

    private void Awake()
    {
        Component[] components = GetComponentsInChildren<TextMeshPro>();
        playerPositionText = components[0].GetComponent<TextMeshPro>();
        nodeDistText = components[1].GetComponent<TextMeshPro>();
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

    }

    public void ReportPlayerOnNode(Node node)
    {
        // Set previous node
        if (node != reportedNode)
            previousNode = reportedNode;

        // Set reported node
        reportedNode = node;

        // Display information about the node the player currently is standing on
        playerPositionText.text = "Player position: " + node.NodeAlphaID;
        if (showDistance)
            nodeDistText.text = "Node dist: " + UtilGraph.ConvertDist(node.Dist);
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

}
