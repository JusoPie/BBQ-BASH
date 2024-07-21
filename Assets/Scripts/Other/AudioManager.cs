using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void PlayGameMusic()
    {
        if (Runner.IsServer)
        {
            RPC_PlayGameMusic();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayGameMusic()
    {
        audioSource.clip = gameMusic;
        audioSource.Play();
    }
}
