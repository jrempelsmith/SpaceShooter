using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Prefabs")]
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Alien alienPrefab;
    [SerializeField] private Coin coinPrefab;

    [Header("Spawn Rate")]
    [SerializeField] [Range(0, 1)] [Tooltip("Chance that an asteroid will spawn instead of an alien")]
    private float spawnPercentageAsteroid;

    [SerializeField] [Range(0, 1)] [Tooltip("Chance that a coin will spawn with an enemy")] 
    private float coinSpawnChance;

    [SerializeField] [Tooltip("Minimum wait time between enemy spawns")] 
    private float spawnCooldownMin;

    [SerializeField] [Tooltip("Maximum wait time between enemy spawns")] 
    private float spawnCooldownMax;

    [Header("Spawn Position")]
    [SerializeField] private float maxSpawnPositionX;
    [SerializeField] private float maxSpawnPositionZ;

    [Header("Alien Settings")]
    [SerializeField] private int alienSpawnHealth;
    [SerializeField] private float alienSpawnSpeed;

    [Header("Asteroid Settings")]
    [SerializeField] private int asteroidSpawnHealth;
    [SerializeField] private float asteroidSpawnSpeed;

    private float timer;
    private float spawnCooldown;
    private bool coolingDown;

    private void Awake()
    {
        timer = 0;
        coolingDown = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!coolingDown)
        {
            SpawnRandomEnemy();
            coolingDown = true;
            spawnCooldown = Random.Range(spawnCooldownMin, spawnCooldownMax);

            if (Random.value < coinSpawnChance)
            {
                Coin newCoin = Instantiate(coinPrefab);
                newCoin.RunSetup(maxSpawnPositionX, maxSpawnPositionZ);
            }
        }
        else
        {
            if (timer >= spawnCooldown)
            {
                timer = 0;
                coolingDown = false;
            }
        }
    }

    private void SpawnRandomEnemy()
    {
        Enemy newEnemy;
        
        if (Random.value < spawnPercentageAsteroid)
        {
            newEnemy = Instantiate(asteroidPrefab);
            newEnemy.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ, asteroidSpawnHealth, asteroidSpawnSpeed);
        }
        else
        {
            newEnemy = Instantiate(alienPrefab);
            newEnemy.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ, alienSpawnHealth, alienSpawnSpeed);
        }
    }
}
