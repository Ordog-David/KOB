using System.Linq;
using UnityEngine;

public class Blessing : MonoBehaviour, IPlayerRespawnListener, ISavegameSavedListener
{
    [SerializeField] private BlessingIndicator indicator;
    [SerializeField] private PlayerStatus player;

    private void Start()
    {
        AddListeners();
        UpdateStatus();
    }

    private void OnValidate()
    {
        AddListeners();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            indicator.SetCollected();
            gameObject.SetActive(false);
        }
    }

    void IPlayerRespawnListener.OnPlayerRespawn()
    {
        UpdateStatus();
    }

    void ISavegameSavedListener.OnSavegameSaved()
    {
        /* We should not call gameObject.SetActive() here, becuse if the game is not saved on a checkpoint, then it
         * would reactivate the checkpoint */
        indicator.SetSaved(IsSaved());
    }

    private void AddListeners()
    {
        player.AddRespawnListener(this);
        SavegameManager.Instance.AddSavegameSavedListener(this);
    }

    private void UpdateStatus()
    {
        var saved = IsSaved();
        indicator.SetSavedAndCollected(saved);
        gameObject.SetActive(saved == false);
    }

    private bool IsSaved()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Contains(name);
    }
}
