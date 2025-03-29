using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    private Vector2 startPos;
    private PlayerStats player;

    [Header("UI Elements")]
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Enemy Counter UI")]
    public TextMeshProUGUI enemyCountText;

    [Header("Portal UI")]
    public GameObject portal;
    public Transform portalSpawnPoint;
    public GameObject notifyEffect;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource backgroundMusic;


    private int lives = 5;
    private int totalEnemies;
    private int remainingEnemies;
    
    public static GameController instance; 

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

    public void StartGlobalCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        if (player != null)
        {
            startPos = player.transform.position;
        }

        CountEnemies();
   

        UpdateEnemyUI();
        UpdateLivesUI();

        if (portal != null)
        {
            portal.SetActive(false); 
        }

        if (notifyEffect != null)
        {
            notifyEffect.SetActive(false); 
        }
    }

    public void PlayerDied()
    {
        lives--;
        UpdateLivesUI();

        if (lives > 0)
        {
            StartCoroutine(RespawnRoutine());
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(3f);

        if (player != null)
        {
            player.Respawn(startPos);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        player.gameObject.SetActive(false);
        StartCoroutine(GameOverRoutine()); 
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f); 
        ResetGame();
    }

    private void ResetGame()
    {
        lives = 5;
        remainingEnemies = totalEnemies;

        UpdateLivesUI();
        UpdateEnemyUI();

        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.Respawn(startPos);
        }

        if (portal != null)
        {
            portal.SetActive(false);
        }

        if (notifyEffect != null)
        {
            notifyEffect.SetActive(false);
        }

        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
        CountEnemies();
    }

    private void UpdateLivesUI()
    {
        if (heartImages == null || heartImages.Length == 0)
        {
            Debug.LogError("HeartImages array is not assigned in the Inspector!");
            return;
        }

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < lives ? fullHeart : emptyHeart;
        }
    }


    private void CountEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
        remainingEnemies = totalEnemies;
    }

    public void EnemyDefeated()
    {
        remainingEnemies--;
        UpdateEnemyUI();

       

        if (remainingEnemies <= 0)
        {
         
            StartCoroutine(SpawnPortalSequence()); 
        }
    }

    private void UpdateEnemyUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = remainingEnemies.ToString();
        }
    }

    private void SpawnPortal()
    {
        StartCoroutine(SpawnPortalSequence());
    }



    private IEnumerator SpawnPortalSequence()
    {
        if (notifyEffect != null)
        {
            notifyEffect.SetActive(true);

           
            Animator notifyAnimator = notifyEffect.GetComponent<Animator>();
            if (notifyAnimator != null)
            {
                notifyAnimator.SetTrigger("Notified");
            }
            if (backgroundMusic != null && backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
           
            AudioSource notifyAudio = notifyEffect.GetComponent<AudioSource>();
            if (notifyAudio != null && notifyAudio.clip != null)
            {
                notifyAudio.Play();
            }
        }

        yield return new WaitForSeconds(5f);


        if (notifyEffect != null)
        {
            notifyEffect.SetActive(false);
        }

        if (portal != null && portalSpawnPoint != null)
        {
            Vector3 spawnPosition = new Vector3(-10f, portalSpawnPoint.position.y, 0);

            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.down, 1f);
            if (hit.collider != null)
            {
                spawnPosition = new Vector3(-10f, portalSpawnPoint.position.y + 2f, 0);
            }

            GameObject portals = Instantiate(portal, spawnPosition, Quaternion.identity);
            portals.SetActive(true);

            Vector3 cameraStartPos = Camera.main.transform.position;
            FollowPlayer followPlayerScript = Camera.main.GetComponent<FollowPlayer>();
            if (followPlayerScript != null)
            {
                followPlayerScript.enabled = false;
            }

            yield return StartCoroutine(MoveCameraToPortal(portals, cameraStartPos));
            yield return new WaitForSeconds(3f);
            StartCoroutine(MoveCameraBackToPlayer(cameraStartPos));

            if (followPlayerScript != null)
            {
                followPlayerScript.enabled = true;
            }
        }
    }

    private IEnumerator MoveCameraToPortal(GameObject portals, Vector3 cameraStartPos)
    {
    
        Vector3 cameraTargetPos = new Vector3(portals.transform.position.x, portals.transform.position.y, Camera.main.transform.position.z);


        float timeToMove = 5f;
        float elapsedTime = 0f;


        while (elapsedTime < timeToMove)
        {
            Camera.main.transform.position = Vector3.Lerp(cameraStartPos, cameraTargetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraTargetPos; 
    }




    private IEnumerator MoveCameraBackToPlayer(Vector3 cameraStartPos)
    {
        float timeToMove = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < timeToMove)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraStartPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraStartPos; 
    }

    public void SetCheckpoint(Vector2 checkpointPosition)
    {
        startPos = checkpointPosition;
    }
}
