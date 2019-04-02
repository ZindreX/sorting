using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlgorithm {

    string AlgorithmName { get; }
    void ResetSetup();

    IEnumerator Demo(GameObject[] list);
    Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);
    IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement);
}
