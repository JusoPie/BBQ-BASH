using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjScript : NetworkBehaviour
{
    [SerializeField] private LayerMask playerHitboxLayerMask;
    [SerializeField] private LayerMask enemyHitboxLayerMask;
    [SerializeField] private float lifeTimeAmount = 0.2f;
    [SerializeField] private int hitDmg = 10;

    [Networked] private TickTimer lifeTimeTimer { get; set; }
    private Collider2D coll;

    private int attackerId; // Add this to track the attacker

    public override void Spawned()
    {
        coll = GetComponent<Collider2D>();
        lifeTimeTimer = TickTimer.CreateFromSeconds(Runner, lifeTimeAmount);
    }

    public void Initialize(int attackerId) // Add this method to initialize the attackerId
    {
        this.attackerId = attackerId;
    }

    public override void FixedUpdateNetwork()
    {
        CheckIfWeHitAPlayerOrEnemy();

        if (lifeTimeTimer.Expired(Runner))
        {
            lifeTimeTimer = TickTimer.None;
            Runner.Despawn(Object);
        }
    }

    private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

    private void CheckIfWeHitAPlayerOrEnemy()
    {
        // Check for hits on player hitboxes
        Runner.LagCompensation.OverlapBox(
            transform.position,
            coll.bounds.size,
            Quaternion.identity,
            Object.InputAuthority,
            hits,
            playerHitboxLayerMask
        );

        foreach (var item in hits)
        {
            if (item.Hitbox != null)
            {
                var player = item.Hitbox.GetComponentInParent<PlayerController>();
                var didNotHitOurOwnPlayer = player != null && player.Object.InputAuthority.PlayerId != Object.InputAuthority.PlayerId;

                if (didNotHitOurOwnPlayer && player.PlayerIsAlive)
                {
                    if (Runner.IsServer)
                    {
                        Debug.Log("Did hit a player");
                        player.GetComponent<PlayerHealthController>().Rpc_ReducePlayerHealth(hitDmg);
                    }

                    Runner.Despawn(Object);
                    return;
                }
            }
        }

        // Check for hits on enemy hitboxes
        Runner.LagCompensation.OverlapBox(
            transform.position,
            coll.bounds.size,
            Quaternion.identity,
            Object.InputAuthority,
            hits,
            enemyHitboxLayerMask
        );

        foreach (var item in hits)
        {
            if (item.Hitbox != null)
            {
                var enemy = item.Hitbox.GetComponentInParent<EnemyController>();
                if (enemy != null)
                {
                    if (Runner.IsServer)
                    {
                        Debug.Log("Did hit an enemy");
                        enemy.TakeDamage(hitDmg, attackerId); // Pass the attackerId here
                    }

                    Runner.Despawn(Object);
                    return;
                }
            }
        }
    }
}


