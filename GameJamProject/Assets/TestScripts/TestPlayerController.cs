using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    public bool canMove = true;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMoveDir;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove)
        {
            ShowInteractPrompt(); // still let prompt appear while paused
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        ShowInteractPrompt();


        // Optional: prevent diagonal movement
        if (movement.x != 0) movement.y = 0;

        // Save last non-zero direction for interaction
        if (movement != Vector2.zero)
        {
            lastMoveDir = movement;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Vector2 targetPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        if (IsWalkable(targetPos))
        {
            rb.MovePosition(targetPos);
        }
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer) == null;
    }

    private void Interact()
    {

        if (lastMoveDir == Vector2.zero) return;

        Vector3 interactPos = transform.position + (Vector3)lastMoveDir;
        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

        Debug.DrawLine(transform.position, interactPos, Color.yellow, 1f);

        if (collider != null)
        {
            Debug.Log("NPC detected!");
            collider.GetComponent<NPCInteraction>()?.Interact();
        }
        else
        {
            Debug.Log("Nothing to interact with");
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (lastMoveDir == Vector2.zero) return;

        Vector3 interactPos = transform.position + (Vector3)lastMoveDir;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactPos, 0.2f);
    }

    public void ShowInteractPrompt()
    {
        if (DialogueManager.IsDialogueOpen) return;
        if (lastMoveDir == Vector2.zero) return;

        Vector3 interactPos = transform.position + (Vector3)lastMoveDir;
        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

        if (collider != null)
        {
            var npc = collider.GetComponent<NPCInteraction>();
            if (npc != null) npc.ShowPrompt(true);
        }
        else
        {
            // hide all NPC prompts just in case
            foreach (var npc in Object.FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None))
            {
                npc.ShowPrompt(false);
            }

        }
    }

}
