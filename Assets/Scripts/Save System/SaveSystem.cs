using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveSystem : MonoBehaviour
{
    [Header("Object references")]
    [SerializeField] private Player player;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Alien alienPrefab;
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Comet cometPrefab;
    [SerializeField] private Coin coinPrefab;


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
            alienSaveDataArray = GetAlienSaveDataArray(),
            asteroidSaveDataArray = GetAsteroidSaveDataArray(),
            cometSaveDataArray = GetCometSaveDataArray(),
            coinSaveDataArray = GetCoinSaveDataArray()
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
        LoadFromAsteroidSaveDataArray(saveGameContainer.asteroidSaveDataArray);
        LoadFromCometSaveDataArray(saveGameContainer.cometSaveDataArray);
        LoadFromCoinSaveDataArray(saveGameContainer.coinSaveDataArray);
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

    private AsteroidSaveData[] GetAsteroidSaveDataArray()
    {
        // Get all Asteroids
        Asteroid[] asteroidsCurrentlyInScene = FindObjectsByType<Asteroid>(FindObjectsInactive.Include);

        // Create an array of the same length for Asteroid Save Data
        AsteroidSaveData[] asteroidSaveDataArray = new AsteroidSaveData[asteroidsCurrentlyInScene.Length];

        // Populate the array with Asteroid save data
        for (int i = 0; i < asteroidsCurrentlyInScene.Length; i++)
        {
            asteroidSaveDataArray[i] = asteroidsCurrentlyInScene[i].GetSaveData();
        }

        return asteroidSaveDataArray;
    }

    private CometSaveData[] GetCometSaveDataArray()
    {
        // Get all Comets
        Comet[] cometsCurrentlyInScene = FindObjectsByType<Comet>(FindObjectsInactive.Include);

        // Create an array of the same length for Comet Save Data
        CometSaveData[] cometSaveDataArray = new CometSaveData[cometsCurrentlyInScene.Length];

        // Populate the array with Comet save data
        for (int i = 0; i < cometsCurrentlyInScene.Length; i++)
        {
            cometSaveDataArray[i] = cometsCurrentlyInScene[i].GetSaveData();
        }

        return cometSaveDataArray;
    }

    private CoinSaveData[] GetCoinSaveDataArray()
    {
        // Get all Coins
        Coin[] coinsCurrentlyInScene = FindObjectsByType<Coin>(FindObjectsInactive.Include);

        // Create an array of the same length for Coin Save Data
        CoinSaveData[] coinSaveDataArray = new CoinSaveData[coinsCurrentlyInScene.Length];

        // Populate the array with Coin save data
        for (int i = 0; i < coinsCurrentlyInScene.Length; i++)
        {
            coinSaveDataArray[i] = coinsCurrentlyInScene[i].GetSaveData();
        }

        return coinSaveDataArray;
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

    private void LoadFromAsteroidSaveDataArray(AsteroidSaveData[] asteroidSaveDataArray)
    {
        // Destroy ALL asteroids at the time of Load
        Asteroid[] asteroidsCurrentlyInScene = FindObjectsByType<Asteroid>(FindObjectsInactive.Include);
        for (int i = 0; i < asteroidsCurrentlyInScene.Length; i++)
        {
            Destroy(asteroidsCurrentlyInScene[i].gameObject);
        }

        // Load the Asteroids from the save
        for (int i = 0; i < asteroidSaveDataArray.Length; i++)
        {
            // instantiate a new Asteroid
            Asteroid newAsteroid = Instantiate(asteroidPrefab);

            // let that Asteroid load its data
            newAsteroid.LoadFromSaveData(asteroidSaveDataArray[i], player);
        }
    }

    private void LoadFromCometSaveDataArray(CometSaveData[] cometSaveDataArray)
    {
        // Destroy ALL comets at the time of Load
        Comet[] cometsCurrentlyInScene = FindObjectsByType<Comet>(FindObjectsInactive.Include);
        for (int i = 0; i < cometsCurrentlyInScene.Length; i++)
        {
            Destroy(cometsCurrentlyInScene[i].gameObject);
        }

        // Load the Comets from the save
        for (int i = 0; i < cometSaveDataArray.Length; i++)
        {
            // instantiate a new Comet
            Comet newComet = Instantiate(cometPrefab);

            // let that Comet load its data
            newComet.LoadFromSaveData(cometSaveDataArray[i]);
        }
    }

    private void LoadFromCoinSaveDataArray(CoinSaveData[] coinSaveDataArray)
    {
        // Destroy ALL coins at the time of Load
        Coin[] coinsCurrentlyInScene = FindObjectsByType<Coin>(FindObjectsInactive.Include);
        for (int i = 0; i < coinsCurrentlyInScene.Length; i++)
        {
            Destroy(coinsCurrentlyInScene[i].gameObject);
        }

        // Load the Coins from the save
        for (int i = 0; i < coinSaveDataArray.Length; i++)
        {
            // instantiate a new Coin
            Coin newCoin = Instantiate(coinPrefab);

            // let that Coin load its data
            newCoin.LoadFromSaveData(coinSaveDataArray[i]);
        }
    }


}
