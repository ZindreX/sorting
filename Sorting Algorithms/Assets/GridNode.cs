using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : Node {

    private int totalCost;

    public void InitGridNode(bool startNode)
    {
        if (startNode)
            totalCost = 0;
        else
            totalCost = UtilSort.INF;
    }

    public override string TotalCost()
    {
        return (totalCost != UtilSort.INF) ? totalCost.ToString() : "INF";
    }
}
