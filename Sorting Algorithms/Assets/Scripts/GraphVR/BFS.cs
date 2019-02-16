using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : GraphAlgorithm, ITraverse {


    public override string AlgorithmName
    {
        get { return UtilGraph.BFS; }
    }

    private string PseudoCode(int lineNr, int i, int j, bool increment)
    {
        //int n = GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements;
        switch (lineNr)
        {
            case 0: return "BFS(G, s):";
            case 1: return string.Format("  Q = []"); 
            case 2: return string.Format("  Q.enqueue({0})", i);
            case 3: return string.Format("  {0}.visited = true", i);
            case 4: return string.Format("  while (Q.Count > 0):");
            case 5: return string.Format("      {0} = Q.dequeue()", i);
            case 6: return string.Format("      for i={0} to {1}:", i, j);
            case 7: return string.Format("          if (!{0}.neighbors[{1}].visited):", i, j);
            case 8: return string.Format("              Q.enqueue({0})", i);
            case 9: return string.Format("              {0}.visited = true", i);
            case 10: return string.Format("          end if");
            case 11: return string.Format("      end for");
            case 12: return string.Format("  end while");
            default: return "lineNr " + lineNr + " not found!";
        }
    }

    public override string CollectLine(int lineNr)
    {
        string temp = PseudoCode(lineNr, 0, 0, true);
        //switch (lineNr)
        //{
        //    case 0: case 6: case 7: case 8: return temp;
        //    case 1: return temp.Replace(GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements.ToString(), "len( list )");
        //    case 2: return temp.Replace((GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements - 1).ToString(), "n-1");
        //    case 3: return temp.Replace((GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements - 1).ToString(), "n-i-1");
        //    case 4: case 5: return temp.Replace(UtilSort.INIT_STATE.ToString(), "list[ j ]").Replace((UtilSort.INIT_STATE - 1).ToString(), "list[ j + 1 ]");
        //    default: return "lineNr " + lineNr + " not found!";
        //}
        return temp;
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
    public IEnumerator Demo(Node node)
    {
        Debug.Log("Starting BFS demo in 3 seconds");

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(node);
        node.Visited = true;
        node.CurrentColor = UtilGraph.VISITED;
        yield return new WaitForSeconds(3f);

        Debug.Log("Starting BFS demo");
        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if (currentNode.VisitedFrom != null)
            {
                currentNode.VisitedFrom.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return new WaitForSeconds(seconds / 2);
            }

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(seconds);

            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                Edge edge = currentNode.Edges[i];
                Node checkingNode = edge.OtherNodeConnected(currentNode);

                // Check if node has already been traversed or already is marked
                if (!checkingNode.Traversed && !checkingNode.Visited) // change?
                {
                    // Add to queue
                    queue.Enqueue(checkingNode);

                    // Mark node
                    checkingNode.Visited = true;
                    checkingNode.CurrentColor = UtilGraph.VISITED;
                    
                    // Mark edge
                    edge.CurrentColor = UtilGraph.VISITED;
                    checkingNode.VisitedFrom = edge;
                }
            }

            currentNode.Traversed = true;
            currentNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;
            yield return new WaitForSeconds(seconds);
        }

        Debug.Log("BFS demo completed");
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
            node = ((TraverseInstruction)instruction).Node;

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
        pseudoCodeViewer.SetCodeLine(lineOfCode, PseudoCode(lineOfCode, i, j, true), Util.HIGHLIGHT_COLOR);

        yield return new WaitForSeconds(seconds);
        beginnerWait = false;
    }
    #endregion
}
