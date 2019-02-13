using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour, ITraverse {

    public IEnumerator Demo(Node node)
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

            if (currentNode.MarkedFrom != null)
            {
                currentNode.MarkedFrom.CurrentColor = UtilGraph.TRAVERSED_COLOR;
                yield return new WaitForSeconds(UtilGraph.seconds / 2);
            }

            currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(UtilGraph.seconds);

            for (int i=0; i < currentNode.Edges.Count; i++)
            {
                Edge edge = currentNode.Edges[i];
                Node checkingNode = edge.OtherNodeConnected(currentNode);

                // Check if node has already been traversed or already is marked
                if (!checkingNode.Traversed && !checkingNode.Marked)
                {
                    // Add to queue
                    queue.Enqueue(checkingNode);

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
            yield return new WaitForSeconds(UtilGraph.seconds);
        }

        Debug.Log("BFS demo completed");
    }



}
