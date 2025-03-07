using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float damage = 10f; 
    [SerializeField] private float damageCooldown = 1f;
    private float lastDamageTime = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null && Time.time > lastDamageTime + damageCooldown)
            {
                player.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
