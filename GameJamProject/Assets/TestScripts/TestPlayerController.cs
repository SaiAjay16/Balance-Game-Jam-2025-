using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 lastMoveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // New: Animator reference
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

        // Optional: prevent diagonal movement
        if (movement.x != 0) movement.y = 0;

        if (movement != Vector2.zero)
        {
            lastMoveDir = movement;
        }

        UpdateAnimation(); // 🎯 New: Update animator based on movement

        ShowInteractPrompt();

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

    private void UpdateAnimation()
    {
        if (movement.x > 0)
        {
            animator.Play("Walk_Right");
        }
        else if (movement.x < 0)
        {
            animator.Play("Walk_Left");
        }
        else if (movement.y > 0)
        {
            animator.Play("PlayerMoveTopAnimation");
        }
        else if (movement.y < 0)
        {
            animator.Play("PlayerMoveDownAnimation");
        }
        else
        {
            // No movement -> Idle facing last direction
            if (lastMoveDir.x > 0)
            {
                animator.Play("Walk_Right"); // for now use walk anims even for idle (can improve later)
            }
            else if (lastMoveDir.x < 0)
            {
                animator.Play("Walk_Left");
            }
            else if (lastMoveDir.y > 0)
            {
                animator.Play("Walk_Up");
            }
            else if (lastMoveDir.y < 0)
            {
                animator.Play("Walk_Down");
            }
        }
    }
}
