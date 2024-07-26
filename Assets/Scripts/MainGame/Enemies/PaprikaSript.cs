using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PaprikaSript : EnemyController
{
    private Animator animator;
    protected override void Start() 
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage, int attackerId) 
    {
        RPC_PlayHitSound();
        base.TakeDamage(damage, attackerId);
        RPC_PlayDamageAnim();
        
        
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayDamageAnim() 
    {
        animator.SetTrigger("DamagePaprika");
    }
    public void RPC_PlayHitSound() 
    {
        SoundFXManager.PlayHitEffect();
    }
}
