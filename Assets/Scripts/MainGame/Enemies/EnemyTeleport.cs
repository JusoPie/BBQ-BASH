using Fusion;
using UnityEngine;

public class EnemyTeleport : NetworkBehaviour
{
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    public void Teleport()
    {
        if (TeleportPointsManager.Instance != null)
        {
            Vector3 newPosition = TeleportPointsManager.Instance.GetRandomTeleportPoint(lastPosition);
            if (newPosition != Vector3.zero)
            {
                Debug.Log($"Teleporting to {newPosition}");
                lastPosition = newPosition;
                transform.position = newPosition;

                if (Object.HasStateAuthority)
                {
                    RPC_UpdatePosition(newPosition);
                }
            }
            else
            {
                Debug.LogWarning("No valid teleport points found!");
            }
        }
        else
        {
            Debug.LogWarning("TeleportPointsManager instance not found!");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}



