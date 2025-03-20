using UnityEngine;

public class ProjecTile : MonoBehaviour
{
    [Header("Far Attack Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int farAttackDamage;
    private float direction;
    private bool hit;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra EnemyHealth
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(farAttackDamage);
        }

        EnemyHealthRound3 enemyHealthRound3 = collision.GetComponent<EnemyHealthRound3>();
        if (enemyHealthRound3 != null)
        {
            enemyHealthRound3.TakeDamage(farAttackDamage);
        }

        // Kiểm tra BossHealth
        BossHealth bossHealth = collision.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(farAttackDamage);
        }
    }


    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}