using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : GraphManager
{
    protected override IEnumerator CreateNodes(int numberOfNodes)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator CreateEdges(Node[] nodes, string mode)
    {
        throw new System.NotImplementedException();
    }

    protected override int GetMaxNumberOfNodes()
    {
        return 1;
    }

}
