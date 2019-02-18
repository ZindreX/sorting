using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    public static int EDGE_ID;

    [SerializeField]
    private int edgeID, cost;
    private float angle; // needed???
    private string graphStructure;

    [SerializeField]
    private bool collisionOccured;

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

        this.graphStructure = graphStructure;

        CurrentColor = Util.STANDARD_COLOR;
        collisionOccured = false;

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
        this.angle = angle;
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

    public Node OtherNodeConnected(Node node)
    {
        return (node == node1) ? node2 : node1;
    }

    public int EdgeAndOtherNodeCombinedCost(Node fromNode)
    {
        return (fromNode == node1) ? (cost + node2.TotalCost) : (cost + node1.TotalCost);
    }

    public bool CollisionOccured
    {
        get { return collisionOccured; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (graphStructure == UtilGraph.RANDOM_GRAPH)
        //{
        //    if (collision.collider.tag == UtilGraph.EDGE) // Hit an edge
        //    {
        //        Debug.Log("Edge " + edgeID + " collided with Edge " + collision.collider.GetComponentInParent<Edge>().EdgeID);
        //        collisionOccured = true;
        //    }

        //    if (collision.collider.tag == UtilGraph.NODE)
        //    {
        //        int nodeID = collision.collider.GetComponentInParent<Node>().NodeID; // cube collides, so need to get from object

        //        if (nodeID != node1.NodeID && nodeID != node2.NodeID) // Hit a node (not connecting nodes)
        //        {
        //            Debug.Log("Edge " + edgeID + " collided with Node " + nodeID);
        //            collisionOccured = true;
        //        }
        //    }
        //}
    }

}
