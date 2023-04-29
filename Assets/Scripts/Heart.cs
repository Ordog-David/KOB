using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject menu;
    [SerializeField] private Image curtain;
    [SerializeField] private Hud hud;
    [SerializeField] private Color neptuniColor;
    [SerializeField] private Color dusiiColor;
    [SerializeField] private Color incubiColor;
    [SerializeField] private Color spiritualiaColor;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            menu.SetActive(true);
            player.SetFrozen(true);
            hud.FreezeTime();
            SavegameManager.Instance.Clear();
        }
    }

    public void OnNeptuniEnding()
    {
        OnEnding(neptuniColor);
    }

    public void OnDusiiEnding()
    {
        OnEnding(dusiiColor);
    }

    public void OnIncubiEnding()
    {
        OnEnding(incubiColor);
    }

    public void OnSpiritualiaEnding()
    {
        OnEnding(spiritualiaColor);
    }

    private void OnEnding(Color endColor)
    {
        menu.SetActive(false);

        var startColor = endColor;
        startColor.a = 0f;

        DOTween.To(() => startColor, x => curtain.color = x, endColor, 15f)
            .OnComplete(() => SceneManager.LoadScene("Main Menu"));
    }
}
