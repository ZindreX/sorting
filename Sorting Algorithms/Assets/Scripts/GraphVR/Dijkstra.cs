using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : GraphAlgorithm, IShortestPath {


    public IEnumerator Demo(Node startNode, Node endNode)
    {
        // Create list
        List<Node> list = new List<Node>();

        // Add starting node and set its cost to 0
        list.Add(startNode);
        startNode.TotalCost = 0;

        Debug.Log("Starting Dijkstra shortest path demo in 3 seconds");
        yield return new WaitForSeconds(3f);


        Debug.Log("Starting Dijkstra shortest path demo");
        while (list.Count > 0)
        {
            // Pull out the element with lowest cost
            Node currentNode = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            currentNode.Traversed = true; /// ????

            //currentNode.CurrentColor = UtilGraph.TRAVERSE_COLOR;
            yield return new WaitForSeconds(seconds);
            currentNode.Marked = true;
            //currentNode.CurrentColor = UtilGraph.MARKED;
            

            // Check all nodes connected with current node
            List<Edge> edges = currentNode.Edges;
            for (int edge=0; edge < edges.Count; edge++)
            {
                // Checking edge
                Edge currentEdge = edges[edge];
                currentEdge.CurrentColor = UtilGraph.MARKED;
                yield return new WaitForSeconds(seconds);

                // Checking node on the other side of the edge
                Node connectedNode = currentEdge.OtherNodeConnected(currentNode);
                //connectedNode.CurrentColor = UtilGraph.MARKED;
                connectedNode.Marked = true;


                // Cost between nodes
                int costFromCurrentToConnected = currentNode.TotalCost + currentEdge.Cost;
                //Debug.Log("Cost between " + currentNode.NodeType + " --> " + connectedNode.NodeType + ": " + costFromCurrentToConnected);
                //Debug.Log(costFromCurrentToConnected + " < " + connectedNode.TotalCost);

                // Update cost of connected node
                if (costFromCurrentToConnected < connectedNode.TotalCost)
                {
                    connectedNode.TotalCost = costFromCurrentToConnected;
                    connectedNode.PrevEdge = currentEdge;
                    yield return new WaitForSeconds(seconds);
                }

                if (!connectedNode.Traversed && !list.Contains(connectedNode))
                    list.Add(connectedNode);

                // Sort list (inverted)
                list.Sort();
            }
        }
        Debug.Log("Dijkstra shortest path demo completed");

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
        Debug.Log("Shortest path from start node to last node marked");
    }

}
