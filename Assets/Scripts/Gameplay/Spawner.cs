using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;



    [Header("Prefabs")]
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Alien alienPrefab;
    [SerializeField] private Comet cometPrefab;
    [SerializeField] private Coin coinPrefab;



    [Header("Enemy Spawn Rate")]

    [Tooltip("Chance that an asteroid will spawn instead of an alien")]
    [SerializeField] [Range(0, 1)] private float spawnPercentageAsteroid;

    [Tooltip("Chance that a coin will spawn with an enemy")]
    [SerializeField] [Range(0, 1)]  private float coinSpawnChance;

    [Tooltip("Minimum wait time between enemy spawns")]
    [SerializeField] private float spawnCooldownMin;

    [Tooltip("Maximum wait time between enemy spawns")]
    [SerializeField] private float spawnCooldownMax;



    [Header("Enemy Spawn Position")]
    [SerializeField] private float maxSpawnPositionX;
    [SerializeField] private float maxSpawnPositionZ;



    [Header("Alien Settings")]
    [SerializeField] private int alienSpawnHealth;
    [SerializeField] private float alienSpawnSpeed;



    [Header("Asteroid Settings")]
    [SerializeField] private int asteroidSpawnHealth;
    [SerializeField] private float asteroidSpawnSpeed;



    [Header("Comet Settings")]
    [SerializeField] private float minCometXRadius;
    [SerializeField] private float maxCometXRadius;
    [SerializeField] private float minCometZRadius;
    [SerializeField] private float maxCometZRadius;
    [SerializeField] [Tooltip("Degrees per second")] private float orbitalSpeed;
    [SerializeField] [Tooltip("Number of seconds between comet spawns")] private float cometSpawnInterval;


    private float timer;
    private float spawnCooldown;
    private bool coolingDown;


    private void Awake()
    {
        timer = 0;
        coolingDown = false;

        InvokeRepeating("SpawnComet", 1f, cometSpawnInterval);
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

    private void SpawnComet()
    {
        Comet newComet = Instantiate(cometPrefab);

        float horizontalRadius = Random.Range(minCometXRadius, maxCometXRadius);
        float verticalRadius = Random.Range(minCometZRadius, maxCometZRadius);

        newComet.RunSetup(orbitalSpeed, horizontalRadius, verticalRadius);
    }
}
