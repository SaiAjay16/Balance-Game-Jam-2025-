using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactPrompt;
    public string[] dialogueLines;


    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(show);
    }

    public void Interact()
    {
        Debug.Log("Player interacted with NPC");
        Object.FindFirstObjectByType<DialogueManager>()?.StartDialogue(dialogueLines);

    }
}
