using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    public static int EDGE_ID;

    // TODO: Direction of edge????

    [SerializeField]
    private int edgeID, cost;
    private float angle;
    private Color currentColor;

    [SerializeField]
    private Node node1, node2;

    public void InitEdge(Node node1, Node node2, int cost)
    {
        edgeID = EDGE_ID++;
        this.node1 = node1;
        this.node2 = node2;
        Cost = cost;
        SetAngle(angle);
        CurrentColor = UtilSort.STANDARD_COLOR;

        // Notify nodes (neighbors / parent / child)
        NotifyNodes(node1, node2);
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; GetComponentInChildren<TextMesh>().text = value.ToString(); }
    }

    private void SetAngle(float angle)
    {
        this.angle = angle;
        transform.Rotate(0f, angle, 0f);
    }

    public Color CurrentColor
    {
        get { return currentColor; }
        set { currentColor = value; GetComponentInChildren<Renderer>().material.color = value; }
    }

    /* *** Grid ***
     * node1 --- node2
     * 
     * *** Tree ***
     *     node1    (parent)
     *      / \
     *     X   X    (one of the X's: child)
     * 
    */
    private void NotifyNodes(Node node1, Node node2)
    {
        node1.AddEdge(this);
        node2.AddEdge(this);

        if (node1.GetComponent(typeof(GridNode)))
        {
            node1.GetComponent<GridNode>().AddNeighbor(node2.GetComponent<GridNode>());
        }
        else if (node1.GetComponent(typeof(TreeNode)))
        {
            //node2.GetComponent<TreeNode>().Parent = node1.GetComponent<TreeNode>();
            //node1.GetComponent<TreeNode>().AddChildren(node2.GetComponent<TreeNode>());
        }
    }

    public Node OtherNodeConnected(Node node)
    {
        return (node == node1) ? node2 : node1;
    }

    public int EdgeAndOtherNodeCombinedCost(Node fromNode)
    {
        return (fromNode == node1) ? (cost + node2.TotalCost) : (cost + node1.TotalCost);
    }

}
