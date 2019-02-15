﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : GraphAlgorithm, ITraverse {

    private bool visitLeftFirst;

    public bool VisistLeftFirst
    {
        set { visitLeftFirst = value; }
    }

    public IEnumerator Demo(Node node)
    {
        Debug.Log("Starting DFS demo in 3 seconds");

        Stack<Node> stack = new Stack<Node>();
        stack.Push(node);
        node.Marked = true;
        node.CurrentColor = UtilGraph.MARKED;

        yield return new WaitForSeconds(3f);

        Debug.Log("Starting DFS demo");
        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();

            if (currentNode.MarkedFrom != null)
            {
                currentNode.MarkedFrom.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return new WaitForSeconds(seconds / 2);
            }

            node.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(seconds);

            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                int visitNode = i;
                if (visitLeftFirst)
                    visitNode = currentNode.Edges.Count - 1 - i;

                Edge edge = currentNode.Edges[visitNode];
                Node checkingNode = edge.OtherNodeConnected(currentNode);

                if (!checkingNode.Traversed && !checkingNode.Marked) // rather check if checkingNode is in stack? (drop marked?)
                {
                    // Put ontop of stack
                    stack.Push(checkingNode);

                    // Mark node
                    checkingNode.Marked = true;
                    checkingNode.CurrentColor = UtilGraph.MARKED;

                    // Mark edge
                    edge.CurrentColor = UtilGraph.MARKED;
                    checkingNode.MarkedFrom = edge;
                }
            }

            currentNode.Traversed = true;
            currentNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;
            yield return new WaitForSeconds(seconds);
        }
    }

    public List<int> VisitNodeOrder(Node startNode)
    {
        throw new System.NotImplementedException();
    }

}