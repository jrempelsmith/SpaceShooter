using UnityEngine;

public class Alien : Enemy
{
    private Asteroid shieldAsteroid;

    public override void OnShot()
    {
        if (shieldAsteroid == null)
        {
            health -= 1;
            InvokeOnLostHealth();
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Show some visual on the line renderer
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
