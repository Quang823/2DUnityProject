using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform bossRoomSpawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransition.instance.StartTransition(bossRoomSpawnPoint.position, other.transform);
        }

        AudioSource portalAudio = GetComponent<AudioSource>();
        if (portalAudio != null)
        {
            portalAudio.Stop();
        }

        Destroy(gameObject, 1f);
    }
}