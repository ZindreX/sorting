using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDetecter : MonoBehaviour {

    [Header("Player")]
    [SerializeField]
    private GameObject player;

    [Space(2)]
    [Header("Goal(s)")]
    [SerializeField]
    private Transform[] areas;

    [SerializeField]
    private float withinValue = 0.6f;
    [SerializeField]
    private bool[] playerWithinAreas;

    private void Awake()
    {
        playerWithinAreas = new bool[areas.Length];
    }


    // Update is called once per frame
    void Update()
    {
        if (areas != null)
        {
            bool flag = false;

            Vector3 playerPos = player.transform.position;

            for (int i=0; i < areas.Length; i++)
            {
                // Check area w/position
                Vector3 areaPos = areas[i].position;

                // Check player relative to area
                float playerRelToAreaX = Mathf.Abs(playerPos.x - areaPos.x);
                float playerRelToAreaZ = Mathf.Abs(playerPos.z - areaPos.z);

                if (playerRelToAreaX < withinValue && playerRelToAreaZ < withinValue)
                    playerWithinAreas[i] = true;
                else
                    playerWithinAreas[i] = true;

                if (playerWithinAreas[i])
                {
                    flag = true;
                    break;
                }
            }
        }

    }

}
