/* 
 * >>> Status of algorithms:
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Algorithm name    |   Standard    |   Tutorial    |   Step-by-step    |   User Test   |                               Comment                                 |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Bubble Sort       |     Yes       |     Yes       |       Yes*        |       Yes     | Step-by-step need some adjustments                                    |
 *    Insertion Sort    |     Yes       |     Yes       |       Yes         |       Yes     | Complete                                                              |
 *    Bucket Sort       |     Yes       |     Yes       |       Yes*        |       Yes*    | Standard/Tutorial/Step-by-step: ok**, user test needs more work       |
 *    Merge Sort        |     Yes       |     No        |       No          |       No      | Standard: OK, Tutorial started/not finished                           |
 *    Quick Sort        |      No       |     No        |       No          |       No      | Not started                                                           |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 * 
 * 
 * *********************************** To do list **********************************************
 * >>> Clean up
 *  > Old unused stuff
 *      - remove
 *  > 
 *
 * > User Test
 *  - fix different levels of difficulty (how much help given, points etc)
 *  - remove UserActionToProceed and decrement ReadyForNext instead ? (down to 0)
 *  
 * > Tutorial
 *  - disable drag (non-vr), touch/grab (vr)
 * 
 * 
 * > ElementManager (randomizing values+)
 * - ElementsBasedOnCase: first gather values then distribute, instead of distribute -> redistribute based on sorting case (worst/best) ?
 * 
 * > AlgorithmManagerBase
 *  - Clean up IsTutorial() (mixed with IsTutorialStep())
 *  
 * > InstructionNr
 *  - remove it from UserTestManager++
 * 
 * 
 * > BucketSort
 *  - Display elements working/sometimes not
 * 
 * *********************************** Remember to fix when moving from Non-VR -> VR **********************************************
 * 
 * > Change 'moving' variable when picking up/releasing a sorting element
 * 
 * 
 * 
 * 
 * 
 * >>> Commented a lot of old methods, check if any bugs/null exceptions occours
 * 
 * 
 * 
 *  *********************************** Implementation ideas **********************************************
 * > User test
 *  - gather all user inputs (during examination?)
 *  - if any mistakes has been done by buser --> show what and how (step/instructions/feedback/animations)
*/