using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlgorithm {

    string AlgorithmName { get; }
    void ResetSetup();

    IEnumerator Tutorial(GameObject[] list);
    Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);

    void ExecuteStepByStepOrder(InstructionBase instruction, bool gotElement, bool increment);
    IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement);
}
