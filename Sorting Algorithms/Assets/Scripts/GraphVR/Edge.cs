using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public abstract class Edge : MonoBehaviour {

    /* -------------------------------------------- Edge ----------------------------------------------------
     * - A connection between two nodes in a graph
     * - When an edge is initialized it notifies the two nodes automatically
    */

    public static int EDGE_ID;

    [SerializeField]
    protected int edgeID, cost;
    protected string graphStructure;
    protected bool displayCost;

    protected Color currentColor, prevColor;

    protected TextMeshPro costText;

    [SerializeField]
    protected float angle;

    [SerializeField]
    protected Node node1, node2;

    private Camera playerCamera;

    private WaitForSeconds focusDuration = new WaitForSeconds(1f);

    protected void InitEdge(Node node1, Node node2, int cost, string graphStructure)
    {
        edgeID = EDGE_ID++;
        name = EdgeType + edgeID + " [" + node1.NodeAlphaID + ", " + node2.NodeAlphaID + "]";
        this.node1 = node1;
        this.node2 = node2;
        this.cost = cost;

        if (cost == UtilGraph.NO_COST)
            DisplayCost(false);

        this.graphStructure = graphStructure; // need???

        // Notify nodes (neighbors / parent / child)
        NotifyNodes(node1, node2);
    }

    private void Awake()
    {
        // Find player object and its camera
       playerCamera = FindObjectOfType<Player>().gameObject.GetComponentInChildren<Camera>();

       // Get textmesh pro
       costText = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        // Rotate text (cost) according to player position, making it readable
        if (displayCost && playerCamera != null)
            costText.transform.LookAt(2 * costText.transform.position -  playerCamera.transform.position);
    }

    public int EdgeID
    {
        get { return edgeID; }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public Node GetNode(int i)
    {
        return (i == 1) ? node1 : node2;
    }

    public void DisplayCost(bool display)
    {
        displayCost = display;
        if (display)
            costText.text = cost.ToString();
        else
            costText.text = "";
    }


    // *** Object settings ***

    public void SetAngle(float angle)
    {
        this.angle = angle;
        transform.Rotate(0f, angle, 0f);
        //transform.Rotate(0, angle, 0, Space.Self);
    }

    public void SetLength(float length)
    {
        GetComponentInChildren<MeshRenderer>().transform.localScale += new Vector3(length - 5f, 0f, 0f);
    }

    public virtual Color CurrentColor
    {
        get { return currentColor; }
        set { prevColor = currentColor; currentColor = value; ChangeApperance(value); }
    }

    public Color PrevColor
    {
        get { return prevColor; }
    }

    private void ChangeApperance(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;

        // Focus on edge cost
        if (cost != UtilGraph.NO_COST)
            StartCoroutine(FocusCostText(color));
    }

    // Highlights the cost of this edge + lifting it up, then reset to original color/pos
    private IEnumerator FocusCostText(Color color)
    {
        costText.color = color;
        costText.transform.position += UtilGraph.increaseHeightOfText;
        yield return focusDuration;
        costText.color = UtilGraph.STANDARD_COST_TEXT_COLOR;
        costText.transform.position -= UtilGraph.increaseHeightOfText;
    }

    // *** Object settings end ***


    // Edge type
    public abstract string EdgeType { get; }

    // Returns the total cost of parameter node + edge cost
    public abstract int EdgeAndOtherNodeCombinedCost(Node fromNode);

    // Returns the node on the other side of this edge
    public abstract Node OtherNodeConnected(Node node);

    /* *** Grid / RandomNode ***
     * node1 --- node2
     * 
     * 
     * *** Tree ***
     * 
     *     node1    (parent)
     *      / \
     *     X   X    (one of the X's: child)
     * 
    */
    protected abstract void NotifyNodes(Node node1, Node node2);
}
