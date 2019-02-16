using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITraverse {

    IEnumerator Demo(Node startNode);
    Dictionary<int, InstructionBase> UserTestInstructions(Node startNode);

}
