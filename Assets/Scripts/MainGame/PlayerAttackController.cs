using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerAttackController : NetworkBehaviour
{
    [Header("For Attacking")]
    [SerializeField] private NetworkPrefabRef hitObj = NetworkPrefabRef.Empty;
    [SerializeField] private Transform hitObjPos;
    [SerializeField] private float delayBetweenAttacks = 0.2f;
    [SerializeField] private float animationResetTime = 0.1f;

    [Networked, HideInInspector] public NetworkBool DidPressAttackKey { get; private set; }
    [Networked] private NetworkButtons buttonsPrev { get; set; }
    [Networked] private TickTimer AttackCoolDown { get; set; }
    [Networked] private NetworkBool isAttacking { get; set; }

    private PlayerController playerController;
    private PlayerVisualController playerVisualController;

    public override void Spawned()
    {
        playerController = GetComponent<PlayerController>();
        playerVisualController = GetComponent<PlayerVisualController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input) && playerController.AcceptAnyInput)
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
            isAttacking = true;
            AttackCoolDown = TickTimer.CreateFromSeconds(Runner, delayBetweenAttacks);

            var hitObject = Runner.Spawn(hitObj, hitObjPos.position, hitObjPos.rotation, Object.InputAuthority);
            var attackObjScript = hitObject.GetComponent<AttackObjScript>();

            if (attackObjScript != null)
            {
                attackObjScript.Initialize(playerController.PlayerID); // Initialize the attacker ID
            }

            StartCoroutine(ResetAttackState());
        }
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(animationResetTime);
        isAttacking = false;
    }

    public NetworkBool GetIsAttacking()
    {
        return isAttacking;
    }
}

