using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : Node {

    private int[] cell;

    [SerializeField]
    private List<GridNode> neighbors;
    
    public void InitGridNode(int[] cell)
    {
        neighbors = new List<GridNode>();
        this.cell = cell;
    }

    public override string NodeType
    {
        get { return "Grid node " + nodeID; }
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

    public void RemoveNeighbor(GridNode node)
    {
        if (neighbors.Contains(node))
        {
            neighbors.Remove(node);
            node.RemoveNeighbor(this);
        }
    }

    public override void UpdateCostText()
    {
        //Debug.Log(cell[0] + ", " + cell[1] + ": " + totalCost);
        GetComponentInChildren<TextMesh>().text = UtilGraph.ConvertIfInf(totalCost.ToString()); //"Z=" + cell[0] + ", X=" + cell[1];
    }

}
