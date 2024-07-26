using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : NetworkBehaviour
{
    [SerializeField] private Animator hitScreenAnimator;
    [SerializeField] private PlayerCameraControler playerCameraController; 
    [SerializeField] private Image fillAmountImg;
    [SerializeField] private TextMeshProUGUI healthAmountText;


    [Networked(OnChanged = nameof(HealthAmountChanged))] private int currentHealthAmount { get; set; }

    private const int MAX_HEALTH_AMOUNT = 100;
    private PlayerController playerController;
    private PlayerVisualController playerVisualController;

    public override void Spawned()
    {
        playerController = GetComponent<PlayerController>();
        playerVisualController = GetComponent<PlayerVisualController>();
        currentHealthAmount = MAX_HEALTH_AMOUNT;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    public void Rpc_ReducePlayerHealth(int damage) 
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
       
        var isLocalPlayer = Utilities.IsLocalPlayer(Object);
        if (isLocalPlayer)
        {
            //HitEffects
            Debug.Log("LOCAL PLAYER GOT HIT!");

            SoundFXManager.PlayHitEffect();

            const string HIT_CLIP_NAME = "HitScreen";
            hitScreenAnimator.Play(HIT_CLIP_NAME);

            var shakeAmount = new Vector3(0.15f, 0, 0);
            playerCameraController.ShakaCamera(shakeAmount);

            playerVisualController.TriggerDamageAnimation();
        }

        else if (!isLocalPlayer) 
        {
            SoundFXManager.PlayHitEffect();
            playerVisualController.TriggerDamageAnimation();
        }

        if (healthAmount <= 0) 
        {
            SoundFXManager.PlayHitEffect();
            playerController.KillPlayer();
            Debug.Log("Player is DEAD!");
        }
    }

    public void ResetHealthAmountToMax() 
    {
        currentHealthAmount = MAX_HEALTH_AMOUNT;
    }
}
