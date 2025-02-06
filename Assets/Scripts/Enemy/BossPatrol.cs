using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Boss")]
    [SerializeField] private Transform boss;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector2 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("EnemyAnamitor")]
    [SerializeField] private Animator anim;
    private bool isPaused = false;
    private void Awake()
    {
        initScale = boss.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }


    private void Update()
    {
        if (isPaused) return;

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
        anim.SetBool("moving", false);
    }




    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0;  

        }
    }

    private void MoveinDirection(int _direction)
    {
        if (isPaused) return;

        idleTimer = 0;
        anim.SetBool("moving", true);

        boss.localScale = new Vector2(initScale.x * _direction, initScale.y);
        boss.position = new Vector2(boss.position.x + Time.deltaTime * _direction * speed,
            boss.position.y);
    }
}
