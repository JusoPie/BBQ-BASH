using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : NetworkBehaviour
{
    [SerializeField] private Image fillAmountImg;
    [SerializeField] private TextMeshProUGUI healthAmountText;


    [Networked(OnChanged = nameof(HealthAmountChanged))] private int currentHealthAmount { get; set; }

    private const int MAX_HEALTH_AMOUNT = 100;

    public override void Spawned()
    {
        currentHealthAmount = MAX_HEALTH_AMOUNT;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    private void Rpc_ReducePlayerHealth(int damage) 
    {
        currentHealthAmount -= damage;
    }

    private static void HealthAmountChanged(Changed<PlayerHealthController> changed)
    {
        var currentHealth = changed.Behaviour.currentHealthAmount;

        changed.LoadOld();
        var oldhealthAmount = changed.Behaviour.currentHealthAmount;

        //only if the current health is not the same as the prev one
        if (currentHealth != oldhealthAmount) 
        {
            changed.Behaviour.UpdateVisuals(currentHealth);

            //we did not respawn of just spawned
            if (currentHealth != MAX_HEALTH_AMOUNT) 
            {
                changed.Behaviour.PlayerGotHit(currentHealth);
            }
        }
    }

    private void UpdateVisuals(int healthAmount) 
    {
        var num = (float)healthAmount / MAX_HEALTH_AMOUNT;
        fillAmountImg.fillAmount = num;
        healthAmountText.text = $"{healthAmount}/{MAX_HEALTH_AMOUNT}";
    }

    private void PlayerGotHit(int healthAmount) 
    {
        var isLocalPlayer = Runner.LocalPlayer == Object.HasInputAuthority;
        if (isLocalPlayer) 
        {
            //todo do hit effects
            Debug.Log("LOCAL PLAYER GOT HIT!");
        }

        if (healthAmount <= 0) 
        {
            //todo kill and respawn the player
            Debug.Log("Player is DEAD!");
        }
    }
}
