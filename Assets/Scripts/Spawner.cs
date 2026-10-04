using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Prefabs")]
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Alien alienPrefab;
    [SerializeField] private Coin coinPrefab;

    [Header("Settings")]
    [SerializeField] [Range(0, 1)] private float spawnPercentageAsteroid;
    [SerializeField] private float maxSpawnPositionX;
    [SerializeField] private float maxSpawnPositionZ;

    private float timer;
    private float spawnCooldown;
    private readonly float spawnCooldownMin = 0.5f;
    private readonly float spawnCooldownMax = 2f;
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

            const float coinSpawnChance = 0.5f;
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
        }
        else
        {
            newEnemy = Instantiate(alienPrefab);
        }

        newEnemy.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ);
    }
}
