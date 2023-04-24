using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnCollision = true;
    [SerializeField] private Dialogue[] dialogues;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D _)
    {
        if (triggerOnCollision)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (triggered == false)
        {
            triggered = true;
            FindObjectOfType<DialogueManager>().StartDialogues(dialogues);
        }
    }
}
