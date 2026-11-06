using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseController : MonoBehaviour
{
    [SerializeField] private LoadGameInstructionSO loadGameInstructionSO;
    [SerializeField] private LevelNumber levelNumber;
    [SerializeField] private SaveSystem saveSystem;

    [SerializeField] private Player player;
    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        player.OnZeroHealth += Player_OnZeroHealth;
        gameTimer.OnTimerFinished += GameTimer_OnTimerFinished;

        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    private void GameTimer_OnTimerFinished(object sender, System.EventArgs e)
    {
        Time.timeScale = 0f;

        winText.gameObject.SetActive(true);

        // Mark levelNumber.levelNumber as completed
        saveSystem.MarkLevelAsComplete(levelNumber.levelNumber);
    }

    private void Player_OnZeroHealth(object sender, System.EventArgs e)
    {
        Time.timeScale = 0f;

        loseText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartLevel(string sceneName)
    {
        loadGameInstructionSO.shouldLoadFromSave = false;

        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
