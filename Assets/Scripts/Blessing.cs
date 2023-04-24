using System.Linq;
using UnityEngine;

public class Blessing : MonoBehaviour, IPlayerRespawnListener
{
    [SerializeField] private PlayerStatus player;

    private void Start()
    {
        gameObject.SetActive(IsSaved() == false);
        player.AddRespawnListener(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            gameObject.SetActive(false);
            player.ShowBlessings();
        }
    }

    public void OnPlayerRespawn()
    {
        gameObject.SetActive(IsSaved() == false);
    }

    private bool IsSaved()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Contains(name);
    }
}
