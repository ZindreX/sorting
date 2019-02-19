﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour, IComparable<Node>, IInstructionAble {

    // Counter for number of active nodes
    public static int NODE_ID;

    // Basic 
    [SerializeField]
    protected int nodeID;
    protected char nodeAlphaID;
    protected Color currentColor;
    protected Animator animator;

    // Instruction variables
    protected InstructionBase instruction;
    protected bool nextMove;

    // Traversal / Shortest path variables
    [SerializeField]
    protected int dist;
    private bool traversed, visited; // need traversed? just check if in list/stack...

    [SerializeField]
    private Edge prevEdge;

    [SerializeField]
    protected List<Edge> edges;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nodeID = NODE_ID++;
        nodeAlphaID = UtilGraph.ConvertIDToAlphabet(nodeID);
        ResetNode();
    }

    protected void InitNode(string algorithm)
    {
        switch (algorithm)
        {
            case UtilGraph.BFS: case UtilGraph.DFS: UpdateNodeText(nodeAlphaID.ToString()); break;
            case UtilGraph.DIJKSTRA: dist = UtilGraph.INF; break;
            default: Debug.LogError("Node text for '" + algorithm + "' not specified."); break;
        }
    }

    public int NodeID
    {
        get { return nodeID; }
    }

    public char NodeAlphaID
    {
        get { return nodeAlphaID; }
    }
       
    public int Dist
    {
        get { return dist; }
        set { dist = value; UpdateNodeText(UtilGraph.ConvertIfInf(value.ToString())); }
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value;
            if (traversed)
                CurrentColor = UtilGraph.TRAVERSED_COLOR;
            else
                CurrentColor = Util.STANDARD_COLOR;
        }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value;
            if (visited)
                CurrentColor = UtilGraph.VISITED;
            else
                CurrentColor = Util.STANDARD_COLOR;
        }
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

    public virtual void AddEdge(Edge edge)
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
        int otherTotalCost = other.Dist;
        if (dist < otherTotalCost)
            return 1;
        else if (dist > otherTotalCost)
            return -1;
        return 0;
    }

    public virtual void ResetNode()
    {
        edges = new List<Edge>();
        traversed = false;
        visited = false;
        //TotalCost = UtilGraph.INF;
        prevEdge = null;
        CurrentColor = UtilGraph.STANDARD_COLOR;
    }

    // *** Instructions ***
    public bool NextMove
    {
        get { return nextMove; }
        set { nextMove = value; }
    }

    public InstructionBase Instruction
    {
        get { return instruction; }
        set { instruction = value; }
    }

    // Instruction methods end

    public abstract string NodeType { get; }
    protected abstract void UpdateNodeText(string text);

}
