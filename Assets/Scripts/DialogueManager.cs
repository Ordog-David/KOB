using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image characterImage;
    [SerializeField] private Text characterName;
    [SerializeField] private Text sentence;
    [SerializeField] private Animator animator;

    private Queue<Dialogue> dialogues;
    private Queue<string> sentences;

    void Start()
    {
        dialogues = new Queue<Dialogue>();
        sentences = new Queue<string>();
    }

    public void StartDialogues(Dialogue[] dialogues)
    {
        this.dialogues.Clear();
        foreach (var dialogue in dialogues)
        {
            this.dialogues.Enqueue(dialogue);
        }

        animator.SetBool("IsOpen", true);

        DisplayNextDialogue();
    }

    private void DisplayNextDialogue()
    {
        if (dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        var dialogue = dialogues.Dequeue();
        characterImage.sprite = dialogue.characterSprite;
        characterName.text = dialogue.characterName;

        sentences.Clear();
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            DisplayNextDialogue();
            return;
        }

        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        this.sentence.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            this.sentence.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
