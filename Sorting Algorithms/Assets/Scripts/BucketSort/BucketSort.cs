using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BucketManager))]
[RequireComponent(typeof(BucketSortManager))]
public class BucketSort : Algorithm {

    public const string CHOOSE_BUCKET = "Choose bucket", PUT_BACK_TO_HOLDER = "Put back to holder", NONE = "None";
    private Dictionary<int, string> pseudoCode;
    private BucketManager bucketManager;
    private BucketSortManager bucketSortManager;

    protected override void Awake()
    {
        bucketManager = GetComponent(typeof(BucketManager)) as BucketManager;
        bucketSortManager = GetComponent(typeof(BucketSortManager)) as BucketSortManager;
        base.Awake();
    }

    public override string AlgorithmName
    {
        get { return Util.BUCKET_SORT; }
    }

    public override string CollectLine(int lineNr)
    {
        switch (lineNr)
        {
            case 0: return "BucketSort(list, n)";
            case 1: return "    buckets = new array of n empty lists";
            case 2: return "    for i=0 to len(list):";
            case 3: return "        index = list[i] * n / MAX_VALUE";
            case 4: return "        buckets[index] <- list[i]";
            case 5: return "    end for";
            //case 6: return "    for i=0 to n:";
            case 6: return "    Sorting each bucket w/InsertionSort";
            //case 8: return "    end for";
            case 7: return "    k = 0";
            case 8: return "    for i=0 to n:";
            case 9: return "        for j={0} to len(buckets[i]):";
            case 10: return "            list[k] = buckets[i][j]";
            case 11: return "            k++";
            case 12: return "        end for";
            case 13: return "    end for";
            default: return "X";
        }

        //string temp = PseudoCode(lineNr, 0, 0, 0, true);
        //switch (lineNr)
        //{
        //    case 0: case 1: return temp.Replace(bucketSortManager.NumberOfBuckets.ToString(), "n");
        //    case 5: case 8: case 13: return temp;
        //    case 2: return temp.Replace((GetComponent<AlgorithmManagerBase>().NumberOfElements - 1).ToString(), "(len( list ) - 1)");
        //    case 3: return temp.Replace((Util.INIT_STATE - 1).ToString(), "index").Replace(Util.INIT_STATE.ToString(), "list[ i ] * n / max_value");
        //    case 4: return temp.Replace((Util.INIT_STATE - 1).ToString(), "index").Replace(Util.INIT_STATE.ToString(), "list[ i ]");
        //    case 6: return temp.Replace((bucketSortManager.NumberOfBuckets - 1).ToString(), "n-1");
        //    case 7: case 10: return temp.Replace("0", "i");
        //    case 11: return temp.Replace("0", "j");
        //    case 12: return temp.Replace("0", "j");

        //    default: return "lineNr " + lineNr + " not found!";
        //}
    }

    // value1 = value of element
    private int bucketIndex, loopRange;
    private string PseudoCode(int lineNr, int i, int j, int k, bool increment)
    {
        switch (lineNr)
        {
            case 0: return string.Format("BucketSort(list, {0}):", bucketSortManager.NumberOfBuckets);
            case 1: return string.Format("    buckets = new array of {0} empty lists", bucketSortManager.NumberOfBuckets);
            case 2: return string.Format("    for i={0} to {1}:", i, (GetComponent<AlgorithmManagerBase>().AlgorithmSettings.NumberOfElements - 1));
            case 3: return string.Format("        {0} = {1} * {2} / {3}", bucketIndex, value1, bucketSortManager.NumberOfBuckets, Util.MAX_VALUE);
            case 4: return string.Format("        buckets[{0}] <- {1}", bucketIndex, value1);
            case 5: return "    end for";
            //case 6: return string.Format("    for i={0} to {1}:", i, j);
            case 6: return "    Sorting each bucket w/InsertionSort";
            //case 8: return "    end for";
            case 7: return "    k = 0";
            case 8: return string.Format("    for i={0} to {1}:", i, (bucketSortManager.NumberOfBuckets - 1));
            case 9: return string.Format("        for j={0} to {1}:", j, loopRange);
            case 10: return string.Format("            list[{0}] = " + value1, k);//buckets[{1}][{2}]", k, i, j);
            case 11: return "            " + k + " = " + (k-1) + " + " + "1";
            case 12: return "        end for";
            case 13: return "    end for";
            default: return "X";
        }
    }

