using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour {

    private int nodeID;
    private Color currentColor;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Node(int nodeID)
    {
        this.nodeID = nodeID;
        CurrentColor = UtilSort.STANDARD_COLOR;
    }


    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
    }

    public abstract string TotalCost();
}
