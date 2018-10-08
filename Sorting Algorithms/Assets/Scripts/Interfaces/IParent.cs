using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChild {

    GameObject Parent { get; set; }
    string MyRole();

}
