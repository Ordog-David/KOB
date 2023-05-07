using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Hud hud;
    [SerializeField] private Dialogue[] dialogues;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && triggered == false && hud.hidden == false)
        {
            triggered = true;
            DialogueManager.Instance.StartDialogues(dialogues);
        }
    }
}
