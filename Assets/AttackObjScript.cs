using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjScript : NetworkBehaviour
{
    [SerializeField] private float lifeTimeAmount = 0.2f;

    [Networked] private TickTimer lifeTimeTimer { get; set; }

    public override void Spawned()
    {
        lifeTimeTimer = TickTimer.CreateFromSeconds(Runner, lifeTimeAmount);
    }

    public override void FixedUpdateNetwork()
    {
        if (lifeTimeTimer.Expired(Runner)) 
        {
            Runner.Despawn(Object);
        }
    }
}
