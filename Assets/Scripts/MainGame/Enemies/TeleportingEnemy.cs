using Fusion;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TeleportingEnemy : EnemyController
{
    private EnemyTeleport enemyTeleport;
    private Animator animator;
    private bool canTakeDamage = true;
    private bool isDead = false; // Flag to prevent multiple deaths
    [SerializeField] private float waitForAnimation = 1f;

    protected override void Start() // Override the Start method
    {
        base.Start();
        enemyTeleport = GetComponent<EnemyTeleport>();
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage, int attackerId) // Override the TakeDamage method
    {
        if (canTakeDamage && !isDead) // Ensure the enemy can take damage and is not dead
        {
            Debug.Log($"Taking damage: {damage} from player: {attackerId}");
            base.TakeDamage(damage, attackerId);
            RPC_PlayShockAnimation();

            // Trigger teleportation when taking damage if still alive
            if (enemyTeleport != null && currentHealth > 0)
            {
                Debug.Log("Initiating teleportation sequence after taking damage");
                canTakeDamage = false;
                StartCoroutine(WaitForShockAnim());
            }
        }
    }

    private IEnumerator WaitForShockAnim()
    {
        yield return new WaitForSeconds(waitForAnimation);
        if (enemyTeleport != null && currentHealth > 0 && !isDead)
        {
            Debug.Log("Teleporting after delay");
            enemyTeleport.Teleport();
        }
        canTakeDamage = true;
    }

    // Additional logic when TeleportingEnemy dies, if any, can be added here
    protected override void Die() // Override the Die method
    {
        if (isDead) return; // Prevent multiple calls to Die
        isDead = true; // Set the isDead flag

        SoundFXManager.PlayHitEffect();
        base.Die();
        // Additional logic specific to TeleportingEnemy can be added here
    }

    public override void ResetState() // Override the ResetState method
    {
        base.ResetState();
        isDead = false; // Reset the isDead flag
        canTakeDamage = true; // Reset canTakeDamage
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayShockAnimation()
    {
        animator.SetTrigger("ShockEmoCorn");
    }

    
}

