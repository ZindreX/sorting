using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Vector3 destination;

    void Start()
    {
        // Set the destination to be the object's position so it will not start off moving
        SetDestination(transform.position);
    }

    void Update()
    {
        // If the object is not at the target destination
        if (destination != transform.position)
        {
            // Move towards the destination each frame until the object reaches it
            IncrementPosition();
        }
    }

    private void IncrementPosition()
    {
        // Calculate the next position
        float delta = speed * Time.deltaTime;
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);

        // Move the object to the next position
        transform.position = nextPosition;
    }

    // Set the destination to cause the object to smoothly glide to the specified location
    public void SetDestination(Vector3 value)
    {
        destination = value;
    }

    public void AddVector3(Vector3 pos)
    {
        destination += pos;
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

}
