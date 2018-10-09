using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void MainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void BubbleSortScene()
    {
        SceneManager.LoadScene(2);
    }

    public void InsertionSortScene()
    {
        SceneManager.LoadScene(3);
    }

    public void MergeSortScene()
    {
        SceneManager.LoadScene(4);
    }

    public void QuickSortScene()
    {
        SceneManager.LoadScene(5);
    }

    public void BucketSortScene()
    {
        SceneManager.LoadScene(6);
    }

    public void Options()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Quiting application.");
        Application.Quit();
    }

}
