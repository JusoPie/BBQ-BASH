using UnityEngine;

public class TeleportPointsManager : MonoBehaviour
{
    public static TeleportPointsManager Instance { get; private set; }
    private Transform[] teleportPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindTeleportPoints();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FindTeleportPoints()
    {
        var points = GameObject.FindGameObjectsWithTag("TeleportPoint");
        teleportPoints = new Transform[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            teleportPoints[i] = points[i].transform;
        }
    }

    public Vector3 GetRandomTeleportPoint()
    {
        if (teleportPoints.Length == 0) return Vector3.zero;
        return teleportPoints[Random.Range(0, teleportPoints.Length)].position;
    }
}

