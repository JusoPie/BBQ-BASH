using Fusion;
using UnityEngine;

public class EnemyTeleport : NetworkBehaviour
{
    public void Teleport()
    {
        if (TeleportPointsManager.Instance != null)
        {
            Vector3 newPosition = TeleportPointsManager.Instance.GetRandomTeleportPoint();
            Debug.Log($"Teleporting to {newPosition}");
            transform.position = newPosition;

            if (Object.HasStateAuthority)
            {
                RPC_UpdatePosition(newPosition);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}


