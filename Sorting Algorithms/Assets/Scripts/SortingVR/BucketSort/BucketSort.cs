using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BucketManager))]
//[RequireComponent(typeof(BucketSortManager))]
public class BucketSort : SortAlgorithm {

    // value1 = value of element
    private int bucketIndex, loopRange;

    public const string CHOOSE_BUCKET = "Choose bucket", PUT_BACK_TO_HOLDER = "Put back to holder", NONE = "None";
    private Dictionary<int, string> pseudoCode;


    [SerializeField]
    private BucketManager bucketManager;

    [SerializeField]
    private BucketSortManager bucketSortManager;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    bucketSortManager = GetComponent(typeof(BucketSortManager)) as BucketSortManager;
    //}

    public override string AlgorithmName
    {
        get { return UtilSort.BUCKET_SORT; }
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;

        switch (lineNr)
        {
            case 0: lineOfCode += string.Format("BucketSort(list, {0}):", bucketSortManager.NumberOfBuckets); break;
            case 1: lineOfCode += string.Format("    buckets = new array of {0} empty lists", bucketSortManager.NumberOfBuckets); break;
            case 2: lineOfCode += string.Format("    for i={0} to {1}:", i, (sortMain.SortSettings.NumberOfElements - 1)); break;
            case 3: lineOfCode += string.Format("        {0} = {1} * {2} / {3}", bucketIndex, value1, bucketSortManager.NumberOfBuckets, UtilSort.MAX_VALUE); break;
            case 4: lineOfCode += string.Format("        buckets[{0}] <- {1}", bucketIndex, value1); break;
            case 5: lineOfCode += "    end for"; break;
            case 6: lineOfCode += "    Sorting each bucket w/InsertionSort"; break; //case 6: return string.Format("    for i={0} to {1}:", i, j);
            case 7: lineOfCode += "    k = 0"; break;
            case 8: lineOfCode += string.Format("    for i={0} to {1}:", i, (bucketSortManager.NumberOfBuckets - 1)); break;
            case 9: lineOfCode += string.Format("        for j={0} to {1}:", j, loopRange); break;
            case 10: lineOfCode += string.Format("            list[{0}] = " + value1, k); break; //buckets[{1}][{2}]", k, i, j);
            case 11: lineOfCode += "            " + k + " = " + (k-1) + " + " + "1"; break;
            case 12: lineOfCode += "        end for"; break;
            case 13: lineOfCode += "    end for"; break;
            default: return "X";
        }
        return lineOfCode;
    }
        //switch (lineNr)
        //{
        //    case 0: return "BucketSort(list, n)";
        //    case 1: return "    buckets = new array of n empty lists";
        //    case 2: return "    for i=0 to len(list)-1:";
        //    case 3: return "        index = list[i] * n / MAX_VALUE";
        //    case 4: return "        buckets[index] <- list[i]";
        //    case 5: return "    end for";
        //    //case 6: return "    for i=0 to n-1:";
        //    case 6: return "    Sorting each bucket w/InsertionSort";
        //    //case 8: return "    end for";
        //    case 7: return "    k = 0";
        //    case 8: return "    for i=0 to n:";
        //    case 9: return "        for j={0} to len(buckets[i]):";
        //    case 10: return "            list[k] = buckets[i][j]";
        //    case 11: return "            k++";
        //    case 12: return "        end for";
        //    case 13: return "    end for";
        //    default: return "X";
        //}

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
        bucketManager.ResetSetup();
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        // > No destination
        skipDict.Add(UtilSort.SKIP_NO_DESTINATION, new List<string>());
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.FIRST_INSTRUCTION);
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.FINAL_INSTRUCTION);
        // > No element
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.SET_VAR_J);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.UPDATE_VAR_J);


        // Bucket sort only
        // > No destination
        skipDict[UtilSort.SKIP_NO_DESTINATION].Add(UtilSort.BUCKET_INDEX_INST);

        // > No element
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.CREATE_BUCKETS_INST);
        skipDict[UtilSort.SKIP_NO_ELEMENT].Add(UtilSort.PHASING_INST);


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
        int minValue = UtilSort.MAX_VALUE, maxValue = 0;
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
        return value * numberOfBuckets / UtilSort.MAX_VALUE; // max + 1 ~?
    }

    #endregion

    #region Bucket Sort: Demo (Visual)
    public override IEnumerator Demo(GameObject[] sortingElements)
    {
        i = 0;
        j = 0;

        // Line 0 (set parameter)
        pseudoCodeViewer.SetCodeLine(CollectLine(0), Util.BLACKBOARD_TEXT_COLOR);

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = bucketSortManager.NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);

        // Line 1 (Create buckets)
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_COLOR);

        // Buckets
        GameObject[] buckets = bucketManager.Buckets;

        // Add elements to buckets
        for (i = 0; i < sortingElements.Length; i++)
        {
            // Line 2 (Update for-loop)
            yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_COLOR);

            // Get element
            GameObject element = sortingElements[i];
            value1 = element.GetComponent<SortingElementBase>().Value;

            // Bucket index
            bucketIndex = BucketIndex(value1, numberOfBuckets);

            // Line 3 (Display bucket index)
            yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_COLOR);

            // Get bucket
            Bucket bucket = buckets[bucketIndex].GetComponent<Bucket>(); // element.GetComponent<SortingElementBase>().Value - minValue);

            // Move element above the bucket and put it inside
            element.transform.position = bucket.transform.position + UtilSort.ABOVE_BUCKET_VR;

            // Line 4 (Put element into bucket)
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_COLOR);
        }

        // Line 5 (end for-loop)
        yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_COLOR);

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
            i = x;
            yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_COLOR);

            // Put elements for display on top of buckets
            int numberOfElementsInBucket = bucket.CurrenHolding.Count;
            for (int y=0; y < numberOfElementsInBucket; y++)
            {
                SortingElementBase element = bucket.GetElementForDisplay(y);
                element.gameObject.active = true;
                element.transform.position += UtilSort.ABOVE_BUCKET_VR;
                yield return demoStepDuration;
            }
        }
        // Line 8 (end for loop)
        //pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
        //yield return new WaitForSeconds(seconds);
        //pseudoCodeViewer.SetCodeLine(8, PseudoCode(8, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

        // Put elements back into list
        k = 0;
        // Line 7 (set k)
        yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_COLOR);

        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = sortMain.HolderManager.GetHolderPositions();
        // while (k < sortingElements.Length && i < numberOfBuckets)
        for (i = 0; i < numberOfBuckets; i++)
        {
            Bucket bucket = buckets[i].GetComponent<Bucket>();

            // number of elements in bucket
            loopRange = bucket.CurrenHolding.Count;

            // Line 8 (For-loop: Concatenate all buckets)
            yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_COLOR);

            for (j = 0; j < loopRange; j++)
            {
                // Line 9 (2nd For-loop: Concatenate all buckets)
                yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_COLOR);

                sortingElements[k] = bucket.RemoveSoringElement().gameObject;
                
                // Value of sorting element
                value1 = sortingElements[k].GetComponent<SortingElementBase>().Value;

                // Move element back to holder
                sortingElements[k].transform.position = holderPos[k] + UtilSort.ABOVE_HOLDER_VR;
                sortingElements[k].transform.rotation = Quaternion.identity;
                sortingElements[k].GetComponent<SortingElementBase>().IsSorted = true;

                // Line 10 (Put element back into list)
                yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_COLOR);

                k++;
                // Line 11 (Update k)
                yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_COLOR);
            }
            // Line 12 (2nd for-inner-loop end)
            yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_COLOR);
        }
        // Line 13 (2nd for-loop end)
        yield return HighlightPseudoCode(CollectLine(13), Util.HIGHLIGHT_COLOR);

        IsTaskCompleted = true;
    }
    #endregion


    #region Execute order from user
    public override void ExecuteStepByStepOrder(InstructionBase instruction, bool gotSortingElement, bool increment)
    {
        // Gather information from instruction
        BucketSortInstruction bucketInstruction = null;
        BucketSortElement sortingElement = null;
        int i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE, k = UtilSort.NO_VALUE;

        if (gotSortingElement)
        {
            bucketInstruction = (BucketSortInstruction)instruction;
            Debug.Log("Debug: " + bucketInstruction.DebugInfo() + "\n");

            // Change internal state of sorting element
            sortingElement = sortMain.ElementManager.GetSortingElement(bucketInstruction.SortingElementID).GetComponent<BucketSortElement>();
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
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], UtilSort.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        List<int> lineOfCode = new List<int>();
        switch (instruction.Instruction)
        {
            case UtilSort.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case UtilSort.CREATE_BUCKETS_INST:
                lineOfCode.Add(1);
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode.Add(2);
                break;

            case UtilSort.BUCKET_INDEX_INST:
                lineOfCode.Add(3);
                value1 = sortingElement.Value;
                bucketIndex = bucketInstruction.BucketID;

                if (increment)
                    sortingElement.IsCompare = bucketInstruction.IsCompare;
                else
                    sortingElement.IsCompare = !bucketInstruction.IsCompare;

                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.MOVE_TO_BUCKET_INST:
                lineOfCode.Add(4);
                value1 = sortingElement.Value;
                bucketIndex = bucketInstruction.BucketID;

                if (increment)
                    sortingElement.IsCompare = bucketInstruction.IsCompare;
                else
                    sortingElement.IsCompare = !bucketInstruction.IsCompare;
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.END_LOOP_INST:
                if (j < 0)
                {
                    switch (j)
                    {
                        case UtilSort.OUTER_LOOP: lineOfCode.Add(5); break;
                        case UtilSort.INNER_LOOP: lineOfCode.Add(12); break;
                        default: Debug.LogError(UtilSort.END_LOOP_INST + ": '" + j + "' loop not found"); break;
                    }
                }
                break;

            case UtilSort.PHASING_INST:
                lineOfCode.Add(6);
                //lineOfCode.Add(7);
                //lineOfCode.Add(8);
                i = 0;
                j = (bucketSortManager.NumberOfBuckets - 1);
                bucketIndex = j;
                bucketManager.AutoSortBuckets();
                break;

            case UtilSort.DISPLAY_ELEMENT:
                bucketManager.PutElementsForDisplay(i);
                break;

            case UtilSort.SET_VAR_J:
                lineOfCode.Add(7);
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (j == UtilSort.NO_VALUE)
                {
                    j = 0;
                    lineOfCode.Add(8);
                }
                else
                    lineOfCode.Add(9);

                loopRange = j;
                break;

            case UtilSort.MOVE_BACK_INST:
                lineOfCode.Add(10);
                value1 = sortingElement.Value;
                k = bucketInstruction.NextHolderID;
                if (increment)
                    sortingElement.IsSorted = bucketInstruction.IsSorted;
                else
                    sortingElement.IsSorted = !bucketInstruction.IsSorted;

                break;

            case UtilSort.UPDATE_VAR_J:
                lineOfCode.Add(11);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode[x]), UtilSort.HIGHLIGHT_COLOR);
        }

        // Move sorting element
        if (gotSortingElement)
        {
            switch (bucketInstruction.Instruction)
            {
                case UtilSort.MOVE_TO_BUCKET_INST:
                    if (increment)
                    {
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + UtilSort.ABOVE_BUCKET_VR;
                    }
                    else
                    {
                        sortingElement.transform.position = bucketSortManager.GetCorrectHolder(bucketInstruction.HolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                        sortingElement.gameObject.SetActive(true);
                    }
                    break;

                case UtilSort.DISPLAY_ELEMENT:
                    if (increment)
                    {
                        sortingElement.CanEnterBucket = false;
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + UtilSort.ABOVE_BUCKET_VR;
                        sortingElement.gameObject.SetActive(true);
                    }
                    else
                    {
                        sortingElement.CanEnterBucket = true;
                        sortingElement.gameObject.SetActive(false);
                    }

                    break;

                case UtilSort.MOVE_BACK_INST:
                    if (increment)
                        sortingElement.transform.position = bucketSortManager.GetCorrectHolder(bucketInstruction.NextHolderID).transform.position + UtilSort.ABOVE_HOLDER_VR;
                    else
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + UtilSort.ABOVE_BUCKET_VR;
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
        int i = UtilSort.NO_VALUE, j = UtilSort.NO_VALUE, k = UtilSort.NO_VALUE;

        if (gotSortingElement)
        {
            bucketInstruction = (BucketSortInstruction)instruction;
            Debug.Log("Debug: " + bucketInstruction.DebugInfo() + "\n");

            // Change internal state of sorting element
            sortingElement = sortMain.ElementManager.GetSortingElement(bucketInstruction.SortingElementID).GetComponent<BucketSortElement>();
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
            pseudoCodeViewer.ChangeColorOfText(prevHighlight[x], UtilSort.BLACKBOARD_TEXT_COLOR);
        }

        // Gather part of code to highlight
        List<int> lineOfCode = new List<int>();
        switch (instruction.Instruction)
        {
            case UtilSort.FIRST_INSTRUCTION:
                lineOfCode.Add(FirstInstructionCodeLine());
                break;

            case UtilSort.CREATE_BUCKETS_INST:
                lineOfCode.Add(1);
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode.Add(2);
                break;

            case UtilSort.BUCKET_INDEX_INST:
                lineOfCode.Add(3);
                value1 = sortingElement.Value;

                sortingElement.IsCompare = bucketInstruction.IsCompare;
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.MOVE_TO_BUCKET_INST:
                lineOfCode.Add(4);
                value1 = sortingElement.Value;
                bucketIndex = bucketInstruction.BucketID;

                sortingElement.IsCompare = bucketInstruction.IsCompare;
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.END_LOOP_INST:
                if (j < 0)
                {
                    switch (j)
                    {
                        case UtilSort.OUTER_LOOP: lineOfCode.Add(5); break;
                        case UtilSort.INNER_LOOP: lineOfCode.Add(14); break;
                        default: Debug.LogError(UtilSort.END_LOOP_INST + ": '" + j + "' loop not found"); break;
                    }
                }
                break;

            case UtilSort.PHASING_INST:
                lineOfCode.Add(6);
                lineOfCode.Add(7);
                lineOfCode.Add(8);
                break;

            case UtilSort.DISPLAY_ELEMENT:
                bucketSortManager.PutElementsForDisplay(i);
                break;

            case UtilSort.SET_VAR_J:
                lineOfCode.Add(9);
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (j == UtilSort.NO_VALUE)
                    lineOfCode.Add(10);
                else
                    lineOfCode.Add(11);
                break;

            case UtilSort.MOVE_BACK_INST:
                lineOfCode.Add(12);
                break;

            case UtilSort.UPDATE_VAR_J:
                lineOfCode.Add(13);
                break;

            case UtilSort.FINAL_INSTRUCTION:
                lineOfCode.Add(FinalInstructionCodeLine());
                break;
        }
        prevHighlight = lineOfCode;

        // Highlight part of code in pseudocode
        for (int x = 0; x < lineOfCode.Count; x++)
        {
            pseudoCodeViewer.SetCodeLine(CollectLine(lineOfCode[x]), UtilSort.HIGHLIGHT_COLOR);
        }

        yield return demoStepDuration;
        sortMain.BeginnerWait = false;
    }
    #endregion


    #region Bucket Sort: User Test
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instructionNr = 0;

        // Line 0 (set parameter)
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.FIRST_INSTRUCTION, instructionNr)); // new InstructionBase(Util.FIRST_INSTRUCTION, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = bucketSortManager.NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);

        // Line 1 (Create buckets)
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.CREATE_BUCKETS_INST, instructionNr)); // new InstructionBase(Util.CREATE_BUCKETS_INST, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Add elements to buckets
        for (int i = 0; i < sortingElements.Length; i++)
        {
            // Line 2 (Update for-loop)
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.FIRST_LOOP, instructionNr, i, UtilSort.OUTER_LOOP, UtilSort.NO_VALUE)); // new InstructionBase(Util.FIRST_LOOP, instructionNr, i, Util.LOOP_ONE, false, false)); // create one unique instruction for each loop, or "cheat" using the parametre?

            // Get element
            BucketSortInstruction element = (BucketSortInstruction)sortingElements[i];
            int bucketIndex = BucketIndex(element.Value, numberOfBuckets);

            // Line 3 (Display bucket index)
            instructions.Add(instructionNr++, new BucketSortInstruction(UtilSort.BUCKET_INDEX_INST, instructionNr, i, UtilSort.NO_VALUE, UtilSort.NO_VALUE, element.SortingElementID, element.Value, true, false, element.HolderID, UtilSort.NO_DESTINATION, bucketIndex)); // new BucketSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, i, Util.NO_VALUE, bucketIndex, Util.BUCKET_INDEX_INST, instructionNr, value, false, true, false));

            // Line 4 (Put element into bucket)
            instructions.Add(instructionNr++, new BucketSortInstruction(UtilSort.MOVE_TO_BUCKET_INST, instructionNr, i, UtilSort.NO_VALUE, UtilSort.NO_VALUE, element.SortingElementID, element.Value, false, false, element.HolderID, UtilSort.NO_DESTINATION, bucketIndex)); // new BucketSortInstruction(sortingElementID, holderID, Util.NO_DESTINATION, i, Util.NO_VALUE, bucketIndex, Util.MOVE_TO_BUCKET_INST, instructionNr, value, false, false, false));
        }

        // Line 5 (end for-loop)
        instructions.Add(instructionNr++, new InstructionLoop(UtilSort.END_LOOP_INST, instructionNr, UtilSort.NO_VALUE, UtilSort.OUTER_LOOP, UtilSort.NO_VALUE)); // new InstructionBase(Util.END_LOOP_INST, instructionNr, Util.NO_VALUE, Util.LOOP_ONE, false, false));

        // Line 6, 7, 8 (make the buckets sort what they hold)
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.PHASING_INST, instructionNr)); // new InstructionBase(Util.PHASING_INST, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

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
                    BucketSortInstruction displayInstruction = new BucketSortInstruction(UtilSort.DISPLAY_ELEMENT, instructionNr, UtilSort.NO_VALUE, UtilSort.NO_VALUE, UtilSort.NO_VALUE, t.SortingElementID, t.Value, false, true, UtilSort.NO_VALUE, UtilSort.NO_DESTINATION, bucketIndex); //new BucketSortInstruction(t.SortingElementID, Util.NO_VALUE, Util.NO_DESTINATION, Util.NO_VALUE, Util.NO_VALUE, bucketIndex, Util.DISPLAY_ELEMENT, instructionNr, t.Value, false, false, true);
                    instructions.Add(instructionNr++, displayInstruction);
                    buckets[bucketIndex].Add(displayInstruction);
                    break;
                }
            }
        }

        int k = 0;
        // Line 9 (For-loop: Concatenate all buckets)
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.SET_VAR_J, instructionNr)); // new InstructionBase(Util.SET_VAR_J, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = sortMain.HolderManager.GetHolderPositions();
        for (int i = 0; i < numberOfBuckets; i++)
        {
            List<BucketSortInstruction> bucket = buckets[i];
            int numberOfElementsInBucket = bucket.Count;

            // Line 10 (For-loop: Concatenate all buckets)
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instructionNr, i, UtilSort.NO_VALUE, k)); // new InstructionBase(Util.UPDATE_LOOP_INST, instructionNr, i, Util.NO_VALUE, false, false));

            for (int j = 0; j < numberOfElementsInBucket; j++)
            {
                // Line 11 (2nd For-loop: Concatenate all buckets)
                instructions.Add(instructionNr++, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instructionNr, i, j, k)); // new InstructionBase(Util.UPDATE_LOOP_INST, instructionNr, i, j, false, false));

                // Line 12 (Put element back into list)
                instructions.Add(instructionNr++, new BucketSortInstruction(UtilSort.MOVE_BACK_INST, instructionNr, i, j, k, bucket[j].SortingElementID, bucket[j].Value, false, true, UtilSort.NO_VALUE, k, bucket[j].BucketID)); // new BucketSortInstruction(bucket[j].SortingElementID, Util.NO_VALUE, k, i, j, bucket[j].BucketID, Util.MOVE_BACK_INST, instructionNr, bucket[j].Value, false, false, true));

                k++;
                // Line 13 (Update k)
                instructions.Add(instructionNr++, new InstructionLoop(UtilSort.UPDATE_VAR_J, instructionNr, UtilSort.NO_VALUE, UtilSort.NO_VALUE, k)); // new InstructionBase(Util.UPDATE_VAR_J, instructionNr, k, Util.NO_VALUE, false, false)); ******
            }
            // Line 14 (2nd for-loop end)
            instructions.Add(instructionNr++, new InstructionLoop(UtilSort.END_LOOP_INST, instructionNr, i, UtilSort.INNER_LOOP, UtilSort.NO_VALUE)); // new InstructionBase(Util.END_LOOP_INST, instructionNr, i, Util.LOOP_TWO, false, false)); **** i=?
        }
        // Line 15 (2nd for-loop end)
        instructions.Add(instructionNr++, new InstructionBase(UtilSort.FINAL_INSTRUCTION, instructionNr)); // new InstructionBase(Util.FINAL_INSTRUCTION, instructionNr, Util.NO_VALUE, Util.NO_VALUE, false, false));

        //status = NONE;
        return instructions;
    }
    #endregion





    // ---------------------------------------- Extras ----------------------------------------





}

