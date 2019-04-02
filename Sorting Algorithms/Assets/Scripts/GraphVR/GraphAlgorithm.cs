using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphAlgorithm : TeachingAlgorithm {

    protected string graphStructure = "Graph";
    protected GraphMain graphMain;

    // Demo variables
    protected Node startNode, currentNode, connectedNode;
    protected Edge edge; // prevEdge;
    protected char startNodeAlpha, node1Alpha, node2Alpha;
    protected int node1Dist, node2Dist, edgeCost, numberOfEdges;
    protected bool startNodeAdded = false;

    // Traverse shared variables
    private bool visitLeftFirst;

    // Shortest path shared variables
    protected bool shortestPathOnToAll;

    public void InitGraphAlgorithm(GraphMain graphMain, string graphStructure, float algorithmSpeed, bool shortestPathOnToAll)
    {
        InitTeachingAlgorithm(algorithmSpeed);

        this.graphMain = graphMain;
        this.graphStructure = graphStructure;
        this.shortestPathOnToAll = shortestPathOnToAll;

        startNodeAlpha = 's';
        node1Alpha = 'w';
        node2Alpha = 'v';
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.EMPTY_LIST_CONTAINER);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.WHILE_LIST_NOT_EMPTY_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.END_IF_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.END_FOR_LOOP_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.END_WHILE_INST);
    }

    public override MainManager MainManager
    {
        get { return graphMain; }
        //set { graphMain = (GraphMain)value; }
    }

    public string GraphStructure
    {
        get { return graphStructure; }
        //set { graphStructure = value; }
    }

    // Use value of node
    protected void SetNodePseudoCode(Node node, int nr)
    {
        switch (nr)
        {
            case 0: startNode = node; startNodeAlpha = node.NodeAlphaID; break;
            case 1: currentNode = node; node1Dist = node.Dist; node1Alpha = node.NodeAlphaID; break;
            case 2: connectedNode = node; node2Dist = node.Dist; node2Alpha = node.NodeAlphaID; break;
        }
    }

    // Insert value
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

    public Node CurrentNode
    {
        get { return currentNode; }
    }

    protected void SetEdge(Edge edge)
    {
        this.edge = edge;
        edge.CurrentColor = UtilGraph.TRAVERSE_COLOR;
        edgeCost = edge.Cost;
    }

    public override float LineSpacing
    {
        get { return UtilGraph.SPACE_BETWEEN_CODE_LINES; }
    }

    public override float FontSize
    {
        get { return 6f; }
    }

    public override float AdjustYOffset
    {
        get { return 1f; }
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
        currentNode = null;
        connectedNode = null;
        startNodeAlpha = 's';
        node1Alpha = 'w';
        node2Alpha = 'v';

        node1Dist = 0;
        node2Dist = 0;
        edgeCost = 0;
        numberOfEdges = 0;
        startNodeAdded = false;
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

    public abstract string GetListType();


}
