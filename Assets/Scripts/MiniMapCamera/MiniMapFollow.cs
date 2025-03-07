using UnityEngine;
using UnityEngine.Splines;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0, 10, -10); 
    public Vector2 minBound; 
    public Vector2 maxBound;

    private float camSize; 
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
