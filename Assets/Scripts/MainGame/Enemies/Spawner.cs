using Fusion;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnRangeX = 8f;
    private float nextSpawnTime;
    private NetworkObject lastSpawnedEnemy;

    private void Update()
    {
        if (Object.HasStateAuthority)
        {
            // Check if the last spawned enemy is deactivated and reset the spawn timer if needed
            if (lastSpawnedEnemy != null && !lastSpawnedEnemy.gameObject.activeInHierarchy)
            {
                lastSpawnedEnemy = null;
                nextSpawnTime = Time.time + spawnInterval;
            }

            // Spawn a new enemy if the timer has elapsed
            if (lastSpawnedEnemy == null && Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = float.MaxValue; // Reset next spawn time to prevent immediate respawn
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, 0);
        lastSpawnedEnemy = Runner.Spawn(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<NetworkObject>();
    }
}


