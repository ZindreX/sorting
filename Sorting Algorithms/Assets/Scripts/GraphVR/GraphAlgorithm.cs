using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphAlgorithm : TeachingAlgorithm {

    // Demo variables
    protected Node node1, node2;
    protected Edge edge;

    // Instruction variables
    protected bool beginnerWait;
    protected int prevHighlightedLineOfCode;







    protected override float GetLineSpacing()
    {
        return UtilGraph.SPACE_BETWEEN_CODE_LINES;
    }

}
