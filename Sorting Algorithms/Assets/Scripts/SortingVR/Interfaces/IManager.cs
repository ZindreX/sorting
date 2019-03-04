using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager {

    void CreateObjects(int numberOfElements, Vector3[] positions);
    void DestroyAndReset();


}
