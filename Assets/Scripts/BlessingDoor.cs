using UnityEngine;

public class BlessingDoor : MonoBehaviour
{
    private int numberOfBlessings;

    private void Start()
    {
        numberOfBlessings = FindObjectsOfType<Blessing>(true).Length;

        UpdateStatus();
    }

    private void Update()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        gameObject.SetActive(AreAllBlessingsVisited() == false);
    }

    private bool AreAllBlessingsVisited()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Length == numberOfBlessings;
    }
}
