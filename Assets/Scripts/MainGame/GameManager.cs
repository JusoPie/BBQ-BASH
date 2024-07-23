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
    [SerializeField] private TextMeshProUGUI scoreText; // UI element to display scores
    [SerializeField] private float matchTimerAmount = 60;

    [Networked] private TickTimer matchTimer { get; set; }
    [Networked] private NetworkDictionary<int, int> playerScores { get; } = new NetworkDictionary<int, int>(); // Networked dictionary for scores

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

        UpdateScoreText(); // Update the score display
    }
    
    public void AddPoints(int playerId, int points)
    {
        if (Runner.IsServer)
        {
            if (playerScores.ContainsKey(playerId))
            {
                int currentPoints = playerScores.Get(playerId);
                playerScores.Set(playerId, currentPoints + points);
            }
            else
            {
                playerScores.Add(playerId, points);
            }

            Debug.Log($"Player {playerId} now has {playerScores.Get(playerId)} points.");

        }
    }

    
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Scores:\n";
            foreach (var kvp in playerScores)
            {
                scoreText.text += $"Player {kvp.Key}: {kvp.Value} points\n";
            }
        }
    }
}



