using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjScript : NetworkBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float lifeTimeAmount = 0.2f;
    [SerializeField] private int hitDmg = 10;

    //Gives error on console??
    [Networked] private TickTimer lifeTimeTimer { get; set; }
    private Collider2D coll;

    public override void Spawned()
    {
        coll = GetComponent<Collider2D>();
        lifeTimeTimer = TickTimer.CreateFromSeconds(Runner, lifeTimeAmount);
    }

    public override void FixedUpdateNetwork()
    {
        CheckIfWeHitAPlayer();

        if (lifeTimeTimer.Expired(Runner)) 
        {
            lifeTimeTimer = TickTimer.None;
            
            Runner.Despawn(Object);
        }
    }

    private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
    private void CheckIfWeHitAPlayer() 
    {
        Runner.LagCompensation.OverlapBox(transform.position, coll.bounds.size, Quaternion.identity,
            Object.InputAuthority, hits, playerLayerMask);

        if (hits.Count > 0) 
        {
            foreach (var item in hits) 
            {
                if (item.Hitbox != null) 
                {
                    var player = item.Hitbox.GetComponentInParent<PlayerController>();
                    var didNotHitOurOwnPlayer = player.Object.InputAuthority.PlayerId != Object.InputAuthority.PlayerId;

                    if (didNotHitOurOwnPlayer && player.PlayerIsAlive) 
                    {
                        if (Runner.IsServer) 
                        {
                            Debug.Log("Did hit a player!");
                            player.GetComponent<PlayerHealthController>().Rpc_ReducePlayerHealth(hitDmg);
                        }
                        
                        Runner.Despawn(Object);
                        break;
                    }
                }
            }
        }
    }
}
