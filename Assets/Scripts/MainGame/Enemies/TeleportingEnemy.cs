using UnityEngine;

public class TeleportingEnemy : EnemyController
{
    private EnemyTeleport enemyTeleport;

    protected override void Start()
    {
        base.Start();
        enemyTeleport = GetComponent<EnemyTeleport>();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log($"Taking damage: {damage}");
        base.TakeDamage(damage);
        // Trigger teleportation when taking damage
        if (enemyTeleport != null && currentHealth > 0)
        {
            Debug.Log("Teleporting after taking damage");
            enemyTeleport.Teleport();
        }
    }

    protected override void Die()
    {
        base.Die();
        // Additional logic when TeleportingEnemy dies, if any
    }
}
