using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gameTimer.RemainingTime <= 0)
        {
            timerText.color = Color.green;
            timerText.text = "Time Remaining: 0.0";
        }
        else if (gameTimer.RemainingTime < 10)
        {
            timerText.color = Color.green;
            timerText.text = "Time Remaining: " + gameTimer.RemainingTime.ToString("F1");
        }
        else if (gameTimer.RemainingTime < 20)
        {
            timerText.color = Color.yellow;
            timerText.text = "Time Remaining: " + gameTimer.RemainingTime.ToString("F0");
        }
        else
        {
            timerText.text = "Time Remaining: " + gameTimer.RemainingTime.ToString("F0");
        }
        
    }
}
