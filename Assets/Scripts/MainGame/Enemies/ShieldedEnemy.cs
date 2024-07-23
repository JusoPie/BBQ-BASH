using UnityEngine;
using Fusion;

public class ShieldedEnemy : EnemyController
{
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
        animator.SetTrigger("DamageMeatman");
    }
}
