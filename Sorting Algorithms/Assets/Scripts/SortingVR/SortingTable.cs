using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingTable : StartExchangePosition {

    [SerializeField]
    private Transform returnPosition;

    protected override void Awake()
    {
        base.Awake();

        Section sortingTableSection = GetComponentInChildren<Section>();
        sortingTableSection.SectionManager = FindObjectOfType<SortSettings>();

        InScenePosition = new Vector3(0f, 0.3f, 0f);
    }

    // The return position is used by sorting elements which hit the floor, which are then returned to this spot
    public Transform ReturnPosition
    {
        get { return returnPosition; }
    }
}
