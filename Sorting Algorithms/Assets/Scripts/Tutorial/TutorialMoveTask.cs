using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoveTask : TutorialTask {

    private bool playerInlocation;

    public override void InitTask()
    {
        playerInlocation = false;
    }

    public override void StopTask()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Util.HEAD_COLLIDER)
        {
            playerInlocation = true;
            taskCompleted = true;
            Progress();
        }
    }

}
