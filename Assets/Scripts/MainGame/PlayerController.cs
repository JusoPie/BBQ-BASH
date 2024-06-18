using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    public bool AcceptAnyInput => PlayerIsAlive && !GameManager.MatchIsOver && !PlayerChatController.IsTyping;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject cam;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float jumpForce = 1000;
    [SerializeField] private GameObject deathHelmet;
    

    

    [Header("Grounded Vars")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundDetectionObj;
    [SerializeField] private GameObject proxyGroundCollider;

    [Networked] public TickTimer RespawnTimer { get; private set; }

    [Networked] public NetworkBool PlayerIsAlive { get; private set; }
    [Networked(OnChanged = nameof(OnNicknameChanged))] private NetworkString<_8> playerName { get; set; }
    [Networked] private NetworkButtons buttonsPrev { get; set; }
 
    [Networked] private Vector2 serverNextSpawnPoint { get; set; }


    [Networked] private NetworkBool isGrounded { get; set; }


    private float horizontal;
    private Rigidbody2D rigid;
    private PlayerVisualController playerVisualController;
    private PlayerHealthController playerHealthController;
    private PlayerAttackController playerAttackController;


    public enum PlayerInputButtons 
    { 
        None,
        Jump,
        Attack
    }


    public override void Spawned()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerVisualController = GetComponentInChildren<PlayerVisualController>();
        playerHealthController = GetComponent<PlayerHealthController>();
        playerAttackController = GetComponent<PlayerAttackController>();

        SetLocalObjects();
        PlayerIsAlive = true;
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

    public void KillPlayer() 
    {
        if (Runner.IsServer) 
        {
            serverNextSpawnPoint = GlobalManagers.Instance.PlayerSpawnerController.GetRandomSpawnPoint();
        }

        PlayerIsAlive = false;
        rigid.simulated = false;
        playerVisualController.TriggerDieAnimation();
        Instantiate(deathHelmet, transform.position, transform.rotation);

        RespawnTimer = TickTimer.CreateFromSeconds(Runner, 5F);
    }

   

    //Happens before anything else Fusion does, network application, reconlation etc
    //Called at the start of the Fusion Update loop, before the Fusion simulation loop.
    //It dires before Fusion does ANY work, envery screen refresh.
    public void BeforeUpdate()
    {
        //We are the local machine
        if (Runner.LocalPlayer == Object.HasInputAuthority && AcceptAnyInput) 
        {
            const string HORIZONTAL = "Horizontal";
            horizontal = Input.GetAxisRaw(HORIZONTAL);
        }
    }

    public override void FixedUpdateNetwork()
    {
        CheckRespawnTimer();
        // will return false if;
        //the client does not have state authority or input authority
        // the requested type of input does not exist in the simulation
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input)) 
        {
            if (AcceptAnyInput)
            {
                rigid.velocity = new Vector2(input.HorizontalInput * moveSpeed, rigid.velocity.y);

                CheckJumpInput(input);


                buttonsPrev = input.NetworkButtons;
            }
            else 
            {
                rigid.velocity = Vector2.zero;
            }
            
        }

        playerVisualController.UpdateScaleTransforms(rigid.velocity);
    }

    private void CheckRespawnTimer() 
    {
        if (PlayerIsAlive) return;

        if (RespawnTimer.Expired(Runner)) 
        {
            RespawnTimer = TickTimer.None;
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        PlayerIsAlive = true;
        rigid.simulated = true;
        rigid.position = serverNextSpawnPoint;
        playerVisualController.TriggerRespawnAnimation();
        playerHealthController.ResetHealthAmountToMax();
    }



    public override void Render()
    {
        playerVisualController.RendererVisuals(rigid.velocity, playerAttackController.GetIsAttacking());
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

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        GlobalManagers.Instance.ObjectPoolingManager.RemoveNetworkObjectFromDic(Object);
        Destroy(gameObject);
    }

    public PlayerData GetPlayerNetworkInput() 
    {
        PlayerData data = new PlayerData();
        data.HorizontalInput = horizontal;
        data.NetworkButtons.Set(PlayerInputButtons.Jump, Input.GetKey(KeyCode.Space));
        data.NetworkButtons.Set(PlayerInputButtons.Attack, Input.GetKey(KeyCode.Mouse0));
        return data;
    }
    
}
