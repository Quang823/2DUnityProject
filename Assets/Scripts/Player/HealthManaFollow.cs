using UnityEngine;

public class HealthManaFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;    

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
