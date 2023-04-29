using UnityEngine;

public class HealthResetter : MonoBehaviour, IPlayerRespawnListener
{
    [SerializeField] private PlayerStatus player;

    private void Start()
    {
        gameObject.SetActive(true);
        AddListener();
    }

    private void OnValidate()
    {
        AddListener();
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
        if (player != null)
        {
            player.AddRespawnListener(this);
        }
        else
        {
            Debug.LogWarning("Cannot reactivate health resetter");
        }
    }
}
