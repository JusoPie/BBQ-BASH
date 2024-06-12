using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : NetworkBehaviour
{
    [Header("For Attacking")]
    [SerializeField] private NetworkPrefabRef hitObj = NetworkPrefabRef.Empty;
    [SerializeField] private Transform hitObjPos;
    [SerializeField] private float delayBetweenattacks = 0.2f;

    [Networked, HideInInspector] public NetworkBool didPressAttackKey { get; private set; }

    [Networked] private NetworkButtons buttonsPrev { get; set; }
    [Networked] private TickTimer attackCoolDown { get; set; }

    private PlayerController playerController;
    private PlayerVisualController playerVisualController;

    public override void Spawned()
    {
        playerController = GetComponent<PlayerController>();
        playerVisualController = GetComponent<PlayerVisualController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input) && playerController.PlayerIsAlive)
        {
            CheckShootInput(input);

            buttonsPrev = input.NetworkButtons;
        }

    }

    private void CheckShootInput(PlayerData input)
    {
        var currentBtns = input.NetworkButtons.GetPressed(buttonsPrev);

        didPressAttackKey = currentBtns.WasPressed(buttonsPrev, PlayerController.PlayerInputButtons.Attack);

        if (currentBtns.WasPressed(buttonsPrev, PlayerController.PlayerInputButtons.Attack) && attackCoolDown.ExpiredOrNotRunning(Runner))
        {
            playerVisualController.TriggerAttackAnimation();
            attackCoolDown = TickTimer.CreateFromSeconds(Runner, delayBetweenattacks);

            Runner.Spawn(hitObj, hitObjPos.position, hitObjPos.rotation, Object.InputAuthority);
        }
        
    }
}