    public override int FirstInstructionCodeLine()
    {
        return 0;
    }

    public override int FinalInstructionCodeLine()
    {
        return 13;
    }

    public override void ResetSetup()
    {
        base.ResetSetup();
        Debug.Log("Nothing to reset?");
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        // > No destination
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FINAL_INSTRUCTION);
        // > No element
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.FIRST_LOOP);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.SET_VAR_J);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.UPDATE_VAR_J);


        // Bucket sort only
        // > No destination
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.BUCKET_INDEX_INST);

        // > No element
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.CREATE_BUCKETS_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.PHASING_INST);


        //skipDict[Util.SKIP_NO_ELEMENT].Add(Util.DISPLAY_ELEMENT);
        //skipDict[Util.SKIP_NO_ELEMENT].Add(Util.MOVE_TO_BUCKET_INST); ***
        //skipDict[Util.SKIP_NO_ELEMENT].Add(Util.MOVE_BACK_INST); ***
    }

    public override void Specials(string method, int number, bool activate)
    {
        switch (method)
        {
            case "Somemethod": FirstInstructionCodeLine(); break; // example: some void method
        }
    }

    #region Bucket Sort: Standard 1
    public static GameObject[] BucketSortStandard(GameObject[] sortingElements, int numberOfBuckets)
    {
        // Find min-/ max values
        int minValue = Util.MAX_VALUE, maxValue = 0;
        for (int i = 0; i < sortingElements.Length; i++)
        {
            int value = sortingElements[i].GetComponent<BucketSortElement>().Value;
            if (value > maxValue)
                maxValue = value;
            if (value < minValue)
                minValue = value;
        }

        // Create empty lists (buckets)
        List<GameObject>[] bucket = new List<GameObject>[maxValue - minValue + 1];
        for (int i = 0; i < bucket.Length; i++)
        {
            bucket[i] = new List<GameObject>();
        }

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            GameObject temp = sortingElements[i];
            bucket[temp.GetComponent<BucketSortElement>().Value - minValue].Add(temp);
        }

        // Put elements back into list
        int k = 0;
        for (int i = 0; i < bucket.Length; i++)
        {
            if (bucket[i].Count > 0)
            {
                for (int j = 0; j < bucket[i].Count; j++)
                {
                    sortingElements[k] = bucket[i][j];
                    k++;
                }
            }
        }
        return sortingElements;
    }
    #endregion

    #region Bucket Sort: Standard 2
    public static GameObject[] BucketSortStandard2(GameObject[] sortingElements, int numberOfBuckets)
    {
        // Find number of elements per bucket, + counter for later use
        int[] numberOfElementsPerBucket = new int[numberOfBuckets], counters = new int[numberOfBuckets];
        for (int i=0; i < sortingElements.Length; i++)
        {
            int index = BucketIndex(sortingElements[i].GetComponent<SortingElementBase>().Value, numberOfBuckets);
            numberOfElementsPerBucket[index] += 1;
            counters[index] += 1;
        }

        // Create empty lists (buckets)
        Dictionary<int, GameObject[]> buckets = new Dictionary<int, GameObject[]>();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            buckets[i] = new GameObject[numberOfElementsPerBucket[i]];
        }

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            int index = BucketIndex(sortingElements[i].GetComponent<SortingElementBase>().Value, numberOfBuckets);
            int placeIn = numberOfElementsPerBucket[index] - counters[index];
            counters[index]--;
            buckets[index][placeIn] = sortingElements[i];
        }

        // Sort each bucket by using Insertion Sort
        for (int i=0; i < numberOfBuckets; i++)
        {
            if (numberOfElementsPerBucket[i] > 0)
                buckets[i] = InsertionSort.InsertionSortStandard(buckets[i]);
        }

        // Put elements back into list
        int k = 0;
        for (int i = 0; i < numberOfBuckets; i++)
        {
            if (buckets[i].Length > 0)
            {
                for (int j = 0; j < buckets[i].Length; j++)
                {
                    sortingElements[k] = buckets[i][j];
                    k++;
                }
            }
        }
        return sortingElements;
    }

    private static int BucketIndex(int value, int numberOfBuckets)
    {
        return value * numberOfBuckets / Util.MAX_VALUE; // max + 1 ~?
    }

    #endregion

    #region Bucket Sort: Tutorial (Visual)
    public override IEnumerator Tutorial(GameObject[] sortingElements)
    {
        // Line 0 (set parameter)
        pseudoCodeViewer.SetCodeLine(0, PseudoCode(0, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = GetComponent<BucketSortManager>().NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);

        // Line 1 (Create buckets)
        pseudoCodeViewer.SetCodeLine(1, PseudoCode(1, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(1, PseudoCode(1, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

        // Buckets
        GameObject[] buckets = bucketManager.Buckets;

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            // Line 2 (Update for-loop)
            pseudoCodeViewer.SetCodeLine(2, PseudoCode(2, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(2, PseudoCode(2, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

            // Get element
            GameObject element = sortingElements[i];
            value1 = element.GetComponent<SortingElementBase>().Value;

            // Bucket index
            bucketIndex = BucketIndex(value1, numberOfBuckets);

            // Line 3 (Display bucket index)
            pseudoCodeViewer.SetCodeLine(3, PseudoCode(3, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(3, PseudoCode(3, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

            // Get bucket
            Bucket bucket = buckets[bucketIndex].GetComponent<Bucket>(); // element.GetComponent<SortingElementBase>().Value - minValue);

            // Move element above the bucket and put it inside
            element.transform.position = bucket.transform.position + Util.ABOVE_BUCKET_VR;

            // Line 4 (Put element into bucket)
            pseudoCodeViewer.SetCodeLine(4, PseudoCode(4, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(4, PseudoCode(4, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);
        }

        // Line 5 (end for-loop)
        pseudoCodeViewer.SetCodeLine(5, PseudoCode(5, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(5, PseudoCode(5, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

        // Display elements
        for (int x=0; x < numberOfBuckets; x++)
        {
            // Line 6 (For-loop: Sort elements in buckets)
            //pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, x, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            //yield return new WaitForSeconds(seconds);
            //pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, x, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

            Bucket bucket = buckets[x].GetComponent<Bucket>();
            bucket.DisplayElements = true;

            // Sort bucket *** TODO: go to insertion sort scene
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);

            // Line 6 (Sort elements in bucket)
            pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, x, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, x, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);


            // Put elements for display on top of buckets
            int numberOfElementsInBucket = bucket.CurrenHolding.Count;
            for (int y=0; y < numberOfElementsInBucket; y++)
            {
                SortingElementBase element = bucket.GetElementForDisplay(y);
                element.gameObject.active = true;
                element.transform.position += Util.ABOVE_BUCKET_VR;
                yield return new WaitForSeconds(seconds);
            }
        }
        // Line 8 (end for loop)
        //pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
        //yield return new WaitForSeconds(seconds);
        //pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

        // Put elements back into list
        int k = 0;
        // Line 7 (set k)
        pseudoCodeViewer.SetCodeLine(7, PseudoCode(7, Util.NO_VALUE, Util.NO_VALUE, 0, true), Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(7, PseudoCode(7, Util.NO_VALUE, Util.NO_VALUE, 0, true), Util.BLACKBOARD_TEXT_COLOR);

        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = GetComponent<HolderManager>().GetHolderPositions();
        // while (k < sortingElements.Length && i < numberOfBuckets)
        for (int i = 0; i < numberOfBuckets; i++)
        {
            Bucket bucket = buckets[i].GetComponent<Bucket>();

            // number of elements in bucket
            loopRange = bucket.CurrenHolding.Count;

            // Line 8 (For-loop: Concatenate all buckets)
            pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, i, Util.NO_VALUE, k, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, i, Util.NO_VALUE, k, true), Util.BLACKBOARD_TEXT_COLOR);

            for (int j = 0; j < loopRange; j++)
            {
                // Line 9 (2nd For-loop: Concatenate all buckets)
                pseudoCodeViewer.SetCodeLine(9, PseudoCode(9, i, j, k, true), Util.HIGHLIGHT_COLOR);
                yield return new WaitForSeconds(seconds);
                pseudoCodeViewer.SetCodeLine(9, PseudoCode(9, i, j, k, true), Util.BLACKBOARD_TEXT_COLOR);

                sortingElements[k] = bucket.RemoveSoringElement().gameObject;
                
                // Value of sorting element
                value1 = sortingElements[k].GetComponent<SortingElementBase>().Value;

                // Move element back to holder
                sortingElements[k].transform.position = holderPos[k] + Util.ABOVE_HOLDER_VR;
                sortingElements[k].transform.rotation = Quaternion.identity;
                sortingElements[k].GetComponent<SortingElementBase>().IsSorted = true;

                // Line 10 (Put element back into list)
                pseudoCodeViewer.SetCodeLine(10, PseudoCode(10, i, j, k, true), Util.HIGHLIGHT_COLOR);
                yield return new WaitForSeconds(seconds);
                pseudoCodeViewer.SetCodeLine(10, PseudoCode(10, i, j, k, true), Util.BLACKBOARD_TEXT_COLOR);

                k++;
                // Line 11 (Update k)
                pseudoCodeViewer.SetCodeLine(11, PseudoCode(11, i, j, k, true), Util.HIGHLIGHT_COLOR);
                yield return new WaitForSeconds(seconds);
                pseudoCodeViewer.SetCodeLine(11, PseudoCode(11, i, j, k, true), Util.BLACKBOARD_TEXT_COLOR);
            }
            // Line 12 (2nd for-inner-loop end)
            pseudoCodeViewer.SetCodeLine(12, PseudoCode(12, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            yield return new WaitForSeconds(seconds);
            pseudoCodeViewer.SetCodeLine(12, PseudoCode(12, i, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);
        }
        // Line 13 (2nd for-loop end)
        pseudoCodeViewer.SetCodeLine(13, PseudoCode(13, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
        yield return new WaitForSeconds(seconds);
        pseudoCodeViewer.SetCodeLine(13, PseudoCode(13, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

        IsSortingComplete = true;
    }
    #endregion


    #region Execute order from user
    public override void ExecuteStepByStepOrder(InstructionBase instruction, bool gotSortingElement, bool increment)
    {
        // Gather information from instruction
        BucketSortInstruction bucketInstruction = null;
        BucketSortElement sortingElement = null;
        int i = Util.NO_VALUE, j = Util.NO_VALUE, k = Util.NO_VALUE;

        if (gotSortingElement)
        {
            bucketInstruction = (BucketSortInstruction)instruction;
            Debug.Log("Debug: " + bucketInstruction.DebugInfo() + "\n");

            // Change internal state of sorting element
            sortingElement = GetComponent<ElementManager>().GetSortingElement(bucketInstruction.SortingElementID).GetComponent<BucketSortElement>();
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], Util.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        List<int> lineOfCode = new List<int>();
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case Util.CREATE_BUCKETS_INST:
                lineOfCode.Add(1);
                break;

            case Util.FIRST_LOOP:
                lineOfCode.Add(2);
                break;

            case Util.BUCKET_INDEX_INST:
                lineOfCode.Add(3);
                value1 = sortingElement.Value;
                bucketIndex = bucketInstruction.BucketID;

                if (increment)
                    sortingElement.IsCompare = bucketInstruction.IsCompare;
                else
                    sortingElement.IsCompare = !bucketInstruction.IsCompare;

                Util.IndicateElement(sortingElement.gameObject);
                break;

            case Util.MOVE_TO_BUCKET_INST:
                lineOfCode.Add(4);
                value1 = sortingElement.Value;
                bucketIndex = bucketInstruction.BucketID;

                if (increment)
                    sortingElement.IsCompare = bucketInstruction.IsCompare;
                else
                    sortingElement.IsCompare = !bucketInstruction.IsCompare;
                Util.IndicateElement(sortingElement.gameObject);
                break;

            case Util.END_LOOP_INST:
                if (j < 0)
                {
                    switch (j)
                    {
                        case Util.LOOP_ONE: lineOfCode.Add(5); break;
                        case Util.LOOP_TWO: lineOfCode.Add(12); break;
                        default: Debug.LogError(Util.END_LOOP_INST + ": '" + j + "' loop not found"); break;
                    }
                }
                break;

            case Util.PHASING_INST:
                lineOfCode.Add(6);
                //lineOfCode.Add(7);
                //lineOfCode.Add(8);
                i = 0;
                j = (bucketSortManager.NumberOfBuckets - 1);
                bucketIndex = j;
                bucketManager.AutoSortBuckets();
                break;

            case Util.DISPLAY_ELEMENT:
                bucketManager.PutElementsForDisplay(i);
                break;

            case Util.SET_VAR_J:
                lineOfCode.Add(7);
                break;

            case Util.UPDATE_LOOP_INST:
                if (j == Util.NO_VALUE)
                {
                    j = 0;
                    lineOfCode.Add(8);
                }
                else
                    lineOfCode.Add(9);

                loopRange = j;
                break;

            case Util.MOVE_BACK_INST:
                lineOfCode.Add(10);
                value1 = sortingElement.Value;
                k = bucketInstruction.NextHolderID;
                if (increment)
                    sortingElement.IsSorted = bucketInstruction.IsSorted;
                else
                    sortingElement.IsSorted = !bucketInstruction.IsSorted;

                break;

            case Util.UPDATE_VAR_J:
                lineOfCode.Add(11);
                break;

            case Util.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(lineOfCode[x], PseudoCode(lineOfCode[x], i, j, k, increment), Util.HIGHLIGHT_COLOR);
        }

        // Move sorting element
        if (gotSortingElement)
        {
            switch (bucketInstruction.Instruction)
            {
                case Util.MOVE_TO_BUCKET_INST:
                    if (increment)
                    {
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + Util.ABOVE_BUCKET_VR;
                    }
                    else
                    {
                        sortingElement.transform.position = bucketSortManager.GetCorrectHolder(bucketInstruction.HolderID).transform.position + Util.ABOVE_HOLDER_VR;
                        sortingElement.gameObject.SetActive(true);
                    }
                    break;

                case Util.DISPLAY_ELEMENT:
                    if (increment)
                    {
                        sortingElement.CanEnterBucket = false;
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + Util.ABOVE_BUCKET_VR;
                        sortingElement.gameObject.SetActive(true);
                    }
                    else
                    {
                        sortingElement.CanEnterBucket = true;
                        sortingElement.gameObject.SetActive(false);
                    }

                    break;

                case Util.MOVE_BACK_INST:
                    if (increment)
                        sortingElement.transform.position = bucketSortManager.GetCorrectHolder(bucketInstruction.NextHolderID).transform.position + Util.ABOVE_HOLDER_VR;
                    else
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + Util.ABOVE_BUCKET_VR;
                    break;
            }
        }
    }
    #endregion

    #region User test display help
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        // Gather information from instruction
        BucketSortInstruction bucketInstruction = null;
        BucketSortElement sortingElement = null;
        int i = Util.NO_VALUE, j = Util.NO_VALUE, k = Util.NO_VALUE;

        if (gotSortingElement)
        {
            bucketInstruction = (BucketSortInstruction)instruction;
            Debug.Log("Debug: " + bucketInstruction.DebugInfo() + "\n");

            // Change internal state of sorting element
            sortingElement = GetComponent<ElementManager>().GetSortingElement(bucketInstruction.SortingElementID).GetComponent<BucketSortElement>();
        }

        if (instruction is InstructionLoop)
        {
            i = ((InstructionLoop)instruction).I;
            j = ((InstructionLoop)instruction).J;
            k = ((InstructionLoop)instruction).K;
        }

        // Remove highlight from previous instruction
        for (int x = 0; x < prevHighlight.Count; x++)
        {
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], Util.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        List<int> lineOfCode = new List<int>();
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case Util.CREATE_BUCKETS_INST:
                lineOfCode.Add(1);
                break;

            case Util.FIRST_LOOP:
                lineOfCode.Add(2);
                break;

            case Util.BUCKET_INDEX_INST:
                lineOfCode.Add(3);
                value1 = sortingElement.Value;

                sortingElement.IsCompare = bucketInstruction.IsCompare;
                Util.IndicateElement(sortingElement.gameObject);
                break;

            case Util.MOVE_TO_BUCKET_INST:
                lineOfCode.Add(4);
                value1 = sortingElement.Value;

                sortingElement.IsCompare = bucketInstruction.IsCompare;
                Util.IndicateElement(sortingElement.gameObject);
                break;

            case Util.END_LOOP_INST:
                if (j < 0)
                {
                    switch (j)
                    {
                        case Util.LOOP_ONE: lineOfCode.Add(5); break;
                        case Util.LOOP_TWO: lineOfCode.Add(14); break;
                        default: Debug.LogError(Util.END_LOOP_INST + ": '" + j + "' loop not found"); break;
                    }
                }
                break;

            case Util.PHASING_INST:
                lineOfCode.Add(6);
                lineOfCode.Add(7);
                lineOfCode.Add(8);
                break;

            case Util.DISPLAY_ELEMENT:
                bucketSortManager.PutElementsForDisplay(i);
                break;

            case Util.SET_VAR_J:
                lineOfCode.Add(9);
                break;

            case Util.UPDATE_LOOP_INST:
                if (j == Util.NO_VALUE)
                    lineOfCode.Add(10);
                else
                    lineOfCode.Add(11);
                break;

            case Util.MOVE_BACK_INST:
                lineOfCode.Add(12);
                break;

            case Util.UPDATE_VAR_J:
                lineOfCode.Add(13);
                break;

            case Util.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(lineOfCode[x], PseudoCode(lineOfCode[x], i, j, k, true), Util.HIGHLIGHT_COLOR);
        }

        yield return new WaitForSeconds(seconds);
        bucketSortManager.BeginnerWait = false;
    }
    #endregion


    #region Bucket Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instructionNr = 0;

        // Line 0 (set parameter)
        instructions.Add(instructionNr++, new InstructionBase(Util.FIRST_INSTRUCTION, instructionNr)); // new InstructionBase(Util.FIRST_INSTRUCTION, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = GetComponent<BucketSortManager>().NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);

        // Line 1 (Create buckets)
        instructions.Add(instructionNr++, new InstructionBase(Util.CREATE_BUCKETS_INST, instructionNr)); // new InstructionBase(Util.CREATE_BUCKETS_INST, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            // Line 2 (Update for-loop)
            instructions.Add(instructionNr++, new InstructionLoop(Util.FIRST_LOOP, instructionNr, i, Util.LOOP_ONE, Util.NO_VALUE)); // new InstructionBase(Util.FIRST_LOOP, instructionNr, i, Util.LOOP_ONE, false, false)); // create one unique instruction for each loop, or "cheat" using the parametre?

            // Get element
            BucketSortInstruction element = (BucketSortInstruction)sortingElements[i];
            int bucketIndex = BucketIndex(element.Value, numberOfBuckets);

            // Line 3 (Display bucket index)
            instructions.Add(instructionNr++, new BucketSortInstruction(Util.BUCKET_INDEX_INST, instructionNr, i, Util.NO_VALUE, Util.NO_VALUE, element.SortingElementID, element.Value, true, false, element.HolderID, Util.NO_DESTINATION, bucketIndex)); // new BucketSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, i, Util.NO_VALUE, bucketIndex, Util.BUCKET_INDEX_INST, instructionNr, value, false, true, false));

            // Line 4 (Put element into bucket)
            instructions.Add(instructionNr++, new BucketSortInstruction(Util.MOVE_TO_BUCKET_INST, instructionNr, i, Util.NO_VALUE, Util.NO_VALUE, element.SortingElementID, element.Value, false, false, element.HolderID, Util.NO_DESTINATION, bucketIndex)); // new BucketSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, i, Util.NO_VALUE, bucketIndex, Util.MOVE_TO_BUCKET_INST, instructionNr, value, false, false, false));
        }

        // Line 5 (end for-loop)
        instructions.Add(instructionNr++, new InstructionLoop(Util.END_LOOP_INST, instructionNr, Util.NO_VALUE, Util.LOOP_ONE, Util.NO_VALUE)); // new InstructionBase(Util.END_LOOP_INST, instructionNr, Util.NO_VALUE, Util.LOOP_ONE, false, false));

        // Line 6, 7, 8 (make the buckets sort what they hold)
        instructions.Add(instructionNr++, new InstructionBase(Util.PHASING_INST, instructionNr)); // new InstructionBase(Util.PHASING_INST, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Sorting elements
        int[] values = new int[sortingElements.Length];
        for (int x = 0; x < sortingElements.Length; x++)
        {
            values[x] = ((BucketSortInstruction)sortingElements[x]).Value;
        }
        int[] sorted = InsertionSort.InsertionSortFixCase(values, false);

        // Creating fictionary buckets
        Dictionary<int, List<BucketSortInstruction>> buckets = new Dictionary<int, List<BucketSortInstruction>>();
        for (int x=0; x < numberOfBuckets; x++)
        {
            buckets[x] = new List<BucketSortInstruction>();
        }

        // Look for correct value and add element to bucket
        for (int x = 0; x < sorted.Length; x++)
        {
            BucketSortInstruction temp = null;
            for (int y = 0; y < sortingElements.Length; y++)
            {
                BucketSortInstruction t = (BucketSortInstruction)sortingElements[y];
                if (sorted[x] == t.Value)
                {
                    int bucketIndex = BucketIndex(t.Value, bucketSortManager.NumberOfBuckets);
                    BucketSortInstruction displayInstruction = new BucketSortInstruction(Util.DISPLAY_ELEMENT, instructionNr, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, t.SortingElementID, t.Value, false, true, Util.NO_VALUE, Util.NO_DESTINATION, bucketIndex); //new BucketSortInstruction(t.SortingElementID, Util.NO_VALUE, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, bucketIndex, Util.DISPLAY_ELEMENT, instructionNr, t.Value, false, false, true);
                    instructions.Add(instructionNr++, displayInstruction);
                    buckets[bucketIndex].Add(displayInstruction);
                    break;
                }
            }
        }

        int k = 0;
        // Line 9 (For-loop: Concatenate all buckets)
        instructions.Add(instructionNr++, new InstructionBase(Util.SET_VAR_J, instructionNr)); // new InstructionBase(Util.SET_VAR_J, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = GetComponent<HolderManager>().GetHolderPositions();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            List<BucketSortInstruction> bucket = buckets[i];
            int numberOfElementsInBucket = bucket.Count;

            // Line 10 (For-loop: Concatenate all buckets)
            instructions.Add(instructionNr++, new InstructionLoop(Util.UPDATE_LOOP_INST, instructionNr, i, Util.NO_VALUE, k)); // new InstructionBase(Util.UPDATE_LOOP_INST, instructionNr, i, Util.NO_VALUE, false, false));

            for (int j = 0; j < numberOfElementsInBucket; j++)
            {
                // Line 11 (2nd For-loop: Concatenate all buckets)
                instructions.Add(instructionNr++, new InstructionLoop(Util.UPDATE_LOOP_INST, instructionNr, i, j, k)); // new InstructionBase(Util.UPDATE_LOOP_INST, instructionNr, i, j, false, false));

                // Line 12 (Put element back into list)
                instructions.Add(instructionNr++, new BucketSortInstruction(Util.MOVE_BACK_INST, instructionNr, i, j, k, bucket[j].SortingElementID, bucket[j].Value, false, true, Util.NO_VALUE, k, bucket[j].BucketID)); // new BucketSortInstruction(bucket[j].SortingElementID, Util.NO_VALUE, k, i, j, bucket[j].BucketID, Util.MOVE_BACK_INST, instructionNr, bucket[j].Value, false, false, true));

                k++;
                // Line 13 (Update k)
                instructions.Add(instructionNr++, new InstructionLoop(Util.UPDATE_VAR_J, instructionNr, Util.NO_VALUE, Util.NO_VALUE, k)); // new InstructionBase(Util.UPDATE_VAR_J, instructionNr, k, Util.NO_VALUE, false, false)); ******
            }
            // Line 14 (2nd for-loop end)
            instructions.Add(instructionNr++, new InstructionLoop(Util.END_LOOP_INST, instructionNr, i, Util.LOOP_TWO, Util.NO_VALUE)); // new InstructionBase(Util.END_LOOP_INST, instructionNr, i, Util.LOOP_TWO, false, false)); **** i=?
        }
        // Line 15 (2nd for-loop end)
        instructions.Add(instructionNr++, new InstructionBase(Util.FINAL_INSTRUCTION, instructionNr)); // new InstructionBase(Util.FINAL_INSTRUCTION, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        //status = NONE;
        return instructions;
    }
    #endregion





    // ---------------------------------------- Extras ----------------------------------------





}

