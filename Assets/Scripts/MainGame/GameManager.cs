using Fusion;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public event Action OnGameIsOver;
    public static bool MatchIsOver { get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float matchTimerAmount = 60;
    [SerializeField] private UIManager uiManager; // Reference to UIManager

    [Networked] private TickTimer matchTimer { get; set; }

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    private void Awake()
    {
        if (GlobalManagers.Instance != null)
        {
            GlobalManagers.Instance.GameManager = this;
        }
    }

    public override void Spawned()
    {
        MatchIsOver = false;
        cam.gameObject.SetActive(false);
        matchTimer = TickTimer.CreateFromSeconds(Runner, matchTimerAmount);
    }

    public override void FixedUpdateNetwork()
    {
        if (matchTimer.Expired(Runner) == false && matchTimer.RemainingTime(Runner).HasValue)
        {
            var timeSpan = TimeSpan.FromSeconds(matchTimer.RemainingTime(Runner).Value);
            var outPut = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            timerText.text = outPut;
        }
        else if (matchTimer.Expired(Runner))
        {
            MatchIsOver = true;
            matchTimer = TickTimer.None;
            OnGameIsOver?.Invoke();
            Debug.Log("MATCH TIMER HAD ENDED!");
        }
    }

    public void AddPoints(int playerId, int points)
    {
        if (!playerScores.ContainsKey(playerId))
        {
            playerScores[playerId] = 0;
            uiManager.InitializePlayerScore(playerId); // Initialize UI for the player
        }

        playerScores[playerId] += points;
        Debug.Log($"Player {playerId} now has {playerScores[playerId]} points.");
        uiManager.UpdatePlayerScore(playerId, playerScores[playerId]); // Update UI with the new score
    }

    public int GetPlayerScore(int playerId)
    {
        return playerScores.ContainsKey(playerId) ? playerScores[playerId] : 0;
    }
}

