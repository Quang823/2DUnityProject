using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset = new Vector3(0, 2, 0); 

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
