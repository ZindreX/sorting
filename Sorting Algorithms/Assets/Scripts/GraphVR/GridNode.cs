﻿using System.Collections;
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

    protected override void UpdateNodeText(string text)
    {
        //Debug.Log(cell[0] + ", " + cell[1] + ": " + totalCost);
        GetComponentInChildren<TextMesh>().text = text; //"Z=" + cell[0] + ", X=" + cell[1];
    }

}
