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

    // BFS / DFS Shares most of instructions
    protected IEnumerator ExecuteDemoTraverseAlgorithm(InstructionBase instruction, bool increment)
    {
        Node currentNode = null, connectedNode = null;

        // Gather information from instruction
        if (instruction is TraverseInstruction)
        {
            TraverseInstruction travInst = (TraverseInstruction)instruction;

            switch (instruction.Instruction)
            {
                case UtilGraph.IF_NOT_VISITED_INST:
                case UtilGraph.END_IF_INST:
                    connectedNode = travInst.Node;
                    break;

                case UtilGraph.ENQUEUE_NODE_INST:
                case UtilGraph.PUSH_INST:
                    if (travInst.Node != graphMain.GraphManager.StartNode)
                        connectedNode = travInst.Node;
                    else
                        currentNode = travInst.Node;
                    break;

                default: currentNode = travInst.Node; break;
            }
            edge = travInst.PrevEdge;

            // Do list visual instruction if there is one
            if (travInst.ListVisualInstruction != null)
                graphMain.ListVisual.ExecuteInstruction(travInst.ListVisualInstruction, increment);
        }
        else if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = 0;
                if (increment)
                    SetNodePseudoCode(currentNode, 0);
                else
                    startNodeAlpha = 's';
                break;

            case UtilGraph.EMPTY_LIST_CONTAINER:
                lineOfCode = 1;
                break;

            case UtilGraph.ENQUEUE_NODE_INST: // BFS
            case UtilGraph.PUSH_INST: // DFS
                if (increment)
                {
                    if (!startNodeAdded)
                    {
                        lineOfCode = 2;
                        SetNodePseudoCode(currentNode, 1);
                        startNodeAdded = true;
                    }
                    else
                    {
                        lineOfCode = 7;
                        SetNodePseudoCode(connectedNode, 2);
                        connectedNode.Visited = ((TraverseInstruction)instruction).VisitInst;
                        if (edge != null)
                            edge.CurrentColor = UtilGraph.VISITED_COLOR;
                    }
                }
                else
                {
                    if (connectedNode == null)
                        startNodeAdded = false;

                    if (!startNodeAdded)
                    {
                        lineOfCode = 2;
                        startNodeAlpha = 's';// SetNodePseudoCode(currentNode, 1);
                    }
                    else
                    {
                        lineOfCode = 7;
                        connectedNode.Visited = !((TraverseInstruction)instruction).VisitInst;


                        if (edge != null)
                        {
                            SetNodePseudoCode(connectedNode, 2);
                            edge.CurrentColor = Util.STANDARD_COLOR;
                        }
                        else
                        {
                            node2Alpha = 'v';
                            startNodeAdded = false;
                        }
                    }
                }
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 3;
                lengthOfList = i.ToString();
                useHighlightColor = UseConditionColor(i != 0);
                break;

            case UtilGraph.DEQUEUE_NODE_INST: // BFS
            case UtilGraph.POP_INST: // DFS
                lineOfCode = 4;
                if (increment)
                {
                    SetNodePseudoCode(currentNode, 1);
                    currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                    if (edge != null)
                        edge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                }
                else
                {
                    node1Alpha = 'w';
                    currentNode.CurrentColor = UtilGraph.VISITED_COLOR;
                    if (edge != null)
                        edge.CurrentColor = UtilGraph.VISITED_COLOR;
                }
                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                lineOfCode = 5;
                if (increment)
                {
                    if (i != UtilGraph.NEIGHBORS_VISITED)
                        SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);

                    useHighlightColor = UseConditionColor(i != UtilGraph.NEIGHBORS_VISITED);
                }
                else
                {
                    if (currentNode == startNode)
                        node1Alpha = 'w';
                }
                break;

            case UtilGraph.IF_NOT_VISITED_INST:
                lineOfCode = 6;
                if (increment)
                {
                    SetNodePseudoCode(connectedNode, 2);
                    useHighlightColor = UseConditionColor(!connectedNode.Visited);

                    if (edge != null)
                    {
                        connectedNode.PrevEdge = edge;
                        edge.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                    }
                    connectedNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                }
                else
                {
                    node2Alpha = 'v';

                    connectedNode.CurrentColor = connectedNode.PrevColor;
                    if (edge != null)
                        edge.CurrentColor = edge.PrevColor;
                }
                break;

            case UtilGraph.END_IF_INST:
                lineOfCode = 8;
                if (increment)
                {
                    if (connectedNode.CurrentColor == UtilGraph.TRAVERSE_COLOR)
                    {
                        connectedNode.CurrentColor = connectedNode.PrevColor;
                        if (edge != null)
                            edge.CurrentColor = edge.PrevColor;
                    }
                }
                else
                {
                    connectedNode.CurrentColor = connectedNode.PrevColor;
                    if (edge != null)
                        edge.CurrentColor = edge.PrevColor;
                }
                break;

            case UtilGraph.END_FOR_LOOP_INST:
                lineOfCode = 9;
                currentNode.Traversed = increment;

                // Destroy current node in list visual / Recreate it
                graphMain.ListVisual.ExecuteInstruction(new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, Util.NO_INSTRUCTION_NR, currentNode), increment);
                break;

            case UtilGraph.END_WHILE_INST:
                lineOfCode = 10;
                IsTaskCompleted = increment;
                break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode), useHighlightColor);

        yield return demoStepDuration;
        graphMain.WaitForSupportToComplete--;
    }

    // BFS / DFS Shares most of instructions
    protected IEnumerator UserTestHighlightTraverse(InstructionBase instruction)
    {
        Node currentNode = null, connectedNode = null;

        // Gather information from instruction
        if (instruction is TraverseInstruction)
        {
            TraverseInstruction travInst = (TraverseInstruction)instruction;

            switch (instruction.Instruction)
            {
                case UtilGraph.IF_NOT_VISITED_INST:
                case UtilGraph.END_IF_INST:
                    connectedNode = travInst.Node;
                    break;

                case UtilGraph.ENQUEUE_NODE_INST:
                case UtilGraph.PUSH_INST:
                    if (travInst.Node != graphMain.GraphManager.StartNode)
                        connectedNode = travInst.Node;
                    else
                        currentNode = travInst.Node;
                    break;

                default: currentNode = travInst.Node; break;
            }
            edge = travInst.PrevEdge;
        }
        else if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                SetNodePseudoCode(currentNode, 0);
                lineOfCode = 0;
                break;

            case UtilGraph.EMPTY_LIST_CONTAINER: lineOfCode = 1; break;
            case UtilGraph.ENQUEUE_NODE_INST: // BFS
            case UtilGraph.PUSH_INST: // DFS
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                if (!startNodeAdded)
                {
                    lineOfCode = 2;
                    SetNodePseudoCode(currentNode, 1);
                    startNodeAdded = true;
                }
                else
                {
                    lineOfCode = 7;
                    SetNodePseudoCode(connectedNode, 2);
                }
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 3;
                lengthOfList = i.ToString();
                useHighlightColor = UseConditionColor(i != 0);
                break;

            case UtilGraph.DEQUEUE_NODE_INST: // BFS
            case UtilGraph.POP_INST: // DFS
                lineOfCode = 4;
                SetNodePseudoCode(currentNode, 1);
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                lineOfCode = 5;
                if (i != UtilGraph.NEIGHBORS_VISITED)
                    SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);

                useHighlightColor = UseConditionColor(i != UtilGraph.NEIGHBORS_VISITED);
                break;

            case UtilGraph.IF_NOT_VISITED_INST:
                lineOfCode = 6;
                SetNodePseudoCode(connectedNode, 2);
                useHighlightColor = UseConditionColor(!connectedNode.Visited);
                break;

            case UtilGraph.END_IF_INST: lineOfCode = 8; break;
            case UtilGraph.END_FOR_LOOP_INST: lineOfCode = 9; break;
            case UtilGraph.END_WHILE_INST: lineOfCode = 10; break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        yield return HighlightPseudoCode(CollectLine(lineOfCode), useHighlightColor);
        graphMain.WaitForSupportToComplete--;
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

    public bool ShortestPathOneToAll
    {
        get { return shortestPathOnToAll; }
        set { shortestPathOnToAll = value; }
    }


    public abstract string GetListType();



}
