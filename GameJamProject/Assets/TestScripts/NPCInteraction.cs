using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool isInteractable = true; // ✅ Toggle in Inspector

    [Header("Prompt")]
    public GameObject interactPrompt;

    [Header("Dialogue")]
    [TextArea(3, 5)]
    public string[] dialogueLines;

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        if (!isInteractable) return; // ❌ Skip showing prompt if NPC is not interactable

        if (interactPrompt != null)
            interactPrompt.SetActive(show);
    }

    public void Interact()
    {
        if (!isInteractable) return; // ❌ Skip interaction if not interactable

        Debug.Log("Player interacted with NPC");

        DialogueManager dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogueLines);
        }
    }
}
