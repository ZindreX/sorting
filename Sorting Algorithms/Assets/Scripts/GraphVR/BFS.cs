using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : GraphAlgorithm, ITraverse {

    [Space(2)]
    [Header("Info plate")]
    [SerializeField]
    private GameObject infoPlate;

    public override string AlgorithmName
    {
        get { return Util.BFS; }
    }

    public override GameObject InfoPlate
    {
        get { return infoPlate; }
    }


    public override string GetListType()
    {
        return Util.QUEUE;
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode += string.Format("BFS({0}, {1}):", graphStructure, startNodeAlpha); break;
            case 1: lineOfCode += string.Format("   queue = [ ]"); break;
            case 2: lineOfCode += string.Format("   queue.Enqueue({0})", startNodeAlpha); break;
            case 3: lineOfCode += string.Format("   while ({0} > 0):", lengthOfList); break;
            case 4: lineOfCode += string.Format("       {0} <- queue.Dequeue()", node1Alpha); break;
            case 5: lineOfCode += string.Format("       for all neighbors of {0} in Graph:", node1Alpha); break; //case 6: lineOfCode += string.Format("       for i={0} to {1}:", i, node1.Edges.Count-1); break;
            case 6: lineOfCode += string.Format("           if (!{0}.visited):", node2Alpha); break;
            case 7: lineOfCode += string.Format("               queue.Enqueue({0})", node2Alpha); break;
            case 8: lineOfCode += string.Format("          end if"); break;
            case 9: lineOfCode += string.Format("      end for"); break;
            case 10: lineOfCode += string.Format("  end while"); break;
            default: return "lineNr " + lineNr + " not found!";
        }
        return lineOfCode;
    }

    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        return "X";
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 10;
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.IF_NOT_VISITED_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.FOR_ALL_NEIGHBORS_INST);
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
    }

    #region BFS Demo
    public IEnumerator TraverseDemo(Node startNode)
    {
        // Line 0: Set graph/start node
        SetNodePseudoCode(startNode, 0); // Pseudocode
        yield return HighlightPseudoCode(CollectLine(0), Util.BLACKBOARD_TEXT_COLOR);

        // Line 1: Create empty list (queue)
        Queue<Node> queue = new Queue<Node>();
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        // Line 2: Add start node
        queue.Enqueue(startNode);
        graphMain.UpdateListVisual(UtilGraph.ADD_NODE, startNode, Util.NO_VALUE); //listVisual.AddListObject(startNode); // Node Representation
        startNode.Visited = true;
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

        lengthOfList = "1";
        while (queue.Count > 0)
        {
            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 3: Update while loop
            yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 4: Dequeue node from queue
            Node currentNode = queue.Dequeue();
            graphMain.UpdateListVisual(UtilGraph.REMOVE_CURRENT_NODE, null, Util.NO_VALUE); // listVisual.RemoveCurrentNode(); // Node Representation
            SetNodePseudoCode(currentNode, 1); // Pseudocode
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // When visiting current node, change color of edge leading to this node
            if (currentNode.PrevEdge != null)
            {
                currentNode.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return demoStepDuration;
            }

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return demoStepDuration;

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 5: Update for-loop (leaf nodes)
            if (currentNode.Edges.Count == 0)
            {
                i = 0;
                yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);
            }

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Go through each edge connected to current node
            for (int i = 0; i < currentNode.Edges.Count; i++)
            {
                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Line 5: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                Edge edge = currentNode.Edges[i];

                // Dont need to check the node we came from
                if (edge == currentNode.PrevEdge)
                    continue;

                Node checkingNode = edge.OtherNodeConnected(currentNode);
                SetNodePseudoCode(checkingNode, 2); // Pseudocode

                // Mark edge
                edge.CurrentColor = UtilGraph.VISITED_COLOR;

                // Line 6: If condition
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Check if node has already been traversed or already is marked
                if (!checkingNode.Visited)
                {
                    // Line 8: Add to queue
                    queue.Enqueue(checkingNode);
                    graphMain.UpdateListVisual(UtilGraph.ADD_NODE, checkingNode, Util.NO_VALUE); //listVisual.AddListObject(checkingNode); // Node Representation
                    checkingNode.Visited = true;
                    yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

                    #region Stop demo
                    // Check if user wants to stop the demo
                    if (graphMain.UserStoppedTask)
                        break;
                    #endregion

                    // Previous edge is this one
                    checkingNode.PrevEdge = edge;
                }

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Line 8: End if statement
                yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion
            }

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 9: End for-loop
            yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            currentNode.Traversed = true;
            lengthOfList = queue.Count.ToString(); // Pseudo code

            graphMain.UpdateListVisual(UtilGraph.DESTROY_CURRENT_NODE, null, Util.NO_VALUE); //listVisual.DestroyCurrentNode(); // Node Representation
        }
        // Line 10: End while-loop
        yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);

        if (graphMain.UserStoppedTask)
            graphMain.UpdateCheckList(Util.DEMO, true);
        else
            isTaskCompleted = true;
    }
    #endregion

    #region BFS User Test instructions
    public Dictionary<int, InstructionBase> TraverseUserTestInstructions(Node startNode)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0;

        // Line 1: Create emtpy list (queue)
        Queue<Node> queue = new Queue<Node>();
        instructions.Add(instNr++, new InstructionBase(UtilGraph.EMPTY_LIST_CONTAINER, instNr));

        // Line 2: Enqueue first node
        queue.Enqueue(startNode);
        startNode.Visited = true;
        instructions.Add(instNr++, new TraverseInstruction(UtilGraph.ENQUEUE_NODE_INST, instNr, startNode, true, false));
        instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.ADD_NODE, instNr, startNode));

        // Line 3: Mark node as visited
        //instructions.Add(instNr++, new TraverseInstruction(UtilGraph.MARK_VISITED_INST, instNr, 0, startNode, true));

        while (queue.Count > 0)
        {
            // Line 4: While loop
            instructions.Add(instNr++, new InstructionLoop(UtilGraph.WHILE_LIST_NOT_EMPTY_INST, instNr, queue.Count));

            // Line 5: Dequeue node
            Node currentNode = queue.Dequeue();
            currentNode.Traversed = true;

            if (currentNode.PrevEdge != null)
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.DEQUEUE_NODE_INST, instNr, currentNode, currentNode.PrevEdge, false, true));
            else
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.DEQUEUE_NODE_INST, instNr, currentNode, false, true));

            instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.REMOVE_CURRENT_NODE, instNr));

            for (int i = 0; i < currentNode.Edges.Count; i++)
            {
                // Line 6: For-loop update
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr, currentNode, false, false)); //new InstructionLoop(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr, i, currentNode.Edges.Count, Util.NO_INDEX_VALUE));

                Edge edge = currentNode.Edges[i];
                Node connectedNode = edge.OtherNodeConnected(currentNode);
                connectedNode.PrevEdge = edge;

                // Line 7: check neighbor
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.IF_NOT_VISITED_INST, instNr, connectedNode, false, false)); // check if correct ***
                // Check if node has already been traversed or already is marked
                if (!connectedNode.Visited)
                {
                    // Line 8: Enqueue node
                    queue.Enqueue(connectedNode);
                    connectedNode.Visited = true;
                    instructions.Add(instNr++, new TraverseInstruction(UtilGraph.ENQUEUE_NODE_INST, instNr, connectedNode, edge, true, false));
                    instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.ADD_NODE, instNr, connectedNode));
                }
                instructions.Add(instNr++, new InstructionBase(UtilGraph.END_IF_INST, instNr));
            }
            instructions.Add(instNr++, new InstructionBase(UtilGraph.END_FOR_LOOP_INST, instNr));
            instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr));
        }
        instructions.Add(instNr++, new InstructionBase(UtilGraph.END_WHILE_INST, instNr));
        return instructions;
    }
    #endregion

    #region New Demo version (Demo/Step-by-step)
    public override IEnumerator ExecuteDemoInstruction(InstructionBase instruction, bool increment)
    {
        Debug.Log(instruction.DebugInfo());

        // Gather information from instruction
        if (instruction is TraverseInstruction)
        {
            if (instruction.Instruction == UtilGraph.IF_NOT_VISITED_INST || instruction.Instruction == UtilGraph.ENQUEUE_NODE_INST && ((TraverseInstruction)instruction).Node != graphMain.GraphManager.StartNode)
                connectedNode = ((TraverseInstruction)instruction).Node;
            else
                currentNode = ((TraverseInstruction)instruction).Node;

            edge = ((TraverseInstruction)instruction).PrevEdge;
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
                SetNodePseudoCode(graphMain.GraphManager.StartNode, 0);
                lineOfCode = 0;
                break;

            case UtilGraph.EMPTY_LIST_CONTAINER:
                lineOfCode = 1;
                break;

            case UtilGraph.ENQUEUE_NODE_INST:
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                if (!startNodeAdded)
                {
                    SetNodePseudoCode(currentNode, 1);
                    lineOfCode = 2;
                    startNodeAdded = true;
                }
                else
                {
                    lineOfCode = 7;
                    connectedNode.Visited = ((TraverseInstruction)instruction).VisitInst;
                    if (edge != null)
                        edge.CurrentColor = UtilGraph.VISITED_COLOR;
                }
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 3;
                lengthOfList = i.ToString();
                break;

            case UtilGraph.DEQUEUE_NODE_INST:
                SetNodePseudoCode(currentNode, 1);
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                lineOfCode = 4;

                currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
                if (edge != null)
                    edge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                SetNodePseudoCode(currentNode, 1);
                lineOfCode = 5;
                break;

            case UtilGraph.IF_NOT_VISITED_INST:
                SetNodePseudoCode(connectedNode, 2);
                lineOfCode = 6;
                break;

            case UtilGraph.END_IF_INST:
                lineOfCode = 8;
                break;

            case UtilGraph.END_FOR_LOOP_INST:
                lineOfCode = 9;
                currentNode.Traversed = true;
                break;

            case UtilGraph.END_WHILE_INST:
                lineOfCode = 10;
                IsTaskCompleted = true;
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
        // Gather information from instruction
        if (gotNode)
            currentNode = ((TraverseInstruction)instruction).Node;

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
                SetNodePseudoCode(graphMain.GraphManager.StartNode, 0);
                lineOfCode = 0;
                break;

            case UtilGraph.EMPTY_LIST_CONTAINER: lineOfCode = 1; break;
            case UtilGraph.ENQUEUE_NODE_INST:
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                if (!startNodeAdded)
                {
                    lineOfCode = 2;
                    startNodeAdded = true;
                }
                else
                    lineOfCode = 7;
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 3;
                lengthOfList = i.ToString();
                break;

            case UtilGraph.DEQUEUE_NODE_INST:
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                useHighlightColor = Util.HIGHLIGHT_MOVE_COLOR;
                lineOfCode = 4;
                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                lineOfCode = 5;
                break;

            case UtilGraph.IF_NOT_VISITED_INST:
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 2);
                lineOfCode = 6;
                break;

            case UtilGraph.END_IF_INST: lineOfCode = 8; break;
            case UtilGraph.END_FOR_LOOP_INST: lineOfCode = 9; break;
            case UtilGraph.END_WHILE_INST: lineOfCode = 10; break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode), useHighlightColor);

        yield return demoStepDuration;
        graphMain.WaitForSupportToComplete--;
    }
    #endregion

}