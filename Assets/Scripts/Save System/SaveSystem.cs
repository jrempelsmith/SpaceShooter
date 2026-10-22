using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    [Header("Object references")]
    [SerializeField] private Player player;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Alien alienPrefab;


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
            gameTimerSaveData = gameTimer.GetSaveData(),
            alienSaveDataArray = GetAlienSaveDataArray()
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
        LoadFromAlienSaveDataArray(saveGameContainer.alienSaveDataArray);
    }

    private AlienSaveData[] GetAlienSaveDataArray()
    {
        // Get all Aliens
        Alien[] aliensCurrentlyInScene = FindObjectsByType<Alien>(FindObjectsInactive.Include);

        // Create an array of the same length for Alien Save Data
        AlienSaveData[] alienSaveDataArray = new AlienSaveData[aliensCurrentlyInScene.Length];

        // Populate the array with Alien save data
        for (int i = 0; i < aliensCurrentlyInScene.Length; i++)
        {
            alienSaveDataArray[i] = aliensCurrentlyInScene[i].GetSaveData();
        }

        return alienSaveDataArray;
    }

    private void LoadFromAlienSaveDataArray(AlienSaveData[] alienSaveDataArray)
    {
        // Destroy ALL aliens at the time of Load
        Alien[] aliensCurrentlyInScene = FindObjectsByType<Alien>(FindObjectsInactive.Include);
        for (int i = 0; i < aliensCurrentlyInScene.Length; i++)
        {
            Destroy(aliensCurrentlyInScene[i].gameObject);
        }

        // Load the Aliens from the save
        for (int i = 0; i < alienSaveDataArray.Length; i++)
        {
            // instantiate a new Alien
            Alien newAlien = Instantiate(alienPrefab);

            // let that Alien load its data
            newAlien.LoadFromSaveData(alienSaveDataArray[i], player);
        }
    }
}
