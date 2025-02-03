using UnityEngine;

public class CoinPicker : MonoBehaviour
{

    public CoinManager coinManager;
    [SerializeField] private int value;
    private bool hasTriggered;
    [SerializeField] AudioSource pickupSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinManager = CoinManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickupSound.Play();
            hasTriggered = true;
            coinManager.ChangeCoins(value);
            Destroy(gameObject);
        }
    }
}
