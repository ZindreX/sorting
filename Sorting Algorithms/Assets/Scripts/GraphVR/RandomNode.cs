using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : Node {


    [SerializeField]
    private List<RandomNode> neighbors;

    public void InitRandomNode()
    {
        neighbors = new List<RandomNode>();
    }

    public override string NodeType
    {
        get { return "Random node " + nodeID; }
    }

    public override void UpdateCostText()
    {
        GetComponentInChildren<TextMesh>().text = UtilGraph.ConvertIfInf(totalCost.ToString());
    }

    public bool IsNeighborWith(RandomNode node)
    {
        return neighbors.Contains(node);
    }

    public void AddNeighbor(RandomNode node)
    {
        if (!neighbors.Contains(node))
        {
            neighbors.Add(node);
            node.AddNeighbor(this);
        }
    }

    public void RemoveNeighbor(RandomNode node)
    {
        if (neighbors.Contains(node))
        {
            neighbors.Remove(node);
            node.RemoveNeighbor(this);
        }
    }


}
