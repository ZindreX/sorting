using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Edge : MonoBehaviour {

    /* -------------------------------------------- Edge ----------------------------------------------------
     * - A connection between two nodes in a graph
     * - When an edge is initialized it notifies the two nodes automatically
    */


    public static int EDGE_ID;

    [SerializeField]
    protected int edgeID, cost;
    protected string graphStructure;
    protected Color currentColor;

    [SerializeField]
    protected float angle;

    [SerializeField]
    protected Node node1, node2;

    protected void InitEdge(Node node1, Node node2, int cost, string graphStructure)
    {
        edgeID = EDGE_ID++;
        name = EdgeType + edgeID + " [" + node1.NodeAlphaID + ", " + node2.NodeAlphaID + "]";
        this.node1 = node1;
        this.node2 = node2;

        if (cost != UtilGraph.NO_COST)
            Cost = cost;

        this.graphStructure = graphStructure; // need???

        // Notify nodes (neighbors / parent / child)
        NotifyNodes(node1, node2);
    }

    public int EdgeID
    {
        get { return edgeID; }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; GetComponentInChildren<TextMesh>().text = value.ToString(); }
    }

    // *** Object settings ***

    public void SetAngle(float angle)
    {
        // Avoid overlap of crossing edges
        //if (angle == -45f || angle == -135f)
        //    GetComponentInChildren<TextMesh>().transform.position += new Vector3(0f, 1f, 0f);

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
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
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
