using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TeleportPointsManager : MonoBehaviour
{
    public static TeleportPointsManager Instance { get; private set; }
    private List<Transform> teleportPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            FindTeleportPoints();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTeleportPoints();
    }

    private void FindTeleportPoints()
    {
        teleportPoints = new List<Transform>();
        var points = GameObject.FindGameObjectsWithTag("TeleportPoint");
        foreach (var point in points)
        {
            teleportPoints.Add(point.transform);
        }
    }

    public Vector3 GetRandomTeleportPoint(Vector3 currentPos)
    {
        if (teleportPoints.Count == 0) return Vector3.zero;

        Vector3 newPosition;
        do
        {
            newPosition = teleportPoints[Random.Range(0, teleportPoints.Count)].position;
        } while (newPosition == currentPos);

        return newPosition;
    }
}


