using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LoadGameInstructionSO loadGameInstructionSO;

    [SerializeField] private SaveFilenameSO saveFilenameSO;

    private string saveMetadataFilePath;



    public void LoadLevel()
    {
        string fileContents;

        if (!File.Exists(saveMetadataFilePath))
        {
            return;
        }

        fileContents = File.ReadAllText(saveMetadataFilePath);

        SaveMetadataContainer saveMetadataContainer = JsonConvert.DeserializeObject<SaveMetadataContainer>(fileContents);

        string sceneName = "Level " + saveMetadataContainer.levelNumber.ToString();

        loadGameInstructionSO.shouldLoadFromSave = true;

        SceneManager.LoadScene(sceneName);
    }


    private void Awake()
    {
        saveMetadataFilePath = Path.Combine(Application.persistentDataPath, saveFilenameSO.saveMetadataFileName + "." + saveFilenameSO.saveGameFileExtension);
    }
}
