using UnityEngine;
using System.Collections;

public class FireTrapScript : MonoBehaviour
{
    Collider2D filecol;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageCooldown = 1f;
    private float lastDamageTime = 0;

    void Start()
    {
        filecol = this.GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }

    public void fireON()
    {
        filecol.enabled = true;
    }

    public void fireOFF()
    {
        filecol.enabled = false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        playerStars = collision.gameObject.GetComponent<PlayerStars>();
    //        playerStars.TakeDamage(damage);
    //    }
    //}

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
