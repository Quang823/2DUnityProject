using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using System.Collections;
using TMPro;

public class Final_Boss_Trigger : MonoBehaviour
{
    public GameObject dangerEffect;
    public Transform boss; 
    public float dangerDuration = 2f;
    private bool hasTriggered = false;

    public float cameraMoveSpeed = 2f; 
    public float cameraZoomOut = 10f;

    public float flyHeight = 5f; 
    public float flyDuration = 1f; 
    public float fadeDurationBoss = 1f; 

 
    public Image fadeImage;
    public TextMeshProUGUI continueText; 
    public float fadeDuration = 2f;
    public string menuSceneName = "MENU"; 

    private void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
        if (continueText != null)
        {
            continueText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(StartBossSequence());

            PlayerMovement player = other.GetComponent<PlayerMovement>();
        }
    }

    private IEnumerator StartBossSequence()
    {

        if (dangerEffect != null)
        {
            dangerEffect.SetActive(true);
            if (dangerEffect.TryGetComponent<Animator>(out var dangerAnimator))
            {
                dangerAnimator.SetTrigger("Flash");
            }
        }

        yield return new WaitForSeconds(dangerDuration);

        if (dangerEffect != null)
        {
            dangerEffect.SetActive(false);
        }

        Vector3 cameraStartPos = Camera.main.transform.position;
        float originalOrthoSize = Camera.main.orthographicSize;

        FollowPlayer followPlayerScript = Camera.main.GetComponent<FollowPlayer>();
        if (followPlayerScript != null)
        {
            followPlayerScript.enabled = false;
        }

        yield return StartCoroutine(MoveCameraToBoss(cameraStartPos, originalOrthoSize));


        yield return StartCoroutine(FlyAndDisappearBoss());

        yield return StartCoroutine(MoveCameraBackToPlayer(cameraStartPos, originalOrthoSize));

        if (followPlayerScript != null)
        {
            followPlayerScript.enabled = true;
        }

        yield return StartCoroutine(FadeToBlackAndShowText());

        SceneManager.LoadScene(menuSceneName);
    }

    private IEnumerator MoveCameraToBoss(Vector3 cameraStartPos, float originalOrthoSize)
    {
        float elapsedTime = 0f;
        Vector3 cameraTargetPos = new Vector3(boss.position.x, boss.position.y, Camera.main.transform.position.z);

        while (elapsedTime < cameraMoveSpeed)
        {

            Camera.main.transform.position = Vector3.Lerp(cameraStartPos, cameraTargetPos, elapsedTime / cameraMoveSpeed);
         
            Camera.main.orthographicSize = Mathf.Lerp(originalOrthoSize, cameraZoomOut, elapsedTime / cameraMoveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraTargetPos;
        Camera.main.orthographicSize = cameraZoomOut;
    }

    private IEnumerator MoveCameraBackToPlayer(Vector3 cameraStartPos, float originalOrthoSize)
    {
        float elapsedTime = 0f;

        while (elapsedTime < cameraMoveSpeed)
        {
          
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraStartPos, elapsedTime / cameraMoveSpeed);
       
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, originalOrthoSize, elapsedTime / cameraMoveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraStartPos;
        Camera.main.orthographicSize = originalOrthoSize;
    }

    private IEnumerator FlyAndDisappearBoss()
    {
        if (boss == null) yield break;

    
        Vector3 startPos = boss.position;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + flyHeight, startPos.z);

        // Bay lên
        float elapsedTime = 0f;
        while (elapsedTime < flyDuration)
        {
            boss.position = Vector3.Lerp(startPos, targetPos, elapsedTime / flyDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        boss.position = targetPos;

      
        SpriteRenderer bossRenderer = boss.GetComponent<SpriteRenderer>();
        if (bossRenderer != null)
        {
            elapsedTime = 0f;
            Color startColor = bossRenderer.color;
            while (elapsedTime < fadeDurationBoss)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDurationBoss);
                bossRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            bossRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }

        boss.gameObject.SetActive(false);
    }

    private IEnumerator FadeToBlackAndShowText()
    {
        // Bật UI
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0); 
        }
        if (continueText != null)
        {
            continueText.gameObject.SetActive(true);
            continueText.color = new Color(1, 1, 1, 0); 
            continueText.text = "To Be Continued";
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeDuration;

            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, alpha); 
            }
            if (continueText != null)
            {
                continueText.color = new Color(1, 1, 1, alpha); 
            }

            yield return null;
        }


        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1);
        }
        if (continueText != null)
        {
            continueText.color = new Color(1, 1, 1, 1);
        }

        yield return new WaitForSeconds(2f);
    }
}