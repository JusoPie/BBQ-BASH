using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextPrefab;
    [SerializeField] private Transform scoreContainer;

    private Dictionary<int, TextMeshProUGUI> playerScoreTexts = new Dictionary<int, TextMeshProUGUI>();

    public void InitializePlayerScore(int playerId)
    {
        if (!playerScoreTexts.ContainsKey(playerId))
        {
            var scoreTextInstance = Instantiate(scoreTextPrefab, scoreContainer);
            scoreTextInstance.text = $"Player {playerId}: 0";
            playerScoreTexts.Add(playerId, scoreTextInstance);
        }
    }

    public void UpdatePlayerScore(int playerId, int score)
    {
        if (playerScoreTexts.ContainsKey(playerId))
        {
            playerScoreTexts[playerId].text = $"Player {playerId}: {score}";
        }
    }
}
