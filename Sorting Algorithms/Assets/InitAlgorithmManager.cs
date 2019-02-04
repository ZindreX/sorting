using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAlgorithmManager : AlgorithmManagerBase
{
    protected override Algorithm InstanceOfAlgorithm
    {
        get { return null; }
    }

    protected override int MovesNeeded
    {
        get { return 0; }
    }

    public override HolderBase GetCorrectHolder(int index)
    {
        return base.GetCorrectHolder(index);
    }

    protected override InstructionBase[] CopyFirstState(GameObject[] sortingElements)
    {
        throw new System.NotImplementedException();
    }

    protected override int PrepareNextInstruction(InstructionBase instruction)
    {
        throw new System.NotImplementedException();
    }
}
