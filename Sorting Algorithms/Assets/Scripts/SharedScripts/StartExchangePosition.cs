using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StartExchangePosition : MonoBehaviour {

    /* - Moves object into scene when needed
     * - Hide otherwise
     * 
     * > Mainly used by the settings menus and other help objects
    */

    protected Vector3 startPos, inScenePosition;

    protected virtual void Awake()
    {
        startPos = transform.position;
    }

    public void SwapPositions()
    {
        Vector3 temp = inScenePosition;
        inScenePosition = startPos;
        startPos = temp;
    }

    public Vector3 InScenePosition
    {
        get { return inScenePosition; }
        set { inScenePosition = value; }
    }

    public void ActiveInScene(bool active)
    {
        if (active)
            transform.position = inScenePosition;
        else
            transform.position = startPos;
    }

}
