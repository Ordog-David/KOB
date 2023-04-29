using System.Linq;
using UnityEngine;

public class RandomDialogueTrigger : MonoBehaviour
{
    private bool triggered;

    private void Start()
    {
        triggered = SavegameManager.Instance.Data.visitedDialogueTriggers.Contains(name);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && triggered == false)
        {
            triggered = true;
            DialogueManager.Instance.StartRandomDialogue(name);
        }
    }
}
