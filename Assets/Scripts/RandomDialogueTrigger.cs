using System.Linq;
using UnityEngine;

public class RandomDialogueTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnCollision = true;

    private bool triggered;

    private void Start()
    {
        triggered = SavegameManager.Instance.Data.visitedDialogueTriggers.Contains(name);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (triggerOnCollision && collider2D.name == "Player")
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (triggered == false)
        {
            triggered = true;
            FindObjectOfType<DialogueManager>().StartRandomDialogue(name);
        }
    }
}
