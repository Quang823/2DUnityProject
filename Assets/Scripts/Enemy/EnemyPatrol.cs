using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector2 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("EnemyAnamitor")]
    [SerializeField] private Animator anim;
    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }
    private bool isPaused = false;

    private void Update()
    {
        if (isPaused) return;

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
        anim.SetBool("moving", false);
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }      
    }
    private void MoveinDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);      
        enemy.localScale = new Vector2(Mathf.Abs(initScale.x) * _direction, initScale.y);
        enemy.position = new Vector2(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y);
    }
}