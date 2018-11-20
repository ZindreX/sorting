/* *********************************** To do list **********************************************
 * > Clean up instruction
 *  - split into more groups (inheritage), ID's, I/J ?
 * 
 * > User Test
 *  - fix different levels of difficulty (how much help given, points etc)
 *      - beginner: ok, but doesnt show the final instruction -> ends the pseudoboard
 *  - remove UserActionToProceed and decrement ReadyForNext instead ? (down to 0)
 *  
 * > Tutorial
 *  - disable drag (non-vr), touch/grab (vr)
 * 
 * > User test (when updating old algorithms)
 *  - make all algorithm implement methods insertion sort uses
 * 
 * > ElementManager
 * - ElementsBasedOnCase: first gather values then distribute, instead of distribute -> redistribute based on sorting case (worst/best) ?
 * 
 * > AlgorithmManagerBase
 *  - Clean up IsTutorial() (mixed with IsTutorialStep())
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
 * >>> Commented a lot of old methods, check if any bugs/null exceptions occours
 * 
 * 
 * 
 *  *********************************** Implementation ideas **********************************************
 * > User test
 *  - gather all user inputs (during examination?)
 *  - if any mistakes has been done by buser --> show what and how (step/instructions/feedback/animations)
*/