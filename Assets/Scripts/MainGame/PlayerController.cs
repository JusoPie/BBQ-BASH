using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject cam;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float jumpForce = 1000;

    [Header("For Attacking")]
    [SerializeField] private NetworkPrefabRef hitObj = NetworkPrefabRef.Empty;
    [SerializeField] private Transform hitObjPos;

    [Header("Grounded Vars")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundDetectionObj;


    [Networked(OnChanged = nameof(OnNicknameChanged))] 
    private NetworkString<_8> playerName { get; set; }
    [Networked] private NetworkButtons buttonsPrev { get; set; }

    [Networked] private NetworkBool isGrounded { get; set; }


    private float horizontal;
    private Rigidbody2D rigid;
    private PlayerVisualController playerVisualController;


    private enum PlayerInputButtons 
    { 
        None,
        Jump,
        Attack
    }


    public override void Spawned()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerVisualController = GetComponent<PlayerVisualController>();

        SetLocalObjects();
    }

    private void SetLocalObjects()
    {
        if (Runner.LocalPlayer == Object.HasInputAuthority)
        {
            cam.SetActive(true);

            var nickName = GlobalManagers.Instance.NetworkRunnerControler.LocalPlayerNickname;
            RpcSetNickName(nickName);
        }
    }

    // Sends RPC to the HOST (from a client)
    //"sources" define which PEER can send the RPC
    //The RpcTargets defines on which it is executed!
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]


    private void RpcSetNickName(NetworkString<_8> nickName) 
    {
        playerName = nickName;
    }

    //for example
    //if i set on spawned method a name called "banana"
    //and then on FUN i change another name which is again "banana"
    private static void OnNicknameChanged(Changed<PlayerController> changed) 
    {


        changed.Behaviour.SetPlayerNickname(changed.Behaviour.playerName);
    }

    private void SetPlayerNickname(NetworkString<_8> nickName) 
    {
        playerNameText.text = nickName + " " + Object.InputAuthority.PlayerId;
    }

   

    //Happens before anything else Fusion does, network application, reconlation etc
    //Called at the start of the Fusion Update loop, before the Fusion simulation loop.
    //It dires before Fusion does ANY work, envery screen refresh.
    public void BeforeUpdate()
    {
        //We are the local machine
        if (Runner.LocalPlayer == Object.HasInputAuthority) 
        {
            const string HORIZONTAL = "Horizontal";
            horizontal = Input.GetAxisRaw(HORIZONTAL);
        }
    }

    public override void FixedUpdateNetwork()
    {
        // will return false if;
        //the client does not have state authority or input authority
        // the requested type of input does not exist in the simulation
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input)) 
        {
            rigid.velocity = new Vector2(input.HorizontalInput * moveSpeed, rigid.velocity.y);
            
            CheckJumpInput(input);

            CheckAttackInput(input);

            buttonsPrev = input.NetworkButtons;
        }

        playerVisualController.UpdateScaleTransforms(rigid.velocity);
    }

    public override void Render()
    {
        playerVisualController.RendererVisuals(rigid.velocity);
    }

    private void CheckJumpInput(PlayerData input) 
    {
        var transform1 = groundDetectionObj.transform;
        isGrounded = (bool)Runner.GetPhysicsScene2D().OverlapBox(transform1.position, transform1.transform.localScale, 0, groundLayer);

        if (isGrounded) 
        {
            var pressed = input.NetworkButtons.GetPressed(buttonsPrev);
            if (pressed.WasPressed(buttonsPrev, PlayerInputButtons.Jump))
            {
                rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            }
        }
        
        
    }

    private void CheckAttackInput(PlayerData input) 
    {
        var pressed = input.NetworkButtons.GetPressed(buttonsPrev);
        if (pressed.WasPressed(buttonsPrev, PlayerInputButtons.Attack))
        {
            playerVisualController.AttackAnimation();
            Runner.Spawn(hitObj, hitObjPos.position, hitObjPos.rotation, Object.InputAuthority);
            Debug.Log("Attacking");
        }
    }

    public PlayerData GetPlayerNetworkInput() 
    {
        PlayerData data = new PlayerData();
        data.HorizontalInput = horizontal;
        data.NetworkButtons.Set(PlayerInputButtons.Jump, Input.GetKey(KeyCode.Space));
        data.NetworkButtons.Set(PlayerInputButtons.Attack, Input.GetKey(KeyCode.RightShift));
        return data;
    }
    
}
