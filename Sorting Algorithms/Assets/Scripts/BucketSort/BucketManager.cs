using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketManager : MonoBehaviour, IManager {

    [SerializeField]
    private GameObject bucketPrefab;

    private GameObject[] buckets;

    void Awake()
    {
        buckets = new GameObject[GetComponent<BucketSortManager>().NumberOfBuckets];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateObjects(int numberOfElements, Vector3[] positions)
    {
        throw new System.NotImplementedException();
    }

    public void DestroyObjects()
    {
        throw new System.NotImplementedException();
    }
}
