using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        StartGame();
    }

    public void NewGame()
    {
        SavegameManager.Instance.Clear();
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
