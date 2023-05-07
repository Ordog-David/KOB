using UnityEngine;

public class HealthResetter : MonoBehaviour, IPlayerRespawnListener
{
    [SerializeField] private PlayerStatus player;

    private bool started = false;

    private void Start()
    {
        gameObject.SetActive(true);
        AddListener();

        started = true;
    }

    private void OnValidate()
    {
        if (started)
        {
            AddListener();
        }
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            gameObject.SetActive(false);
        }
    }

    void IPlayerRespawnListener.OnPlayerRespawn()
    {
        gameObject.SetActive(true);
    }

    private void AddListener()
    {
        player.AddRespawnListener(this);
    }

    private void RemoveListener()
    {
        player.RemoveRespawnListener(this);
    }
}
