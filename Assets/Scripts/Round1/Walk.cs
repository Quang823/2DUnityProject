using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField] private Transform posA; // Điểm A
    [SerializeField] private Transform posB; // Điểm B
    [SerializeField] private float speed = 2f;
    [SerializeField] private Animator animator;

    private bool isMoving = true; // Control movement
    private Vector3 target; // Mục tiêu di chuyển
    private bool isMovingRight = true; // Hướng di chuyển

    void Start()
    {
        if (posA == null || posB == null)
        {
            Debug.LogError("posA or posB is not assigned.");
            return;
        }

        target = posB.position; // Bắt đầu di chuyển về posB
    }

    void Update()
    {
        if (posA == null || posB == null || !isMoving) return;

        MoveCharacter();
    }

    void MoveCharacter()
    {
        // Chỉ di chuyển trên trục X, giữ nguyên Y & Z
        float newX = Mathf.MoveTowards(transform.position.x, target.x, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Khi đạt đến target, đổi hướng
        if (Mathf.Abs(transform.position.x - target.x) <= 0.1f)
        {
            target = (target == posA.position) ? posB.position : posA.position;
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        isMovingRight = (target.x > transform.position.x);

        // Đổi hướng bằng cách lật scale X
        Vector3 newScale = transform.localScale;
        newScale.x = isMovingRight ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        // Cập nhật animation nếu có
        if (animator != null)
        {
            animator.SetBool("isMovingRight", isMovingRight);
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    // Resume movement
    public void StartMoving()
    {
        isMoving = true;
    }

}
