using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : TraverseAlgorithm {

    public static IEnumerator Demo(Node node)
    {
        Debug.Log("Starting BFS demo in 3 seconds");

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(node);
        node.Marked = true;
        node.CurrentColor = UtilGraph.MARKED;
        yield return new WaitForSeconds(3f);

        Debug.Log("Starting BFS demo");
        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(seconds);
            currentNode.CurrentColor = UtilGraph.STANDARD_COLOR;

            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                Edge edge = currentNode.Edges[i];
                Node checkingNode = edge.OtherNodeConnected(currentNode);
                edge.CurrentColor = UtilGraph.TRAVERSED_COLOR;                  // edge color???

                // Check if node has already been traversed or already is marked
                if (!checkingNode.Traversed && !checkingNode.Marked)
                {
                    queue.Enqueue(checkingNode);
                    checkingNode.Marked = true;
                    checkingNode.CurrentColor = UtilGraph.MARKED;
                }
            }

            currentNode.Traversed = true;
            currentNode.CurrentColor = UtilGraph.TRAVERSED_COLOR;
            yield return new WaitForSeconds(seconds);
        }

        Debug.Log("BFS demo completed");
    }



}
