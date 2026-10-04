using UnityEngine;

public class Alien : Enemy
{
    public override void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ)
    {
        base.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ);

        Vector3 centralTargetPoint = player.transform.position;
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f;

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
    }
}
