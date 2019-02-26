using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : Node {

    private int[] cell;

    [SerializeField]
    private List<GridNode> neighbors;
    
    public void InitGridNode(string algorithm, int[] cell)
    {
        InitNode(algorithm);
        neighbors = new List<GridNode>();
        this.cell = cell;
    }

    public override string NodeType
    {
        get { return UtilGraph.GRID_NODE; }
    }

    public int[] Cell
    {
        get { return cell; }
    }

    public int NumberOfNeighbors()
    {
        return neighbors.Count;
    }

    public bool IsAlreadyNeighbor(GridNode node)
    {
        return neighbors.Contains(node);
    }

    public void AddNeighbor(GridNode node)
    {
        if (!neighbors.Contains(node))
        {
            neighbors.Add(node);
            //node.AddNeighbor(this);
        }
    }

    public void RemoveNeighbor(GridNode node)
    {
        if (neighbors.Contains(node))
        {
            neighbors.Remove(node);
            node.RemoveNeighbor(this);
        }
    }
}
