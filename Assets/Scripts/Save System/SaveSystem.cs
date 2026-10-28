using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class SaveSystem : MonoBehaviour
{
    public enum SaveType
    {
        Alien,
        Asteroid,
        Comet,
        Coin
    }

    private readonly Dictionary<SaveType, System.Type> saveTypeDictionary = new()
    {
        [SaveType.Alien] = typeof(AlienSaveData),
        [SaveType.Asteroid] = typeof(AsteroidSaveData),
        [SaveType.Comet] = typeof(CometSaveData),
        [SaveType.Coin] = typeof(CoinSaveData)
    };

    private Dictionary<SaveType, GameObject> saveTypePrefabDictionary;

    [Header("Object references")]
    [SerializeField] private Player player;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject cometPrefab;
    [SerializeField] private GameObject coinPrefab;


    [Header("File name")]
    [SerializeField] private string saveFileName;
    [SerializeField] private string saveGameFileExtension;

    private string saveGameFilePath;

    private void Awake()
    {
        saveGameFilePath = Path.Combine(Application.persistentDataPath, saveFileName + "." + saveGameFileExtension);

        saveTypePrefabDictionary = new()
        {
            [SaveType.Alien] = alienPrefab,
            [SaveType.Asteroid] = asteroidPrefab,
            [SaveType.Comet] = cometPrefab,
            [SaveType.Coin] = coinPrefab
        };
    }

    public void SaveGame()
    {
        GetObjectSaveDataArray();

        SaveGameContainer saveGameContainer = new SaveGameContainer
        {
            playerSaveData = player.GetSaveData(),
            gameTimerSaveData = gameTimer.GetSaveData(),

            objectSaveDataArray = GetObjectSaveDataArray()

            //alienSaveDataArray = GetAlienSaveDataArray(),
            //asteroidSaveDataArray = GetAsteroidSaveDataArray(),
            //cometSaveDataArray = GetCometSaveDataArray(),
            //coinSaveDataArray = GetCoinSaveDataArray()
        };

        // 2. Serialize container as string
        string saveGameContainerString = JsonConvert.SerializeObject(saveGameContainer, Formatting.Indented);

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

        LoadFromObjectSaveDataArray(saveGameContainer.objectSaveDataArray);

        //LoadFromAlienSaveDataArray(saveGameContainer.alienSaveDataArray);
        //LoadFromAsteroidSaveDataArray(saveGameContainer.asteroidSaveDataArray);
        //LoadFromCometSaveDataArray(saveGameContainer.cometSaveDataArray);
        //LoadFromCoinSaveDataArray(saveGameContainer.coinSaveDataArray);
    }

    private SaveEntity[] GetObjectSaveDataArray()
    {
        List<ISaveable> iSaveablesCurrentlyInScene = new List<ISaveable>();

        // Get all ISaveables
        GameObject[] gameObjectsCurrentlyInScene = FindObjectsByType<GameObject>(FindObjectsInactive.Include);

        foreach (GameObject obj in gameObjectsCurrentlyInScene)
        {
            if (obj.TryGetComponent(out ISaveable iSaveable)) 
            {
                iSaveablesCurrentlyInScene.Add(iSaveable);
            }
        }

        // Create an array of the same length for object Save Data
        SaveEntity[] objectSaveDataArray = new SaveEntity[iSaveablesCurrentlyInScene.Count];

        // Populate the array with object save data
        for (int i = 0; i < objectSaveDataArray.Length; i++)
        {
            objectSaveDataArray[i] = new SaveEntity 
            {
                state = iSaveablesCurrentlyInScene[i].GetSaveData(),
                type = iSaveablesCurrentlyInScene[i].GetSaveType()
            };
        }

        return objectSaveDataArray;
    }

    

    private void LoadFromObjectSaveDataArray(SaveEntity[] objectSaveDataArray)
    {
        // Get all ISaveables
        List<ISaveable> iSaveablesCurrentlyInScene = new List<ISaveable>();
        GameObject[] gameObjectsCurrentlyInScene = FindObjectsByType<GameObject>(FindObjectsInactive.Include);

        foreach (GameObject obj in gameObjectsCurrentlyInScene)
        {
            if (obj.TryGetComponent(out ISaveable iSaveable))
            {
                iSaveablesCurrentlyInScene.Add(iSaveable);
            }
        }

        // Destroy ALL ISaveables at the time of Load
        for (int i = 0; i < iSaveablesCurrentlyInScene.Count; i++)
        {
            Destroy(iSaveablesCurrentlyInScene[i].GetGameObject());
        }


        // Load the ISaveables from the save
        for (int i = 0; i < objectSaveDataArray.Length; i++)
        {
            // json representation of the state
            JObject jsonObject = (JObject)objectSaveDataArray[i].state;

            // get the Type of save data (AlienSaveData)
            System.Type objectType = saveTypeDictionary[objectSaveDataArray[i].type];

            // convert the JSON object to the save data type (AlienSaveData)
            var objectSaveData = jsonObject.ToObject(objectType);

            GameObject loadedGameObject = Instantiate(saveTypePrefabDictionary[objectSaveDataArray[i].type]);

            if (!loadedGameObject.TryGetComponent(out ISaveable loadedISaveable))
            {
                Debug.LogError("Did not find ISaveable, something went wrong");
            }

            loadedISaveable.LoadFromSaveData(objectSaveData, player);
        }
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
            alienSaveDataArray[i] = (AlienSaveData)aliensCurrentlyInScene[i].GetSaveData();
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
            asteroidSaveDataArray[i] = (AsteroidSaveData)asteroidsCurrentlyInScene[i].GetSaveData();
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
            cometSaveDataArray[i] = (CometSaveData)cometsCurrentlyInScene[i].GetSaveData();
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
            coinSaveDataArray[i] = (CoinSaveData)coinsCurrentlyInScene[i].GetSaveData();
        }

        return coinSaveDataArray;
    }

    //private void LoadFromAlienSaveDataArray(AlienSaveData[] alienSaveDataArray)
    //{
    //    // Destroy ALL aliens at the time of Load
    //    Alien[] aliensCurrentlyInScene = FindObjectsByType<Alien>(FindObjectsInactive.Include);
    //    for (int i = 0; i < aliensCurrentlyInScene.Length; i++)
    //    {
    //        Destroy(aliensCurrentlyInScene[i].gameObject);
    //    }

    //    // Load the Aliens from the save
    //    for (int i = 0; i < alienSaveDataArray.Length; i++)
    //    {
    //        // instantiate a new Alien
    //        Alien newAlien = Instantiate(alienPrefab);

    //        // let that Alien load its data
    //        newAlien.LoadFromSaveData(alienSaveDataArray[i], player);
    //    }
    //}

    //private void LoadFromAsteroidSaveDataArray(AsteroidSaveData[] asteroidSaveDataArray)
    //{
    //    // Destroy ALL asteroids at the time of Load
    //    Asteroid[] asteroidsCurrentlyInScene = FindObjectsByType<Asteroid>(FindObjectsInactive.Include);
    //    for (int i = 0; i < asteroidsCurrentlyInScene.Length; i++)
    //    {
    //        Destroy(asteroidsCurrentlyInScene[i].gameObject);
    //    }

    //    // Load the Asteroids from the save
    //    for (int i = 0; i < asteroidSaveDataArray.Length; i++)
    //    {
    //        // instantiate a new Asteroid
    //        Asteroid newAsteroid = Instantiate(asteroidPrefab);

    //        // let that Asteroid load its data
    //        newAsteroid.LoadFromSaveData(asteroidSaveDataArray[i], player);
    //    }
    //}

    //private void LoadFromCometSaveDataArray(CometSaveData[] cometSaveDataArray)
    //{
    //    // Destroy ALL comets at the time of Load
    //    Comet[] cometsCurrentlyInScene = FindObjectsByType<Comet>(FindObjectsInactive.Include);
    //    for (int i = 0; i < cometsCurrentlyInScene.Length; i++)
    //    {
    //        Destroy(cometsCurrentlyInScene[i].gameObject);
    //    }

    //    // Load the Comets from the save
    //    for (int i = 0; i < cometSaveDataArray.Length; i++)
    //    {
    //        // instantiate a new Comet
    //        Comet newComet = Instantiate(cometPrefab);

    //        // let that Comet load its data
    //        newComet.LoadFromSaveData(cometSaveDataArray[i]);
    //    }
    //}

    //private void LoadFromCoinSaveDataArray(CoinSaveData[] coinSaveDataArray)
    //{
    //    // Destroy ALL coins at the time of Load
    //    Coin[] coinsCurrentlyInScene = FindObjectsByType<Coin>(FindObjectsInactive.Include);
    //    for (int i = 0; i < coinsCurrentlyInScene.Length; i++)
    //    {
    //        Destroy(coinsCurrentlyInScene[i].gameObject);
    //    }

    //    // Load the Coins from the save
    //    for (int i = 0; i < coinSaveDataArray.Length; i++)
    //    {
    //        // instantiate a new Coin
    //        Coin newCoin = Instantiate(coinPrefab);

    //        // let that Coin load its data
    //        newCoin.LoadFromSaveData(coinSaveDataArray[i]);
    //    }
    //}


}
