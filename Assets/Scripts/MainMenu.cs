using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource buttonPressSFX;

    public void Quit()
    {
        this.PlaySoundThen(buttonPressSFX, () => Application.Quit());
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
        this.PlaySoundThen(buttonPressSFX, () => SceneManager.LoadScene("Main"));
    }

    public void PlaySound()
    {
        this.PlaySound(buttonPressSFX);
    }
}
