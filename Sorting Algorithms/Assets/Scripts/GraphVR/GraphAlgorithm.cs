using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphAlgorithm : TeachingAlgorithm {

    protected string graphStructure = "Graph";
    protected GraphMain graphMain;

    // Demo variables
    protected Node node1, node2;
    protected Edge edge;
    protected char node1Alpha = 's', node2Alpha = 'w';
    protected int node1Dist, node2Dist, edgeCost, numberOfEdges;

    // Traverse shared variables
    private bool visitLeftFirst;

    // Shortest path shared variables
    protected bool shortestPathOnToAll;

    // Instruction variables
    protected int prevHighlightedLineOfCode;

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.EMPTY_QUEUE_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.WHILE_LIST_NOT_EMPTY_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.END_IF_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.END_FOR_LOOP_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.END_WHILE_INST);
    }

    public override MainManager MainManager
    {
        get { return graphMain; }
        set { graphMain = (GraphMain)value; }
    }

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

    protected void SetNodePseudoCode(Node node, int nr, int value)
    {
        SetNodePseudoCode(node, nr);
        node.Dist = value;
        switch (nr)
        {
            case 1: node1Dist = value; break;
            case 2: node2Dist = value; break;
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

    protected override Vector2 GetLineRTDelta()
    {
        return new Vector2(10, 2);
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

    public bool VisitLeftFirst
    {
        get { return visitLeftFirst; }
        set { visitLeftFirst = value; }
    }

    public bool ShortestPathOneToAll
    {
        get { return shortestPathOnToAll; }
        set { shortestPathOnToAll = value; }
    }
}
