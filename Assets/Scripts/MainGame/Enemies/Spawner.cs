using Fusion;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float initialSpawnDelay = 3f; // New variable to control the initial spawn delay
    private float nextSpawnTime;
    private NetworkObject lastSpawnedEnemy;

    private void Start()
    {
        // Set the initial spawn time to the current time plus the initial delay
        nextSpawnTime = Time.time + initialSpawnDelay;

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
        }
    }

    private void Update()
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

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Cannot spawn enemy because the enemy prefab is not assigned.");
            return;
        }

        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, 0);
        NetworkObject networkObject = Runner.Spawn(enemyPrefab, spawnPosition, Quaternion.identity)?.GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("Failed to spawn enemy. Runner.Spawn returned null or the prefab does not have a NetworkObject component.");
            return;
        }

        lastSpawnedEnemy = networkObject;
    }
}
