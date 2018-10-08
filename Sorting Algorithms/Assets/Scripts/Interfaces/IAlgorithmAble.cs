using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlgorithmAble {

    string GetAlgorithmName();
    void ResetSetup();
    Dictionary<int, string> PseudoCode { get; set; }

    IEnumerator Tutorial(GameObject[] list);
    Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list);

}
