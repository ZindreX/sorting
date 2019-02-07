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

        firstHolderPosition = new Vector3(gameObject.transform.position.x + (UtilSort.MAX_NUMBER_OF_ELEMENTS - numberOfHolders) * (UtilSort.SPACE_BETWEEN_HOLDERS/2), gameObject.transform.position.y, gameObject.transform.position.z);

       holders = new GameObject[numberOfHolders]; // ***
        for (int x = 0; x < numberOfHolders; x++)
        {
            holders[x] = Instantiate(holderPrefab, firstHolderPosition + new Vector3((x * UtilSort.SPACE_BETWEEN_HOLDERS), 0f, 0f), Quaternion.identity);
            switch (GetComponent<AlgorithmManagerBase>().Algorithm.AlgorithmName)
            {
                case UtilSort.BUBBLE_SORT: holders[x].AddComponent<BubbleSortHolder>(); break;
                case UtilSort.INSERTION_SORT: holders[x].AddComponent<InsertionSortHolder>(); break;
                case UtilSort.BUCKET_SORT: holders[x].AddComponent<BucketSortHolder>(); break;
                case UtilSort.MERGE_SORT: holders[x].AddComponent<MergeSortHolder>(); break;
                default: Debug.LogError("Add subclass for holder!"); break;
            }
            
            holders[x].GetComponent<HolderBase>().Parent = gameObject; // null(?): add C# script to holder / sorting elements
        }
        containsHolders = true;
    }

    // Destroys all holders + reset
    public void DestroyObjects()
    {
        UtilSort.DestroyObjects(holders);
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
