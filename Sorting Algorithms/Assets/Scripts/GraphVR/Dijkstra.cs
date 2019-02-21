using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : GraphAlgorithm, IShortestPath {


    public override string AlgorithmName
    {
        get { return Util.DIJKSTRA; }
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0:  lineOfCode += "Dijkstra(G, s):"; break;
            case 1:  lineOfCode += "   Set all vertices of G to inifity"; break;
            case 2:  lineOfCode += string.Format("   list = []  // Priority queue"); break;
            case 3:  lineOfCode += string.Format("   list.Add({0})", node1Alpha); break;
            case 4:  lineOfCode += string.Format("   {0}.Dist = 0", node1Alpha); break;
            case 5:  lineOfCode += string.Format("   while ({0} > 0):", lengthOfList); break;
            case 6:  lineOfCode += string.Format("       {0} <- list.PriorityRemove()", node1Alpha); break;
            case 7:  lineOfCode += string.Format("       for i={0} to {1}:", i, numberOfEdges); break;
            case 8:  lineOfCode += string.Format("           if ({0}.Dist={1} + edge({0}, {2}).Cost={3} < {2}.Dist={4}):", node1Alpha, node1Dist, node2Alpha, edgeCost, UtilGraph.ConvertIfInf(node2Dist.ToString())); break;
            case 9:  lineOfCode += string.Format("              {0}.Dist = {1}", node2Alpha, (node1Dist + edgeCost)); break;
            case 10: lineOfCode += string.Format("              {0}.Prev = {1}", node2Alpha, node1Alpha); break;
            case 11: lineOfCode += string.Format("              list.PriorityAdd({0})", node2Alpha); break;
            case 12: lineOfCode += string.Format("          end if"); break;
            case 13: lineOfCode += string.Format("      end for"); break;
            case 14: lineOfCode += string.Format("  end while"); break;
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
        return 14;
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
    }

    // Dijkstra on Tree: must start at 0, or atleast have end node in the same subtree underneath start node
    #region Dijkstra Demo
    public IEnumerator Demo(Node startNode, Node endNode)
    {
        listVisual.SearchingForID = endNode.NodeAlphaID;
        // Line 1: Set all vertices of G to inifity
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        // Line 2: Create (priority) list
        List<Node> list = new List<Node>();
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

        // Line 3: Add starting node and set its cost to 0
        list.Add(startNode);
        listVisual.PriorityAdd(startNode.NodeAlphaID, 0, 0); // List visual
        SetNodePseudoCode(startNode, 1, 0); // PseudoCode (line 3+4)
        yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

        // Line 4: Set total cost (Dist) of start node to 0
        yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);

        lengthOfList = "1";
        while (list.Count > 0)
        {
            // Line 5: Update while-loop
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

            // Line -: Pull out the element with lowest cost
            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            listVisual.RemoveAndMoveElementOut(); // List visual
            SetNodePseudoCode(currentNode, 1, currentNode.Dist); // PseudoCode

            // Line 6: Mark traversed (remove?)
            currentNode.Traversed = true; /// ????
            yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

            // Line ???
            currentNode.Visited = true;
            yield return new WaitForSeconds(seconds);

            // Stop search if end node found
            if (currentNode == endNode)
                break;

            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;
            numberOfEdges = edges.Count; // Pseudocode

            // Line 7: Update for-loop (if no nodes connected)
            if (numberOfEdges == 0)
            {
                i = 0;
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);
            }

            for (int i=0; i < numberOfEdges; i++)
            {
                // Line 7: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

                // Checking edge
                Edge currentEdge = edges[i];
                SetEdge(currentEdge); // Pseudocode
                currentEdge.CurrentColor = UtilGraph.VISITED_COLOR;
                yield return new WaitForSeconds(seconds);

                // Checking node on the other side of the edge
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                SetNodePseudoCode(connectedNode, 2, connectedNode.Dist); // PseudoCode
                connectedNode.Visited = true;

                // Cost between nodes
                int newDistNode2 = node1Dist + edgeCost;

                // Line 8: If statement
                yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                // Update cost of connected node
                if (newDistNode2 < connectedNode.Dist)
                {
                    // Line 9: Update total cost (Dist) of connected node (w)
                    connectedNode.Dist = newDistNode2;
                    yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

                    // Line 10: Update prev edge (Prev) of connected node (w)
                    connectedNode.PrevEdge = currentEdge;
                    yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);

                    //list.Add(connectedNode);
                }

                if (!connectedNode.Traversed && !list.Contains(connectedNode))
                {
                    list.Add(connectedNode);
                    // Sort list (inverted)
                    list.Sort();
                    // Line 11: Add to list
                    yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);

                    // List visual
                    int index = (list.Count - 1 ) - list.IndexOf(connectedNode); // OBS: list is inverted (removes the last element instead of index 0)
                    listVisual.PriorityAdd(connectedNode.NodeAlphaID, connectedNode.Dist, index);
                }

                // Line 12: End if
                yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_COLOR);
            }
            // Line 13: End for-loop
            yield return HighlightPseudoCode(CollectLine(13), Util.HIGHLIGHT_COLOR);

            lengthOfList = list.Count.ToString(); // PseudoCode
            listVisual.DestroyOutElement(); // List visual
        }
        // Line 14: End while-loop
        yield return HighlightPseudoCode(CollectLine(14), Util.HIGHLIGHT_COLOR);

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
