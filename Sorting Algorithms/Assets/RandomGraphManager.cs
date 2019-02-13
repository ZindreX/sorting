using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGraphManager : GraphManager {

    private int numberOfNodes, numberOfEdges;

    protected override void InitGraph(int[] graphStructure)
    {
        numberOfNodes = graphStructure[0];
        numberOfEdges = graphStructure[1];
    }

    public override int GetMaxNumberOfNodes()
    {
        return numberOfNodes;
    }

    public override int GetNumberOfEdges()
    {
        return numberOfEdges;
    }

    protected override void CreateNodes(string structure)
    {
        
    }

    protected override void CreateEdges(string mode)
    {
        throw new System.NotImplementedException();
    }

    public override Node GetNode(int a, int b)
    {
        throw new System.NotImplementedException();
    }
}
