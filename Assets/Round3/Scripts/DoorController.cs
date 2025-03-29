using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    private bool isDoorActive = false; // Biến để kiểm tra cửa đã được kích hoạt chưa
    public Image fadeScreen; // Tham chiếu đến fadeScreen để thực hiện fade
    public string menuSceneName = "MAIN MENU"; // Tên scene menu

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        anim.SetTrigger("activeDoor"); // Kích hoạt animation của cửa
        isDoorActive = true; // Đánh dấu cửa đã được kích hoạt
    }

    // Phát hiện khi người chơi chạm vào cửa
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDoorActive && other.CompareTag("Player")) // Kiểm tra nếu cửa active và va chạm với player
        {
            if (GameController.instance != null)
            {
                GameController.instance.StartGlobalCoroutine(FadeAndReturnToMenu()); // Gọi qua GameController giống OnGemCollected
            }
            else
            {
                Debug.LogError("GameController.instance is NULL!");
            }
        }
    }

    private IEnumerator FadeAndReturnToMenu()
    {
        if (fadeScreen == null) yield break;

        for (float t = 0; t < 1.5f; t += Time.deltaTime)
        {
            fadeScreen.color = new Color(0, 0, 0, t / 1.5f);
            yield return null;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
            Debug.Log("Player destroyed before returning to MAIN MENU");
        }
        else
        {
            Debug.LogWarning("No player found with tag 'Player'");
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(menuSceneName);
    }
}
