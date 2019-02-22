using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphAlgorithm : TeachingAlgorithm {

    protected string graphStructure = "Graph";

    // Demo variables
    protected Node node1, node2;
    protected Edge edge;
    protected char node1Alpha = 's', node2Alpha = 'w';
    protected int node1Dist, node2Dist, edgeCost, numberOfEdges;

    // Instruction variables
    protected bool beginnerWait;
    protected int prevHighlightedLineOfCode;


    public string GraphStructure
    {
        get { return graphStructure; }
        set { graphStructure = value; }
    }

    protected void SetNodePseudoCode(Node node, int nr)
    {
        switch (nr)
        {
            case 1: node1 = node; node1Dist = node.Dist; node1Alpha = node.NodeAlphaID; break;
            case 2: node2 = node; node2Dist = node.Dist; node2Alpha = node.NodeAlphaID; break;
        }
    }

    protected void SetEdge(Edge edge)
    {
        this.edge = edge;
        edge.CurrentColor = UtilGraph.TRAVERSE_COLOR;
        edgeCost = edge.Cost;
    }

    protected override float GetLineSpacing()
    {
        return UtilGraph.SPACE_BETWEEN_CODE_LINES;
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
        node1 = null;
        node2 = null;
        node1Alpha = 's';
        node2Alpha = 'w';
        node1Dist = 0;
        edgeCost = 0;
        numberOfEdges = 0;
    }


    //public void NodeRepTest()
    //{
    //    Dictionary<Node, NodeRepresentation> t = new Dictionary<Node, NodeRepresentation>();
    //    Node n;
    //    t[n].ValueChanged()
    //}

}
