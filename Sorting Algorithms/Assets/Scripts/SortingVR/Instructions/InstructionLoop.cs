using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionLoop : InstructionBase {

    protected int i, j, k;

    public InstructionLoop(string instruction, int instructionNr, int i)
    : base(instruction, instructionNr)
    {
        this.i = i;
    }


    public InstructionLoop(string instruction, int instructionNr, int i, int j)
    : base(instruction, instructionNr)
    {
        this.i = i;
        this.j = j;
    }

    public InstructionLoop(string instruction, int instructionNr, int i, int j, int k)
        : base(instruction, instructionNr)
    {
        this.i = i;
        this.j = j;
        this.k = k;
    }

    public int I
    {
        get { return i; }
    }

    public int J
    {
        get { return j; }
    }

    public int K
    {
        get { return k; }
    }

    public override string DebugInfo()
    {
        return base.DebugInfo() + "| i=" + i + ", j=" + j + ", k=" + k;
    }

}
