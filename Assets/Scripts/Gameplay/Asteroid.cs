using UnityEngine;

public class Asteroid : Enemy
{
    public override void OnShot()
    {
        health -= 1;
        InvokeOnLostHealth();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ, int health, float speed)
    {
        base.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ, health, speed);

        const float centralRegionRange = 4f;

        float randomX = Random.Range(-centralRegionRange, centralRegionRange);
        float randomZ = Random.Range(-centralRegionRange, centralRegionRange);

        Vector3 centralTargetPoint = new Vector3(randomX, 0, randomZ);
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f; // large multiplier to ensure it moves all the way through the level
    }

    public AsteroidSaveData GetSaveData()
    {
        Vector3Data asteroidPosition = new Vector3Data
        {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };

        Vector3Data asteroidTargetPoint = new Vector3Data
        {
            x = targetPoint.x,
            y = targetPoint.y,
            z = targetPoint.z
        };

        AsteroidSaveData asteroidSaveData = new AsteroidSaveData
        {
            position = asteroidPosition,
            targetPoint = asteroidTargetPoint,
            health = Health
        };

        return asteroidSaveData;
    }

    public void LoadFromSaveData(AsteroidSaveData saveData, Player player)
    {
        Vector3 asteroidPosition = new Vector3(saveData.position.x, saveData.position.y, saveData.position.z);
        Vector3 asteroidTargetPoint = new Vector3(saveData.targetPoint.x, saveData.targetPoint.y, saveData.targetPoint.z);

        transform.position = asteroidPosition;
        targetPoint = asteroidTargetPoint;
        Health = saveData.health;
        this.player = player;

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
    }
}
