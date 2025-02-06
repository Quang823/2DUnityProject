using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public GameObject miniMap;
    public GameObject enemyQuantity;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StartTransition(Vector3 newPosition, Transform player)
    {
        StartCoroutine(FadeAndMove(newPosition, player));
    }

    private IEnumerator FadeAndMove(Vector3 newPosition, Transform player)
    {
        yield return StartCoroutine(FadeToBlack());

        player.position = newPosition;
        Camera.main.transform.position = new Vector3(newPosition.x, newPosition.y, Camera.main.transform.position.z);

        yield return StartCoroutine(FadeFromBlack());
    }


    private IEnumerator FadeToBlack()
    {
        if (miniMap != null) miniMap.SetActive(false);
        if (enemyQuantity != null) enemyQuantity.SetActive(false);

        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }




    private IEnumerator FadeFromBlack()
    {
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}
