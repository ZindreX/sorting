using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITraverse {

    IEnumerator Demo(Node startNode);
    List<int> VisitNodeOrder(Node startNode);

}
