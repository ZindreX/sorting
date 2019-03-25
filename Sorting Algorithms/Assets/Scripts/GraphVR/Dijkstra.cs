using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : GraphAlgorithm, IShortestPath {

    [Space(2)]
    [Header("Info plate")]
    [SerializeField]
    private GameObject infoPlate;


    private bool objectiveFound = false;
    private string ifStatementContent;

    public override void InitTeachingAlgorithm(float algorithmSpeed)
    {
        ifStatementContent = "w.Dist + edge(w, v).Cost < v.Dist";
        base.InitTeachingAlgorithm(algorithmSpeed);
    }


    public override string AlgorithmName
    {
        get { return Util.DIJKSTRA; }
    }

    public string IfStatementContent
    {
        get { return ifStatementContent; }
        set { ifStatementContent = value; }
    }

    public override GameObject InfoPlate
    {
        get { return infoPlate; }
    }

    public override string GetListType()
    {
        return UtilGraph.PRIORITY_LIST;
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0:  lineOfCode += string.Format("Dijkstra({0}, {1}):", graphStructure, startNodeAlpha); break;
            case 1:  lineOfCode += "   Set all vertices of Graph to infinity"; break;
            case 2:  lineOfCode += string.Format("   list = [ ]         // Priority queue"); break;
            case 3:  lineOfCode += string.Format("   list.Add({0})", startNodeAlpha); break;
            case 4:  lineOfCode += string.Format("   {0}.Dist = 0", startNodeAlpha); break;
            case 5:  lineOfCode += string.Format("   while ({0} > 0):", lengthOfList); break;
            case 6:  lineOfCode += string.Format("       {0} <- list.PriorityRemove()", node1Alpha); break;
            case 7:  lineOfCode += string.Format("       for all untraversed neighbors of {0} in Graph:", node1Alpha); break;
            case 8:  lineOfCode += string.Format("           Visit neighbor {0}", node2Alpha); break;
            case 9:  lineOfCode += string.Format("           if ({0})", ifStatementContent); break;   //"           if ({0}.Dist={1} + edge({0}, {2}).Cost={3} < {2}.Dist={4}):", node1Alpha, node1Dist, node2Alpha, edgeCost, UtilGraph.ConvertDist(node2Dist)); break;
            case 10: lineOfCode += string.Format("              {0}.Dist = {1}", node2Alpha, (node1Dist + edgeCost)); break;
            case 11: lineOfCode += string.Format("              {0}.Prev = {1}", node2Alpha, node1Alpha); break;
            case 12: lineOfCode += string.Format("              list.PriorityAdd({0})", node2Alpha); break;
            case 13: lineOfCode += string.Format("          end if"); break;
            case 14: lineOfCode += string.Format("      end for"); break;
            case 15: lineOfCode += string.Format("  end while"); break;
            default: return "lineNr " + lineNr + " not found!";
        }
        return lineOfCode;
    }

    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        switch (lineNr)
        {
            //case 6: return init ? "       w <- list.PriorityRemove()" : "       " + node1Alpha + " <- list.PriorityRemove()";
            //case 7: return init ? "       for all neighbors of w in Graph:" : "       for all neighbors of " + node1Alpha + " in Graph:";
            //case 8: return init ? "           Visit neighbor v" : "           Visit neighbor " +  node2Alpha;
            case 9: return init ? "           if (" + node1Alpha + ".Dist + edge(" + node1Alpha + ", " + node2Alpha + ").Cost < " + node2Alpha + ".Dist):" : "           if (" + node1Dist + " + " + edgeCost + " < " + UtilGraph.ConvertDist(node2Dist) + "):";
            default: return "X";
        }
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 15;
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.FOR_ALL_NEIGHBORS_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.SET_START_NODE_DIST_TO_ZERO);
        
        //skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.UPDATE_CONNECTED_NODE_DIST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.UPDATE_CONNECTED_NODE_PREV_EDGE);

        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.HAS_NODE_REPRESENTATION); // ?
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.PRIORITY_ADD_NODE); // ?
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.UPDATE_LIST_VISUAL_VALUE_AND_POSITION); // ?
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.END_NODE_FOUND);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.MARK_END_NODE);

        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.SET_ALL_NODES_TO_INFINITY);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilGraph.SET_START_NODE_DIST_TO_ZERO);
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
        objectiveFound = false;
        ifStatementContent = "";
    }

    // Dijkstra on Tree: must start at 0, or atleast have end node in the same subtree underneath start node
    #region Dijkstra Demo
    public IEnumerator ShortestPathDemo(Node startNode, Node endNode)
    {
        // Line 0: Set graph/start node
        SetNodePseudoCode(startNode, 1); // Pseudocode
        yield return HighlightPseudoCode(CollectLine(0), Util.BLACKBOARD_TEXT_COLOR);

        // Line 1: Set all vertices of G to inifity
        graphMain.GraphManager.SetAllNodesDist(UtilGraph.INF);
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        // Line 2: Create (priority) list
        List<Node> list = new List<Node>();
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

        // Line 3: Add starting node and set its cost to 0
        list.Add(startNode);
        startNode.Visited = true;
        SetNodePseudoCode(startNode, 1, 0); // PseudoCode (line 3+4)
        graphMain.UpdateListVisual(UtilGraph.PRIORITY_ADD_NODE, startNode, 0); //listVisual.PriorityAdd(startNode, 0); // List visual
        yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

        // Line 4: Set total cost (Dist) of start node to 0
        yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);

        lengthOfList = "1";
        while (list.Count > 0 && !objectiveFound)
        {
            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 5: Update while-loop
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            //
            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            currentNode.DisplayEdgeCost(true);

            SetNodePseudoCode(currentNode, 1); // PseudoCode
            graphMain.UpdateListVisual(UtilGraph.REMOVE_CURRENT_NODE, null, Util.NO_VALUE); // listVisual.RemoveCurrentNode(); // List visual
            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;

            // Line 6: Remove element with lowest distance
            yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Stop search if end node found and we dont want shortest path to all - stop when first visited instead? (not always global optimal)
            if (!shortestPathOnToAll && currentNode == endNode)
            {
                objectiveFound = true;
                continue;
            }

            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;
            numberOfEdges = edges.Count; // Pseudocode

            // Line 7: Update for-loop (if no nodes connected)
            if (numberOfEdges == 0)
            {
                i = 0; // not used anymore
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);
            }

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            for (int i=0; i < numberOfEdges; i++)
            {
                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Line 7: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Checking edge
                Edge currentEdge = edges[i];

                // Dont check edge we came from
                if (currentEdge == currentNode.PrevEdge)
                    continue;

                // Checking node on the other side of the edge
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                if (connectedNode == null || connectedNode.Traversed)
                    continue;//currentEdge.CurrentColor = Util.STANDARD_COLOR;

                SetEdge(currentEdge); // Pseudocode
                yield return demoStepDuration;

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                SetNodePseudoCode(connectedNode, 2); // PseudoCode

                if (!connectedNode.Visited)
                    connectedNode.Visited = true;

                // Line 8: visit connected node
                yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Cost between nodes
                int currentDistAndEdgeCost = node1Dist + edgeCost;
                ifStatementContent = currentDistAndEdgeCost +  " < " + UtilGraph.ConvertDist(node2Dist);

                // Line 9: If statement
                yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Update cost of connected node
                if (currentDistAndEdgeCost < connectedNode.Dist)
                {
                    // Line 10: Update total cost (Dist) of connected node (w)
                    connectedNode.Dist = currentDistAndEdgeCost;
                    yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);

                    #region Stop demo
                    // Check if user wants to stop the demo
                    if (graphMain.UserStoppedTask)
                        break;
                    #endregion

                    // Line 11: Update prev edge (Prev) of connected node (w)
                    connectedNode.PrevEdge = currentEdge;
                    yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);

                    #region Stop demo
                    // Check if user wants to stop the demo
                    if (graphMain.UserStoppedTask)
                        break;
                    #endregion


                    if (!list.Contains(connectedNode))
                        list.Add(connectedNode);

                    // Sort list such that the lowest distance node gets out first
                    list.Sort();

                    // List visual
                    if (graphMain.GraphSettings.Difficulty < Util.ADVANCED)
                    {
                        int index = list.IndexOf(connectedNode); //(list.Count - 1 ) - list.IndexOf(connectedNode); // OBS: list is inverted (removes the last element instead of index 0)
                        //int currentNodeRepIndex = graphMain.ListVisual.ListIndexOf(connectedNode); // Create method/action code ???

                        if (!graphMain.CheckListVisual(UtilGraph.HAS_NODE_REPRESENTATION, connectedNode)) // listVisual.HasNodeRepresentation(connectedNode))
                            graphMain.UpdateListVisual(UtilGraph.PRIORITY_ADD_NODE, connectedNode, index); // listVisual.PriorityAdd(connectedNode, index); // Node representation
                        else
                            yield return graphMain.ListVisual.UpdateValueAndPositionOf(connectedNode, index); // Create method/action code ???
                    }

                    #region Stop demo
                    // Check if user wants to stop the demo
                    if (graphMain.UserStoppedTask)
                        break;
                    #endregion

                    // Line 12: Add to list
                    yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_COLOR);
                }

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                currentEdge.CurrentColor = UtilGraph.VISITED_COLOR;

                // Line 13: End if
                yield return HighlightPseudoCode(CollectLine(13), Util.HIGHLIGHT_COLOR);
            }

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 14: End for-loop
            yield return HighlightPseudoCode(CollectLine(14), Util.HIGHLIGHT_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            currentNode.Traversed = true;

            lengthOfList = list.Count.ToString(); // PseudoCode
            graphMain.UpdateListVisual(UtilGraph.DESTROY_CURRENT_NODE, null, Util.NO_VALUE); // listVisual.DestroyCurrentNode(); // Node Representation
        }
        // Line 15: End while-loop
        yield return HighlightPseudoCode(CollectLine(15), Util.HIGHLIGHT_COLOR);

        if (graphMain.UserStoppedTask)
            graphMain.UpdateCheckList(Util.DEMO, true);
        else
            isTaskCompleted = true;
    }
    #endregion

    // Dijkstra on Tree: must start at 0, or atleast have end node in the same subtree underneath start node
    #region Dijkstra Demo No pseudocode for fast runthrough
    public IEnumerator DemoNoPseudocode(Node startNode, Node endNode)
    {
        // Line 2: Create (priority) list
        List<Node> list = new List<Node>();

        // Line 3: Add starting node and set its cost to 0
        list.Add(startNode);
        startNode.Dist = 0;
        startNode.Visited = true;

        while (list.Count > 0 && !objectiveFound)
        {
            //
            yield return demoStepDuration;

            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;

            // Stop search if end node found and we dont want shortest path to all
            if (!shortestPathOnToAll && currentNode == endNode)
                objectiveFound = true;

            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;
            for (int i = 0; i < edges.Count; i++)
            {
                // Checking edge
                Edge currentEdge = edges[i];

                // Dont check edge we came from
                if (currentEdge == currentNode.PrevEdge)
                    continue;

                // Checking node on the other side of the edge
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                if (connectedNode == null)
                {
                    currentEdge.CurrentColor = UtilGraph.STANDARD_COLOR;
                    continue;
                }

                if (!connectedNode.Visited)
                    connectedNode.Visited = true;

                // Cost between nodes
                int currentDistAndEdgeCost = currentNode.Dist + currentEdge.Cost;

                // Update cost of connected node
                if (currentDistAndEdgeCost < connectedNode.Dist)
                {
                    // Line 9: Update total cost (Dist) of connected node (w)
                    connectedNode.Dist = currentDistAndEdgeCost;

                    // Line 10: Update prev edge (Prev) of connected node (w)
                    connectedNode.PrevEdge = currentEdge;

                    if (!connectedNode.Traversed) // && !list.Contains(connectedNode)) // was outside if
                    {
                        if (!list.Contains(connectedNode))
                            list.Add(connectedNode);

                        list.Sort();
                    }
                }
                currentEdge.CurrentColor = UtilGraph.VISITED_COLOR;
            }
            currentNode.Traversed = true;
        }
        // Line 14: End while-loop
        IsTaskCompleted = true;
    }
    #endregion


    #region Shortest path Instructions
    public Dictionary<int, InstructionBase> ShortestPathUserTestInstructions(Node startNode, Node endNode)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0;

        // Line 0: (update startnode)
        instructions.Add(instNr++, new InstructionBase(Util.FIRST_INSTRUCTION, instNr));

        // Line 1: Set all vertices of G to inifity
        graphMain.GraphManager.SetAllNodesDist(UtilGraph.INF);
        instructions.Add(instNr++, new InstructionBase(UtilGraph.SET_ALL_NODES_TO_INFINITY, instNr));

        // Line 2: Create (priority) list
        List<Node> list = new List<Node>();
        instructions.Add(instNr++, new InstructionBase(UtilGraph.EMPTY_LIST_CONTAINER, instNr));


        // Line 3: Add starting node and set its cost to 0
        list.Add(startNode);
        startNode.Visited = true;
        startNode.Dist = 0;
        instructions.Add(instNr++, new TraverseInstruction(UtilGraph.ADD_NODE, instNr, startNode, true, false));
        instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.PRIORITY_ADD_NODE, instNr, startNode, 0));

        // Line 4: Set total cost (Dist) of start node to 0
        instructions.Add(instNr++, new InstructionBase(UtilGraph.SET_START_NODE_DIST_TO_ZERO, instNr));
        instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.SET_START_NODE_DIST_TO_ZERO, instNr, startNode, 0));

        
        while (list.Count > 0 && !objectiveFound)
        {
            // Line 5: Update while-loop
            instructions.Add(instNr++, new InstructionLoop(UtilGraph.WHILE_LIST_NOT_EMPTY_INST, instNr, list.Count));

            //
            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            // Line 6: Remove element with lowest distance
            instructions.Add(instNr++, new TraverseInstruction(UtilGraph.PRIORITY_REMOVE_NODE, instNr, currentNode, false, true));
            instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.REMOVE_CURRENT_NODE, instNr));

            // Stop search if end node found and we dont want shortest path to all - stop when first visited instead? (not always global optimal)
            if (!shortestPathOnToAll && currentNode == endNode)
            {
                instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.END_NODE_FOUND, instNr, currentNode));
                objectiveFound = true;
                break;
            }

            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;

            // Line 7: Update for-loop (if no nodes connected)
            if (edges.Count == 0)
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr, currentNode, false, false));

            for (int i = 0; i < edges.Count; i++)
            {
                // Line 7: Update for-loop
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr, currentNode, false, false));

                // Checking edge
                Edge currentEdge = edges[i];

                // Dont check edge we came from
                if (currentEdge == currentNode.PrevEdge)
                    continue;

                // Checking node on the other side of the edge (directed edge scenario)
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                if (connectedNode == null || connectedNode.Traversed)
                    continue;

                if (!connectedNode.Visited)
                    connectedNode.Visited = true;

                // Cost between nodes
                int currentDistAndEdgeCost = currentNode.Dist + currentEdge.Cost;

                // Line 8: Visit connected node
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.VISIT_CONNECTED_NODE, instNr, connectedNode, currentEdge, true, false));

                // Line 9: If statement
                instructions.Add(instNr++, new ShortestPathInstruction(UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN, instNr, currentNode, connectedNode, currentEdge));

                // Update cost of connected node
                if (currentDistAndEdgeCost < connectedNode.Dist)
                {
                    // Line 10: Update total cost (Dist) of connected node (w)
                    connectedNode.Dist = currentDistAndEdgeCost;
                    instructions.Add(instNr++, new ShortestPathInstruction(UtilGraph.UPDATE_CONNECTED_NODE_DIST, instNr, currentNode, connectedNode, currentEdge));

                    // Line 11: Update prev edge (Prev) of connected node (w)
                    instructions.Add(instNr++, new ShortestPathInstruction(UtilGraph.UPDATE_CONNECTED_NODE_PREV_EDGE, instNr, connectedNode, currentEdge));
                    connectedNode.PrevEdge = currentEdge;


                    if (!list.Contains(connectedNode))
                        list.Add(connectedNode);

                    // Sort list such that the lowest distance node gets out first
                    list.Sort();


                    // List visual
                    int index = list.IndexOf(connectedNode);

                    instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.HAS_NODE_REPRESENTATION, instNr, connectedNode, index, UtilGraph.PRIORITY_ADD_NODE, UtilGraph.UPDATE_LIST_VISUAL_VALUE_AND_POSITION));

                    // Line 12: Add to list
                    instructions.Add(instNr++, new InstructionBase(UtilGraph.PRIORITY_ADD_NODE, instNr));
                }
                // Line 13: End if
                instructions.Add(instNr++, new InstructionBase(UtilGraph.END_IF_INST, instNr));
            }
            // Line 14: End for-loop
            instructions.Add(instNr++, new InstructionBase(UtilGraph.END_FOR_LOOP_INST, instNr));
            instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr));

            currentNode.Traversed = true;
        }
        // Line 15: End while-loop
        instructions.Add(instNr++, new InstructionBase(UtilGraph.END_WHILE_INST, instNr));

        // Gather instruction for backtracking
        graphMain.GraphManager.ShortestPatBacktrackingInstructions(instructions, instNr);     
        return instructions;
    }
    #endregion


    #region New Demo
    public override IEnumerator ExecuteDemoInstruction(InstructionBase instruction, bool increment)
    {
        Node currentNode = null, connectedNode = null;
        Edge currentEdge = null, prevEdge = null;

        // Gather information from instruction
        if (instruction is TraverseInstruction)
        {
            if (instruction.Instruction == UtilGraph.ADD_NODE || instruction.Instruction == UtilGraph.PRIORITY_REMOVE_NODE || instruction.Instruction == UtilGraph.FOR_ALL_NEIGHBORS_INST)
                currentNode = ((TraverseInstruction)instruction).Node;
            else
                connectedNode = ((TraverseInstruction)instruction).Node;

            currentEdge = ((TraverseInstruction)instruction).PrevEdge;
        }
        else if (instruction is ShortestPathInstruction)
        {
            currentNode = ((ShortestPathInstruction)instruction).CurrentNode;
            connectedNode = ((ShortestPathInstruction)instruction).ConnectedNode;
            currentEdge = ((ShortestPathInstruction)instruction).CurrentEdge;
            prevEdge = ((ShortestPathInstruction)instruction).PrevEdge;
        }
        else if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            //j = ((InstructionLoop)instruction).J;
            //k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = 0;
                if (increment)
                    SetNodePseudoCode(graphMain.GraphManager.StartNode, 0);
                else
                    startNodeAlpha = 's';
                break;

            case UtilGraph.SET_ALL_NODES_TO_INFINITY:
                lineOfCode = 1;
                if (increment)
                    graphMain.GraphManager.SetAllNodesDist(UtilGraph.INF);
                else
                    graphMain.GraphManager.SetAllNodesDist(UtilGraph.INIT_NODE_DIST);
                break;

            case UtilGraph.EMPTY_LIST_CONTAINER:
                lineOfCode = 2;
                break;

            case UtilGraph.ADD_NODE:
                SetNodePseudoCode(currentNode, 0); // start node
                lineOfCode = 3;
                if (increment)
                    currentNode.Visited = ((TraverseInstruction)instruction).VisitInst;
                else
                    currentNode.Visited = !((TraverseInstruction)instruction).VisitInst;
                break;

            case UtilGraph.SET_START_NODE_DIST_TO_ZERO:
                lineOfCode = 4;
                if (increment)
                    startNode.Dist = 0;
                else
                    startNode.Dist = UtilGraph.INF;
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 5;
                lengthOfList = i.ToString();
                break;

            case UtilGraph.PRIORITY_REMOVE_NODE:
                lineOfCode = 6;

                if (increment)
                {
                    // Hide all edge cost to make it easier to see node distances
                    graphMain.GraphManager.MakeEdgeCostVisible(false);
                    SetNodePseudoCode(currentNode, 1);

                    yield return demoStepDuration;

                    // Show the next node we'll work from
                    currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                    currentNode.DisplayEdgeCost(true);

                    // Display edge costs again
                    graphMain.GraphManager.MakeEdgeCostVisible(true);
                }
                else
                {
                    currentNode.CurrentColor = UtilGraph.VISITED_COLOR;
                    currentNode.DisplayEdgeCost(false);
                }

                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                SetNodePseudoCode(currentNode, 1);
                lineOfCode = 7;
                break;

            case UtilGraph.VISIT_CONNECTED_NODE:
                SetNodePseudoCode(connectedNode, 2);
                lineOfCode = 8;

                if (increment)
                {
                    connectedNode.Visited = ((TraverseInstruction)instruction).VisitInst;
                    if (currentEdge != null)
                        currentEdge.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                }
                else
                {
                    connectedNode.Visited = !((TraverseInstruction)instruction).VisitInst;
                    if (currentEdge != null)
                        currentEdge.CurrentColor = Util.STANDARD_COLOR;
                }

                break;

            case UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN:
                SetNodePseudoCode(connectedNode, 2);
                lineOfCode = 9;

                if (increment)
                {
                    edgeCost = currentEdge.Cost;
                    ifStatementContent = CreateIfStatementContent(this.currentNode.Dist, edgeCost, connectedNode.Dist);
                }
                else
                {
                    ifStatementContent = "";
                }
                break;

            case UtilGraph.UPDATE_CONNECTED_NODE_DIST:
                lineOfCode = 10;
                if (increment)
                    connectedNode.Dist = ((ShortestPathInstruction)instruction).ConnectedNodeNewDist;
                else
                    connectedNode.Dist = ((ShortestPathInstruction)instruction).ConnectedNodeOldDist;
                break;

            case UtilGraph.UPDATE_CONNECTED_NODE_PREV_EDGE:
                lineOfCode = 11;
                break;

            case UtilGraph.PRIORITY_ADD_NODE:
                lineOfCode = 12;
                break;

            case UtilGraph.END_IF_INST:
                lineOfCode = 13;
                if (increment)
                {
                    this.connectedNode.PrevEdge = prevEdge;
                    if (prevEdge != null)
                        prevEdge.CurrentColor = UtilGraph.VISITED_COLOR;
                }
                else
                {
                    this.connectedNode.PrevEdge = ((ShortestPathInstruction)instruction).OldPrevEdge;
                    if (connectedNode.PrevEdge != null)
                        this.connectedNode.PrevEdge.CurrentColor = Util.STANDARD_COLOR;
                }
                break;

            case UtilGraph.END_FOR_LOOP_INST:
                lineOfCode = 14;
                if (this.currentNode != null)
                    this.currentNode.Traversed = increment;
                break;

            case UtilGraph.END_WHILE_INST:
                lineOfCode = 15;
                IsTaskCompleted = increment;
                break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode), useHighlightColor);

        yield return demoStepDuration;
        graphMain.WaitForSupportToComplete--;
    }
    #endregion



    #region User Test Highlight Pseudocode
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotNode)
    {
        Node currentNode = null, connectedNode = null;
        Edge currentEdge = null, prevEdge = null;

        // Gather information from instruction
        if (gotNode)
        {
            //Debug.Log("Got node");
            if (instruction is TraverseInstruction)
            {
                if (instruction.Instruction == UtilGraph.ADD_NODE || instruction.Instruction == UtilGraph.PRIORITY_REMOVE_NODE || instruction.Instruction == UtilGraph.FOR_ALL_NEIGHBORS_INST)
                    currentNode = ((TraverseInstruction)instruction).Node;
                else
                    connectedNode = ((TraverseInstruction)instruction).Node;
            }
            else if (instruction is ShortestPathInstruction)
            {
                currentNode = ((ShortestPathInstruction)instruction).CurrentNode;
                connectedNode = ((ShortestPathInstruction)instruction).ConnectedNode;
                currentEdge = ((ShortestPathInstruction)instruction).CurrentEdge;
                prevEdge = ((ShortestPathInstruction)instruction).PrevEdge;
            }
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            //j = ((InstructionLoop)instruction).J;
            //k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = 0;
                SetNodePseudoCode(graphMain.GraphManager.StartNode, 0);
                useHighlightColor = Util.BLACKBOARD_TEXT_COLOR;
                break;

            case UtilGraph.SET_ALL_NODES_TO_INFINITY:
                lineOfCode = 1;
                break;

            case UtilGraph.EMPTY_LIST_CONTAINER:
                lineOfCode = 2;
                break;

            case UtilGraph.ADD_NODE:
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                lineOfCode = 3;
                break;

            case UtilGraph.SET_START_NODE_DIST_TO_ZERO:
                lineOfCode = 4;
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 5;
                lengthOfList = i.ToString();
                break;

            case UtilGraph.PRIORITY_REMOVE_NODE:
                SetNodePseudoCode(currentNode, 1);
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                lineOfCode = 6;
                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                SetNodePseudoCode(currentNode, 1);
                lineOfCode = 7;
                break;

            case UtilGraph.VISIT_CONNECTED_NODE:
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                SetNodePseudoCode(connectedNode, 2);
                lineOfCode = 8;
                break;

            case UtilGraph.IF_DIST_PLUS_EDGE_COST_LESS_THAN:
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                SetNodePseudoCode(connectedNode, 2);
                lineOfCode = 9;
                edgeCost = currentEdge.Cost;
                break;

            case UtilGraph.UPDATE_CONNECTED_NODE_DIST:
                //connectedNode.Dist = ((ShortestPathInstruction)instruction).NodeDistAndEdgeCostTotal();
                lineOfCode = 10;
                break;

            case UtilGraph.UPDATE_CONNECTED_NODE_PREV_EDGE:
                //connectedNode.PrevEdge = prevEdge;
                lineOfCode = 11;
                break;

            case UtilGraph.PRIORITY_ADD_NODE:
                lineOfCode = 12;
                break;

            case UtilGraph.END_IF_INST: lineOfCode = 13; break;
            case UtilGraph.END_FOR_LOOP_INST: lineOfCode = 14; break;
            case UtilGraph.END_WHILE_INST: lineOfCode = 15; break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode), Util.HIGHLIGHT_COLOR);

        yield return demoStepDuration;
        graphMain.WaitForSupportToComplete--;
    }
    #endregion




    private string CreateIfStatementContent(int currentNodeDist, int edgeCost, int connectedNodeDist)
    {
        string result = currentNodeDist + " + " + edgeCost;
        if (currentNodeDist + edgeCost < connectedNodeDist)
            result += " < " + UtilGraph.ConvertDist(connectedNode.Dist);
        else
            result += " > " + UtilGraph.ConvertDist(connectedNode.Dist);
        return result;
    }



    // *** Debugging ***

    private string PrintList(List<Node> list)
    {
        string result = "Dijkstra:                    ";
        for (int i = 0; i < list.Count; i++)
        {
            result += "[" + list[i].NodeAlphaID + "," + list[i].Dist + "], ";
        }
        return result;
    }
}
