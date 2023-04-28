using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIActions : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject spiritsBlessingIndicator;
    private bool hudHidden;
    private bool timeFrozen;
    private Text timerText;

    public void OnQuit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SavegameManager.Instance.Save();
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void OnHudHide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (hudHidden == false)
            {
                menu.SetActive(false);
                menuBackground.SetActive(false);
                hudHidden = true;
            }
            else
            {
                menu.SetActive(true);
                menuBackground.SetActive(true);
                hudHidden = false;
            }
        }
    }

    public void FreezeTime()
    {
        timeFrozen = true;
    }

    private void Start()
    {
        timerText = timer.GetComponent<Text>();
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
