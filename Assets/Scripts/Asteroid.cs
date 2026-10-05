using UnityEngine;

public class Asteroid : Enemy
{
    public override void OnShot()
    {
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ)
    {
        base.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ);

        const float centralRegionRange = 4f;

        Vector3 centralTargetPoint = new Vector3(Random.Range(-centralRegionRange, centralRegionRange), 0, Random.Range(-centralRegionRange, centralRegionRange));
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f;
    }
}
