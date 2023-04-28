using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject menu;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private GameObject curtain;
    [SerializeField] private UIActions uiActions;
    [SerializeField] private Color NeptuniColor;
    [SerializeField] private Color DusiiColor;
    [SerializeField] private Color IncubiColor;
    [SerializeField] private Color SpiritualiaColor;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            menu.SetActive(true);
            playerMovement.SetFrozen(true);
            uiActions.FreezeTime();
            SavegameManager.Instance.Reset();
        }
    }

    public void OnNeptuniEnding()
    {
        OnEnding(NeptuniColor);
    }

    public void OnDusiiEnding()
    {
        OnEnding(DusiiColor);
    }

    public void OnIncubiEnding()
    {
        OnEnding(IncubiColor);
    }

    public void OnSpiritualiaEnding()
    {
        OnEnding(SpiritualiaColor);
    }

    private void OnEnding(Color endColor)
    {
        menu.SetActive(false);

        var startColor = endColor;
        startColor.a = 0f;

        var image = curtain.GetComponent<Image>();

        DOTween.To(() => startColor, x => image.color = x, endColor, 15f)
            .OnComplete(() => SceneManager.LoadScene("Main Menu"));
    }
}
