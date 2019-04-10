using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingTable : MonoBehaviour, IMoveAble {

    [SerializeField]
    private Transform returnPosition;

    private Vector3 startPos, moveOutPos;

    private void Awake()
    {
        startPos = transform.position;
        moveOutPos = startPos + new Vector3(5f, 10f, 0f);

        Section sortingTableSection = GetComponentInChildren<Section>();
        sortingTableSection.SectionManager = FindObjectOfType<SettingsBase>();
    }

    public void SetTableActive(bool active)
    {
        if (active)
            MoveOut();
        else
            MoveBack();
    }

    public void MoveOut()
    {
        transform.position = moveOutPos;
    }

    public void MoveBack()
    {
        transform.position = startPos;
    }

    public Transform ReturnPosition
    {
        get { return returnPosition; }
    }
}
