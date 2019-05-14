using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BucketManager))]
//[RequireComponent(typeof(BucketSortManager))]
public class BucketSort : SortAlgorithm {

    // value1 = value of element
    private int bucketIndex;

    public const string CHOOSE_BUCKET = "Choose bucket", PUT_BACK_TO_HOLDER = "Put back to holder", NONE = "None";
    private Dictionary<int, string> pseudoCode;

    private string kPlus1 = "k + 1", numberOfBuckets = "N", bucketSize = "len(buckets[i])", bucketIndexStr = "list[i] * N / MAX_VALUE";

    [SerializeField]
    private BucketManager bucketManager;

    [SerializeField]
    private BucketSortManager bucketSortManager;

    public override void InitTeachingAlgorithm(float algorithmSpeed)
    {
        element1Value = "list[i]";
        element2Value = "buckets[i][j]";

        base.InitTeachingAlgorithm(algorithmSpeed);
    }

    public override string AlgorithmName
    {
        get { return Util.BUCKET_SORT; }
    }

    public override string CollectLine(int lineNr)
    {
        string lineOfCode = lineNr.ToString() + Util.PSEUDO_SPLIT_LINE_ID;
        switch (lineNr)
        {
            case 0: lineOfCode += string.Format("BucketSort({0}, {1}):", listValues, numberOfBuckets); break;
            case 1: lineOfCode += string.Format("    buckets = new array of {0} empty lists", numberOfBuckets); break;
            case 2: lineOfCode += string.Format("    for i={0} to {1}:", i, lengthOfList); break;
            case 3: lineOfCode += string.Format("        bucket = {0}", bucketIndexStr); break;
            case 4: lineOfCode += string.Format("        buckets[{0}] <- {1}", bucketIndex, element1Value); break;
            case 5: lineOfCode += "    end for"; break;
            case 6: lineOfCode += "    Sorting each bucket w/InsertionSort"; break;
            case 7: lineOfCode += "    k = 0"; break;
            case 8: lineOfCode += string.Format("    for i={0} to {1}:", i, numberOfBuckets); break;
            case 9: lineOfCode += string.Format("        for j={0} to {1}:", j, bucketSize); break;
            case 10: lineOfCode += string.Format("            list[{0}] = {1}", k, element2Value); break; // 
            case 11: lineOfCode += "            k = " + kPlus1; break;
            case 12: lineOfCode += "        end for"; break;
            case 13: lineOfCode += "    end for"; break;
            default: return Util.INVALID_PSEUDO_CODE_LINE;
        }
        return lineOfCode;
    }

