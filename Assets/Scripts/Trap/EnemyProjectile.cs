using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 3f;

    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private bool hit;
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void ActivateProjectile(float direction)
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;

        rb.linearVelocity = new Vector2(speed * direction, 0);
        transform.localScale = new Vector3(direction, 1, 1);
    }


    private void Update()
    {
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(direction * movementSpeed);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return;

        hit = true;
        coll.enabled = false;

        if (anim != null)
            anim.SetTrigger("explodes");
        else
            gameObject.SetActive(false);
    }


    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
