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

        Vector3 centralTargetPoint = new Vector3(Random.Range(-centralRegionRange, centralRegionRange), 0, Random.Range(-centralRegionRange, centralRegionRange));
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f;
    }
}
