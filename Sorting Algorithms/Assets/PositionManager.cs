using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour {

    [Header("Player")]
    [SerializeField]
    private GameObject player;

    [Space(2)]
    [Header("Goal(s)")]
    [SerializeField]
    private GameObject currentGoal;

    [SerializeField]
    private float withinValue = 0.6f;
    [SerializeField]
    private bool playerWithinGoalPosition = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentGoal != null)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 goalPos = currentGoal.transform.position;

            float playerRelToGoalX = Mathf.Abs(playerPos.x - goalPos.x);
            float playerRelToGoalZ = Mathf.Abs(playerPos.z - goalPos.z);

            if (playerRelToGoalX < withinValue && playerRelToGoalZ < withinValue)
                PlayerWithinGoalPosition = true;
            else
                PlayerWithinGoalPosition = false;
        }
		
	}

    public GameObject CurrentGoal
    {
        get { return currentGoal; }
        set { currentGoal = value; }
    }

    public bool PlayerWithinGoalPosition
    {
        get { return playerWithinGoalPosition; }
        set { playerWithinGoalPosition = value; }
    }

}
