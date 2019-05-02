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


    public override void InitTeachingAlgorithm(float algorithmSpeed)
    {
        base.InitTeachingAlgorithm(algorithmSpeed);
        numberOfElements = sortMain.SortSettings.NumberOfElements;
    }

    public override string AlgorithmName
    {
        get { return Util.MERGE_SORT; }
    }

    public override string CollectLine(int lineNr)
    {
        return "0: No pseudocode for merge sort yet.";
    }

    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        throw new System.NotImplementedException();
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
            case "....": FirstInstructionCodeLine(); break; // example: some void method
        }
    }

    public MergeSortHolder GetExtraHolder(int holderID)
    {
        int splitHolderIndex = numberOfElements - holderID;
        return (splitHolderIndex >= 0 && splitHolderIndex < splitHolders.Count) ? splitHolders[splitHolderIndex] : null;
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

    #region Merge Sort: OLD Demo visual (not working)
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
        Vector3 pos = sortMain.HolderManager.GetHolder(aboveHolder).transform.position + splitHolderPos;

        GameObject splitHolder = Instantiate(holderPrefab, pos, Quaternion.identity);
        splitHolder.AddComponent<MergeSortHolder>();

        MergeSortHolder mergeSortHolder = splitHolder.GetComponent<MergeSortHolder>();

        //pivotHolder = pivotHolderClone.GetComponent<InsertionSortHolder>();

        // Mark as split holder
        mergeSortHolder.IsSplitHolder = true;

        // Set gameobject parent
        mergeSortHolder.SuperElement = GetComponentInParent<SortMain>();

        // Make the pivot holder position visible
        //PivotHolderVisible(true);
        return mergeSortHolder;
    }

    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        throw new System.NotImplementedException();
    }

    #region Merge Sort: All Moves User Test TODO: implement
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] list)
    {
        instructions = new Dictionary<int, InstructionBase>();
        instNr = 0;
        nextMergeSortHolderID = list.Length;

        MergeSortInstruction[] mergeSortInitInst = new MergeSortInstruction[list.Length];
        for (int i=0; i < list.Length; i++)
        {
            mergeSortInitInst[i] = (MergeSortInstruction)list[i];
        }

        MergeSortInstructions(mergeSortInitInst);

        return instructions;
    }

    private Dictionary<int, InstructionBase> instructions;
    private int instNr;
    private int nextMergeSortHolderID;
    private bool firstSplit = true;
    public const string CREATE_HOLDER = "Create holder", MOVE_ELEMENT_TO_MERGE_HOLDER = "Move element to merge holder", MERGE_START = "Merge start", MERGE_ELEMENT = "Merge element";
    public const string MERGE_REMAINING_ELEMENT = "Merge remaining element";

    private MergeSortInstruction[] MergeSortInstructions(MergeSortInstruction[] list)
    {
        Debug.Log("Test: Length " + list.Length);
        if (list.Length <= 1)
        {
            return list;
        }

        int leftLength = list.Length / 2;
        int rightLength = list.Length - leftLength;

        MergeSortInstruction[] left = new MergeSortInstruction[leftLength];
        MergeSortInstruction[] right = new MergeSortInstruction[rightLength];


        int leftHolderID = -1, rightHolderID = -1;
        if (firstSplit)
        {
            // Create new holders
            leftHolderID = 0;
            rightHolderID = numberOfElements / 2;
            instructions.Add(instNr++, new MergeSortHolderInstruction(CREATE_HOLDER, instNr, leftHolderID));
            instructions.Add(instNr++, new MergeSortHolderInstruction(CREATE_HOLDER, instNr, rightHolderID));
            firstSplit = false;
        }
        else
        {
            // When the number of extra holders are equal to the number of elements, place half at the same holder they are standing on, the rest the next holder to the right
            leftHolderID = list[0].HolderID;
            rightHolderID = leftHolderID + 1;
            instructions.Add(instNr++, new MergeSortHolderInstruction(CREATE_HOLDER, instNr, rightHolderID));
        }

        for (int x = 0; x < list.Length; x++)
        {
            if (x < leftLength)
            {
                left[x] = list[x];
                instructions.Add(instNr++, new MergeSortInstruction(MOVE_ELEMENT_TO_MERGE_HOLDER, instNr, x, Util.NO_VALUE, Util.NO_VALUE, list[x].SortingElementID, list[x].HolderID, leftHolderID, list[x].Value, false, false));
            }
            else
            {
                right[x - rightLength] = list[x];
                instructions.Add(instNr++, new MergeSortInstruction(MOVE_ELEMENT_TO_MERGE_HOLDER, instNr, x, Util.NO_VALUE, Util.NO_VALUE, list[x].SortingElementID, list[x].HolderID, rightHolderID, list[x].Value, false, false));
            }
        }

        left = MergeSortInstructions(left);
        right = MergeSortInstructions(right);

        return MergeInstructions(left, right);
    }

    private MergeSortInstruction[] MergeInstructions(MergeSortInstruction[] left, MergeSortInstruction[] right)
    {
        Debug.Log("Merge: left length = " + left.Length + ", right length = " + right.Length);

        MergeSortInstruction[] result = new MergeSortInstruction[left.Length + right.Length];
        int l = 0, r = 0;

        instructions.Add(instNr++, new InstructionBase(MERGE_START, instNr));

        while (l < left.Length && r < right.Length)
        {
            instructions.Add(instNr++, new MergeSortInstruction(UtilSort.COMPARE_START_INST, instNr, l + r, Util.NO_VALUE, Util.NO_VALUE, left[l].SortingElementID, left[l].HolderID, UtilSort.NO_DESTINATION, left[l].Value, true, false));
            instructions.Add(instNr++, new MergeSortInstruction(UtilSort.COMPARE_START_INST, instNr, l + r, Util.NO_VALUE, Util.NO_VALUE, right[r].SortingElementID, right[r].HolderID, UtilSort.NO_DESTINATION, right[r].Value, true, false));

            if (left[l].Value <= right[r].Value)
            {
                result[l + r] = left[l];
                l += 1;
            }
            else
            {
                result[l + r] = right[r];
                r += 1;
            }

            // Add merged element
            int q = l + r - 1;
            bool isSorted = left.Length + right.Length == sortMain.SortSettings.NumberOfElements;
            instructions.Add(instNr++, new MergeSortInstruction(MERGE_ELEMENT, instNr, l + r, Util.NO_VALUE, Util.NO_VALUE, result[q].SortingElementID, result[q].HolderID, l + r, result[q].Value, false, isSorted));

        }

        while (l < left.Length)
        {
            result[l + r] = left[l];
            l += 1;

            // Add remaining merging element
            int q = l - 1;
            bool isSorted = left.Length + right.Length == sortMain.SortSettings.NumberOfElements;
            instructions.Add(instNr++, new MergeSortInstruction(MERGE_REMAINING_ELEMENT, instNr, l, 0, Util.NO_VALUE, result[q].SortingElementID, result[q].HolderID, l, result[q].Value, false, isSorted));
        }

        while (r < right.Length)
        {
            result[l + r] = right[r];
            r += 1;

            // Add remaining merging element
            int q = r - 1;
            bool isSorted = left.Length + right.Length == sortMain.SortSettings.NumberOfElements;
            instructions.Add(instNr++, new MergeSortInstruction(MERGE_REMAINING_ELEMENT, instNr, r, 1, Util.NO_VALUE, result[q].SortingElementID, result[q].HolderID, r, result[q].Value, true, isSorted));
        }
        return result;
    }
    #endregion

    public override IEnumerator ExecuteDemoInstruction(InstructionBase instruction, bool increment)
    {
        // Gather information from instruction
        MergeSortInstruction mergeSortInstruction = null;
        MergeSortHolderInstruction mergeSortHolderInstruction = null;
        MergeSortElement sortingElement = null;
        MergeSortHolder holder = null;

        if (instruction is MergeSortInstruction)
        {
            mergeSortInstruction = (MergeSortInstruction)instruction;
            sortingElement = sortMain.ElementManager.GetSortingElement(mergeSortInstruction.SortingElementID).GetComponent<MergeSortElement>();
            holder = sortMain.HolderManager.GetHolder(mergeSortInstruction.HolderID).GetComponent<MergeSortHolder>();
        }
        else if (instruction is MergeSortHolderInstruction)
        {
            mergeSortHolderInstruction = (MergeSortHolderInstruction)instruction;

        }
        else if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            //k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = 0;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = 0;

                if (increment)
                {
                    lengthOfList = sortMain.SortSettings.NumberOfElements.ToString();
                }
                else
                {
                    lengthOfList = "N";
                }
                break;

            case CREATE_HOLDER:
                lineOfCode = 0;

                if (increment)
                {
                    CreateSplitHolder(mergeSortHolderInstruction.MergeHolderID);
                }
                else
                {
                    Destroy(splitHolders[splitHolders.Count - 1].gameObject);
                }
                break;

            case MOVE_ELEMENT_TO_MERGE_HOLDER:
                lineOfCode = 0;
                if (increment)
                {
                    sortingElement.transform.position = mergeSortManager.GetCorrectHolder(mergeSortInstruction.NextHolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                }
                else
                {
                    sortingElement.transform.position = sortMain.HolderManager.GetHolder(mergeSortInstruction.HolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                }
                break;

            case MERGE_START:
                lineOfCode = 0;
                if (increment)
                {

                }
                else
                {

                }
                break;

            case UtilSort.COMPARE_START_INST:
                lineOfCode = 0;

                if (increment)
                {
                    sortingElement.IsCompare = mergeSortInstruction.IsCompare;
                    sortingElement.IsSorted = mergeSortInstruction.IsSorted;
                }
                else
                {
                    sortingElement.IsCompare = !mergeSortInstruction.IsCompare;
                    if (mergeSortInstruction.HolderID == sortingElement.SortingElementID) // works for worst case, none might be buggy
                        sortingElement.IsSorted = mergeSortInstruction.IsSorted;
                    else
                        sortingElement.IsSorted = !mergeSortInstruction.IsSorted;
                }

                //PreparePseudocodeValue(sortingElement.Value, 2);
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case MERGE_ELEMENT:
                lineOfCode = 0;

                if (increment)
                {
                    sortingElement.transform.position = sortMain.HolderManager.GetHolder(mergeSortInstruction.NextHolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                }
                else
                {
                    sortingElement.transform.position = sortMain.HolderManager.GetHolder(mergeSortInstruction.HolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                }
                break;

            case MERGE_REMAINING_ELEMENT:
                lineOfCode = 0;

                if (increment)
                {
                    sortingElement.transform.position = sortMain.HolderManager.GetHolder(mergeSortInstruction.NextHolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                }
                else
                {
                    sortingElement.transform.position = sortMain.HolderManager.GetHolder(mergeSortInstruction.HolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                }
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode = 0;
                break;
        }
        prevHighlightedLineOfCode = lineOfCode;

        // Highlight part of code in pseudocode
        pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode), Util.HIGHLIGHT_COLOR);
        yield return demoStepDuration;
        sortMain.WaitForSupportToComplete--;
    }
}
