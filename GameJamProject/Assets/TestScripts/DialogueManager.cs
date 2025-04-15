using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private PlayerMovement player; // ← your actual script name

    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.03f;

    private string[] lines;
    private int index;
    private bool isTyping;
    private bool isDialogueActive = false;

    public static bool IsDialogueOpen = false;

    public void StartDialogue(string[] dialogueLines)
    {
        if (isDialogueActive) return;

        IsDialogueOpen = true;
        isDialogueActive = true;

        // Get player reference and disable movement
        player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
            player.canMove = false;

        // Hide any visible interact prompts immediately
        foreach (var npc in Object.FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None))
        {
            npc.ShowPrompt(false);
        }

        Debug.Log("StartDialogue CALLED");

        lines = dialogueLines;
        index = 0;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void Update()
    {
        if (!dialogueBox.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                isTyping = false;
            }
            else
            {
                index++;
                if (index < lines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    CloseDialogue();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        isTyping = false;
        isDialogueActive = false;
        IsDialogueOpen = false;

        if (player != null)
        {
            player.canMove = true;
            player.ShowInteractPrompt();
        }
    }
}
