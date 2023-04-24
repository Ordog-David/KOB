using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject menu;
    [SerializeField] private Light2D globalLight;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            menu.SetActive(true);
            playerMovement.SetFrozen(true);
        }
    }

    public void OnMenuItemClicked()
    {
        menu.SetActive(false);

        DOTween.To(() => globalLight.color, x => globalLight.color = x, Color.black, 15f)
            .OnComplete(() => SceneManager.LoadScene("Main Menu"));
    }
}
