using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GemManager : MonoBehaviour
{
    public static GemManager instance;
    public List<Sprite> collectedGems = new List<Sprite>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGem(Sprite gemSprite, int levelIndex)
    {
        while (collectedGems.Count <= levelIndex)
        {
            collectedGems.Add(null);
        }

        if (collectedGems[levelIndex] == null)
        {
            collectedGems[levelIndex] = gemSprite;
        }
        else
        {
            Debug.LogWarning($"Gem tại level {levelIndex} đã tồn tại, không thêm mới!");
        }

        UpdateUI();
    }


   private void UpdateUI()
{
    GameObject[] gemSlots = GameObject.FindGameObjectsWithTag("GemSlot");

 
    System.Array.Sort(gemSlots, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

    for (int i = 0; i < gemSlots.Length; i++)
    {
        Image slotImage = gemSlots[i].GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.rectTransform.sizeDelta = new Vector2(100, 100);

            if (i < collectedGems.Count && collectedGems[i] != null)
            {
                slotImage.sprite = collectedGems[i];
                slotImage.color = Color.white;
                slotImage.preserveAspect = true;
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1, 1, 1, 0);
            }
        }
    }
}


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }
}
