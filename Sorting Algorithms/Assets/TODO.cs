/* 
 * >>> Status of algorithms:
 * ---------------------------------------------------------------------------------------------------------------------------------
 *    Algorithm name    |   Standard    |   Tutorial    |   Step-by-step    |   User Test   |                   Comment
 * ---------------------------------------------------------------------------------------------------------------------------------
 *    Bubble Sort       |     Yes       |     Yes       |       Yes*        |       Yes     | Step-by-step need some adjustments
 *    Insertion Sort    |     Yes       |     Yes       |       Yes         |       Yes     | Complete
 *    Merge Sort        |     Yes       |     No        |       No          |       No      | Tutorial not completed yet, user test not started
 *    Quick Sort        |      No       |     No        |       No          |       No      | Not started
 *    Bucket Sort       |     Yes       |     Yes*      |       No?         |       No      | Implement user test + get stuff up on the blackboard
 * ---------------------------------------------------------------------------------------------------------------------------------
 * 
 * 
 * *********************************** To do list **********************************************
 * > Clean up instruction
 *  - split into more groups (inheritage), ID's, I/J ?
 * 
 * > User Test
 *  - fix different levels of difficulty (how much help given, points etc)
 *  - remove UserActionToProceed and decrement ReadyForNext instead ? (down to 0)
 *  
 * > Tutorial
 *  - disable drag (non-vr), touch/grab (vr)
 * 
 * > User test (when updating old algorithms)
 *  - make all algorithm implement methods insertion sort uses
 * 
 * > ElementManager (randomizing values+)
 * - ElementsBasedOnCase: first gather values then distribute, instead of distribute -> redistribute based on sorting case (worst/best) ?
 * 
 * > AlgorithmManagerBase
 *  - Clean up IsTutorial() (mixed with IsTutorialStep())
 * 
 * > Sorting element / holder
 *  - Clean up: remove all, create 1 prefab and add the respectful component in ElementManager/HolderManager on instantiation
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
 * >>> Commented a lot of old methods, check if any bugs/null exceptions occours
 * 
 * 
 * 
 *  *********************************** Implementation ideas **********************************************
 * > User test
 *  - gather all user inputs (during examination?)
 *  - if any mistakes has been done by buser --> show what and how (step/instructions/feedback/animations)
*/