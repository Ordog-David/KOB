using Extensions;
using UnityEngine;

public class BlessingDoor : MonoBehaviour
{
    [SerializeField] private AudioSource openingSFX;

    private int numberOfBlessings;
    private bool soundPlayed;

    private void Start()
    {
        numberOfBlessings = FindObjectsOfType<Blessing>(true).Length;
        soundPlayed = false;

        UpdateStatus();
    }

    private void Update()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (gameObject.activeInHierarchy == true && AreAllBlessingsVisited() == true && soundPlayed == false)
        {
            soundPlayed = true;
            this.PlaySoundThen(openingSFX, () => gameObject.SetActive(false));
        }
    }

    private bool AreAllBlessingsVisited()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Length == numberOfBlessings;
    }
}
