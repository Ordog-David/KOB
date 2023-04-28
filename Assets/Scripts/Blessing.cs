using System.Linq;
using UnityEngine;

public class Blessing : MonoBehaviour, IPlayerRespawnListener, ISavegameSavedListener
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private PlayerStatus player;
    private BlessingIndicator indicatorScript;

    private void Start()
    {
        indicatorScript = indicator.GetComponent<BlessingIndicator>();
        indicatorScript.SetSaved(IsSaved());
        gameObject.SetActive(IsSaved() == false);
        player.AddRespawnListener(this);
        SavegameManager.Instance.AddSavegameSavedListener(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            indicatorScript.SetCollected();
            gameObject.SetActive(false);
        }
    }

    public void OnPlayerRespawn()
    {
        indicatorScript.SetSaved(IsSaved());
        gameObject.SetActive(IsSaved() == false);
    }

    private bool IsSaved()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Contains(name);
    }

    public void OnSavegameSaved()
    {
        if (IsSaved())
        {
            indicatorScript.OnSavegameSaved();
        }
    }

}
