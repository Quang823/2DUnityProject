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

    [Header("Boss UI")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject dangerEffect; 


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
    }

    private void UpdateLivesUI()
    {
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
            SpawnBoss();
        }
    }

    private void UpdateEnemyUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = remainingEnemies.ToString();
        }
    }

    private void SpawnBoss()
    {
        StartCoroutine(SpawnBossSequence());
    }

    private IEnumerator SpawnBossSequence()
    {
        if (dangerEffect != null)
        {
            dangerEffect.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        if (dangerEffect != null)
        {
            dangerEffect.SetActive(false);
        }

        if (bossPrefab != null && bossSpawnPoint != null)
        {
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log("Boss xuất hiện!");

            // Kiểm tra nếu boss có BossController, gán targetPosition để boss biết nơi di chuyển đến
            BossController bossController = boss.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.targetPosition = GameObject.Find("BossTargetPoint").transform;
            }
        }
        else
        {
            Debug.LogWarning("Boss chưa được thiết lập, vui lòng thêm bossPrefab!");
        }
    }


    private IEnumerator BossEnterMap(GameObject boss)
    {
        Vector3 targetPosition = new Vector3(0, boss.transform.position.y, 0); // Điều chỉnh vị trí mục tiêu vào map
        float speed = 2f;

        while (Vector3.Distance(boss.transform.position, targetPosition) > 0.1f)
        {
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Khi boss đã vào map, bắt đầu tấn công player
        boss.GetComponent<BossController>().StartAttack();
    }

    public void SetCheckpoint(Vector2 checkpointPosition)
    {
        startPos = checkpointPosition;
    }
}
