using UnityEngine;

public class Alien : Enemy
{
    private Asteroid shieldAsteroid;

    public override void OnShot()
    {
        if (shieldAsteroid == null)
        {
            health -= 1;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            shieldAsteroid.OnShot();
        }
    }

    public override void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ)
    {
        base.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ);

        Vector3 centralTargetPoint = player.transform.position;
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f;

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
    }
}
