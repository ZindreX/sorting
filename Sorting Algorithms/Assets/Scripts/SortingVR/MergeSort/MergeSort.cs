using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : SortAlgorithm {

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

    [SerializeField]
    private MergeSortManager mergeSortManager;

    private int numberOfElements, numberOfSplits;
    private Vector3 splitHolderPos = new Vector3(0f, 0.2f, 0.2f);
    private Dictionary<int, int[]> holderIndex = new Dictionary<int, int[]>();
    
    private void Start()
    {
        //numberOfElements = sortMain.SortSettings.NumberOfElements;
        //numberOfSplits = FindNumberOfSplits(numberOfElements, 0);
        //Debug.Log(numberOfSplits);
        //splitLeft = numberOfSplits;
        //splitRight = numberOfSplits;

        ////
        //holderIndex.Add(8, new int[8] { 0, 4, 2, 1, 3, 6, 5, 7 });
        //holderIndex.Add(6, new int[6] { 0, 3, 2, 1, 5, 4 });
        //holderIndex.Add(4, new int[4] { 0, 2, 1, 3 });
        //holderIndex.Add(2, new int[2] { 0, 1 });
    }

    public override string AlgorithmName
    {
        get { return Util.MERGE_SORT; }
    }

    public override string CollectLine(int lineNr)
    {
        return "0: No pseudocode for merge sort yet.";
    }

    public override int FirstInstructionCodeLine()
    {
        return 0;
    }

    public override int FinalInstructionCodeLine()
    {
        return 1;
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
        for (int x=0; x < splitHolders.Count; x++)
        {
            //Destroy(extraHolders[x])
        }
    }

    public override void Specials(string method, int number, bool activate)
    {
        switch (method)
        {
            case "Somemethod": FirstInstructionCodeLine(); break; // example: some void method
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
    public static GameObject[] MergeSortStandard(GameObject[] list)
    {
        Debug.Log("Demo: " + DebugList(list));

        if (list.Length <= 1)
        {
            Debug.Log("Only 1 element left, return!");
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

        Debug.Log("Left: " + DebugList(left));
        left = MergeSortStandard(left);

        Debug.Log("Right: " + DebugList(right));
        right = MergeSortStandard(right);

        Debug.Log("Start merge: " + DebugList(left) + " + " + DebugList(right));
        return MergeStandard(left, right);
    }

    private static GameObject[] MergeStandard(GameObject[] left, GameObject[] right)
    {
        Debug.Log("Merging: " + DebugList(left) + " + " + DebugList(right));

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

        Debug.Log("Result: " + DebugList(result));
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


    // Finds the number of splits required before merging
    private int FindNumberOfSplits(int listLength, int N)
    {
        return (listLength != 1) ? FindNumberOfSplits(listLength / 2, N + 1) : N;
    }

    private int splitLeft, splitRight, mergesAdded = 0, atMerge = 0;
    private const string LEFT = "Left", RIGHT = "Right";
    private int split = -1, leftIndex = 0, rightIndex = 0;

    private static string DebugList(GameObject[] list)
    {
        string result = "";
        for (int i=0; i < list.Length; i++)
        {
            result += "[" + list[i].GetComponent<SortingElementBase>().Value + "], ";
        }
        return result;
    }

    private GameObject[] leftList, rightList;
    private Dictionary<int, GameObject[]> leftSplits = new Dictionary<int, GameObject[]>();
    private Dictionary<int, GameObject[]> rightSplits = new Dictionary<int, GameObject[]>();
    private Dictionary<int, GameObject[]> allMerges = new Dictionary<int, GameObject[]>();

    #region Merge Sort: Demo visual
    public override IEnumerator Demo(GameObject[] list)
    {
        Debug.Log("Demo: " + DebugList(list));
        yield return demoStepDuration;

        
        // Check if list only has 1 element
        if (list.Length <= 1)
        {
            Debug.Log("Only 1 element left, return!");
            yield return list;
            yield break;
            Debug.Log("Nani");
        }

        split++;

        if (splitHolders.Count < numberOfElements)
        {
            switch (split)
            {

                case 0:
                    for (int i = 0; i < 2; i++)
                    {
                        splitHolders.Add(CreateSplitHolder(holderIndex[numberOfElements][splitHolders.Count]));
                    }
                    //leftIndex = splitHolders.Count - 2;
                    //rightIndex = splitHolders.Count - 1;
                    break;

                case 1: case 2: case 3: case 4: case 5: case 6: case 7:
                    int hIndex = holderIndex[numberOfElements][splitHolders.Count];
                    splitHolders.Add(CreateSplitHolder(hIndex));

                    //if (split == 3)
                    //    leftIndex = 2; // holderIndex[numberOfElements][2]; // 2
                    //else if (split == 4 || split == 5)
                    //    leftIndex = 1; // holderIndex[numberOfElements][1]; // 4
                    //else
                    //    leftIndex = 5; //holderIndex[numberOfElements][5]; // 6

                    //rightIndex = hIndex;

                    break;

                default: Debug.Log("No split set"); break;
            }
            leftIndex = splitHolders.Count - 2;
            rightIndex = splitHolders.Count - 1;
        }

        // Otherwise split list into two lists
        int leftLength = list.Length / 2;
        int rightLength = list.Length - leftLength;
        GameObject[] left = new GameObject[leftLength];
        GameObject[] right = new GameObject[rightLength];

        for (int x=0; x < list.Length; x++)
        {
            MergeSortElement element = list[x].GetComponent<MergeSortElement>();
            MergeSortHolder leftHolder = splitHolders[leftIndex];
            MergeSortHolder rightHolder = splitHolders[rightIndex];

            if (x < leftLength)
            {
                
                left[x] = list[x];

                // Move element left list
                element.transform.position = leftHolder.transform.position + UtilSort.ABOVE_HOLDER_VR + leftHolder.NextElementPos();
                leftHolder.HoldingNumberOfElements++;
            }
            else if (leftLength != 0 && rightLength != 0) // added
            {
                right[x - rightLength] = list[x];

                // Move element to right list
                element.transform.position = rightHolder.transform.position + UtilSort.ABOVE_HOLDER_VR + rightHolder.NextElementPos();
                rightHolder.HoldingNumberOfElements++;
            }

            yield return demoStepDuration;
        }


        // Recursively do it until only 1 element remain in each list
        Debug.Log("Left: " + DebugList(left));
        //leftList = left;
        yield return Demo(left);

        Debug.Log("Right: " + DebugList(right));
        //rightList = right;
        yield return Demo(right);

        // Then merge the two lists
        Debug.Log("Start merge: " + DebugList(left) + " + " + DebugList(right));
        if (left.Length == 1 && right.Length == 1)
            yield return DemoMerge(left, right);
        else
        {
            Debug.Log("Split: " + split + ", atMerge: " + atMerge + " of " + mergesAdded);
            foreach (KeyValuePair<int, GameObject[]> entry in allMerges)
            {
                Debug.Log("Entry: " + entry.Key + ": " + DebugList(entry.Value));
            }


            GameObject[] l = allMerges[atMerge];
            GameObject[] r = allMerges[atMerge+1];
            atMerge += 2;
            yield return DemoMerge(l, r);
        }
    }

    private GameObject[] DemoMerge(GameObject[] left, GameObject[] right)
    {
        Debug.Log("Merging: " + DebugList(left) + " + " + DebugList(right));

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

        allMerges.Add(mergesAdded++, result);
        Debug.Log("Result: " + DebugList(result));
        return result;
    }
    #endregion

    private List<MergeSortHolder> splitHolders = new List<MergeSortHolder>();
    private MergeSortHolder CreateSplitHolder(int aboveHolder)
    {
        // Instantiate
        Vector3 pos = sortMain.HolderPositions[aboveHolder] + splitHolderPos;

        GameObject splitHolder = Instantiate(holderPrefab, pos, Quaternion.identity);
        splitHolder.AddComponent<MergeSortHolder>();

        MergeSortHolder mergeSortHolder = splitHolder.GetComponent<MergeSortHolder>();

        //pivotHolder = pivotHolderClone.GetComponent<InsertionSortHolder>();

        // Mark as split holder
        //mergeSortHolder.IsSplitHolder = true;

        // Set gameobject parent
        mergeSortHolder.SuperElement = GetComponentInParent<SortMain>();
        // Make the pivot holder position visible
        //PivotHolderVisible(true);
        return mergeSortHolder;
    }


    public override void ExecuteStepByStepOrder(InstructionBase instruction, bool gotElement, bool increment)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        throw new System.NotImplementedException();
    }
    
    #region Merge Sort: All Moves User Test TODO: implement
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        return instructions;
    }
    #endregion


}
