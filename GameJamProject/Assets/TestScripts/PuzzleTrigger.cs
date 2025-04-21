using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    private bool playerInRange = false;
    private PuzzleManager puzzleManager;

    private void Start()
    {
        puzzleManager = Object.FindFirstObjectByType<PuzzleManager>(); //Find puzzle manager in the scene.
    }


    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            OpenPuzzle();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OpenPuzzle()
    {
        if (puzzleManager != null)
        {
            puzzleManager.OpenPuzzle(); // ✅ Call the fade-in method from PuzzleManager
            Debug.Log("Puzzle opened!");
        }
        else
        {
            Debug.LogError("PuzzleManager not found in scene!");
        }
    }
}
