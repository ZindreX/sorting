﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionManager : MonoBehaviour {

    [Header("Goal(s)")]
    [SerializeField]
    private Node currentGoal, reportedNode;

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
                PlayerWithinGoalPosition = true;
            else
                PlayerWithinGoalPosition = false;
        }

    }

    public void ReportPlayerOnNode(Node node)
    {
        playerPositionText.text = "Player position: " + node.NodeAlphaID;
        if (showDistance)
            nodeDistText.text = "Node dist: " + UtilGraph.ConvertIfInf(node.Dist);
    }

    public Node CurrentGoal
    {
        get { return currentGoal; }
        set { currentGoal = value; }
    }

    public bool PlayerWithinGoalPosition
    {
        get { return playerWithinGoalPosition; }
        set { playerWithinGoalPosition = value; }
    }

}