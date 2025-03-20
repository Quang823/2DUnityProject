using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Transform topPosition;
    public Transform bottomPosition;
    public float platformSpeed = 2f;

    private bool playerOnPlatform = false;

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (playerOnPlatform)
        {
            transform.position = Vector3.MoveTowards(transform.position, topPosition.position, platformSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPosition.position, platformSpeed * Time.deltaTime);
        }
    }

    public void SetPlatformActive(bool isActive)
    {
        playerOnPlatform = isActive;
    }
}
