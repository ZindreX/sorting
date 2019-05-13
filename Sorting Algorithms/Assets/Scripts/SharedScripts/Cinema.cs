using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinema : MonoBehaviour {

    [SerializeField]
    private GameObject floorPrefab;

    private bool canCreate;


    private Vector3 current, forward = new Vector3(0f, 1f, 1f), right = new Vector3(1f, 1f, 0f);
    private WaitForSeconds buildDuration = new WaitForSeconds(1f);

    private BoxCollider wallTrigger;

    private void Awake()
    {
        current = transform.position;
        canCreate = false;
    }

    private void Update()
    {
        if (canCreate)
        {
            canCreate = false;
            StartCoroutine(CreateNewFloor());
        }
    }

    private IEnumerator CreateNewFloor()
    {
        current += forward;
        Instantiate(floorPrefab, current, Quaternion.identity);
        yield return buildDuration;
        canCreate = true;
    }

}
