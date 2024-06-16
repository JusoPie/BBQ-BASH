using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button returnToLobbyBtn;
    [SerializeField] private GameObject childObj;

    // Start is called before the first frame update
    void Start()
    {
        GlobalManagers.Instance.GameManager.OnGameIsOver += OnMatchIsOver;
        returnToLobbyBtn.onClick.AddListener(() => GlobalManagers.Instance.NetworkRunnerControler.ShutDownRunner());
    }

    private void OnMatchIsOver()
    {
        childObj.SetActive(true);
    }

    private void OnDestroy()
    {
        GlobalManagers.Instance.GameManager.OnGameIsOver -= OnMatchIsOver;
    }
}
