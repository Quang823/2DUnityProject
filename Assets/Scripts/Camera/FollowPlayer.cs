using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;  

    public Vector2 minBounds; 
    public Vector2 maxBounds; 

    private Camera cam;

    void Start()
    {
        cam = Camera.main; 
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;


        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

 
        transform.position = new Vector3(clampedX, clampedY, desiredPosition.z);
    }
}
