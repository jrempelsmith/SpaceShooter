using System.Collections.Generic;
using UnityEngine;

public class Alien : Enemy
{
    private struct AsteroidConnection
    {
        public Asteroid asteroid;
        public LineRenderer lineRenderer;
    }

    private const float connectionLineWidth = 0.3f;
    private List<AsteroidConnection> connectedAsteroids;
    [SerializeField] private Material connectionLineMaterial;

    private void Awake()
    {
        connectedAsteroids = new List<AsteroidConnection>();
    }

    protected override void Update()
    {
        base.Update();

        // loop backwards through the list so we can remove items while iterating
        for (int i = connectedAsteroids.Count - 1; i >= 0; i--)
        {
            AsteroidConnection cnxn = connectedAsteroids[i];
            if (cnxn.asteroid == null)
            {
                Destroy(cnxn.lineRenderer.gameObject);
                connectedAsteroids.RemoveAt(i);
            }
            else
            {
                cnxn.lineRenderer.SetPosition(0, transform.position);
                cnxn.lineRenderer.SetPosition(1, cnxn.asteroid.transform.position);

                // Raycast from the alien to the asteroid to check for the player
                Ray ray = new Ray(transform.position, cnxn.asteroid.transform.position - transform.position);
                float distance = Vector3.Distance(transform.position, cnxn.asteroid.transform.position);
                if (Physics.SphereCast(ray, connectionLineWidth / 2f, out RaycastHit hitInfo, distance))
                {
                    if (hitInfo.transform == player.transform)
                    {
                        // Player is touching the laser, so it takes damage
                        player.TakeDamage();

                        Destroy(cnxn.lineRenderer.gameObject);
                        connectedAsteroids.RemoveAt(i);
                    }
                }
            }
        }
    }

    public override void OnShot()
    {
        health -= 1;
        InvokeOnLostHealth();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.TryGetComponent(out Asteroid asteroid))
        {
            foreach (AsteroidConnection cnxn in connectedAsteroids)
            {
                if (cnxn.asteroid == asteroid)
                {
                    return; // Already connected
                }
            }

            GameObject newChild = new GameObject();
            newChild.transform.parent = transform;
            newChild.transform.localPosition = Vector3.zero;
            LineRenderer newLineRenderer = newChild.AddComponent<LineRenderer>();
            newLineRenderer.widthMultiplier = connectionLineWidth;
            newLineRenderer.material = connectionLineMaterial;
            connectedAsteroids.Add(new AsteroidConnection
            {
                asteroid = asteroid,
                lineRenderer = newLineRenderer
            });
        }
    }

    public override void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ, int health, float speed)
    {
        base.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ, health, speed);

        Vector3 centralTargetPoint = player.transform.position;
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f; // large multiplier to ensure it moves all the way through the level

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
    }
}
