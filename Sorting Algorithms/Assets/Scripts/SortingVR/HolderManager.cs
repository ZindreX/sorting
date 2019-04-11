using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderManager : MonoBehaviour, IManager {

    /* -------------------------------------------- Manage all holders --------------------------------------------
     * TODO: 
     * 
    */

    // Initialization
    public GameObject holderPrefab, sortingTableHoldersObj;

    // GameObject
    private GameObject[] holders;

    [SerializeField]
    private Transform firstHolderPosition;
    private bool containsHolders = false;
    private SortMain superElement;

    public void InitManager()
    {
        superElement = GetComponent<SortMain>();
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

        Vector3 firstHolderPos = new Vector3(firstHolderPosition.position.x + (UtilSort.MAX_NUMBER_OF_ELEMENTS - numberOfHolders) * (UtilSort.SPACE_BETWEEN_HOLDERS/2), firstHolderPosition.position.y, firstHolderPosition.position.z);

       holders = new GameObject[numberOfHolders]; // ***
        for (int x = 0; x < numberOfHolders; x++)
        {
            holders[x] = Instantiate(holderPrefab, firstHolderPos + new Vector3((x * UtilSort.SPACE_BETWEEN_HOLDERS), 0f, 0f), Quaternion.identity);
            switch (superElement.GetTeachingAlgorithm().AlgorithmName)
            {
                case UtilSort.BUBBLE_SORT: holders[x].AddComponent<BubbleSortHolder>(); break;
                case UtilSort.INSERTION_SORT: holders[x].AddComponent<InsertionSortHolder>(); break;
                case UtilSort.BUCKET_SORT: holders[x].AddComponent<BucketSortHolder>(); break;
                case UtilSort.MERGE_SORT: holders[x].AddComponent<MergeSortHolder>(); break;
                default: Debug.LogError("Add subclass for holder!"); break;
            }
            
            holders[x].GetComponent<HolderBase>().SuperElement = superElement; // null(?): add C# script to holder / sorting elements
            holders[x].transform.parent = sortingTableHoldersObj.transform;
        }
        containsHolders = true;

        // TODO: incease width of table
        //Vector3 lastHolder = holders[holders.Length - 1].transform.position;
        //sortingTableHoldersObj.GetComponentInChildren<MeshRenderer>().transform.localScale += new Vector3(lastHolder.x - firstHolderPos.x, 0f, 0f);
    }

    // Destroys all holders + reset
    public void DestroyAndReset()
    {
        UtilSort.DestroyObjects(holders);
        containsHolders = false;
        HolderBase.HOLDER_NR = 0;
        superElement = null;
    }

    public Vector3[] GetHolderPositions()
    {
        Vector3[] positions = new Vector3[holders.Length];
        for (int x=0; x < holders.Length; x++)
        {
            if (holders[x] != null)
                positions[x] = holders[x].transform.position;
        }
        return positions;
    }
}
