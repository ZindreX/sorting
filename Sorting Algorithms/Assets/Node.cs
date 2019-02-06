using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    private int nodeID, totalCost;
    private Color currentColor;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Node(int nodeID, bool startNode)
    {
        this.nodeID = nodeID;
        CurrentColor = Util.STANDARD_COLOR;

        if (startNode)
            totalCost = 0;
        else
            totalCost = Util.INF;
    }

    public string TotalCost()
    {
        return (totalCost != Util.INF) ? totalCost.ToString() : "INF";
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
    }



}
