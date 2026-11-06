using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private LoadGameInstructionSO loadGameInstructionSO;

    public void LoadLevel(string sceneName)
    {
        loadGameInstructionSO.shouldLoadFromSave = false;

        SceneManager.LoadScene(sceneName);
    }
}
