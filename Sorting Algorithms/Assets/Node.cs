using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour, IComparable<Node> {

    public static int NODE_ID;

    [SerializeField]
    protected int nodeID;

    [SerializeField]
    protected int totalCost;
    private bool traversed, marked; // need marked? just check if in list/stack...

    protected Color currentColor;

    [SerializeField]
    private Edge prevEdge; // shortest path

    [SerializeField]
    protected List<Edge> edges;
    private Edge markedFrom;

    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nodeID = NODE_ID++;
        edges = new List<Edge>();
        traversed = false;
        marked = false;
        TotalCost = UtilGraph.INF;
        prevEdge = null;
    }

    public int NodeID
    {
        get { return nodeID; }
    }

    public int TotalCost
    {
        get { return totalCost; }
        set { totalCost = value; UpdateCostText(); }
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value;
            if (traversed)
                CurrentColor = UtilGraph.TRAVERSED_COLOR;
            else
                CurrentColor = UtilGraph.STANDARD_COLOR;
        }
    }

    public bool Marked
    {
        get { return marked; }
        set { marked = value;
            if (marked)
                CurrentColor = UtilGraph.MARKED;
            else
                CurrentColor = UtilGraph.STANDARD_COLOR;
        }
    }

    public Edge MarkedFrom
    {
        get { return markedFrom; }
        set { markedFrom = value; }
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
    }

    public List<Edge> Edges
    {
        get { return edges; }
    }

    public void AddEdge(Edge edge)
    {
        if (!edges.Contains(edge))
            edges.Add(edge);
    }

    public void RemoveEdge(Edge edge)
    {
        if (edges.Contains(edge))
            edges.Remove(edge);
    }

    public Edge PrevEdge
    {
        get { return prevEdge; }
        set { prevEdge = value; }
    }

    // ******************************************************************* OBS: inverted
    public int CompareTo(Node other)
    {
        int otherTotalCost = other.TotalCost;
        if (totalCost < otherTotalCost)
            return 1;
        else if (totalCost > otherTotalCost)
            return -1;
        return 0;
    }

    public abstract string NodeType { get; }
    public abstract void UpdateCostText();
}
