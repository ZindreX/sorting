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
        switch (lineNr)
        {
            case 0: return "BFS(G, s):";
            case 1: return "    Q = []    // Empty queue";
            case 2: return "    Q.enqueue(s)";
            case 3: return "    s.visited = true";
            case 4: return "    while (Q.Count > 0):";
            case 5: return "        v = Q.dequeue()";
            case 6: return "        for all neighbors w of v in Graph G:";
            case 7: return "            if (!w.visited):";
            case 8: return "                Q.enqueue(w)";
            case 9: return "                w.visited = true";
            case 10: return "           end if";
            case 11: return "       end for";
            case 12: return "   end while";
            default: return "lineNr " + lineNr + " not found!";
        }
    }

    private string PseudoCode(int lineNr, int i)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode += "BFS(G, s):"; break;
            case 1: lineOfCode += string.Format("   Q = []"); break;
            case 2: lineOfCode += string.Format("   Q.enqueue({0})", node1.ConvertIDToAlphabet()); break;
            case 3: lineOfCode += string.Format("   {0}.visited = true", node1.ConvertIDToAlphabet()); break;
            case 4: lineOfCode += string.Format("   while ({0} > 0):", i); break;
            case 5: lineOfCode += string.Format("       v = {0}", node1.ConvertIDToAlphabet()); break;
            case 6: lineOfCode += string.Format("       for i={0} to {1}:", i, node1.Edges.Count-1); break;
            case 7: lineOfCode += string.Format("           if (!v.neighbors[{1}].visited):", node1.ConvertIDToAlphabet(), i); break;
            case 8: lineOfCode += string.Format("               Q.enqueue({0})", node1.ConvertIDToAlphabet()); break;
            case 9: lineOfCode += string.Format("               {0}.visited = true", node1.ConvertIDToAlphabet()); break;
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
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
    }

    #region BFS Demo
    public IEnumerator Demo(Node startNode)
    {
        // Line 1: Create empty list (queue)
        Queue<Node> queue = new Queue<Node>();
        yield return HighlightPseudoCode(PseudoCode(1, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 2: Add start node
        node1 = startNode; // Pseudocode
        queue.Enqueue(startNode);
        yield return HighlightPseudoCode(PseudoCode(2, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 3: Mark as visited
        startNode.Visited = true;
        yield return HighlightPseudoCode(PseudoCode(3, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        while (queue.Count > 0)
        {
            // Line 4: Update while loop
            yield return HighlightPseudoCode(PseudoCode(4, queue.Count), Util.HIGHLIGHT_COLOR);

            // Line 5: Dequeue node from queue
            Node currentNode = queue.Dequeue();
            node1 = currentNode; // Pseudocode
            yield return HighlightPseudoCode(PseudoCode(5, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

            if (currentNode.PrevEdge != null)
            {
                currentNode.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return new WaitForSeconds(seconds / 2);
            }

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(seconds);

            // Line 6: Update for-loop (leaf nodes)
            if (currentNode.Edges.Count == 0)
                yield return HighlightPseudoCode(PseudoCode(6, 0), Util.HIGHLIGHT_COLOR);

            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                node1 = currentNode; // Pseudocode

                // Line 6: Update for-loop
                yield return HighlightPseudoCode(PseudoCode(6, i), Util.HIGHLIGHT_COLOR);

                Edge edge = currentNode.Edges[i];

                // Mark edge
                edge.CurrentColor = UtilGraph.TRAVERSED_COLOR;

                Node checkingNode = edge.OtherNodeConnected(currentNode);
                node1 = checkingNode; // Pseudocode

                // Line 7: If condition
                yield return HighlightPseudoCode(PseudoCode(7, i), Util.HIGHLIGHT_COLOR);

                // Check if node has already been traversed or already is marked
                if (!checkingNode.Traversed && !checkingNode.Visited) // change?
                {
                    // Line 8: Add to queue
                    queue.Enqueue(checkingNode);
                    yield return HighlightPseudoCode(PseudoCode(8, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

                    // Line 9: Mark node
                    checkingNode.Visited = true;
                    yield return HighlightPseudoCode(PseudoCode(9, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);
                    
                    // Previous edge is this one
                    checkingNode.PrevEdge = edge;
                }
                // Line 10: End if statement
                yield return HighlightPseudoCode(PseudoCode(10, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);
            }
            // Line 11: End for-loop
            yield return HighlightPseudoCode(PseudoCode(11, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);
            currentNode.Traversed = true;

        }
        // Line 12: End while-loop
        yield return HighlightPseudoCode(PseudoCode(12, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        IsTaskCompleted = true;
    }
    #endregion

    #region BFS User Test instructions
    public Dictionary<int, InstructionBase> UserTestInstructions(Node startNode)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0;
        List<int> visitOrder = new List<int>();

        // Line 1: Create emtpy list (queue)
        Queue<Node> queue = new Queue<Node>();
        instructions.Add(instNr++, new InstructionBase(UtilGraph.EMPTY_QUEUE_INST, instNr));

        // Line 2: Enqueue first node
        queue.Enqueue(startNode);
        instructions.Add(instNr++, new TraverseInstruction(UtilGraph.ENQUEUE_NODE_INST, instNr, startNode, false));

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

            visitOrder.Add(currentNode.NodeID); // Extra (remove?)

            for (int i = 0; i < currentNode.Edges.Count; i++)
            {
                // Line 6: For-loop update
                instructions.Add(instNr++, new InstructionLoop(UtilGraph.FOR_ALL_NEIGHBORS_INST, instNr, i, currentNode.Edges.Count, Util.NO_INDEX_VALUE));

                Edge edge = currentNode.Edges[i];
                Node checkingNode = edge.OtherNodeConnected(currentNode);

                // Line 7: check neighbor
                instructions.Add(instNr++, new TraverseInstruction(UtilGraph.IF_NOT_VISITED_INST, instNr, checkingNode, !checkingNode.Visited)); // check if correct ***
                // Check if node has already been traversed or already is marked
                if (!checkingNode.Traversed && !checkingNode.Visited)
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
        // Gather information from instruction
        int i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE, k = UtilSort.NO_VALUE;

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
        int lineOfCode = UtilSort.NO_VALUE;
        switch (instruction.Instruction)
        {
            case UtilGraph.EMPTY_QUEUE_INST: lineOfCode = 1; break;
            case UtilGraph.ENQUEUE_NODE_INST:
                if (i == 0)
                    lineOfCode = 2;
                else
                    lineOfCode = 8;
                break;
            case UtilGraph.MARK_VISITED_INST:
                if (i == 0)
                    lineOfCode = 3;
                else
                    lineOfCode = 9;
                break;
            case UtilGraph.WHILE_LIST_NOT_EMPTY_INST: lineOfCode = 4; break;
            case UtilGraph.DEQUEUE_NODE_INST: lineOfCode = 5; break;
            case UtilGraph.FOR_ALL_NEIGHBORS_INST: lineOfCode = 6; break;
            case UtilGraph.IF_NOT_VISITED_INST: lineOfCode = 7; break;
            case UtilGraph.END_IF_INST: lineOfCode = 10; break;
            case UtilGraph.END_FOR_LOOP_INST: lineOfCode = 11; break;
            case UtilGraph.END_WHILE_INST: lineOfCode = 12; break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(PseudoCode(lineOfCode, i), Util.HIGHLIGHT_COLOR);

        yield return new WaitForSeconds(seconds);
        beginnerWait = false;
    }
    #endregion
}
