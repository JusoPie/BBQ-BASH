using Cinemachine;
using UnityEngine;

public class PlayerCameraControler : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource ImpulseSource;

    public void ShakaCamera(Vector3 shakeAmout) 
    {
        ImpulseSource.GenerateImpulse(shakeAmout);
    }
}
