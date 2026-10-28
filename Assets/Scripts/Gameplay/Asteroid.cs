using UnityEngine;

public class Asteroid : Enemy, ISaveable
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

    public object GetSaveData()
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

    public void LoadFromSaveData(object saveData, Player player)
    {
        AsteroidSaveData asteroidSaveData = (AsteroidSaveData)saveData;

        Vector3 asteroidPosition = new Vector3(asteroidSaveData.position.x, asteroidSaveData.position.y, asteroidSaveData.position.z);
        Vector3 asteroidTargetPoint = new Vector3(asteroidSaveData.targetPoint.x, asteroidSaveData.targetPoint.y, asteroidSaveData.targetPoint.z);

        transform.position = asteroidPosition;
        targetPoint = asteroidTargetPoint;
        Health = asteroidSaveData.health;
        this.player = player;

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
    }

    SaveSystem.SaveType ISaveable.GetSaveType()
    {
        return SaveSystem.SaveType.Asteroid;
    }

    GameObject ISaveable.GetGameObject()
    {
        return gameObject;
    }
}
