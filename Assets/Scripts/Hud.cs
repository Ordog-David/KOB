using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject hudBackgroundCanvas;
    [SerializeField] private Text timerText;
    [SerializeField] private AudioSource buttonPressSFX;

    public bool hidden;
    private bool timeFrozen;

    public void OnQuit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SavegameManager.Instance.Save();
            this.PlaySoundThen(buttonPressSFX, () => SceneManager.LoadScene("Main Menu"));
        }
    }

    public void OnHudToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (hidden == false)
            {
                hudCanvas.SetActive(false);
                hudBackgroundCanvas.SetActive(false);
                hidden = true;
            }
            else
            {
                hudCanvas.SetActive(true);
                hudBackgroundCanvas.SetActive(true);
                hidden = false;
            }
        }
    }

    public void FreezeTime()
    {
        timeFrozen = true;
    }

    private void Update()
    {
        if (timeFrozen == false)
        {
            SavegameManager.Instance.Data.elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(SavegameManager.Instance.Data.elapsedTime);
        }
    }

    private string FormatTime(float time)
    {
        var hours = (int)(time / 3600);
        var minutes = (int)((time % 3600) / 60);
        var seconds = (int)(time % 60);
        var microseconds = (int)(time * 1000) % 1000;

        return string.Format("{0:d}:{1:d2}:{2:d2}.{3:d3}", hours, minutes, seconds, microseconds);
    }
}
