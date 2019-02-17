using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphAlgorithm : TeachingAlgorithm {

    protected bool beginnerWait;

    protected Node node;

    protected int prevHighlightedLineOfCode;

    protected override float GetLineSpacing()
    {
        return UtilGraph.SPACE_BETWEEN_CODE_LINES;
    }

}
