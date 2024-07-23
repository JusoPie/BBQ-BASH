using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RespawnPanel : SimulationBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI respawnAmountText;
    [SerializeField] private GameObject childObj;

    public override void FixedUpdateNetwork()
    {
        if (Utilities.IsLocalPlayer(Object)) 
        {
            var timerISRunning = playerController.RespawnTimer.IsRunning;
            childObj.SetActive(timerISRunning);

            if (timerISRunning && playerController.RespawnTimer.RemainingTime(Runner).HasValue)
            {
                var time = playerController.RespawnTimer.RemainingTime(Runner).Value;
                var roundInt = Mathf.RoundToInt(time);
                respawnAmountText.text = roundInt.ToString();
            }
        }
        
    }
}
