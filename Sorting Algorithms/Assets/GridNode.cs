using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : Node {

    private int totalCost;
    private int[] cell;

    [SerializeField]
    private List<GridNode> neighbors;
    
    public void InitGridNode(int[] cell, bool startNode)
    {
        neighbors = new List<GridNode>();
        this.cell = cell;

        if (startNode)
            totalCost = 0;
        else
            totalCost = UtilSort.INF;
    }

    public int[] Cell
    {
        get { return cell; }
    }

    public void AddNeighbor(GridNode node)
    {
        if (!neighbors.Contains(node))
        {
            neighbors.Add(node);
            node.AddNeighbor(this);
        }
    }

    public override string TotalCost()
    {
        return (totalCost != UtilSort.INF) ? totalCost.ToString() : "INF";
    }

    public override void UpdateCostText()
    {
        GetComponentInChildren<TextMesh>().text = "Z=" + cell[0] + ", X=" + cell[1];
    }
}
