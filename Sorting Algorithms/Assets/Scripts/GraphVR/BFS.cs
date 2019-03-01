using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : GraphAlgorithm, ITraverse {

    public override string AlgorithmName
    {
        get { return UtilGraph.BFS; }
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode += string.Format("BFS({0}, {1}):", graphStructure, node1Alpha); break;
            case 1: lineOfCode += string.Format("   queue = [ ]"); break;
            case 2: lineOfCode += string.Format("   queue.Enqueue({0})", node1Alpha); break;
            case 3: lineOfCode += string.Format("   {0}.visited = true", node1Alpha); break;
            case 4: lineOfCode += string.Format("   while ({0} > 0):", lengthOfList); break;
            case 5: lineOfCode += string.Format("       {0} <- queue.Dequeue()", node1Alpha); break;
            case 6: lineOfCode += string.Format("       for all neighbors of {0} in Graph:", node1Alpha); break; //case 6: lineOfCode += string.Format("       for i={0} to {1}:", i, node1.Edges.Count-1); break;
            case 7: lineOfCode += string.Format("           if (!{0}.visited):", node2Alpha); break;
            case 8: lineOfCode += string.Format("               queue.Enqueue({0})", node2Alpha); break;
            case 9: lineOfCode += string.Format("               {0}.visited = true", node2Alpha); break;
            case 10: lineOfCode += string.Format("          end if"); break;
            case 11: lineOfCode += string.Format("      end for"); break;
            case 12: lineOfCode += string.Format("  end while"); break;
            default: return "lineNr " + lineNr + " not found!";
        }
        return lineOfCode;
    }

    public override int FirstInstructionCodeLine()
    {
        return 1;
    }

    public override int FinalInstructionCodeLine()
    {
        return 12;
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.ENQUEUE_NODE_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.MARK_VISITED_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.IF_NOT_VISITED_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilGraph.FOR_ALL_NEIGHBORS_INST);
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
    }

    #region BFS Demo
    public IEnumerator Demo(Node startNode)
    {
        // Line 0: Set graph/start node
        SetNodePseudoCode(startNode, 1); // Pseudocode
        yield return HighlightPseudoCode(CollectLine(0), Util.BLACKBOARD_TEXT_COLOR);
        
        // Line 1: Create empty list (queue)
        Queue<Node> queue = new Queue<Node>();
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        // Line 2: Add start node
        queue.Enqueue(startNode);
        listVisual.AddListObject(startNode); // Node Representation
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

        // Line 3: Mark as visited
        startNode.Visited = true;
        yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

        lengthOfList = "1";
        while (queue.Count > 0)
        {
            // Line 4: Update while loop
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);

            // Line 5: Dequeue node from queue
            Node currentNode = queue.Dequeue();
            listVisual.RemoveCurrentNode(); // Node Representation
            SetNodePseudoCode(currentNode, 1); // Pseudocode
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

            if (currentNode.PrevEdge != null)
            {
                currentNode.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return demoStepDuration;
            }

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return demoStepDuration;

            // Line 6: Update for-loop (leaf nodes)
            if (currentNode.Edges.Count == 0)
            {
                i = 0;
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);
            }

            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                // Line 6: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

                Edge edge = currentNode.Edges[i];

                // Mark edge
                edge.CurrentColor = UtilGraph.TRAVERSED_COLOR;

                Node checkingNode = edge.OtherNodeConnected(currentNode);
                SetNodePseudoCode(checkingNode, 2); // Pseudocode

                // Line 7: If condition
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

                // Check if node has already been traversed or already is marked
                if (!checkingNode.Visited)
                {
                    // Line 8: Add to queue
                    queue.Enqueue(checkingNode);
                    listVisual.AddListObject(checkingNode); // Node Representation
                    yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                    // Line 9: Mark node
                    checkingNode.Visited = true;
                    yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);
                    
                    // Previous edge is this one
                    checkingNode.PrevEdge = edge;
                }
                // Line 10: End if statement
                yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);
            }
            // Line 11: End for-loop
            yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);
            currentNode.Traversed = true;

            lengthOfList = queue.Count.ToString(); // Pseudo code
            listVisual.DestroyCurrentNode(); // Node Representation
        }
        // Line 12: End while-loop
        yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_COLOR);

        IsTaskCompleted = true;
    }
    #endregion

    #region BFS User Test instructions
    public Dictionary<int, InstructionBase> UserTestInstructions(Node startNode)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0;

        // Line 1: Create emtpy list (queue)
        Queue<Node> queue = new Queue<Node>();
        instructions.Add(instNr++, new InstructionBase(UtilGraph.EMPTY_QUEUE_INST, instNr));

        // Line 2: Enqueue first node
        queue.Enqueue(startNode);
        instructions.Add(instNr++, new TraverseInstruction(UtilGraph.ENQUEUE_NODE_INST, instNr, 0, startNode, false));

        // Line 3: Mark node as visited
        startNode.Visited = true;
        instructions.Add(instNr++, new TraverseInstruction(UtilGraph.MARK_VISITED_INST, instNr, 0, startNode, true));

        while (queue.Count > 0)
        {
            // Line 4: While loop
            instructions.Add(instNr++, new InstructionLoop(UtilGraph.WHILE_LIST_NOT_EMPTY_INST, instNr, queue.Count));

            // Line 5: Dequeue node
            Node currentNode = queue.Dequeue();
            instructions.Add(instNr++, new TraverseInstruction(UtilGraph.DEQUEUE_NODE_INST, instNr, currentNode, true));

            for (int i = 0; i < currentNode.Edges.Count; i++)
            {
                // Line 6: For-loop update
                instructions.Add(instNr++, new InstructionLoop(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr, i, currentNode.Edges.Count, Util.NO_INDEX_VALUE));

                Edge edge = currentNode.Edges[i];
                Node checkingNode = edge.OtherNodeConnected(currentNode);

                // Line 7: check neighbor
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.IF_NOT_VISITED_INST, instNr, checkingNode, !checkingNode.Visited)); // check if correct ***
                // Check if node has already been traversed or already is marked
                if (!checkingNode.Visited)
                {
                    // Line 8: Enqueue node
                    queue.Enqueue(checkingNode);
                    instructions.Add(instNr++, new TraverseInstruction(UtilGraph.ENQUEUE_NODE_INST, instNr, checkingNode, false));

                    // LINE 9: Mark node
                    checkingNode.Visited = true;
                    instructions.Add(instNr++, new TraverseInstruction(UtilGraph.MARK_VISITED_INST, instNr, 1, checkingNode, true));
                }
                instructions.Add(instNr++, new InstructionBase(UtilGraph.END_IF_INST, instNr));
            }
            instructions.Add(instNr++, new InstructionBase(UtilGraph.END_FOR_LOOP_INST, instNr));
            currentNode.Traversed = true;
        }
        instructions.Add(instNr++, new InstructionBase(UtilGraph.END_WHILE_INST, instNr));
        return instructions;
    }
    #endregion

    #region User Test Highlight Pseudocode
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotNode)
    {
        Debug.Log("Starting highlighting pseudocode");

        // Gather information from instruction
        if (gotNode)
            node1 = ((TraverseInstruction)instruction).Node;

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        switch (instruction.Instruction)
        {
            case UtilGraph.EMPTY_QUEUE_INST: lineOfCode = 1; break;
            case UtilGraph.ENQUEUE_NODE_INST:
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                if (i == 0)
                    lineOfCode = 2;
                else
                    lineOfCode = 8;
                break;

            case UtilGraph.MARK_VISITED_INST:
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                if (i == 0)
                    lineOfCode = 3;
                else
                    lineOfCode = 9;
                break;

            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST:
                lineOfCode = 4;
                lengthOfList = i.ToString();
                break;

            case UtilGraph.DEQUEUE_NODE_INST:
                lineOfCode = 5;
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                break;

            case UtilGraph.FOR_ALL_NEIGHBORS_INST:
                lineOfCode = 6;
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 1);
                break;

            case UtilGraph.IF_NOT_VISITED_INST:
                lineOfCode = 7;
                SetNodePseudoCode(((TraverseInstruction)instruction).Node, 2);
                break;

            case UtilGraph.END_IF_INST: lineOfCode = 10; break;
            case UtilGraph.END_FOR_LOOP_INST: lineOfCode = 11; break;
            case UtilGraph.END_WHILE_INST: lineOfCode = 12; break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode), Util.HIGHLIGHT_COLOR);

        yield return demoStepDuration;
        graphMain.BeginnerWait = false;
        Debug.Log("Pseudocode highlighted!");
    }
    #endregion
}
