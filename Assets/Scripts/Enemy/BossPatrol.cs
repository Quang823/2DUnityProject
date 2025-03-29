using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Boss")]
    [SerializeField] private Transform boss;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Movement parameters")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseSpeed = 3f;
    private Vector2 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration = 1f;
    private float idleTimer;

    [Header("Y Range Check")]
    [SerializeField] private float yOffset = 0.5f; // Số dương: dí khi player ở trên; Số âm: dí khi player ở dưới

    [Header("BossAnimator")]
    [SerializeField] private Animator anim;
    private bool isPaused = false;

    private void Awake()
    {
        initScale = boss.localScale;
        if (boss == null) boss = transform;
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
        float bossY = boss.position.y;
        float leftX = leftEdge.position.x;
        float rightX = rightEdge.position.x;

        bool inXRange = playerX > leftX && playerX < rightX;
        bool inYRange;

        if (yOffset >= 0)
        {
   
            inYRange = playerY >= bossY && playerY <= bossY + yOffset;
        }
        else
        {
         
            inYRange = playerY <= bossY && playerY >= bossY + yOffset; 
        }

      
        return inXRange && inYRange; 
    }

    private void ChasePlayer()
    {
        if (anim != null) anim.SetBool("moving", true);
        idleTimer = 0;

        float direction = Mathf.Sign(player.position.x - boss.position.x);
        boss.localScale = new Vector2(-initScale.x * direction, initScale.y);

        float moveSpeed = chaseSpeed > 0 ? chaseSpeed : speed;
        float newX = boss.position.x + Time.deltaTime * direction * moveSpeed;
        boss.position = new Vector2(newX, boss.position.y);
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            if (boss.position.x >= leftEdge.position.x)
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
            if (boss.position.x <= rightEdge.position.x)
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
        boss.localScale = new Vector2(-initScale.x * _direction, initScale.y);
        float newX = boss.position.x + Time.deltaTime * _direction * speed;
        boss.position = new Vector2(newX, boss.position.y);
    }
}