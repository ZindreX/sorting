using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour, IComparable<Node>, IInstructionAble {

    // Counter for number of active nodes
    public static int NODE_ID;

    // Basic 
    [SerializeField]
    protected int nodeID;
    protected char nodeAlphaID;
    protected string algorithm;
    protected bool isStartNode, isEndNode;

    protected Color currentColor;
    protected Animator animator;
    protected TextMesh textNodeID, textNodeDist;

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

        Component[] textHolders = GetComponentsInChildren(typeof(TextMesh));
        textNodeID = textHolders[0].GetComponent<TextMesh>();
        textNodeDist = textHolders[1].GetComponent<TextMesh>();

        nodeID = NODE_ID++;
        NodeAlphaID = UtilGraph.ConvertIDToAlphabet(nodeID);
        name = NodeType + nodeID + "(" + nodeAlphaID + ")";
        ResetNode();
    }

    protected void InitNode(string algorithm)
    {
        SetNodeTextID(true);

        this.algorithm = algorithm;
        if (algorithm == Util.DIJKSTRA)
            Dist = UtilGraph.INF;
        else
            textNodeDist.text = "";
    }

    public int NodeID
    {
        get { return nodeID; }
    }

    public char NodeAlphaID
    {
        get { return nodeAlphaID; }
        set { nodeAlphaID = value; }
    }

    private void SetNodeTextID(bool useAlpha)
    {
        if (useAlpha)
            textNodeID.text = UtilGraph.ConvertIDToAlphabet(nodeID).ToString();
        else
            textNodeID.text = nodeID.ToString();
    }

    public bool IsStartNode
    {
        get { return isStartNode; }
        set { isStartNode = value; }
    }

    public bool IsEndNode
    {
        get { return isEndNode; }
        set { isEndNode = value; }
    }
       
    public int Dist
    {
        get { return dist; }
        set { dist = value; UpdateTextNodeDist(value); }
    }

    private void UpdateTextNodeDist(int newDist)
    {
        textNodeDist.text = UtilGraph.ConvertIfInf(newDist);
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value; CurrentColor = UtilGraph.TRAVERSED_COLOR; }
    }

    public bool Visited
    {
        get { return visited; }
        set { visited = value; CurrentColor = UtilGraph.VISITED_COLOR; }
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

    // Obs: inverted [biggest, ..., smallest]
    public int CompareTo(Node other)
    {
        if (dist < other.Dist)
            return 1;
        else if (dist > other.Dist)
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
        isEndNode = false;
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
  

}
