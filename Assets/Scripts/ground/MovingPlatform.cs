using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform topPosition; 
    public Transform bottomPosition; 
    public float speed = 2f;

    private bool playerOnPlatform = false; 

    void Update()
    {
        if (playerOnPlatform)
        {
            transform.position = Vector3.MoveTowards(transform.position, topPosition.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPosition.position, speed * Time.deltaTime);
        }
    }

    public void SetPlatformActive(bool isActive)
    {
        playerOnPlatform = isActive;
    }
}
