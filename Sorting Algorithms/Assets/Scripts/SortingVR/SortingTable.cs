using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingTable : MonoBehaviour {

    [SerializeField]
    private Transform returnPosition;

    private void Awake()
    {
        Section sortingTableSection = GetComponentInChildren<Section>();
        sortingTableSection.SectionManager = FindObjectOfType<SettingsBase>();
    }


    public Transform ReturnPosition
    {
        get { return returnPosition; }
    }
}
