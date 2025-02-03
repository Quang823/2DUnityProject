using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform topPosition; // V? trí cao nh?t b?c th?m s? ?i ??n
    public Transform bottomPosition; // V? trí th?p nh?t (ban ??u)
    public float speed = 2f; // T?c ?? di chuy?n

    private bool playerOnPlatform = false; // Ki?m tra player có kích ho?t c? hay không

    void Update()
    {
        if (playerOnPlatform)
        {
            // Di chuy?n lên n?u player kích ho?t c?
            transform.position = Vector3.MoveTowards(transform.position, topPosition.position, speed * Time.deltaTime);
        }
        else
        {
            // Di chuy?n xu?ng n?u không có player
            transform.position = Vector3.MoveTowards(transform.position, bottomPosition.position, speed * Time.deltaTime);
        }
    }

    public void SetPlatformActive(bool isActive)
    {
        playerOnPlatform = isActive;
    }
}
