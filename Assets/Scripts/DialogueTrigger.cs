using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnCollision = true;
    [SerializeField] private Dialogue[] dialogues;

    private bool triggered = false;

    public void TriggerDialogue()
    {
        if (triggered == false)
        {
            triggered = true;
            FindObjectOfType<DialogueManager>().StartDialogues(dialogues);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (triggerOnCollision)
        {
            TriggerDialogue();
        }
    }
}
