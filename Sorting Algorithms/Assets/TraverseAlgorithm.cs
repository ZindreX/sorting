using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseAlgorithm : MonoBehaviour {

    protected static float seconds = 0.5f;



    protected static IEnumerator MarkNode(Node node)
    {
        node.CurrentColor = UtilGraph.TRAVERSE_COLOR;
        yield return new WaitForSeconds(seconds);
        node.CurrentColor = UtilGraph.STANDARD_COLOR;
    }


}
