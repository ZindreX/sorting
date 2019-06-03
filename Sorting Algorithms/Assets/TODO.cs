/* 
 * >>> Status of sorting algorithms:
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Algorithm name    |   Non-3D/VR   |   Tutorial    |   Step-by-step    |   User Test   |                               Comment                                 |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Bubble Sort       |     Yes       |     Yes       |       Yes         |       Yes     | Complete                                                              |
 *    Insertion Sort    |     Yes       |     Yes       |       Yes*        |       Yes     | Complete                                                              |
 *    Bucket Sort       |     Yes       |     Yes       |       Yes*        |       Yes*    | Complete* (insertion sort phase not implemented)                      |
 *    Merge Sort        |     Yes       |     No        |       No          |       No      | Standard: OK, instruction not fixed                                   |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 * 
 *  * >>> Status of graph algorithms:
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *    Algorithm name    |   Demo/Step-by-step    |       User Test          |                               Comment                                                 |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 *          BFS         |         Yes            |          Yes             | Complete* (Backward-step not complete)                                                |
 *          DFS         |         Yes            |          Yes             | Complete* (Backward-step not complete)                                                |
 *        Dijkstra      |         Yes            |          Yes             | Complete* (Backward-step not complete)                                                |
 * -----------------------------------------------------------------------------------------------------------------------------------------------------------------|
 * 
 * 
 * *********************************** To do list **********************************************
 * > User Test
 *  - remove UserActionToProceed and decrement ReadyForNext instead ? (down to 0)
 *  
 * > ElementManager (cleanup: (duplicate) randomizing values)
 * - ElementsBasedOnCase: first gather values then distribute, instead of distribute -> redistribute based on sorting case (worst/best) ?
 * 
 *  
 * 
 * *********************************** Bugs to fix ********************************************** 
 * >>> BucketSort
 *  > New Demo:
 *      - Step-by-step: backwards needs more work
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
 *  - if any mistakes has been done by user --> show what and how (step/instructions/feedback/animations)
 *  
 *  
 *  
 *  
 *  
 *  
 *  *********************************** Readme stuff **********************************************                   <----------------------------------------------------------- Readme!
 *  
 * > VHS: Destroyed?
 * - Check filepath (TODO: add video file to project)
 *  
 *  
 *  
*/