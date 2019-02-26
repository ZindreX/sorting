using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnDirectedEdge : Edge {

    /* -------------------------------------------- Undirected Edge ----------------------------------------------------
     * Node1 ------ Node2 (Node1 can reach node2, and vice versa)
     * 
     * 
    */

    public void InitUndirectedEdge(Node node1, Node node2, int cost, string graphStructure)
    {
        InitEdge(node1, node2, cost, graphStructure);
    }

    public override string EdgeType
    {
        get { return UtilGraph.UNDIRECTED_EDGE; }
    }


    public override Node OtherNodeConnected(Node node)
    {
        return (node == node1) ? node2 : node1;
    }

    protected override void NotifyNodes(Node node1, Node node2)
    {
        if (node1.GetComponent(typeof(GridNode)))
        {
            node1.GetComponent<GridNode>().AddNeighbor(node2.GetComponent<GridNode>());
            node2.GetComponent<GridNode>().AddNeighbor(node1.GetComponent<GridNode>());
        }
        else if (node1.GetComponent(typeof(TreeNode)))
        {
            node2.GetComponent<TreeNode>().Parent = node1.GetComponent<TreeNode>();
            node1.GetComponent<TreeNode>().AddChildren(node2.GetComponent<TreeNode>());
        }
        else if (node1.GetComponent(typeof(RandomNode)))
        {
            node1.GetComponent<RandomNode>().AddNeighbor(node2.GetComponent<RandomNode>());
            node2.GetComponent<RandomNode>().AddNeighbor(node1.GetComponent<RandomNode>());
        }

        // Add edge in nodes
        node1.AddEdge(this);
        node2.AddEdge(this);
    }

    public override int EdgeAndOtherNodeCombinedCost(Node fromNode)
    {
        return (fromNode == node1) ? (node1.Dist + cost) : (node2.Dist + cost);
    }


}
