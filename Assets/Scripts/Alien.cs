using UnityEngine;

public class Alien : Enemy
{
    private Asteroid shieldAsteroid;
    private LineRenderer shieldAsteroidConnectionLineRenderer;

    private void Awake()
    {
        shieldAsteroidConnectionLineRenderer = GetComponent<LineRenderer>();
        shieldAsteroidConnectionLineRenderer.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        // Now update the line renderer
        if (shieldAsteroid == null)
        {
            shieldAsteroidConnectionLineRenderer.enabled = false;
        }
        else
        {
            shieldAsteroidConnectionLineRenderer.SetPosition(0, transform.position);
            shieldAsteroidConnectionLineRenderer.SetPosition(1, shieldAsteroid.transform.position);
        }
    }

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
            // Show some visual on the line renderer, e.g. change start and end colors for a moment (Coroutine)
            shieldAsteroid.OnShot();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (shieldAsteroid == null && other.TryGetComponent(out Asteroid asteroid))
        {
            shieldAsteroid = asteroid;
            shieldAsteroidConnectionLineRenderer.enabled = true;
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
