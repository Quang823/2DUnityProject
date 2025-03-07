using UnityEngine;

public class GroundMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;  
    public float leftBoundary = -5f;  
    public float rightBoundary = 5f; 

    private int direction = 1;

    void Update()
    {

        transform.position += Vector3.right * direction * speed * Time.deltaTime;


        if (transform.position.x >= rightBoundary || transform.position.x <= leftBoundary)
        {
            direction *= -1;
        }
    }
}
