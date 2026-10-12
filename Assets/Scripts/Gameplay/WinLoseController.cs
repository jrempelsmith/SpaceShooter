using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        player.OnZeroHealth += Player_OnZeroHealth;
        gameTimer.OnTimerFinished += GameTimer_OnTimerFinished;

        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    private void GameTimer_OnTimerFinished(object sender, System.EventArgs e)
    {
        Time.timeScale = 0f;

        winText.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    private void Player_OnZeroHealth(object sender, System.EventArgs e)
    {
        Time.timeScale = 0f;

        loseText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }

    public void RestartLevel(string sceneName)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
