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

    public Transform ReturnPosition
    {
        get { return returnPosition; }
    }
}
