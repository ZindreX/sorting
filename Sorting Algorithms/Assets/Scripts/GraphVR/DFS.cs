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
        throw new System.NotImplementedException();
    }

    public override int FirstInstructionCodeLine()
    {
        return 0;
    }

    public override int FinalInstructionCodeLine()
    {
        return 0;
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
    public IEnumerator Demo(Node node)
    {
        Debug.Log("Starting DFS demo in 3 seconds");

        Stack<Node> stack = new Stack<Node>();
        stack.Push(node);
        node.Visited = true;
        node.CurrentColor = UtilGraph.VISITED;

        yield return new WaitForSeconds(3f);

        Debug.Log("Starting DFS demo");
        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();

            // 
            if (currentNode.PrevEdge != null)
            {
                currentNode.PrevEdge.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return new WaitForSeconds(seconds / 2);
            }

            node.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(seconds);


            // Go through each edge connected to current node
            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                // Fix index according to chosen behavior (e.g. tree: visit left or right child first)
                int visitNode = i;
                if (visitLeftFirst)
                    visitNode = currentNode.Edges.Count - 1 - i;

                Edge edge = currentNode.Edges[visitNode];
                Node checkingNode = edge.OtherNodeConnected(currentNode);

                if (!checkingNode.Traversed && !checkingNode.Visited) // rather check if checkingNode is in stack? (drop marked?)
                {
                    // Put ontop of stack
                    stack.Push(checkingNode);

                    // Mark node
                    checkingNode.Visited = true;
                    checkingNode.CurrentColor = UtilGraph.VISITED;

                    // Mark edge
                    edge.CurrentColor = UtilGraph.VISITED;
                    checkingNode.PrevEdge = edge;
                }
            }

            currentNode.Traversed = true;
            currentNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;
            yield return new WaitForSeconds(seconds);
        }
        Debug.Log("DFS demo completed");
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
