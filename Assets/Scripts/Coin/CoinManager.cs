using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coin;
    [SerializeField] private TMP_Text coinsDisplay;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (coinsDisplay != null)
        {
            coinsDisplay.text = coin.ToString();
        }
    }




    public void ChangeCoins(int amount) 
    {
        coin += amount;
    }
}
