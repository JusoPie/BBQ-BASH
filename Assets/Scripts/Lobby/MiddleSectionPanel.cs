using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiddleSectionPanel : LobbyPanelBase
{
    [Header("MiddleSectionPanel Vars")]
    [SerializeField] private Button joinRandomRoomBtn;
    [SerializeField] private Button joinRoomByArgBtn;
    [SerializeField] private Button createRoomBtn;

    [SerializeField] private TMP_InputField joinRoomByArgInputField;
    [SerializeField] private TMP_InputField createRoomInputField;
    private NetworkRunnerControler networkRunnerControler;

    public override void InitPanel(LobbyUIManager uIManager)
    {
        base.InitPanel(uIManager);

        networkRunnerControler = GlobalManagers.Instance.NetworkRunnerControler;
        joinRandomRoomBtn.onClick.AddListener(JoinRandomRoom);
        joinRoomByArgBtn.onClick.AddListener(() => CreateRoom(GameMode.Client, joinRoomByArgInputField.text));
        createRoomBtn.onClick.AddListener(() => CreateRoom(GameMode.Host, createRoomInputField.text));
    }



    private void CreateRoom(GameMode mode, string field)
    {
        if (field.Length >= 2) 
        {
            Debug.Log($"-------{mode}-------");
            networkRunnerControler.StartGame(mode, field);
        }
    }


    private void JoinRandomRoom()
    {
        Debug.Log($"-------JoinRandomRoom!-------");
        networkRunnerControler.StartGame(GameMode.AutoHostOrClient, roomName: string.Empty);
    }
}
