using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : GraphAlgorithm, IShortestPath {


    public override string AlgorithmName
    {
        get { return UtilGraph.DIJKSTRA; }
    }

    public override string CollectLine(int lineNr)
    {
        switch (lineNr)
        {
            case 0: return "Dijkstra(G, s):";
            case 1: return "    Set all vertices of G to infinity";
            case 2: return "    Q = []    // Empty priority queue";
            case 3: return "    Q.Add(s)";
            case 4: return "    s.Dist = 0";
            case 5: return "    while (len(Q) > 0):";
            case 6: return "        v = Q.ExtractMin()";
            case 7: return "        v.traversed = true"; //
            case 8: return "        for neighbors w of v in Graph G:";
            case 9: return "            if (v.Dist + edge(v, w).cost < w.Dist):";
            case 10: return "               w.Dist = v.Dist + edge(v, w).cost";
            case 11: return "               w.Prev = v";
            case 12: return "               Q.PriorityAdd(w)";
            case 13: return "           end if";
            case 14: return "       end for";
            case 15: return "   end while";
            default: return "lineNr " + lineNr + " not found!";
        }
    }

    private string PseudoCode(int lineNr, int i)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode += "Dijkstra(G, s):"; break;
            case 1: lineOfCode += "   Set all vertices of G to inifity"; break;
            case 2: lineOfCode += string.Format("   Q = []  // Empty priority queue"); break;
            case 3: lineOfCode += string.Format("   Q.Add({0})", node1.ConvertIDToAlphabet()); break;
            case 4: lineOfCode += string.Format("   {0}.Dist = 0", node1.ConvertIDToAlphabet()); break;
            case 5: lineOfCode += string.Format("   while ({0} > 0):", i); break;
            case 6: lineOfCode += string.Format("       v = {0}", node1.ConvertIDToAlphabet()); break;
            case 7: lineOfCode += string.Format("       {0}.traversed = true", node1.ConvertIDToAlphabet()); break;
            case 8: lineOfCode += string.Format("       for i={0} to {1}:", i, node1.Edges.Count - 1); break;
            case 9: lineOfCode += string.Format("           if (v.Dist={0} + edge(v, w).Cost={1} < w.Dist={2}):", node1.TotalCost, edge.Cost, UtilGraph.ConvertIfInf(node2.TotalCost.ToString())); break;
            case 10: lineOfCode += string.Format("              {0}.Dist = {1}", node2.ConvertIDToAlphabet(), (node1.TotalCost + edge.Cost)); break;
            case 11: lineOfCode += string.Format("              {0}.Prev = {1}", node2.ConvertIDToAlphabet(), node1.ConvertIDToAlphabet()); break;
            case 12: lineOfCode += string.Format("              Q.PriorityAdd({0})", node2.ConvertIDToAlphabet()); break;
            case 13: lineOfCode += string.Format("          end if"); break;
            case 14: lineOfCode += string.Format("      end for"); break;
            case 15: lineOfCode += string.Format("  end while"); break;
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
        return 15;
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
    }

    #region Dijkstra Demo
    public IEnumerator Demo(Node startNode, Node endNode)
    {
        // Line 1: Set all vertices of G to inifity
        yield return HighlightPseudoCode(PseudoCode(1, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 2: Create (priority) list
        List<Node> list = new List<Node>();
        yield return HighlightPseudoCode(PseudoCode(2, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 3: Add starting node and set its cost to 0
        node1 = startNode; // Pseudocode
        list.Add(startNode);
        yield return HighlightPseudoCode(PseudoCode(3, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Line 4: Set total cost (Dist) of start node to 0
        startNode.TotalCost = 0;
        yield return HighlightPseudoCode(PseudoCode(4, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        while (list.Count > 0)
        {
            // Line 5: Update while-loop
            yield return HighlightPseudoCode(PseudoCode(5, list.Count), Util.HIGHLIGHT_COLOR);

            // Line 6: Pull out the element with lowest cost
            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            node1 = currentNode; // Pseudocode
            yield return HighlightPseudoCode(PseudoCode(6, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

            // Line 7: Mark traversed (remove?)
            currentNode.Traversed = true; /// ????
            yield return HighlightPseudoCode(PseudoCode(7, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

            // Line ???
            currentNode.Visited = true;
            yield return new WaitForSeconds(seconds);

            // Stop search if end node found
            if (currentNode == endNode)
                break;


            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;

            // Line 8: Update for-loop (if no nodes connected)
            if (edges.Count == 0)
                yield return HighlightPseudoCode(PseudoCode(8, 0), Util.HIGHLIGHT_COLOR);

            for (int i=0; i < edges.Count; i++)
            {
                // Line 8: Update for-loop
                yield return HighlightPseudoCode(PseudoCode(8, i), Util.HIGHLIGHT_COLOR);

                // Checking edge
                Edge currentEdge = edges[i];
                edge = currentEdge; // Pseudocode
                currentEdge.CurrentColor = UtilGraph.VISITED;
                yield return new WaitForSeconds(seconds);

                // Checking node on the other side of the edge
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                node2 = connectedNode; // Pseudocode
                connectedNode.Visited = true;

                // Cost between nodes
                int costFromCurrentToConnected = currentNode.TotalCost + currentEdge.Cost;

                // Line 9: If statement
                yield return HighlightPseudoCode(PseudoCode(9, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

                // Update cost of connected node
                if (costFromCurrentToConnected < connectedNode.TotalCost)
                {
                    // Line 10: Update total cost (Dist) of connected node (w)
                    connectedNode.TotalCost = costFromCurrentToConnected;
                    yield return HighlightPseudoCode(PseudoCode(10, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

                    // Line 11: Update prev edge (Prev) of connected node (w)
                    connectedNode.PrevEdge = currentEdge;
                    yield return HighlightPseudoCode(PseudoCode(11, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

                    //list.Add(connectedNode);
                }

                if (!connectedNode.Traversed && !list.Contains(connectedNode))
                    list.Add(connectedNode);

                // Sort list (inverted)
                list.Sort();

                // Line 13: End if
                yield return HighlightPseudoCode(PseudoCode(13, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);
            }
            // Line 14: End for-loop
            yield return HighlightPseudoCode(PseudoCode(14, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);
        }
        // Line 15: End while-loop
        yield return HighlightPseudoCode(PseudoCode(14, Util.NO_VALUE), Util.HIGHLIGHT_COLOR);

        // Start backtracking from end node back to start node
        while (true)
        {
            // Change color of node
            endNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;

            // Change color of edge leading to previous node
            Edge backtrackEdge = endNode.PrevEdge;
            backtrackEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;

            // Set "next" node
            endNode = backtrackEdge.OtherNodeConnected(endNode);
            yield return new WaitForSeconds(seconds);


            if (endNode.PrevEdge == null)
            {
                endNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                break;
            }
        }
        IsTaskCompleted = true;
    }
    #endregion

    #region User Test Highlight Pseudocode
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotNode)
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
