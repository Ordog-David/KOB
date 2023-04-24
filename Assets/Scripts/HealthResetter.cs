using UnityEngine;

public class HealthResetter : MonoBehaviour, IPlayerRespawnListener
{
    [SerializeField] private PlayerStatus player;

    private void Start()
    {
        gameObject.SetActive(true);
        player.AddRespawnListener(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPlayerRespawn()
    {
        gameObject.SetActive(true);
    }
}
