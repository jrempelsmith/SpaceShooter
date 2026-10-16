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
}
