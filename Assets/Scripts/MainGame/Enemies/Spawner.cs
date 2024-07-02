using Fusion;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    private float nextSpawnTime;

    private void Update()
    {
        if (Object.HasStateAuthority && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        Runner.Spawn(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
