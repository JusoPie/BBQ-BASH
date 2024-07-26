using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : SimulationBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button leaveMatchBtn;
    [SerializeField] private Button quitGameBtn;
    [SerializeField] public GameObject childObj;

    void Start()
    {
        leaveMatchBtn.onClick.AddListener(() => GlobalManagers.Instance.NetworkRunnerControler.ShutDownRunner());
        exitBtn.onClick.AddListener(ExitPauseMenu);
        quitGameBtn.onClick.AddListener(QuitGame);
    }

    public override void FixedUpdateNetwork()
    {
        // Moved exitBtn and quitGameBtn listeners to Start()
    }

    public void TogglePauseMenu()
    {
        childObj.SetActive(!childObj.activeSelf);
    }

    private void ExitPauseMenu()
    {
        childObj.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
