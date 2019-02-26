using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedEdge : Edge {

    /* -------------------------------------------- Directed Edge ----------------------------------------------------
     * > PathBothWaysActive = false
     *   Node1 ------> Node2 (Node1 can reach node2, not vice versa)
     * 
     * > PathBothWaysActive = true
     *   Node1 <-----> Node2 (Node1 can reach node2, and vice versa)
     *  
     * 
    */

    private bool pathBothWaysActive;

    public void InitDirectedEdge(Node node1, Node node2, int cost, string graphStructure, bool pathBothWaysActive)
    {
        InitEdge(node1, node2, cost, graphStructure);
        this.pathBothWaysActive = pathBothWaysActive;

        if (pathBothWaysActive)
            OpenPathBothWays();
    }

    public override string EdgeType
    {
        get { return pathBothWaysActive ? UtilGraph.SYMMETRIC_EDGE : UtilGraph.DIRECED_EDGE; }
    }

    public bool IsPointingAwayFrom(Node node)
    {
        return node == node1;
    }

    public override Color CurrentColor
    {
        get { return currentColor; }
        set {
            currentColor = value;
            Component[] arrow = GetComponentsInChildren(typeof(MeshRenderer));
            for (int i=0; i < arrow.Length-1; i++)
            {
                arrow[i].GetComponent<Renderer>().material.color = value;
            }
        }
    }

    public bool PathBothWaysActive
    {
        get { return pathBothWaysActive; }
        set { pathBothWaysActive = value; }
    }

    public void OpenPathBothWays()
    {
        // Open path way for both ways
        pathBothWaysActive = true;

        // Notify neighbors
        if (node1.GetComponent(typeof(GridNode)))
        {
            node2.GetComponent<GridNode>().AddNeighbor(node1.GetComponent<GridNode>());
        }
        //else if (node1.GetComponent(typeof(TreeNode)))
        //{
        //    node2.GetComponent<TreeNode>().Parent = node1.GetComponent<TreeNode>();
        //    node1.GetComponent<TreeNode>().AddChildren(node2.GetComponent<TreeNode>());
        //}
        else if (node1.GetComponent(typeof(RandomNode)))
        {
            node2.GetComponent<RandomNode>().AddNeighbor(node1.GetComponent<RandomNode>());
        }

        node2.AddEdge(this);
        // Visual: TODO
    }

    public void ClosePathBothWays()
    {
        // Open path way for both ways
        pathBothWaysActive = false;

        // Notify neighbors
        if (node1.GetComponent(typeof(GridNode)))
        {
            node2.GetComponent<GridNode>().RemoveNeighbor(node1.GetComponent<GridNode>());
        }
        //else if (node1.GetComponent(typeof(TreeNode)))
        //{
        //    node2.GetComponent<TreeNode>().Parent = node1.GetComponent<TreeNode>();
        //    node1.GetComponent<TreeNode>().AddChildren(node2.GetComponent<TreeNode>());
        //}
        else if (node1.GetComponent(typeof(RandomNode)))
        {
            node2.GetComponent<RandomNode>().RemoveNeighbor(node1.GetComponent<RandomNode>());
        }

        // Visual: TODO
    }

    public override Node OtherNodeConnected(Node node)
    {
        if (pathBothWaysActive || node is TreeNode)
            return (node == node1) ? node2 : node1;
        return (node == node1) ? node2 : null;
    }

    protected override void NotifyNodes(Node node1, Node node2)
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
    }

    public override int EdgeAndOtherNodeCombinedCost(Node fromNode)
    {
        if (pathBothWaysActive)
            return (fromNode == node1) ? (node1.Dist + cost) : (node2.Dist + cost);
        return (fromNode == node1) ? (node1.Dist + cost) : UtilGraph.INF;
    }

}
