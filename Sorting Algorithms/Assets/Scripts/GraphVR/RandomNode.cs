using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : Node {


    [SerializeField]
    private List<RandomNode> neighbors;

    public void InitRandomNode(string algorithm)
    {
        InitNode(algorithm);
        neighbors = new List<RandomNode>();
    }

    public override string NodeType
    {
        get { return "Random node " + nodeID; }
    }

    protected override void UpdateNodeText(string text)
    {
        GetComponentInChildren<TextMesh>().text = text;
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
