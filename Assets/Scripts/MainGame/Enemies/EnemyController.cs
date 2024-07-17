using Fusion;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] protected int currentHealth; 
    [SerializeField] private int points = 10;
    private int lastAttackerId; // Track the ID of the last player who attacked
    [SerializeField] private NetworkObject deathEffect;

    protected virtual void Start() 
    {
        ResetState();
    }

    public override void FixedUpdateNetwork()
    {
        // Update logic if necessary
    }

    public virtual void TakeDamage(int damage, int attackerId) 
    {
        if (Object.HasStateAuthority)
        {
            currentHealth -= damage;
            lastAttackerId = attackerId; // Update the last attacker ID
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void ResetState()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Die() 
    {
        // Award points to the last player who attacked
        if (lastAttackerId != 0)
        {
            GlobalManagers.Instance.GameManager.AddPoints(lastAttackerId, points);

        }

        RPC_SpawnDeathEffect();
        Runner.Despawn(Object); // Despawn the enemy
        
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SpawnDeathEffect()
    {
        if (deathEffect != null)
        {
            Runner.Spawn(deathEffect, transform.position, Quaternion.identity);
        }
    }

    
}






