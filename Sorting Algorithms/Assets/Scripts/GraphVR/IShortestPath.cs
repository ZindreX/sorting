using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShortestPath {

    IEnumerator ShortestPathDemo(Node startNode, Node endNode);
    Dictionary<int, InstructionBase> ShortestPathUserTestInstructions(Node startNode, Node endNode);

}
