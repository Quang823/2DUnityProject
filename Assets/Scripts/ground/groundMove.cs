using UnityEngine;

public class GroundMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;  // T?c ?? di chuy?n
    public float leftBoundary = -5f;  // Gi?i h?n trái
    public float rightBoundary = 5f;  // Gi?i h?n ph?i

    private int direction = 1;

    void Update()
    {
        // Di chuy?n ??i t??ng
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // ??i h??ng khi ch?m ranh gi?i
        if (transform.position.x >= rightBoundary || transform.position.x <= leftBoundary)
        {
            direction *= -1; // ??o chi?u
        }
    }
}
