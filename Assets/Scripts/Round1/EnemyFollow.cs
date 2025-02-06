using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            ChasePlayer(distance);
        }
    }

    void ChasePlayer(float distance)
    {
        // Move towards the player if within chase range
        if (distance <= chaseRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }

        // Flip direction to face the player
        FlipDirection();

        // Attack if within range and cooldown has passed
        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void FlipDirection()
    {
        Vector3 scale = transform.localScale;
        if (player.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x); // Face right
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // Face left
        }
        transform.localScale = scale;
    }

    void Attack()
    {
        Debug.Log("Enemy bites the player!");
        lastAttackTime = Time.time;
    }
}

