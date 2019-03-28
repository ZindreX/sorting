using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingTable : MonoBehaviour {

    private void Awake()
    {
        Section sortingTableSection = GetComponentInChildren<Section>();
        sortingTableSection.SectionManager = FindObjectOfType<SettingsBase>();
    }
}
