using UnityEngine;

public class BossEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform spawnPoint;
    public float spawnInterval = 5f; 
    public int maxEnemies = 5; 

    private float spawnTimer = 0f; 
    private int enemiesSpawned = 0; 

    void Update()
    {
        if (enemiesSpawned < maxEnemies)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f; 
            }
        }
    }

    void SpawnEnemy()
    {

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemiesSpawned++;
    }
}