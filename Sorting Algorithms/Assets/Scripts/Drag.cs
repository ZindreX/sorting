using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    //[SerializeField]
    //private float dragSensitivity = 3f;

    // Drag and drop (mouse)
    private Vector3 distance;
    private float posX, posY;

    private void OnMouseDown()
    {
        distance = cam.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - distance.x;
        posY = Input.mousePosition.y - distance.y;
    }

    private void OnMouseDrag()
    {
        Vector3 currentPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, distance.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(currentPos);
        transform.position = worldPos;
    }
}
