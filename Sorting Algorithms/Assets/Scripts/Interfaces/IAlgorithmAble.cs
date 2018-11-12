using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlgorithmAble {

    string AlgorithmName { get; }
    void ResetSetup();

    IEnumerator Tutorial(GameObject[] list);
    Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);

}
