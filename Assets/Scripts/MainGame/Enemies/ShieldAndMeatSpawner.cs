using Fusion;
using UnityEngine;

public class ShieldAndMeatSpawner : EnemyController
{
    [SerializeField] private GameObject spawnedEnemyPrefab;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage, int attackerId)
    {
        base.TakeDamage(damage, attackerId);
        RPC_PlayDamageAnim();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayDamageAnim()
    {
        animator.SetTrigger("ShieldDamage");
    }

    protected override void Die()
    {
        if (spawnedEnemyPrefab != null)
        {
            Vector3 position = transform.position;
            Quaternion rotation = Quaternion.identity;

            Debug.Log($"Spawning new enemy at position: {position} with rotation: {rotation}");

            // Ensure the new enemy is spawned correctly
            NetworkObject spawnedEnemy = Runner.Spawn(spawnedEnemyPrefab, position, rotation);

            if (spawnedEnemy == null)
            {
                Debug.LogError("Failed to spawn new enemy.");
            }
            else
            {
                Debug.LogError("Spawned enemy prefab is not assigned.");
            }
            base.Die();

       
        }
       
    }
}

