using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : Algorithm {

    /* --------------------------------------------------- Merge Sort --------------------------------------------------- 
     * 1) Recursive aproach till just 1 element left in list
     * 2) Then do merge
     * 
     * ---------------------------------------------------   Merge    ---------------------------------------------------
     * # Merges two lists together by comparing elements 1 by 1 from each list
     * # Add these to a new list, which now are sorted
     */

    [SerializeField]
    private GameObject holderPrefab;
    private HolderBase[] splitHolders;

    private Vector3 increaseHeight = new Vector3(0f, 1f, 0f);
    private Dictionary<int, string> pseudoCode;

    // User test
    private Dictionary<int, InstructionBase> allMoves;

    void Awake()
    {
        allMoves = new Dictionary<int, InstructionBase>();
    }

    public override string GetAlgorithmName()
    {
        return Util.MERGE_SORT;
    }

    public override Dictionary<int, string> PseudoCode
    {
        get { return pseudoCode; }
        set { pseudoCode = value; }
    }

    public override void ResetSetup()
    {
        for (int x=0; x < extraHolders.Count; x++)
        {
            //Destroy(extraHolders[x])
        }
    }


    // Tutorial & User test stuff

    private List<GameObject> extras = new List<GameObject>();
    public GameObject GetExtraHolder(int holder)
    {
        return extras[holder];
    }


    // Test: OK
    #region Merge Sort: Standard
    public static GameObject[] MergeSort1(GameObject[] list)
    {
        if (list.Length <= 1)
        {
            return list;
        }

        int leftLength = list.Length / 2;
        int rightLength = list.Length - leftLength;
        GameObject[] left = new GameObject[leftLength];
        GameObject[] right = new GameObject[rightLength];
        for (int x = 0; x < list.Length; x++)
        {
            if (x < leftLength)
            {
                left[x] = list[x];
            }
            else
            {
                right[x - rightLength] = list[x];
            }
        }

        left = MergeSort1(left);
        right = MergeSort1(right);
        return Merge(left, right);

    }

    private static GameObject[] Merge(GameObject[] left, GameObject[] right)
    {
        GameObject[] result = new GameObject[left.Length + right.Length];

        int l = 0, r = 0;
        while (l < left.Length && r < right.Length)
        {
            if (left[l].GetComponent<SortingElementBase>().Value <= right[r].GetComponent<SortingElementBase>().Value)
            {
                result[l + r] = left[l];
                l += 1;
            }
            else
            {
                result[l + r] = right[r];
                r += 1;
            }
        }

        while (l < left.Length)
        {
            result[l + r] = left[l];
            l += 1;
        }

        while (r < right.Length)
        {
            result[l + r] = right[r];
            r += 1;
        }

        return result;
    }
    #endregion

    // Test: ----
    #region Merge Sort: Standard (Iterative)
    public void MergeSortIterative(GameObject[] list, int N)
    {
        int currentSize, leftStart;

        for (currentSize = 1; currentSize <= N - 1; currentSize = 2 * currentSize)
        {
            for (leftStart = 0; leftStart < N - 1; leftStart = 2 * currentSize)
            {
                int mid = leftStart + currentSize - 1;
                int rightEnd = Mathf.Min(leftStart + 2 * currentSize - 1, N - 1);

                MergeIterative(list, leftStart, mid, rightEnd);
            }
        }
    }

    private void MergeIterative(GameObject[] list, int l, int m, int r)
    {
        int i, j, k;
        int leftLength = m - l + 1;
        int rightLength = r - m;

        GameObject[] left = new GameObject[leftLength];
        GameObject[] right = new GameObject[rightLength];

        for (i = 0; i < leftLength; i++)
        {
            left[i] = list[l + i];
        }
        for (j = 0; j < rightLength; j++)
        {
            right[j] = list[m + 1 + j];
        }

        i = 0;
        j = 0;
        k = l;
        while (i < leftLength && j < rightLength)
        {
            if (left[i].GetComponent<MergeSortElement>().Value <= right[j].GetComponent<MergeSortElement>().Value)
            {
                list[k] = left[i];
                i++;
            }
            else
            {
                list[k] = right[j];
                j++;
            }
            k++;
        }

        while (i < leftLength)
        {
            list[k] = left[i];
            i++;
            k++;
        }
        while (j < rightLength)
        {
            list[k] = right[j];
            j++;
            k++;
        }
    }
    #endregion


    #region Test 1
    // Help function
    private Dictionary<int, GameObject[]> workingOn;

    // Finds the number of splits required before merging
    private int FindNumberOfSplits(int listLength, int N)
    {
        return (listLength != 1) ? FindNumberOfSplits(listLength / 2, N + 1) : N;
    }

    public IEnumerator TestTutorial(GameObject[] list)
    {
        Dictionary<int, Dictionary<int, GameObject[]>> allSplits = new Dictionary<int, Dictionary<int, GameObject[]>>();
        int N = list.Length, mod;
        int temp = N;
        int splits = FindNumberOfSplits(N, 0);
        Debug.Log(splits);
        while (splits > 0)
        {
            // Split list into two and display result
            GameObject[] leftTemp, rightTemp;
            mod = temp % 2;
            temp = temp / 2;
            if (mod == 1)
                leftTemp = new GameObject[temp + mod];
            else
                leftTemp = new GameObject[temp];

            rightTemp = new GameObject[temp];

            splits--;
            yield return new WaitForSeconds(seconds);
        }
    }
    #endregion


    private Dictionary<int, Color> groupColors = new Dictionary<int, Color>();
    #region Test 2
    public IEnumerator TutorialTest2(GameObject[] list)
    {
        yield return new WaitForSeconds(seconds);

    }
    #endregion



    private Dictionary<int, Dictionary<string, GameObject[]>> allSplits = new Dictionary<int, Dictionary<string, GameObject[]>>();
    private Dictionary<int, GameObject[]> extraHolders = new Dictionary<int, GameObject[]>();
    private int split = -1;
    private static readonly string LEFT = "Left", RIGHT = "Right";

    #region Merge Sort: All Moves Tutorial TODO: implement
    public override IEnumerator Tutorial(GameObject[] list)
    {
        return MS(list, 0, list.Length);
        //return MergeSortTutorial(list, 0);
    }

    private Vector3 leftPos, rightPos;
    private IEnumerator MergeSortTutorial(GameObject[] list, int split)
    {
        if (list.Length <= 1)
        {
            Debug.Log("Length <= 1");
            // Start merging from here?
            //return list;
        }
        else
        {
            Debug.Log("Tutorial: split=" + split);

            // Find positions for the new holders
            if (split < 1)
            {
                leftPos = GetComponent<AlgorithmManagerBase>().HolderPositions[split * 2] + increaseHeight;
                rightPos = GetComponent<AlgorithmManagerBase>().HolderPositions[(split * 2) + 4] + increaseHeight;
            }
            else
            {
                //leftPos += GetComponent<AlgorithmManagerBase>().HolderPositions[]
            }

            // Create two new holders to split the input list
            extraHolders[split] = Util.CreateObjects(holderPrefab, 2, new Vector3[] { leftPos, rightPos }, GetComponent<AlgorithmManagerBase>().gameObject);

            // Start splitting list in to two equal* size lists
            // Find the size of each list
            int leftLength = list.Length / 2;
            int rightLength = list.Length - leftLength;

            // Create list objects and start distributing elements to them
            Dictionary<string, GameObject[]> temp = new Dictionary<string, GameObject[]>();
            temp.Add(LEFT, new GameObject[leftLength]);
            temp.Add(RIGHT, new GameObject[rightLength]);
            for (int x = 0; x < list.Length; x++)
            {
                if (x < leftLength)
                {
                    temp[LEFT][x] = list[x];
                    list[x].transform.position = leftPos + increaseHeight * 2;
                    yield return new WaitForSeconds(seconds);
                }
                else
                {
                    temp[RIGHT][x - rightLength] = list[x];
                    list[x].transform.position = rightPos + increaseHeight * 2;
                    yield return new WaitForSeconds(seconds);
                }
            }

            // Make visible for left and right lists
            //left = Tutorial2(left);
            //right = Tutorial2(right);
            //return MergeAllMovesTutorial(left, right);

            allSplits[split] = temp;
            split++;

            Debug.Log("Tutorial on left");
            StartCoroutine(MergeSortTutorial(temp[LEFT], split));

            Debug.Log("Tutorial on right");
            StartCoroutine(MergeSortTutorial(temp[RIGHT], split));

            Debug.Log("Merging");
            StartCoroutine(MergeTutorial(allSplits[split][LEFT], allSplits[split][RIGHT])); // return? // TODO: continue splitting + merging...
        }
    }

    private IEnumerator MergeTutorial(GameObject[] left, GameObject[] right)
    {
        Debug.Log("Merge tutorial starting...");
        // Making a list with the length of the two parameters combined
        GameObject[] result = new GameObject[left.Length + right.Length];

        int l = 0, r = 0;
        while (l < left.Length && r < right.Length)
        {
            if (left[l].GetComponent<SortingElementBase>().Value <= right[r].GetComponent<SortingElementBase>().Value)
            {
                result[l + r] = left[l];
                l += 1;
            }
            else
            {
                result[l + r] = right[r];
                r += 1;
            }
        }

        while (l < left.Length)
        {
            result[l + r] = left[l];
            l += 1;
        }

        while (r < right.Length)
        {
            result[l + r] = right[r];
            r += 1;
        }

        yield return new WaitForSeconds(seconds);
        
        // Make visible the new combined list
        //return result;
    }
    #endregion


    /* l is for left index and r 
    is right index of the sub-array  
    of arr to be sorted */
    public IEnumerator MS(GameObject[] arr, int l, int r)
    {
        if (l < r)
        {
            // Same as (l+r)/2 but avoids  
            // overflow for large l & h 
            int m = l + (r - l) / 2;
            Debug.Log("MS: l=" + l + ", m=" + m + ", r=" + r);

            // Create holder (at 1st element's position i arr)
            extras.Add(CreateHolder(l));
            extras.Add(CreateHolder(m+1));

            for (int x=l; x < m; x++)
            {
                //Debug.Log("X=" + x + ", l=" + l + ", m=" + m);
                arr[x].transform.position = extras[extras.Count - 2].transform.position + increaseHeight * 2;
                yield return new WaitForSeconds(seconds);
            }

            for (int y=m; y < r; y++)
            {
                //Debug.Log("Y=" + y);
                arr[y].transform.position = extras[extras.Count - 1].transform.position + increaseHeight * 2;
                yield return new WaitForSeconds(seconds);
            }

            Debug.Log("Starting merge sort on left: [" + l + " -> " + m + "]");
            StartCoroutine(MS(arr, l, m));


            Debug.Log("Starting merge sort on right: [" + (m+1) + " -> " + r + "]");
            StartCoroutine(MS(arr, m + 1, r));

            
            Debug.Log("Starting merge: [" + l + " -> " + m + " -> " + r + "]");
            StartCoroutine(M(arr, l, m, r));
        }
        yield return new WaitForSeconds(seconds * 4);
    }

    /* Function to merge the two haves 
    arr[l..m] and arr[m+1..r] of array  
    arr[] */
    private IEnumerator M(GameObject[] arr, int l, int m, int r)
    {
        int i, j, k;
        int n1 = m - l + 1;
        int n2 = r - m;

        /* create temp arrays */
        GameObject[] L = new GameObject[n1];
        GameObject[] R = new GameObject[n2];

        /* Copy data to temp arrays 
        L[] and R[] */
        for (i = 0; i < n1; i++)
            L[i] = arr[l + i];
        for (j = 0; j < n2; j++)
            R[j] = arr[m + 1 + j];

        /* Merge the temp arrays back  
        into arr[l..r]*/
        i = 0;
        j = 0;
        k = l;
        while (i < n1 && j < n2)
        {
            if (L[i].GetComponent<SortingElementBase>().Value <= R[j].GetComponent<SortingElementBase>().Value)
            {
                arr[k] = L[i];
                arr[k].transform.position = GetComponent<AlgorithmManagerBase>().HolderPositions[k]; // testing
                yield return new WaitForSeconds(seconds);
                i++;
            }
            else
            {
                arr[k] = R[j];
                arr[k].transform.position = GetComponent<AlgorithmManagerBase>().HolderPositions[k]; // testing
                yield return new WaitForSeconds(seconds);
                j++;
            }
            k++;
        }

        /* Copy the remaining elements of 
        L[], if there are any */
        while (i < n1)
        {
            arr[k] = L[i];
            arr[k].transform.position = GetComponent<AlgorithmManagerBase>().HolderPositions[k]; // testing
            yield return new WaitForSeconds(seconds);
            i++;
            k++;
        }

        /* Copy the remaining elements of 
        R[], if there are any */
        while (j < n2)
        {
            arr[k] = R[j];
            arr[k].transform.position = GetComponent<AlgorithmManagerBase>().HolderPositions[k]; // testing
            yield return new WaitForSeconds(seconds);
            j++;
            k++;
        }
        yield return new WaitForSeconds(seconds);
    }

    private GameObject CreateHolder(int q)
    {
        Vector3 pos = GetComponent<AlgorithmManagerBase>().HolderPositions[q] + increaseHeight;
        GameObject holder = Instantiate(holderPrefab, pos, Quaternion.identity);
        holder.GetComponent<HolderBase>().Parent = gameObject;
        return holder;
    }




    // User test

    #region Merge Sort: All Moves User Test TODO: implement
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        MergeSortAllMoves(list);
        return allMoves;
    }

    private GameObject[] MergeSortAllMoves(InstructionBase[] list)
    {
        if (list.Length <= 1)
        {
            // Make visible for list reached 1 element
            //return list;
        }
        //yield return new WaitForSeconds(seconds);

        int leftLength = list.Length / 2;
        int rightLength = list.Length - leftLength;
        GameObject[] left = new GameObject[leftLength];
        GameObject[] right = new GameObject[rightLength];
        for (int x = 0; x < list.Length; x++)
        {
            //if (x < leftLength)
            //{
            //    left[x] = list[x];
            //}
            //else
            //{
            //    right[x - rightLength] = list[x];
            //}
        }

        // Make visible for left and right lists
        //left = MergeSortAllMovesTutorial(left);
        //right = MergeSortAllMovesTutorial(right);
        return MergeUserTest(left, right);

    }

    private GameObject[] MergeUserTest(GameObject[] left, GameObject[] right)
    {
        GameObject[] result = new GameObject[left.Length + right.Length];

        int l = 0, r = 0;
        while (l < left.Length && r < right.Length)
        {
            if (left[l].GetComponent<SortingElementBase>().Value <= right[r].GetComponent<SortingElementBase>().Value)
            {
                result[l + r] = left[l];
                l += 1;
            }
            else
            {
                result[l + r] = right[r];
                r += 1;
            }
        }

        while (l < left.Length)
        {
            result[l + r] = left[l];
            l += 1;
        }

        while (r < right.Length)
        {
            result[l + r] = right[r];
            r += 1;
        }
        // Make visible the new combined list
        return result;
    }
    #endregion

}
