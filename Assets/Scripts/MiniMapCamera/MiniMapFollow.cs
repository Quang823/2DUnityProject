using UnityEngine;
using UnityEngine.Splines;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player; // Nhân vật
    public Vector3 offset = new Vector3(0, 10, -10); // Điều chỉnh góc nhìn MiniMap
    public Vector2 minBound; // Giới hạn nhỏ nhất của MiniMap Camera
    public Vector2 maxBound; // Giới hạn lớn nhất của MiniMap Camera

    private float camSize; // Kích thước camera (orthographicSize)
    private Camera miniMapCam;

    void Start()
    {
        miniMapCam = GetComponent<Camera>();
        camSize = miniMapCam.orthographicSize;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = player.position + offset;
            float camWidth = camSize * miniMapCam.aspect;
            newPosition.x = Mathf.Clamp(newPosition.x, minBound.x + camWidth, maxBound.x - camWidth);
            newPosition.y = transform.position.y;
            newPosition.z = -10;

            transform.position = newPosition;
        }
    }

}
