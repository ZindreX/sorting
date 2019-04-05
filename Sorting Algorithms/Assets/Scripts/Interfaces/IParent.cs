using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISortSubElement {

    SortMain SuperElement { get; set; }
    string MyRole();

}
