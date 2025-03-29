using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Movement parameters")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float chaseSpeed = 6f;
    private Vector2 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration = 1f;
    private float idleTimer;

    [Header("Y Range Check")]
    [SerializeField] private float yTolerance = 0.5f;

    [Header("EnemyAnimator")]
    [SerializeField] private Animator anim;
    private bool isPaused = false;

    private void Awake()
    {
        initScale = enemy.localScale;
        if (enemy == null) enemy = transform;
    }

    private void OnDisable()
    {
        if (anim != null) anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (isPaused) return;

        if (player == null || leftEdge == null || rightEdge == null)
        {
            Patrol();
            return;
        }

        if (IsPlayerInRange())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private bool IsPlayerInRange()
    {
        float playerX = player.position.x;
        float playerY = player.position.y;
        float enemyY = enemy.position.y;
        float leftX = leftEdge.position.x;
        float rightX = rightEdge.position.x;


        bool inXRange = playerX > leftX && playerX < rightX;
        bool inYRange = Mathf.Abs(playerY - enemyY) <= yTolerance;

        return inXRange && inYRange;
    }

    private void ChasePlayer()
    {
        if (anim != null) anim.SetBool("moving", true);
        idleTimer = 0;

        float direction = Mathf.Sign(player.position.x - enemy.position.x);
        enemy.localScale = new Vector2(Mathf.Abs(initScale.x) * direction, initScale.y);

        float moveSpeed = chaseSpeed > 0 ? chaseSpeed : speed;
        float newX = enemy.position.x + Time.deltaTime * direction * moveSpeed;
        enemy.position = new Vector2(newX, enemy.position.y);
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveinDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveinDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    public void PausePatrol()
    {
        isPaused = true;
        if (anim != null) anim.SetBool("moving", false);
    }

    private void DirectionChange()
    {
        if (anim != null) anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void MoveinDirection(int _direction)
    {
        idleTimer = 0;
        if (anim != null) anim.SetBool("moving", true);
        enemy.localScale = new Vector2(Mathf.Abs(initScale.x) * _direction, initScale.y);
        float newX = enemy.position.x + Time.deltaTime * _direction * speed;
        enemy.position = new Vector2(newX, enemy.position.y);
    }
}