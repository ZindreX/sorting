﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : GraphAlgorithm, ITraverse {

    public override string AlgorithmName
    {
        get { return Util.DFS; }
    }

    public override string CollectLine(int lineNr)
    {
        string codeLine = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        
        switch (lineNr)
        {
            case 0: codeLine += string.Format("DFS({0}, {1}):", graphStructure, node1Alpha); break;
            case 1: codeLine += "    stack = [ ]"; break;
            case 2: codeLine += string.Format("    stack.Push({0})", node1Alpha); break;
            case 3: codeLine += string.Format("    {0}.visited = true", node1Alpha); break;
            case 4: codeLine += string.Format("    while ({0} > 0):", lengthOfList); break;
            case 5: codeLine += string.Format("        {0} <- stack.Pop()", node1Alpha); break;
            case 6: codeLine += string.Format("        for all neighbors of {0} in Graph:", node1Alpha); break;
            case 7: codeLine += string.Format("            if (!{0}.visited):", node2Alpha); break;
            case 8: codeLine += string.Format("                stack.Push({0})", node2Alpha); break;
            case 9: codeLine += string.Format("                {0}.visited = true", node2Alpha); break;
            case 10: codeLine += "           end if"; break;
            case 11: codeLine += "       end for"; break;
            case 12: codeLine += "   end while"; break;
            default: return "lineNr " + lineNr + " not found!";
        }
        return codeLine;
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
        // Line 0: Set graph/start node
        SetNodePseudoCode(startNode, 1);
        yield return HighlightPseudoCode(CollectLine(0), Util.BLACKBOARD_TEXT_COLOR);

        // Line 1: Create an empty list (stack)
        Stack<Node> stack = new Stack<Node>();
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        // Line 2: Push start node
        stack.Push(startNode);
        listVisual.AddListObject(startNode); // Node Representation
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

        // Line 3: Mark as visited
        startNode.Visited = true;
        yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

        lengthOfList = "1";
        while (stack.Count > 0)
        {
            // Line 4: Update while-loop
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);

            // Line 5: Pop node from stack
            Node currentNode = stack.Pop();
            listVisual.RemoveCurrentNode(); // Node Representation
            SetNodePseudoCode(currentNode, 1);
            yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

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
            {
                i = 0;
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);
            }

            // Go through each edge connected to current node
            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                // Line 6: Update for-loop
                this.i = i;
                yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

                // Fix index according to chosen behavior (e.g. tree: visit left or right child first)
                //int visitNode = i;
                //if (visitLeftFirst)
                //    visitNode = currentNode.Edges.Count - 1 - i;

                Edge edge = currentNode.Edges[i]; // visitNode];                
                Node checkingNode = edge.OtherNodeConnected(currentNode);
                SetNodePseudoCode(checkingNode, 2); // Pseudocode

                // Mark edge
                edge.CurrentColor = UtilGraph.VISITED_COLOR;

                // Line 7: If statement (condition)
                yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

                if (!checkingNode.Visited)
                {
                    // Line 8: Push node on top of stack
                    stack.Push(checkingNode);
                    listVisual.AddListObject(checkingNode); // Node Representation
                    yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

                    // Line 9: Mark node
                    checkingNode.Visited = true;

                    // Previous edge
                    checkingNode.PrevEdge = edge;

                    yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);
                }
                // Line 10: End if statement
                yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);                    

            }
            // Line 11: End for-loop
            yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);                    

            currentNode.Traversed = true;
            lengthOfList = stack.Count.ToString(); // Pseudocode stack size

            listVisual.DestroyOutElement(); // Node Representation
        }
        // Line 12: End while-loop
        yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_COLOR);                    

        IsTaskCompleted = true;
    }
    #endregion

    #region DFS Demo recursive
    public IEnumerator DemoRecursive(Node node)
    {
        if (node.PrevEdge != null)
            node.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;

        SetNodePseudoCode(node, 1);
        //listVisual.RemoveAndMoveElementOut(); // List visual
        yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

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
