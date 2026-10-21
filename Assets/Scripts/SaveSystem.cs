using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    [Header("Object references")]
    [SerializeField] private Player player;
    [SerializeField] private GameTimer gameTimer;


    [Header("File name")]
    [SerializeField] private string saveFileName;
    [SerializeField] private string saveGameFileExtension;

    private string saveGameFilePath;

    private void Awake()
    {
        saveGameFilePath = Path.Combine(Application.persistentDataPath, saveFileName + "." + saveGameFileExtension);
    }

    public void SaveGame()
    {
        SaveGameContainer saveGameContainer = new SaveGameContainer
        {
            playerSaveData = player.GetSaveData(),
            gameTimerSaveData = gameTimer.GetSaveData()
        };

        // 2. Serialize container as string
        string saveGameContainerString = JsonConvert.SerializeObject(saveGameContainer);

        // 3. Write string to text file
        File.WriteAllText(saveGameFilePath, saveGameContainerString);
    }
    
    public void TryLoadGame()
    {
        string fileContents;

        if (!File.Exists(saveGameFilePath))
        {
            return;
        }

        fileContents = File.ReadAllText(saveGameFilePath);

        SaveGameContainer saveGameContainer = JsonConvert.DeserializeObject<SaveGameContainer>(fileContents);

        PlayerSaveData playerSaveData = saveGameContainer.playerSaveData;
        GameTimerSaveData gameTimerSaveData = saveGameContainer.gameTimerSaveData;

        player.LoadFromSaveData(playerSaveData);
        gameTimer.LoadFromSaveData(gameTimerSaveData);
    }
}
