using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvasController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Button cancelBtn;
    private NetworkRunnerControler networkRunnerControler;


    private void Start()
    {
        networkRunnerControler = GlobalManagers.Instance.NetworkRunnerControler;
        networkRunnerControler.OnStartedRunnerConnection += OnStartedRunnerConnection;
        networkRunnerControler.OnPlayerJoinedSucessfully += OnPlayerJoinedSucesfully;

        cancelBtn.onClick.AddListener(networkRunnerControler.ShutDownRunner);
        this.gameObject.SetActive(false);
    }

    private void OnPlayerJoinedSucesfully()
    {
        const string CLIP_NAME = "Out";
        StartCoroutine(Utilities.PlayAnimAndSetStateWhenFinished(gameObject, animator, CLIP_NAME, false));
    }

    private void OnStartedRunnerConnection()
    {
        this.gameObject.SetActive(true);
        const string CLIP_NAME = "In";
        StartCoroutine(Utilities.PlayAnimAndSetStateWhenFinished(gameObject, animator, CLIP_NAME));
    }

    

    private void OnDestroy()
    {
        networkRunnerControler.OnStartedRunnerConnection -= OnStartedRunnerConnection;
        networkRunnerControler.OnPlayerJoinedSucessfully -= OnPlayerJoinedSucesfully;
    }
}
