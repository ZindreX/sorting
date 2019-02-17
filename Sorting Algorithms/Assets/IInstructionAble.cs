using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInstructionAble {

    bool NextMove { get; set; }
    InstructionBase Instruction { get; set; }

}
