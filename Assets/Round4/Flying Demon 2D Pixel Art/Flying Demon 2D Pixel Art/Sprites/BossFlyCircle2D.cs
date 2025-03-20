using UnityEngine;

public class BossFlyCircle2D : MonoBehaviour
{
    public Transform centerPoint; // ?i?m trung tâm c?a vòng tròn
    public float radius = 3f; // Bán kính vòng tròn
    public float speed = 2f; // T?c ?? bay

    private float angle = 0f;

    void Update()
    {
        //angle += speed * Time.deltaTime;
        //float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
        //float y = centerPoint.position.y + Mathf.Sin(angle) * radius;
        //transform.position = new Vector2(x, y);
        Vector2 startPosition = centerPoint.position;
        transform.position = new Vector2(startPosition.x + Mathf.PingPong(Time.time * speed, 6) - 3, startPosition.y);
    }
}
