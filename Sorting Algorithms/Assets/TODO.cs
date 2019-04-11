/* 
 * >>> Status of algorithms:
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Algorithm name    |   Standard    |   Tutorial    |   Step-by-step    |   User Test   |                               Comment                                 |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Bubble Sort       |     Yes       |     Yes       |       Yes         |       Yes     | Complete                                                              |
 *    Insertion Sort    |     Yes       |     Yes       |       Yes*        |       Yes     | Complete*: found small bug in step-by-step                            |
 *    Bucket Sort       |     Yes       |     Yes       |       Yes*        |       Yes*    | Standard/Tutorial/Step-by-step: ok**, user test needs more work       |
 *    Merge Sort        |     Yes       |     No        |       No          |       No      | Standard: OK, Tutorial started/not finished                           |
 *    Quick Sort        |      No       |     No        |       No          |       No      | Not started                                                           |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 * 
 * 
 * *********************************** To do list **********************************************
 * > User Test
 *  - fix different levels of difficulty (how much help given, points etc)
 *  - remove UserActionToProceed and decrement ReadyForNext instead ? (down to 0)
 *  
 * > Tutorial
 *  - disable drag (non-vr), touch/grab (vr)
 * 
 * 
 * > ElementManager (cleanup: (duplicate) randomizing values)
 * - ElementsBasedOnCase: first gather values then distribute, instead of distribute -> redistribute based on sorting case (worst/best) ?
 * 
 * > AlgorithmManagerBase
 *  - Clean up IsTutorial() (mixed with IsTutorialStep())
 *  
 * > InstructionNr
 *  - remove it from UserTestManager++
 * 
 * 
 * 
 * *********************************** Bugs to fix **********************************************
 * 
 * >>> Bubble sort
 *  > Any new bugs found?
 *  
 *  
 * >>> Insertion sort
 *  > step-by-step: almost done, isSorted buggy (case: none)
 *  
 *  
 * >>> BucketSort
 *  > User test: ready?
 *  > New Demo:
 *      - Last for-loops buggy pseudocode
 *      - Step-by-step: backwards needs more work
 * 
 * 
 * 
 * *********************************** Remember to fix when moving from Non-VR -> VR **********************************************
 * 
 * > Change 'moving' variable when picking up/releasing a sorting element
 * 
 * 
 * 
 * 
 * 
 *  *********************************** Implementation ideas **********************************************
 * 
 *  
 * > Cognitive Curiosity
 * - give more information about runtime (n^2, log(n) etc.) + test?
 * 
 * > User test
 *  - gather all user inputs (during examination?)
 *  - if any mistakes has been done by buser --> show what and how (step/instructions/feedback/animations)
*/