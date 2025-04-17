using UnityEngine;

public class SnakeEnemyAi : MonoBehaviour
{
     public float moveSpeed = 2f;
    public float followSpeed = 3.5f;
    public float patrolDistance = 2f;
    public float detectionRadius = 3f;
    public float maxFollowDistance = 6f;
    public int maxHealth = 10;
    public int attackDamage = 2;
    public float attackCooldown = 1.5f;

    private int currentHealth;
    private Vector2 startPosition;
    private Vector2 leftPatrolPoint;
    private Vector2 rightPatrolPoint;
    private bool movingRight = true;
    private bool isFollowing = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private Transform player;
    private Animator animator;

    private void Start()
    {
        startPosition = transform.position;
        leftPatrolPoint = startPosition + Vector2.left * patrolDistance;
        rightPatrolPoint = startPosition + Vector2.right * patrolDistance;

        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        float distanceFromStart = Vector2.Distance(startPosition, transform.position);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (isFollowing)
        {
            // If player is out of range, return to start
            if (distanceToPlayer > detectionRadius || distanceFromStart > maxFollowDistance)
            {
                isFollowing = false;
                return;
            }

            FollowPlayer();

            // Attack if close
            if (distanceToPlayer < 0.8f && Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
        else
        {
            // Check if player is within detection radius
            if (distanceToPlayer < detectionRadius)
            {
                isFollowing = true;
                return;
            }

            Patrol();
        }
    }

    private void Patrol()
    {
        Vector2 target = movingRight ? rightPatrolPoint : leftPatrolPoint;
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            movingRight = !movingRight;

        animator.SetBool("isWalking", true);
    }

    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        animator.SetBool("isWalking", true);
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
        lastAttackTime = Time.time;

        // Damage player (you'd need a player health script)
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("die");
        // Disable collider and script
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
