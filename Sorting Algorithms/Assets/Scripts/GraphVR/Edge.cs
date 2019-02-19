using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    /* -------------------------------------------- Edge ----------------------------------------------------
     * - A connection between two nodes in a graph
     * - When an edge is initialized it notifies the two nodes automatically
    */


    public static int EDGE_ID;

    [SerializeField]
    private int edgeID, cost;
    private string graphStructure;
    private Color currentColor;

    [SerializeField]
    private Node node1, node2;

    public void InitEdge(Node node1, Node node2, int cost, string graphStructure)
    {
        edgeID = EDGE_ID++;
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

    private void SetAngle(float angle)
    {
        transform.Rotate(0f, angle, 0f);
    }

    public void SetLength(float length)
    {
        GetComponentInChildren<MeshRenderer>().transform.localScale += new Vector3(length - 5f, 0f, 0f);
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
    }

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
    private void NotifyNodes(Node node1, Node node2)
    {
        if (node1.GetComponent(typeof(GridNode)))
        {
            node1.GetComponent<GridNode>().AddNeighbor(node2.GetComponent<GridNode>());
        }
        else if (node1.GetComponent(typeof(TreeNode)))
        {
            node2.GetComponent<TreeNode>().Parent = node1.GetComponent<TreeNode>();
            node1.GetComponent<TreeNode>().AddChildren(node2.GetComponent<TreeNode>());
        }
        else if (node1.GetComponent(typeof(RandomNode)))
        {
            node1.GetComponent<RandomNode>().AddNeighbor(node2.GetComponent<RandomNode>());
        }

        // Add edge in nodes
        node1.AddEdge(this);
        node2.AddEdge(this);
    }

    // Returns the node on the other side of this edge
    public Node OtherNodeConnected(Node node)
    {
        return (node == node1) ? node2 : node1;
    }

    // Returns the total cost of parameter node + edge cost
    public int EdgeAndOtherNodeCombinedCost(Node fromNode)
    {
        return (fromNode == node1) ? (cost + node2.Dist) : (cost + node1.Dist);
    }


}
