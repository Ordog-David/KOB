using Extensions;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Blessing : MonoBehaviour, IPlayerRespawnListener, ISavegameSavedListener
{
    [SerializeField] private BlessingIndicator indicator;
    [SerializeField] private PlayerStatus player;
    [SerializeField] private AudioSource collectedSFX;
    [SerializeField] private AudioSource lostSFX;
    private SpriteRenderer spriteRenderer;
    private Collider2D blessingCollider;
    private Light2D blessingLight;

    private bool started = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        blessingCollider = GetComponent<CapsuleCollider2D>();
        blessingLight = GetComponentInChildren<Light2D>();
        AddListeners();
        UpdateStatus();

        started = true;
    }

    private void OnValidate()
    {
        if (started)
        {
            AddListeners();
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            spriteRenderer.enabled = false;
            blessingCollider.enabled = false;
            blessingLight.enabled = false;
            indicator.SetCollected();
            this.PlaySoundThen(collectedSFX, () => gameObject.SetActive(false));
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

    private void RemoveListeners()
    {
        player.RemoveRespawnListener(this);
        SavegameManager.Instance.RemoveSavegameSavedListener(this);
    }

    private void UpdateStatus()
    {
        var saved = IsSaved();
        var respawned = gameObject.activeInHierarchy == false && saved == false;

        indicator.SetSavedAndCollected(saved);
        gameObject.SetActive(saved == false);

        if (respawned)
        {
            lostSFX.Play();
            spriteRenderer.enabled = true;
            blessingCollider.enabled = true;
            blessingLight.enabled = true;
        }
    }

    private bool IsSaved()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Contains(name);
    }
}
