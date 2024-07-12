using UnityEngine;

public class TeleportingEnemy : EnemyController
{
    private EnemyTeleport enemyTeleport;

    protected override void Start() // Override the Start method
    {
        base.Start();
        enemyTeleport = GetComponent<EnemyTeleport>();
    }

    public override void TakeDamage(int damage, int attackerId) // Override the TakeDamage method
    {
        Debug.Log($"Taking damage: {damage} from player: {attackerId}");
        base.TakeDamage(damage, attackerId);

        // Trigger teleportation when taking damage if still alive
        if (enemyTeleport != null && currentHealth > 0)
        {
            Debug.Log("Teleporting after taking damage");
            enemyTeleport.Teleport();
        }
    }

    // Additional logic when TeleportingEnemy dies, if any, can be added here
    protected override void Die() // Override the Die method
    {
        base.Die();
        // Additional logic specific to TeleportingEnemy can be added here
    }
}

