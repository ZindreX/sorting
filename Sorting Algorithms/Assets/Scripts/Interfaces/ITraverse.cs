using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITraverse {

    IEnumerator TraverseDemo(Node startNode);
    Dictionary<int, InstructionBase> TraverseUserTestInstructions(Node startNode);

}
