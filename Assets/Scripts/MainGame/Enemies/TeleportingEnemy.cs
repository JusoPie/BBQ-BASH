using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TeleportingEnemy : EnemyController
{
    private EnemyTeleport enemyTeleport;
    private Animator animator;
    private bool canTakeDamage = true;
    [SerializeField] private float waitForAnimation = 1f;
    


    protected override void Start() // Override the Start method
    {
        base.Start();
        enemyTeleport = GetComponent<EnemyTeleport>();
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage, int attackerId) // Override the TakeDamage method
    {
        if (canTakeDamage == true)
        {
            Debug.Log($"Taking damage: {damage} from player: {attackerId}");
            base.TakeDamage(damage, attackerId);
            animator.SetTrigger("ShockEmoCorn");

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
        if (enemyTeleport != null && currentHealth > 0)
        {
            Debug.Log("Teleporting after delay");
            enemyTeleport.Teleport();
        }
        canTakeDamage = true;
    }

    // Additional logic when TeleportingEnemy dies, if any, can be added here
    protected override void Die() // Override the Die method
    {

        base.Die();
        // Additional logic specific to TeleportingEnemy can be added here
    }
}
