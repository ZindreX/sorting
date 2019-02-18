using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : GraphAlgorithm, ITraverse {

    private bool visitLeftFirst;

    public override string AlgorithmName
    {
        get { return UtilGraph.DFS; }
    }

    public bool VisistLeftFirst
    {
        set { visitLeftFirst = value; }
    }

    public override string CollectLine(int lineNr)
    {
        switch (lineNr)
        {
            case 0: return "DFS(G, s):";
            case 1: return "    Q = []    // empty stack";
            case 2: return "    Q.Push(s)";
            case 3: return "    s.visited = true";
            case 4: return "    while (Q.Count > 0):";
            case 5: return "        v = Q.Pop()";
            case 6: return "        for all neighbors w of v in Graph G:";
            case 7: return "            if (!w.visited):";
            case 8: return "                Q.Push(w)";
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
            case 0: lineOfCode += "DFS(G, s):"; break;
            case 1: lineOfCode += string.Format("   Q = []"); break;
            case 2: lineOfCode += string.Format("   Q.Push({0})", node1.ConvertIDToAlphabet()); break;
            case 3: lineOfCode += string.Format("   {0}.visited = true", node1.ConvertIDToAlphabet()); break;
            case 4: lineOfCode += string.Format("   while ({0} > 0):", i); break;
            case 5: lineOfCode += string.Format("       v = {0}", node1.ConvertIDToAlphabet()); break;
            case 6: lineOfCode += string.Format("       for i={0} to {1}:", i, node1.Edges.Count - 1); break;
            case 7: lineOfCode += string.Format("           if (!v.neighbors[{1}].visited):", node1.ConvertIDToAlphabet(), i); break;
            case 8: lineOfCode += string.Format("               Q.Push({0})", node1.ConvertIDToAlphabet()); break;
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

    #region DFS Demo
    public IEnumerator Demo(Node startNode)
    {
        // Line 1: Create an empty list (stack)
        Stack<Node> stack = new Stack<Node>();
        yield return HighlightPseudoCode(PseudoCode(1, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 2: Push start node
        stack.Push(startNode);
        node1 = startNode; // Pseudocode
        yield return HighlightPseudoCode(PseudoCode(2, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 3: Mark as visited
        startNode.Visited = true;
        yield return HighlightPseudoCode(PseudoCode(3, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        while (stack.Count > 0)
        {
            // Line 4: Update while-loop
            yield return HighlightPseudoCode(PseudoCode(4, stack.Count), Util.HIGHLIGHT_COLOR);

            // Line 5: Pop node from stack
            Node currentNode = stack.Pop();
            node1 = currentNode; // Pseudocode
            yield return HighlightPseudoCode(PseudoCode(5, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

            // When visiting current node, change color of edge leading to this node
            if (currentNode.PrevEdge != null)
            {
                currentNode.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return new WaitForSeconds(seconds / 2);
            }

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR; //
            yield return new WaitForSeconds(seconds);

            // Line 6: Update for-loop (leaf nodes)
            if (currentNode.Edges.Count == 0)
                yield return HighlightPseudoCode(PseudoCode(6, 0), Util.HIGHLIGHT_COLOR);

            // Go through each edge connected to current node
            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                node1 = currentNode; // Pseudocode

                // Line 6: Update for-loop
                yield return HighlightPseudoCode(PseudoCode(6, i), Util.HIGHLIGHT_COLOR);

                // Fix index according to chosen behavior (e.g. tree: visit left or right child first)
                int visitNode = i;
                if (visitLeftFirst)
                    visitNode = currentNode.Edges.Count - 1 - i;

                Edge edge = currentNode.Edges[visitNode];                
                Node checkingNode = edge.OtherNodeConnected(currentNode);
                node1 = checkingNode; // Pseudocode

                // Mark edge
                edge.CurrentColor = UtilGraph.VISITED;

                // Line 7: If statement (condition)
                yield return HighlightPseudoCode(PseudoCode(7, i), Util.HIGHLIGHT_COLOR);

                if (!checkingNode.Traversed && !checkingNode.Visited) // rather check if checkingNode is in stack? (drop marked?)
                {
                    // Line 8: Push node on top of stack
                    stack.Push(checkingNode);
                    yield return HighlightPseudoCode(PseudoCode(8, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

                    // Line 9: Mark node
                    checkingNode.Visited = true;

                    // Previous edge
                    checkingNode.PrevEdge = edge;

                    yield return HighlightPseudoCode(PseudoCode(9, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);                    
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

    #region User Test instructions
    public Dictionary<int, InstructionBase> UserTestInstructions(Node startNode)
    {
        return null;
    }
    #endregion


    #region User Test Highlight Pseudocode
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotNode)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
