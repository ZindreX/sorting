using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderManager : MonoBehaviour, IManager {

    /* -------------------------------------------- Manage all holders --------------------------------------------
     * TODO: 
     * 
    */

    // Initialization
    public GameObject holderPrefab;

    // GameObject
    private GameObject[] holders;

    private Vector3 firstHolderPosition; // -2.092, 2.1, 29.5
    private bool containsHolders = false;

    void Awake()
    {
        firstHolderPosition = gameObject.transform.position;
        holders = new GameObject[GetComponent<AlgorithmManagerBase>().NumberOfElements]; // ***
    }

    public GameObject[] Holders
    {
        get { return holders; }
    }

    public HolderBase GetHolder(int index)
    {
        return (index >= 0 && index < holders.Length) ? holders[index].GetComponent<HolderBase>() : null;
    }

    // Creates the holders
    public void CreateObjects(int numberOfHolders, Vector3[] positions)
    {
        if (containsHolders)
            return;

        for (int x = 0; x < numberOfHolders; x++)
        {
            holders[x] = Instantiate(holderPrefab, firstHolderPosition + new Vector3(x, 0f, 0f), Quaternion.identity);
            holders[x].GetComponent<HolderBase>().Parent = gameObject; // Mergesort & Bucketsort null exception?
        }
        containsHolders = true;
    }

    // Destroys all holders + reset
    public void DestroyObjects()
    {
        Util.DestroyObjects(holders);
        containsHolders = false;
        HolderBase.HOLDER_NR = 0;
    }

    public Vector3[] GetHolderPositions()
    {
        Vector3[] positions = new Vector3[holders.Length];
        for (int x=0; x < holders.Length; x++)
        {
            positions[x] = holders[x].transform.position;
        }
        return positions;
    }
}