    protected override string PseudocodeLineIntoSteps(int lineNr, bool init)
    {
        switch (lineNr)
        {
            case 3: return init ? "        bucket = list[" + i + "] * N / MAX_VALUE" : "        bucket = " + element1Value  + " * " + numberOfBuckets + " / " + sortMain.ElementManager.MaxValue; // UtilSort.MAX_VALUE;
            //case 4: return init ? "        buckets[bucket] <- list[i]" : "        buckets[" + bucketIndex + "] <- list[" + i + "]";
            case 10: return init ? "            list[k] = buckets[i][j]" : "            list[" + k + "] = buckets[" + i + "][" + j + "]";
            case 11: return init ? "            k = k + 1" : "            k = " + k + " + 1";
            default: return Util.INVALID_PSEUDO_CODE_LINE;
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

        element1Value = "list[i]";
        element2Value = "buckets[i][j]";
        kPlus1 = "k + 1";
        numberOfBuckets = "N";
        bucketSize = "len(buckets[i])";
        bucketIndexStr = "list[i] * N / MAX_VALUE";

        bucketManager.ResetSetup();
    }

    public override void AddSkipAbleInstructions()
    {
        base.AddSkipAbleInstructions();
        // > No destination
        skipDict.Add(Util.SKIP_NO_DESTINATION, new List<string>());
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FIRST_INSTRUCTION);
        skipDict[Util.SKIP_NO_DESTINATION].Add(Util.FINAL_INSTRUCTION);

        // > No element
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilSort.FIRST_LOOP);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.SET_VAR_K);
        skipDict[Util.SKIP_NO_ELEMENT].Add(Util.UPDATE_VAR_K);


        // Bucket sort only
        // > No destination
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilSort.BUCKET_INDEX_INST);
        skipDict[Util.SKIP_NO_DESTINATION].Add(UtilSort.DISPLAY_ELEMENT);

        // > No element
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilSort.CREATE_BUCKETS_INST);
        skipDict[Util.SKIP_NO_ELEMENT].Add(UtilSort.PHASING_INST);
    }

    public override void Specials(string method, int number, bool activate)
    {
        switch (method)
        {
            case "Somemethod": FirstInstructionCodeLine(); break; // example: some void method
        }
    }

    #region Bucket Sort: Standard 1
    public static GameObject[] BucketSortStandard(GameObject[] sortingElements, int numberOfBuckets, int min, int max)
    {
        // Find min-/ max values
        int minValue = max, maxValue = min;
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
    public static GameObject[] BucketSortStandard2(GameObject[] sortingElements, int numberOfBuckets, int max)
    {
        // Find number of elements per bucket, + counter for later use
        int[] numberOfElementsPerBucket = new int[numberOfBuckets], counters = new int[numberOfBuckets];
        for (int i=0; i < sortingElements.Length; i++)
        {
            int index = BucketIndex(sortingElements[i].GetComponent<SortingElementBase>().Value, numberOfBuckets, max);
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
            int index = BucketIndex(sortingElements[i].GetComponent<SortingElementBase>().Value, numberOfBuckets, max);
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

    private static int BucketIndex(int value, int numberOfBuckets, int maxValue)
    {
        return value * numberOfBuckets / maxValue; // max + 1 ~?                            // TODO: negative values
    }

    #endregion

    #region Bucket Sort: Demo (Visual) -- NOT USED ANYMORE
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
        yield return HighlightPseudoCode(CollectLine(1), Util.HIGHLIGHT_STANDARD_COLOR);

        // Buckets
        GameObject[] buckets = null;
        if (bucketManager != null)
            buckets = bucketManager.Buckets;

        // Add elements to buckets
        for (i = 0; i < sortingElements.Length; i++)
        {
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Line 2 (Update for-loop)
            yield return HighlightPseudoCode(CollectLine(2), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Get element
            GameObject element = sortingElements[i];
            PreparePseudocodeValue(element.GetComponent<SortingElementBase>().Value, 1);

            // Bucket index
            bucketIndex = BucketIndex(value1, numberOfBuckets, maxValue);

            // Line 3 (Display bucket index)
            yield return HighlightPseudoCode(CollectLine(3), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Get bucket
            Bucket bucket = buckets[bucketIndex].GetComponent<Bucket>(); // element.GetComponent<SortingElementBase>().Value - minValue);

            // Move element above the bucket and put it inside
            element.transform.position = bucket.transform.position + UtilSort.ABOVE_BUCKET_VR;

            // Line 4 (Put element into bucket)
            yield return HighlightPseudoCode(CollectLine(4), Util.HIGHLIGHT_STANDARD_COLOR);
        }

        // Line 5 (end for-loop)
        yield return HighlightPseudoCode(CollectLine(5), Util.HIGHLIGHT_STANDARD_COLOR);

        // Display elements
        for (int x=0; x < numberOfBuckets; x++)
        {
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Line 6 (For-loop: Sort elements in buckets)
            //pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, x, Util.NO_VALUE, Util.NO_VALUE, true), Util.HIGHLIGHT_COLOR);
            //yield return new WaitForSeconds(seconds);
            //pseudoCodeViewer.SetCodeLine(6, PseudoCode(6, x, Util.NO_VALUE, Util.NO_VALUE, true), Util.BLACKBOARD_TEXT_COLOR);

            Bucket bucket = buckets[x].GetComponent<Bucket>();
            bucket.SetEnterTrigger(false);

            // Sort bucket *** TODO: go to insertion sort scene
            bucket.CurrenHolding = InsertionSort.InsertionSortStandard2(bucket.CurrenHolding);

            // Line 6 (Sort elements in bucket)
            i = x;
            yield return HighlightPseudoCode(CollectLine(6), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            // Put elements for display on top of buckets
            int numberOfElementsInBucket = bucket.CurrenHolding.Count;
            for (int y=0; y < numberOfElementsInBucket; y++)
            {
                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

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
        yield return HighlightPseudoCode(CollectLine(7), Util.HIGHLIGHT_STANDARD_COLOR);

        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = sortMain.HolderManager.GetHolderPositions();
        // while (k < sortingElements.Length && i < numberOfBuckets)
        for (i = 0; i < numberOfBuckets; i++)
        {
            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;

            Bucket bucket = buckets[i].GetComponent<Bucket>();

            // number of elements in bucket
            bucketSize = bucket.CurrenHolding.Count.ToString();

            // Line 8 (For-loop: Concatenate all buckets)
            yield return HighlightPseudoCode(CollectLine(8), Util.HIGHLIGHT_STANDARD_COLOR);

            for (j = 0; j < bucket.CurrenHolding.Count; j++)
            {
                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Line 9 (2nd For-loop: Concatenate all buckets)
                yield return HighlightPseudoCode(CollectLine(9), Util.HIGHLIGHT_STANDARD_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                sortingElements[k] = bucket.RemoveSoringElement().gameObject;
                
                // Value of sorting element
                PreparePseudocodeValue(sortingElements[k].GetComponent<SortingElementBase>().Value, 2);

                // Move element back to holder
                sortingElements[k].transform.position = holderPos[k] + UtilSort.ABOVE_HOLDER_VR;
                sortingElements[k].transform.rotation = Quaternion.identity;
                sortingElements[k].GetComponent<SortingElementBase>().IsSorted = true;

                // Line 10 (Put element back into list)
                yield return HighlightPseudoCode(CollectLine(10), Util.HIGHLIGHT_STANDARD_COLOR);

                // Check if user wants to stop the demo
                if (sortMain.UserStoppedTask)
                    break;

                // Line 11 (Update k)
                yield return HighlightPseudoCode(CollectLine(11), Util.HIGHLIGHT_STANDARD_COLOR);
                k++;
            }
            // Line 12 (2nd for-inner-loop end)
            yield return HighlightPseudoCode(CollectLine(12), Util.HIGHLIGHT_STANDARD_COLOR);

            // Check if user wants to stop the demo
            if (sortMain.UserStoppedTask)
                break;
        }
        // Line 13 (2nd for-loop end)
        yield return HighlightPseudoCode(CollectLine(13), Util.HIGHLIGHT_STANDARD_COLOR);


        if (sortMain.UserStoppedTask)
            sortMain.UpdateCheckList(Util.DEMO, true);
        else
            isTaskCompleted = true;
    }
    #endregion


    #region New Demo / Step-by-step
    public override IEnumerator ExecuteDemoInstruction(InstructionBase instruction, bool increment)
    {
        // Gather information from instruction
        BucketSortInstruction bucketInstruction = null;
        BucketSortElement sortingElement = null;

        if (instruction is BucketSortInstruction)
        {
            bucketInstruction = (BucketSortInstruction)instruction;

            // Change internal state of sorting element
            sortingElement = sortMain.ElementManager.GetSortingElement(bucketInstruction.SortingElementID).GetComponent<BucketSortElement>();
            bucketIndex = bucketInstruction.BucketID;
        }

        if (instruction is InstructionLoop)
        {
            InstructionLoop loopInst = (InstructionLoop)instruction;
            i = loopInst.I;
            j = loopInst.J;
            k = loopInst.K;
            loopType = loopInst.LoopType;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);


        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = FirstInstructionCodeLine();
                SetBuckets(increment);
                break;

            case UtilSort.CREATE_BUCKETS_INST:
                lineOfCode = 1;
                SetBuckets(increment);
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode = 2;

                if (increment)
                {
                    SetLengthOfList();
                    useHighlightColor = UseConditionColor(i != lengthOfListInteger);
                }
                else
                {
                    if (i == 0)
                        lengthOfList = "len(list)";
                    else
                        i -= 1;
                }

                break;

            case UtilSort.BUCKET_INDEX_INST:
                lineOfCode = 3;
                PreparePseudocodeValue(sortingElement.Value, 1);
                bucketIndex = bucketInstruction.BucketID;

                if (increment)
                {
                    sortingElement.IsCompare = bucketInstruction.IsCompare;
                    bucketIndexStr = bucketIndex.ToString();
                }
                else
                {
                    sortingElement.IsCompare = !bucketInstruction.IsCompare;
                    bucketIndexStr = "list[i] * N / MAX_VALUE";
                }

                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.MOVE_TO_BUCKET_INST:
                lineOfCode = 4;

                if (increment)
                {
                    PreparePseudocodeValue(sortingElement.Value, 1);
                    bucketIndex = bucketInstruction.BucketID;
                    sortingElement.IsCompare = bucketInstruction.IsCompare;
                }
                else
                {
                    element1Value = "list[i]";
                    //bucketIndex = ""; // TODO ?
                    sortingElement.IsCompare = !bucketInstruction.IsCompare;
                }
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.END_LOOP_INST:
                switch (loopType)
                {
                    case UtilSort.OUTER_LOOP:lineOfCode = 5; break;
                    case UtilSort.INNER_LOOP: lineOfCode = 12; break;
                    default: Debug.LogError(UtilSort.END_LOOP_INST + ": '" + loopType + "' loop not found"); break;
                }                
                break;

            case UtilSort.PHASING_INST:
                lineOfCode = 6;
                i = 0; // ????
                j = (bucketSortManager.NumberOfBuckets - 1);
                bucketIndex = j;

                // Sort buckets
                bucketManager.AutoSortBuckets();
                break;

            case UtilSort.DISPLAY_ELEMENT: // TODO: Fix
                //sortMain.WaitForSupportToComplete++; // add to list?
                StartCoroutine(bucketManager.PutElementsForDisplay(bucketInstruction.BucketID));

                if (!increment)
                    bucketManager.GetBucket(bucketIndex).SetEnterTrigger(true);

                break;

            case Util.SET_VAR_K:
                lineOfCode = 7;
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (loopType == UtilSort.OUTER_LOOP) // 2nd loop (outher)
                {
                    //j = 0;
                    lineOfCode = 8;
                    if (increment)
                    {
                        numberOfBuckets = bucketSortManager.NumberOfBuckets.ToString();
                        useHighlightColor = UseConditionColor(i != j);
                    }
                    else
                    {
                        if (i > 0)
                            i -= 1;
                        else
                            numberOfBuckets = "N";
                    }

                }
                else // 2nd loop (inner)
                {
                    lineOfCode = 9;
                    if (increment)
                    {
                        bucketSize = k.ToString();
                        useHighlightColor = UseConditionColor(j != k);
                    }
                    else
                    {
                        if (j > 0)
                            j--;
                        else
                            bucketSize = k.ToString();
                    }
                }
                break;

            case UtilSort.MOVE_BACK_INST:
                lineOfCode = 10;
                k = bucketInstruction.NextHolderID; // ???

                if (increment)
                {
                    PreparePseudocodeValue(sortingElement.Value, 2);
                    sortingElement.IsSorted = bucketInstruction.IsSorted;
                }
                else
                {
                    element2Value = "buckets[" + i + "][" + j + "]";
                    sortingElement.IsSorted = !bucketInstruction.IsSorted;
                }

                break;

            case Util.UPDATE_VAR_K:
                lineOfCode = 11;
                if (increment)
                    kPlus1 = (k + 1).ToString();
                else
                {
                    //if (k > 0)
                    //    kPlus1 = (k - 1).ToString();
                    //else
                    kPlus1 = "k + 1";
                }
                break;

            case Util.FINAL_INSTRUCTION:
                lineOfCode = FinalInstructionCodeLine();
                IsTaskCompleted = increment;
                break;
        }

        // Highlight part of code in pseudocode
        if (instruction.Instruction == UtilSort.DISPLAY_ELEMENT)
            yield return null;
        else
        {
            yield return HighlightPseudoCode(CollectLine(lineOfCode), useHighlightColor);
            prevHighlightedLineOfCode = lineOfCode;
        }

        // Move sorting element
        if (instruction is BucketSortInstruction)
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
                        bucketManager.GetBucket(bucketIndex).RemoveSortingElement(sortingElement);
                        sortingElement.RigidBody.constraints = RigidbodyConstraints.None;
                    }
                    break;

                case UtilSort.DISPLAY_ELEMENT:
                    if (increment)
                    {
                        sortingElement.transform.position = bucketManager.GetBucket(bucketInstruction.BucketID).transform.position + UtilSort.ABOVE_BUCKET_VR;
                        sortingElement.gameObject.SetActive(true);
                    }
                    else
                    {
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

        // Display element reports itself when it's done
        if (instruction.Instruction != UtilSort.DISPLAY_ELEMENT)
            sortMain.WaitForSupportToComplete--;
    }
    #endregion

    #region User test display help
    public override IEnumerator UserTestHighlightPseudoCode(InstructionBase instruction, bool gotSortingElement)
    {
        // Gather information from instruction
        BucketSortInstruction bucketInstruction = null;
        BucketSortElement sortingElement = null;

        if (gotSortingElement)
        {
            bucketInstruction = (BucketSortInstruction)instruction;

            // Change internal state of sorting element
            sortingElement = sortMain.ElementManager.GetSortingElement(bucketInstruction.SortingElementID).GetComponent<BucketSortElement>();
        }

        if (instruction is InstructionLoop)
        {
            InstructionLoop loopInst = (InstructionLoop)instruction;
            i = loopInst.I;
            j = loopInst.J;
            k = loopInst.K;
            loopType = loopInst.LoopType;
        }

        // Remove highlight from previous instruction
        pseudoCodeViewer.ChangeColorOfText(prevHighlightedLineOfCode, Util.BLACKBOARD_TEXT_COLOR);

        // Gather part of code to highlight
        int lineOfCode = Util.NO_VALUE;
        useHighlightColor = Util.HIGHLIGHT_STANDARD_COLOR;
        switch (instruction.Instruction)
        {
            case Util.FIRST_INSTRUCTION:
                lineOfCode = FirstInstructionCodeLine();
                SetLengthOfList();
                numberOfBuckets = bucketSortManager.NumberOfBuckets.ToString();
                break;

            case UtilSort.CREATE_BUCKETS_INST:
                lineOfCode = 1;
                break;

            case UtilSort.FIRST_LOOP:
                lineOfCode = 2;
                useHighlightColor = UseConditionColor(i != lengthOfListInteger);
                break;

            case UtilSort.BUCKET_INDEX_INST:
                lineOfCode = 3;
                PreparePseudocodeValue(sortingElement.Value, 1);
                bucketIndex = bucketInstruction.BucketID;
                bucketIndexStr = bucketIndex.ToString();

                //sortingElement.IsCompare = bucketInstruction.IsCompare;
                UtilSort.IndicateElement(sortingElement.gameObject);
                break;

            case UtilSort.MOVE_TO_BUCKET_INST:
                lineOfCode = 4;
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                PreparePseudocodeValue(sortingElement.Value, 1);
                UtilSort.IndicateElement(sortingElement.gameObject);

                //sortingElement.IsCompare = bucketInstruction.IsCompare;
                break;

            case UtilSort.END_LOOP_INST:
                switch (loopType)
                {
                    case UtilSort.OUTER_LOOP: lineOfCode = 5; break;
                    case UtilSort.INNER_LOOP: lineOfCode = 12; break;
                    default: Debug.LogError(UtilSort.END_LOOP_INST + ": '" + loopType + "' loop not found"); break;
                }
                break;

            case UtilSort.PHASING_INST:
                lineOfCode = 6;
                break;

            case Util.SET_VAR_K:
                lineOfCode = 7;
                break;

            case UtilSort.UPDATE_LOOP_INST:
                if (loopType == UtilSort.OUTER_LOOP) // 2nd loop (outher)
                {
                    //j = 0;
                    lineOfCode = 8;
                    useHighlightColor = UseConditionColor(i != j);
                }
                else // 2nd loop (inner)
                {
                    lineOfCode = 9;
                    bucketSize = k.ToString();
                    useHighlightColor = UseConditionColor(j != k);
                }
                break;

            case UtilSort.MOVE_BACK_INST:
                lineOfCode = 10;
                useHighlightColor = Util.HIGHLIGHT_USER_ACTION;
                PreparePseudocodeValue(sortingElement.Value, 2);
                break;

            case Util.UPDATE_VAR_K:
                lineOfCode = 11;
                kPlus1 = (k + 1).ToString();
                break;

            case Util.FINAL_INSTRUCTION:
                lineOfCode = FinalInstructionCodeLine();
                break;
        }
        // Highlight part of code in pseudocode
        yield return HighlightPseudoCode(CollectLine(lineOfCode), useHighlightColor);
        
        // Mark prev for next round
        prevHighlightedLineOfCode = lineOfCode;

        sortMain.WaitForSupportToComplete--;
    }
    #endregion


    #region Bucket Sort: Instructions
    public override Dictionary<int, InstructionBase> UserTestInstructions(InstructionBase[] sortingElements)
    {
        Dictionary<int, InstructionBase> instructions = new Dictionary<int, InstructionBase>();
        int instNr = 0;

        // Line 0 (set parameter)
        instructions.Add(instNr, new InstructionBase(Util.FIRST_INSTRUCTION, instNr++));

        // Create buckets
        Vector3[] pos = new Vector3[1] { bucketManager.FirstBucketPosition };
        int numberOfBuckets = bucketSortManager.NumberOfBuckets;
        bucketManager.CreateObjects(numberOfBuckets, pos);

        // Line 1 (Create buckets)
        instructions.Add(instNr, new InstructionBase(UtilSort.CREATE_BUCKETS_INST, instNr++));

        int x;
        // Add elements to buckets
        for (x = 0; x < sortingElements.Length; x++)
        {
            // Line 2 (Update for-loop)
            instructions.Add(instNr, new InstructionLoop(UtilSort.FIRST_LOOP, instNr++, x, Util.NO_VALUE, Util.NO_VALUE)); // TODO: create one unique instruction for each loop

            // Get element
            BucketSortInstruction element = (BucketSortInstruction)sortingElements[x];
            int bucketIndex = BucketIndex(element.Value, numberOfBuckets, maxValue);

            // Line 3 (Display bucket index)
            instructions.Add(instNr, new BucketSortInstruction(UtilSort.BUCKET_INDEX_INST, instNr++, x, Util.NO_VALUE, Util.NO_VALUE, element.SortingElementID, element.Value, true, false, element.HolderID, UtilSort.NO_DESTINATION, bucketIndex));

            // Line 4 (Put element into bucket)
            instructions.Add(instNr, new BucketSortInstruction(UtilSort.MOVE_TO_BUCKET_INST, instNr++, x, Util.NO_VALUE, Util.NO_VALUE, element.SortingElementID, element.Value, false, false, element.HolderID, UtilSort.NO_DESTINATION, bucketIndex));
        }
        // Line 2: condition
        instructions.Add(instNr, new InstructionLoop(UtilSort.FIRST_LOOP, instNr++, x, Util.NO_VALUE, Util.NO_VALUE));

        // Line 5 (end for-loop)
        instructions.Add(instNr, new InstructionLoop(UtilSort.END_LOOP_INST, instNr++, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, UtilSort.OUTER_LOOP));

        // Line 6 (make the buckets sort what they hold)
        instructions.Add(instNr, new InstructionBase(UtilSort.PHASING_INST, instNr++));

        // Sorting elements
        int[] values = new int[sortingElements.Length];
        for (int y = 0; y < sortingElements.Length; y++)
        {
            values[y] = ((BucketSortInstruction)sortingElements[y]).Value;
        }
        int[] sorted = InsertionSort.InsertionSortFixCase(values, false);

        // Creating fictionary buckets
        Dictionary<int, List<BucketSortInstruction>> buckets = new Dictionary<int, List<BucketSortInstruction>>();
        for (int y=0; y < numberOfBuckets; y++)
        {
            buckets[y] = new List<BucketSortInstruction>();
        }

        // Look for correct value and add element to bucket
        for (int r = 0; r < sorted.Length; r++)
        {
            for (int s = 0; s < sortingElements.Length; s++)
            {
                BucketSortInstruction t = (BucketSortInstruction)sortingElements[s];
                if (sorted[r] == t.Value)
                {
                    int bucketIndex = BucketIndex(t.Value, bucketSortManager.NumberOfBuckets, maxValue);
                    BucketSortInstruction displayInstruction = new BucketSortInstruction(UtilSort.DISPLAY_ELEMENT, instNr, Util.NO_VALUE, Util.NO_VALUE, Util.NO_VALUE, t.SortingElementID, t.Value, false, true, Util.NO_VALUE, UtilSort.NO_DESTINATION, bucketIndex);
                    instructions.Add(instNr++, displayInstruction);
                    buckets[bucketIndex].Add(displayInstruction);
                    break;
                }
            }
        }

        int i, j, k = 0;
        // Line 7 (For-loop: Concatenate all buckets)
        instructions.Add(instNr, new InstructionBase(Util.SET_VAR_K, instNr++));

        // Holder positions (where the sorting elements initialized)
        Vector3[] holderPos = sortMain.HolderManager.GetHolderPositions();
        for (i = 0; i < numberOfBuckets; i++)
        {
            List<BucketSortInstruction> bucket = buckets[i];
            int numberOfElementsInBucket = bucket.Count;

            // Line 8 (For-loop: Concatenate all buckets)
            instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, numberOfBuckets, k, UtilSort.OUTER_LOOP));

            for (j = 0; j < numberOfElementsInBucket; j++)
            {
                // Line 9 (2nd For-loop: Concatenate all buckets)
                instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j, numberOfElementsInBucket, UtilSort.INNER_LOOP));

                // Line 10 (Put element back into list)
                instructions.Add(instNr, new BucketSortInstruction(UtilSort.MOVE_BACK_INST, instNr++, i, j, k, bucket[j].SortingElementID, bucket[j].Value, false, true, Util.NO_VALUE, k, bucket[j].BucketID)); 

                // Line 11 (Update k)
                instructions.Add(instNr, new InstructionLoop(Util.UPDATE_VAR_K, instNr++, i, j, k)); 
                k++;
            }
            // Line 9: condition
            instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, j, numberOfElementsInBucket, UtilSort.INNER_LOOP));
            // Line 12 (2nd for-loop end)
            instructions.Add(instNr, new InstructionLoop(UtilSort.END_LOOP_INST, instNr++, i, Util.NO_VALUE, k, UtilSort.INNER_LOOP));
        }
        // Line 8: condition
        instructions.Add(instNr, new InstructionLoop(UtilSort.UPDATE_LOOP_INST, instNr++, i, numberOfBuckets, k, UtilSort.OUTER_LOOP));
        // Line 13 (2nd for-loop end)
        instructions.Add(instNr, new InstructionBase(Util.FINAL_INSTRUCTION, instNr++));

        return instructions;
    }
    #endregion





    // ---------------------------------------- Extras ----------------------------------------

    private void SetBuckets(bool increment)
    {
        if (increment)
            numberOfBuckets = bucketSortManager.NumberOfBuckets.ToString();
        else
            numberOfBuckets = "N";
    }



}

