using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNicknamePanel : LobbyPanelBase
{
    [Header("CreateNicknamePanel Vars")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button createNicknameBtn;
    private const int MIN_CHAR_FOR_NICKNAME = 2;

    public override void InitPanel(LobbyUIManager lobbyUIManager)
    {
        base.InitPanel(lobbyUIManager);
        createNicknameBtn.interactable = false;
        createNicknameBtn.onClick.AddListener(OnClickCreateNickname);
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string arg0)
    {
        createNicknameBtn.interactable = arg0.Length > MIN_CHAR_FOR_NICKNAME;
    }

    private void OnClickCreateNickname() 
    { 
     var nickName = inputField.text;
        if (nickName.Length >= MIN_CHAR_FOR_NICKNAME) 
        {
            GlobalManagers.Instance.NetworkRunnerControler.SetPlayerNickname(nickName);

            base.ClosePanel();
            lobbyUIManager.ShowPanel(LobbyPanelType.MiddleSetionPanel);
            
        }
    }
}
