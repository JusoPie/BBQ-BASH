using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : NetworkBehaviour
{
    [Header("For Attacking")]
    [SerializeField] private NetworkPrefabRef hitObj = NetworkPrefabRef.Empty;
    [SerializeField] private Transform hitObjPos;
    [SerializeField] private float delayBetweenAttacks = 0.2f;

    [Networked, HideInInspector] public NetworkBool DidPressAttackKey { get; private set; }

    [Networked] private NetworkButtons buttonsPrev { get; set; }
    [Networked] private TickTimer AttackCoolDown { get; set; }

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
            CheckAttackInput(input);

            buttonsPrev = input.NetworkButtons;
        }

    }

    private void CheckAttackInput(PlayerData input)
    {
        var currentBtns = input.NetworkButtons.GetPressed(buttonsPrev);

        DidPressAttackKey = currentBtns.WasPressed(buttonsPrev, PlayerController.PlayerInputButtons.Attack);

        if (currentBtns.WasPressed(buttonsPrev, PlayerController.PlayerInputButtons.Attack) && AttackCoolDown.ExpiredOrNotRunning(Runner))
        {

            AttackCoolDown = TickTimer.CreateFromSeconds(Runner, delayBetweenAttacks);

            Runner.Spawn(hitObj, hitObjPos.position, hitObjPos.rotation, Object.InputAuthority);
        }
        
    }
}
