using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : GraphAlgorithm, IShortestPath {

    private bool objectiveFound = false;

    public override string AlgorithmName
    {
        get { return Util.DIJKSTRA; }
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0:  lineOfCode += string.Format("Dijkstra({0}, {1}):", graphStructure, node1Alpha); break;
            case 1:  lineOfCode += "   Set all vertices of Graph to infinity"; break;
            case 2:  lineOfCode += string.Format("   list = [ ]         // Priority queue"); break;
            case 3:  lineOfCode += string.Format("   list.Add({0})", node1Alpha); break;
            case 4:  lineOfCode += string.Format("   {0}.Dist = 0", node1Alpha); break;
            case 5:  lineOfCode += string.Format("   while ({0} > 0):", lengthOfList); break;
            case 6:  lineOfCode += string.Format("       {0} <- list.PriorityRemove()", node1Alpha); break;
            case 7:  lineOfCode += string.Format("       for all neighbors of {0} in Graph:", node1Alpha); break;
            case 8:  lineOfCode += string.Format("           if ({0}.Dist={1} + edge({0}, {2}).Cost={3} < {2}.Dist={4}):", node1Alpha, node1Dist, node2Alpha, edgeCost, UtilGraph.ConvertIfInf(node2Dist)); break;
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
    public IEnumerator ShortestPathDemo(Node startNode, Node endNode)
    {
        // Line 1: Set all vertices of G to inifity
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
            // Line 5: Update while-loop
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

            //
            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            SetNodePseudoCode(currentNode, 1); // PseudoCode
            graphMain.UpdateListVisual(UtilGraph.REMOVE_CURRENT_NODE, null, Util.NO_VALUE); // listVisual.RemoveCurrentNode(); // List visual
            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;

            // Line 6: Remove element with lowest distance
            yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

            // Stop search if end node found and we dont want shortest path to all - stop when first visited instead? (not always global optimal)
            if (!shortestPathOnToAll && currentNode == endNode)
                objectiveFound = true;

            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;
            numberOfEdges = edges.Count; // Pseudocode

            // Line 7: Update for-loop (if no nodes connected)
            if (numberOfEdges == 0)
            {
                i = 0; // not used anymore
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);
            }

            for (int i=0; i < numberOfEdges; i++)
            {
                // Line 7: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

                // Checking edge
                Edge currentEdge = edges[i];

                // Dont check edge we came from
                if (currentEdge == currentNode.PrevEdge)
                    continue;

                SetEdge(currentEdge); // Pseudocode
                yield return demoStepDuration;

                // Checking node on the other side of the edge
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                if (connectedNode == null)
                {
                    currentEdge.CurrentColor = UtilGraph.STANDARD_COLOR;
                    continue;
                }

                SetNodePseudoCode(connectedNode, 2); // PseudoCode

                if (!connectedNode.Visited)
                    connectedNode.Visited = true;

                // Cost between nodes
                int currentDistAndEdgeCost = node1Dist + edgeCost;

                // Line 8: If statement
                yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                // Update cost of connected node
                if (currentDistAndEdgeCost < connectedNode.Dist)
                {
                    // Line 9: Update total cost (Dist) of connected node (w)
                    connectedNode.Dist = currentDistAndEdgeCost;
                    yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

                    // Line 10: Update prev edge (Prev) of connected node (w)
                    connectedNode.PrevEdge = currentEdge;
                    yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);

                    if (!connectedNode.Traversed) // && !list.Contains(connectedNode)) // was outside if
                    {
                        if (!list.Contains(connectedNode))
                           list.Add(connectedNode);

                        // Sort list such that the lowest distance node gets out first
                        list.Sort();

                        // List visual
                        int index = list.IndexOf(connectedNode); //(list.Count - 1 ) - list.IndexOf(connectedNode); // OBS: list is inverted (removes the last element instead of index 0)
                        int currentNodeRepIndex = graphMain.ListVisual.ListIndexOf(connectedNode); // Create method/action code ???

                        if (!graphMain.CheckListVisual(UtilGraph.HAS_NODE_REPRESENTATION, connectedNode)) // listVisual.HasNodeRepresentation(connectedNode))
                            graphMain.UpdateListVisual(UtilGraph.PRIORITY_ADD_NODE, connectedNode, index); // listVisual.PriorityAdd(connectedNode, index); // Node representation
                        else
                            yield return graphMain.ListVisual.UpdateValueAndPositionOf(connectedNode, index); // Create method/action code ???


                        //Debug.Log(listVisual.PrintList());

                        // Line 11: Add to list
                        yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);
                    }
                }

                currentEdge.CurrentColor = UtilGraph.VISITED_COLOR;

                // Line 12: End if
                yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_COLOR);
            }
            // Line 13: End for-loop
            yield return HighlightPseudoCode(CollectLine(13), Util.HIGHLIGHT_COLOR);

            currentNode.Traversed = true;

            lengthOfList = list.Count.ToString(); // PseudoCode
            graphMain.UpdateListVisual(UtilGraph.DESTROY_CURRENT_NODE, null, Util.NO_VALUE); // listVisual.DestroyCurrentNode(); // Node Representation
        }
        // Line 14: End while-loop
        yield return HighlightPseudoCode(CollectLine(14), Util.HIGHLIGHT_COLOR);
        IsTaskCompleted = true;
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

    #region User Test Highlight Pseudocode
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotNode)
    {
        throw new System.NotImplementedException();
    }
    #endregion



    // *** Debugging ***

    private string PrintList(List<Node> list)
    {
        string result = "Dijkstra:                    ";
        for (int i=0; i < list.Count; i++)
        {
            result += "[" + list[i].NodeAlphaID + "," + list[i].Dist + "], ";
        }
        return result;
    }

    public Dictionary<int, InstructionBase> ShortestPathUserTestInstructions(Node startNode, Node endNode)
    {
        throw new System.NotImplementedException();
    }
}
