using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : Algorithm {

    /* --------------------------------------------------- Insertion Sort --------------------------------------------------- 
     * 1) Starts from 2nd and checks if it is less than the first
     * 2) Moves if smaller
     * *) Repeat, but now check all slots to the left of pivot
     */

    [SerializeField]
    private GameObject pivotHolderPrefab;
    private GameObject pivotHolderClone; // TODO: change to Holder
    private InsertionSortHolder pivotHolder;
    private Vector3 tutorialHeight1 = new Vector3(0f, 1f, 0f), tutorialHeight2;
    private Vector3 userTestHeight = new Vector3(0f, 1.5f, 0f);

    private Dictionary<int, string> pseudoCode;

    private void Awake()
    {
        tutorialHeight2 = tutorialHeight1 + new Vector3(0f, 0.2f, 0f);
    }

    public override string GetAlgorithmName()
    {
        return Util.INSERTION_SORT;
    }

    public HolderBase PivotHolder
    {
        get { return pivotHolder; }
    }

    public override Dictionary<int, string> PseudoCode
    {
        get { return pseudoCode; }
        set { pseudoCode = value; }
    }


    public override void ResetSetup()
    {
        Destroy(pivotHolder);
    }

    public void CreatePivotHolder()
    {
        // Instantiate
        Vector3 pos;
        if (GetComponent<AlgorithmManagerBase>().IsTutorial)
            pos = GetComponent<AlgorithmManagerBase>().HolderPositions[1] + tutorialHeight1;
        else
            pos = GetComponent<AlgorithmManagerBase>().HolderPositions[1] + userTestHeight;
        pivotHolderClone = Instantiate(pivotHolderPrefab, pos, Quaternion.identity);
        pivotHolder = pivotHolderClone.GetComponent<InsertionSortHolder>();
        // Mark as pivotholder
        pivotHolder.IsPivotHolder = true;
        // Set gameobject parent
        pivotHolder.Parent = gameObject;
        // Make the pivot holder position visible
        PivotHolderVisible(true);
    }


    #region Insertion Sort: Standard (No visuals)
    public static SortingElementBase[] InsertionSortStandard(SortingElementBase[] list, bool reverse)
    {
        int i = 1;
        while (i < list.Length)
        {
            SortingElementBase pivot = list[i];
            int j = i - 1;
            while (j >= 0)
            {
                if (reverse && pivot.Value > list[j].Value || pivot.Value < list[j].Value)
                {
                    list[j + 1] = list[j];
                    j -= 1;
                }
            }
            list[j + 1] = pivot;
            i += 1;
        }
        return list;
    }
    #endregion

    #region Insertion Sort: All Moves Tutorial (Visuals)
    public override IEnumerator Tutorial(GameObject[] list)
    {
        // Create pivot holder
        CreatePivotHolder();

        Vector3 temp = new Vector3();
        int i = 1;
        while (i < list.Length)
        {
            // Get pivot
            GameObject pivotObj = list[i];
            InsertionSortElement pivot = list[i].GetComponent<InsertionSortElement>();
            pivot.IsPivot = true;

            // Get pivot's initial position
            temp = pivot.transform.position;

            // Place pivot holder above the pivot element
            pivotHolder.transform.position = temp + tutorialHeight1;

            // Place the pivot on top of the pivot holder
            pivot.transform.position = temp + tutorialHeight2;

            // Wait to show the pivot
            yield return new WaitForSeconds(seconds);

            // Get index of first element to the left of the pivot and compare
            int j = i - 1;

            // Start comparing until find the correct position is found
            // Set first values here to display on blackboard
            pivotValue = pivot.Value;
            compareToValue = list[j].GetComponent<InsertionSortElement>().Value;
            while (pivotValue < compareToValue)
            {
                // Pivot is smaller, start moving compare element
                // Prepare the element to compare with
                GameObject compareObj = list[j];
                InsertionSortElement compare = compareObj.GetComponent<InsertionSortElement>();

                // Compare element's position
                Vector3 temp2 = compare.transform.position;

                // Moving other element one index to the right
                compare.transform.position = temp;
                // Updating list
                list[j + 1] = compareObj;

                // Preparing for next step
                temp = temp2;
                j -= 1;

                // Check if there are more elements to compare the pivot with
                if (j >= 0)
                    // More elements to evaluate
                    compareToValue = list[j].GetComponent<InsertionSortElement>().Value;
                else
                {
                    // Make sure that the last compare element is marked as sorted
                    compare.IsSorted = true; //list[j + 1].GetComponent<SortingElement>().IsSorted = true;
                    break;
                }
                // Move pivot out and place it ontop of pivot holder (above holder it check whether it's put the element)
                pivotHolder.transform.position = temp + tutorialHeight1;
                pivot.transform.position = temp + tutorialHeight2;

                // Wait to show the pivot being moved
                yield return new WaitForSeconds(seconds);
            }
            // Make sure the 1st element is marked sorted in the first round
            if (i == 1 && pivotValue >= compareToValue)
            {
                InsertionSortElement firstSortingElement = list[0].GetComponent<InsertionSortElement>();
                firstSortingElement.IsSorted = true;
                firstSortingElement.transform.position += tutorialHeight1;
            }

            // Finish off the pivots work
            pivot.IsSorted = true;
            pivot.IsPivot = false;
            pivot.transform.position = temp;
            // Return pivot object to list, and increment 'i'
            list[j + 1] = pivotObj;
            i += 1;
            yield return new WaitForSeconds(seconds);
        }
        // Mark the last element sorted
        list[list.Length - 1].GetComponent<InsertionSortElement>().IsSorted = true;

        // Finished off; remove pivot holder
        PivotHolderVisible(false);
        IsSortingComplete = true;
    }
    #endregion

    #region Insertion Sort: All Moves (Debug log: OK!)
    // ElementState[] deep copy of our list we want to sort
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        // Create pivot holder
        CreatePivotHolder();

        Dictionary<int, InstructionBase> allMoves = new Dictionary<int, InstructionBase>();

        InsertionSortInstruction compareElement;

        int i = 1, move = 1;
        while (i < sortingElements.Length)
        {
            int temp1 = ((InsertionSortInstruction)sortingElements[i]).HolderID; // pivot: temp1 -> temp2*
            InsertionSortInstruction pivot = new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[i]).SortingElementID, temp1, pivotHolder.HolderID, Util.PIVOT_START_INST, ((InsertionSortInstruction)sortingElements[i]).Value, true, false, false);
            sortingElements[i] = pivot;

            // Add this move (Pivot moved in pivot position)
            allMoves.Add(move, pivot);
            move++;

            int j = i - 1;
            while (true)
            {
                // Choose a new compare element
                compareElement = new InsertionSortInstruction(((InsertionSortInstruction)sortingElements[j]).SortingElementID, j, Util.NO_INSTRUCTION, Util.COMPARE_START_INST, ((InsertionSortInstruction)sortingElements[j]).Value, false, true, sortingElements[j].IsSorted);
                sortingElements[j] = compareElement;
                //
                allMoves.Add(move, compareElement);
                move++;

                // Pivot larger than compare element, place compare element
                if (pivot.Value >= compareElement.Value)
                {
                    allMoves.Add(move, new InsertionSortInstruction(compareElement.SortingElementID, j, Util.NO_INSTRUCTION, Util.COMPARE_END_INST, compareElement.Value, false, false, true));
                    move++;
                    break;
                }

                // Pivot is less than compare element, switch their spots
                int temp2 = compareElement.HolderID;
                sortingElements[j + 1] = compareElement;
                sortingElements[j] = pivot;
                j -= 1;

                // Add this move (compare element switched to pivot/next position)
                allMoves.Add(move, new InsertionSortInstruction(compareElement.SortingElementID, compareElement.HolderID, temp1, Util.SWITCH_INST, compareElement.Value, false, false, true));
                move++;

                // temp2 is open spot, temp1 will be given to next compare element or place pivot there
                temp1 = temp2;

                if (j < 0)
                    break;
            }

            sortingElements[j + 1] = pivot;
            i += 1;

            // Add this move (pivot sorted)
            allMoves.Add(move, new InsertionSortInstruction(pivot.SortingElementID, pivotHolder.HolderID, temp1, Util.PIVOT_END_INST, pivot.Value, false, false, true));
            move += 1;
        }
        return allMoves;
    }
    #endregion


    // ----------------------------- Help functions -----------------------------

    private void PivotHolderVisible(bool enable)
    {
        pivotHolderClone.GetComponentInChildren<MeshRenderer>().enabled = enable;
    }
}
