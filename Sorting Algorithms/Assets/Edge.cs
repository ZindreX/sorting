using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    private int cost;
    private float angle;
    private Color currentColor;

    [SerializeField]
    private Node node1, node2;

    public void InitEdge(Node node1, Node node2, int cost)
    {
        this.node1 = node1;
        this.node2 = node2;
        this.cost = cost;
        SetAngle(angle);
        CurrentColor = UtilSort.STANDARD_COLOR;

        // Notify nodes (neighbors / parent / child)
        NotifyNodes(node1, node2);
    }

    private void SetAngle(float angle)
    {
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
}
