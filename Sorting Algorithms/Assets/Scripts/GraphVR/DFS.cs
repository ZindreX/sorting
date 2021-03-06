﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : GraphAlgorithm, ITraverse {

    public override string AlgorithmName
    {
        get { return Util.DFS; }
    }

    public override bool CanPerformBackStep
    {
        get { return false; }
    }

    public override string GetListType()
    {
        return UtilGraph.STACK;
    }

    public override string CollectLine(int lineNr)
    {
        string codeLine = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        
        switch (lineNr)
        {
            case 0: codeLine += string.Format("DFS({0}, {1}):", graphStructure, startNodeAlpha); break;
            case 1: codeLine += "    stack = [ ]"; break;
            case 2: codeLine += string.Format("    stack.Push({0})", startNodeAlpha); break;
            case 3: codeLine += string.Format("    while ({0} > 0):", lengthOfList); break;
            case 4: codeLine += string.Format("        {0} <- stack.Pop()", node1Alpha); break;
            case 5: codeLine += string.Format("        for all neighbors of {0} in Graph:", node1Alpha); break;
            case 6: codeLine += string.Format("            if (!{0}.visited):", node2Alpha); break;
            case 7: codeLine += string.Format("                stack.Push({0})", node2Alpha); break;
            case 8: codeLine += "           end if"; break;
            case 9: codeLine += "       end for"; break;
            case 10: codeLine += "   end while"; break;
            default: return Util.INVALID_PSEUDO_CODE_LINE;
        }
        return codeLine;
    }

    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        return Util.INVALID_PSEUDO_CODE_LINE;
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

    #region DFS Demo
    public IEnumerator TraverseDemo(Node startNode)
    {
        // Line 0: Set graph/start node
        SetNodePseudoCode(startNode, 0);
        yield return HighlightPseudoCode(CollectLine(0), Util.BLACKBOARD_TEXT_COLOR);

        // Line 1: Create an empty list (stack)
        Stack<Node> stack = new Stack<Node>();
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_STANDARD_COLOR);

        // Line 2: Push start node
        stack.Push(startNode);
        graphMain.UpdateListVisual(UtilGraph.ADD_NODE, startNode, Util.NO_VALUE); //listVisual.AddListObject(startNode); // Node Representation
        startNode.Visited = true;
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_STANDARD_COLOR);

        lengthOfList = "1";
        while (stack.Count > 0)
        {
            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 3: Update while-loop
            yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_STANDARD_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Line 4: Pop node from stack
            Node currentNode = stack.Pop();
            graphMain.UpdateListVisual(UtilGraph.REMOVE_CURRENT_NODE, null, Util.NO_VALUE); //listVisual.RemoveCurrentNode(); // Node Representation
            SetNodePseudoCode(currentNode, 1);
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_STANDARD_COLOR);

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
                yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_STANDARD_COLOR);
            }

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            // Go through each edge connected to current node
            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Line 5: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_STANDARD_COLOR);

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

                // Line 6: If statement (condition)
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_STANDARD_COLOR);

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                if (!checkingNode.Visited)
                {
                    // Line 8: Push node on top of stack
                    stack.Push(checkingNode);
                    graphMain.UpdateListVisual(UtilGraph.ADD_NODE, checkingNode, Util.NO_VALUE); //listVisual.AddListObject(checkingNode); // Node Representation
                    checkingNode.Visited = true;
                    yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_STANDARD_COLOR);

                    #region Stop demo
                    // Check if user wants to stop the demo
                    if (graphMain.UserStoppedTask)
                        break;
                    #endregion

                    // Previous edge
                    checkingNode.PrevEdge = edge;
                }

                #region Stop demo
                // Check if user wants to stop the demo
                if (graphMain.UserStoppedTask)
                    break;
                #endregion

                // Line 8: End if statement
                yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_STANDARD_COLOR);

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
            yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_STANDARD_COLOR);

            #region Stop demo
            // Check if user wants to stop the demo
            if (graphMain.UserStoppedTask)
                break;
            #endregion

            currentNode.Traversed = true;
            lengthOfList = stack.Count.ToString(); // Pseudocode stack size

            graphMain.UpdateListVisual(UtilGraph.DESTROY_CURRENT_NODE, null, Util.NO_VALUE); //listVisual.DestroyCurrentNode(); // Node Representation
        }
        // Line 10: End while-loop
        yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_STANDARD_COLOR);

        if (graphMain.UserStoppedTask)
            graphMain.UpdateCheckList(Util.DEMO, true);
        else
            isTaskCompleted = true;
    }
    #endregion

    #region DFS Demo recursive
    public IEnumerator DemoRecursive(Node node)
    {
        if (node.PrevEdge != null)
            node.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;

        SetNodePseudoCode(node, 1);
        //listVisual.RemoveAndMoveElementOut(); // List visual
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_STANDARD_COLOR);

        // Line 3: Mark as visited
        //node.Visited = true;
        //yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

        node.Traversed = true; // ??

        for (int i=0; i < node.Edges.Count; i++)
        {
            Edge nextEdge = node.Edges[i];
            Node nextNode = nextEdge.OtherNodeConnected(node);
            //SetNodePseudoCode(nextNode, 2); // Pseudocode

            if (!nextNode.Visited)
            {
                // Line 8: Push node on top of stack
                nextEdge.CurrentColor = UtilGraph.VISITED_COLOR;
                //listVisual.AddListObject(nextNode.NodeAlphaID); // Visual list
                //yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                // Line 9: Mark node
                nextNode.Visited = true;
                //yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

                yield return DemoRecursive(nextNode);
                //listVisual.DestroyOutElement(); // List visual
            }
            //else
            //{
            //    nextEdge.CurrentColor = UtilGraph.VISITED;
            //    nextNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            //    yield return new WaitForSeconds(seconds);
            //    nextNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;
            //}

        }
        //listVisual.DestroyOutElement();
    }


    #endregion

    #region New Demo version (Demo/Step-by-step)
    public override IEnumerator ExecuteDemoInstruction(InstructionBase instruction, bool increment)
    {
        yield return ExecuteDemoTraverseAlgorithm(instruction, increment); // BFS / DFS Shares most of instructions
    }
    #endregion

    #region User Test Highlight Pseudocode
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotNode)
    {
        yield return UserTestHighlightTraverse(instruction);
    }
    #endregion

    #region DFS Traverse Instructions
    public Dictionary<int, InstructionBase> TraverseUserTestInstructions(Node startNode)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0;

        // Line 0: Update start node
        instructions.Add(instNr, new TraverseInstruction(Util.FIRST_INSTRUCTION, instNr++, startNode, false, false));

        // Line 1: Create an empty list (stack)
        Stack<Node> stack = new Stack<Node>();
        instructions.Add(instNr, new InstructionBase(UtilGraph.EMPTY_LIST_CONTAINER, instNr++));

        // Line 2: Push start node
        stack.Push(startNode);
        startNode.Visited = true;
        ListVisualInstruction addStartNode = new ListVisualInstruction(UtilGraph.ADD_NODE, instNr, startNode);
        instructions.Add(instNr, new TraverseInstruction(UtilGraph.PUSH_INST, instNr++, startNode, true, false, addStartNode));

        while (stack.Count > 0)
        {
            // Line 3: Update while-loop
            instructions.Add(instNr, new InstructionLoop(UtilGraph.WHILE_LIST_NOT_EMPTY_INST, instNr++, stack.Count));

            // Line 4: Pop node from stack
            Node currentNode = stack.Pop();
            currentNode.Traversed = true;
            ListVisualInstruction removeCurrentNode = new ListVisualInstruction(UtilGraph.REMOVE_CURRENT_NODE, instNr, currentNode, 0);
            instructions.Add(instNr, new TraverseInstruction(UtilGraph.POP_INST, instNr++, currentNode, currentNode.PrevEdge, false, true, removeCurrentNode));

            // Go through each edge connected to current node
            // Line 5: Update for-loop
            instructions.Add(instNr, new TraverseInstruction(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr++, currentNode, false, false));
            for (int i = 0; i < currentNode.Edges.Count; i++)
            {
                Edge edge = currentNode.Edges[i];
                Node connectedNode = edge.OtherNodeConnected(currentNode);

                // No need to check the edge we came from
                if (edge == currentNode.PrevEdge)
                    continue;

                // Optimizing check
                //if (connectedNode.Visited || connectedNode.Traversed)
                //    continue;

                // Line 6: If statement (condition)
                instructions.Add(instNr, new TraverseInstruction(UtilGraph.IF_NOT_VISITED_INST, instNr++, connectedNode, edge, false, false));

                if (!connectedNode.Visited)
                {
                    // Line 7: Push node on top of stack
                    stack.Push(connectedNode);
                    connectedNode.Visited = true;
                    ListVisualInstruction addConnectedNode = new ListVisualInstruction(UtilGraph.ADD_NODE, instNr, connectedNode);
                    instructions.Add(instNr, new TraverseInstruction(UtilGraph.PUSH_INST, instNr++, connectedNode, edge, true, false, addConnectedNode));

                    // Set prev edge
                    connectedNode.PrevEdge = edge;
                }
                // Line 8: End if statement
                instructions.Add(instNr, new TraverseInstruction(UtilGraph.END_IF_INST, instNr++, connectedNode, edge, false, false));
            }
            // Line 5: Update for-loop
            instructions.Add(instNr, new InstructionLoop(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr++, UtilGraph.NEIGHBORS_VISITED));
            // Line 9: End for-loop
            instructions.Add(instNr, new TraverseInstruction(UtilGraph.END_FOR_LOOP_INST, instNr++, currentNode, false, false)); // <-- instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr, currentNode));
        }
        // Line 3: condition
        instructions.Add(instNr, new InstructionLoop(UtilGraph.WHILE_LIST_NOT_EMPTY_INST, instNr++, 0));
        // Line 10: End while-loop
        instructions.Add(instNr, new InstructionBase(UtilGraph.END_WHILE_INST, instNr++));

        instructions.Add(instNr, new InstructionBase(Util.FINAL_INSTRUCTION, instNr++));
        return instructions;
    }
    #endregion

    #region DFS Recursive Instructions
    private Dictionary<int, InstructionBase> recursiveInstructions;
    private int instNr;
    public Dictionary<int, InstructionBase> RecursiveInstructions(Node startNode)
    {
        Debug.Log("Preparing for recursive instruction farming");
        recursiveInstructions = new Dictionary<int, InstructionBase>();
        instNr = 0;

        // Line 0: Update start node
        recursiveInstructions.Add(instNr, new TraverseInstruction(Util.FIRST_INSTRUCTION, instNr++, startNode, false, false));

        // Line 2: Push start node
        startNode.Visited = true;
        ListVisualInstruction addStartNode = new ListVisualInstruction(UtilGraph.ADD_NODE, instNr, startNode);
        recursiveInstructions.Add(instNr, new TraverseInstruction(UtilGraph.PUSH_INST, instNr++, startNode, true, false, addStartNode));

        Debug.Log("Starting recursive instruction farming");
        StartCoroutine(RecursiveDFS(startNode));

        // Line 10: End while-loop
        //recursiveInstructions.Add(instNr++, new InstructionBase(UtilGraph.END_WHILE_INST, instNr));

        return recursiveInstructions;
    }

    private IEnumerator RecursiveDFS(Node currentNode)
    {
        // Line 4: Pop node from stack
        currentNode.Traversed = true;
        ListVisualInstruction removeCurrentNode = new ListVisualInstruction(UtilGraph.REMOVE_CURRENT_NODE, instNr, currentNode, 0);
        recursiveInstructions.Add(instNr, new TraverseInstruction(UtilGraph.POP_INST, instNr++, currentNode, currentNode.PrevEdge, false, true, removeCurrentNode));

        for (int i = 0; i < currentNode.Edges.Count; i++)
        {
            Edge connectedEdge = currentNode.Edges[i];
            Node connectedNode = connectedEdge.OtherNodeConnected(currentNode);

            // Line 6: If statement (condition)
            recursiveInstructions.Add(instNr, new TraverseInstruction(UtilGraph.IF_NOT_VISITED_INST, instNr++, connectedNode, connectedEdge, false, false));
            if (!connectedNode.Visited)
            {
                connectedNode.PrevEdge = connectedEdge;

                // Line 7: Push node on top of stack
                ListVisualInstruction addConnectedNode = new ListVisualInstruction(UtilGraph.ADD_NODE, instNr, connectedNode);
                recursiveInstructions.Add(instNr, new TraverseInstruction(UtilGraph.PUSH_INST, instNr++, connectedNode, connectedEdge, true, false, addConnectedNode));

                connectedNode.Visited = true;

                // Line 9: End for-loop (comment out the next line to see the order of all the nodes traversed - not as planned, but kinda useful)
                recursiveInstructions.Add(instNr, new TraverseInstruction(UtilGraph.END_FOR_LOOP_INST, instNr++, currentNode, false, false)); // <-- instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr, currentNode));
                yield return RecursiveDFS(connectedNode);
            }
            // Line 8: End if statement
            //recursiveInstructions.Add(instNr++, new TraverseInstruction(UtilGraph.END_IF_INST, instNr, connectedNode, edge, false, false));
        }
        // Line 9: End for-loop
        //recursiveInstructions.Add(instNr++, new TraverseInstruction(UtilGraph.END_FOR_LOOP_INST, instNr, currentNode, false, false)); // <-- instructions.Add(instNr++, new ListVisualInstruction(UtilGraph.DESTROY_CURRENT_NODE, instNr, currentNode));
    }
    #endregion
}
