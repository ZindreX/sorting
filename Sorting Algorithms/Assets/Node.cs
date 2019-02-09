using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour {

    public static int NODE_ID;

    private bool traversed, marked;

    [SerializeField]
    protected int nodeID;
    protected Color currentColor;

    [SerializeField]
    protected List<Edge> edges;

    [SerializeField]
    protected string space = "Ignore space";

    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nodeID = NODE_ID++;
        edges = new List<Edge>();
        traversed = false;
        marked = false;
    }

    private void Update()
    {
        UpdateCostText();
    }

    public bool Traversed
    {
        get { return traversed; }
        set { traversed = value; }
    }

    public bool Marked
    {
        get { return marked; }
        set { marked = value; }
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

    public abstract string NodeID { get; }
    public abstract string TotalCost();
    public abstract void UpdateCostText();

}
