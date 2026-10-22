using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
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

    private Comet connectedComet;
    [SerializeField] private LineRenderer cometConnectionLineRenderer;
    [SerializeField] private GameObject shieldIcon;
    private const float shieldVisibleTime = 1f;

    private void Awake()
    {
        connectedAsteroids = new List<AsteroidConnection>();
    }

    protected override void Update()
    {
        base.Update();

        // loop backwards through the list of connectedAsteroids so we can remove Asteroids (if needed) while iterating
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

        if (connectedComet != null)
        {
            cometConnectionLineRenderer.SetPosition(0, transform.position);
            cometConnectionLineRenderer.SetPosition(1, connectedComet.transform.position);
        }
    }

    public override void OnShot()
    {
        if (connectedComet == null)
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
            StartCoroutine(ShowShield());
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

        else if (connectedComet == null && other.TryGetComponent(out Comet comet))
        {
            connectedComet = comet;
            cometConnectionLineRenderer.enabled = true;
        }
    }

    public override void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ, int health, float speed)
    {
        base.RunSetup(player, maxSpawnPositionX, maxSpawnPositionZ, health, speed);

        Vector3 centralTargetPoint = player.transform.position;
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f; // large multiplier to ensure it moves all the way through the level

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);

        shieldIcon.transform.rotation = Quaternion.Euler(90, 0, 0); // upright
    }

    private IEnumerator ShowShield()
    {
        shieldIcon.SetActive(true);
        yield return new WaitForSeconds(shieldVisibleTime);
        shieldIcon.SetActive(false);
    }

    public AlienSaveData GetSaveData()
    {
        Vector3Data alienPosition = new Vector3Data
        {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };

        Vector3Data alienTargetPoint = new Vector3Data
        {
            x = targetPoint.x,
            y = targetPoint.y,
            z = targetPoint.z
        };

        AlienSaveData alienSaveData = new AlienSaveData
        {
            position = alienPosition,
            targetPoint = alienTargetPoint,
            health = Health
        };

        return alienSaveData;
    }

    public void LoadFromSaveData(AlienSaveData saveData, Player player)
    {
        Vector3 alienPosition = new Vector3(saveData.position.x, saveData.position.y, saveData.position.z);
        Vector3 alienTargetPoint = new Vector3(saveData.targetPoint.x, saveData.targetPoint.y, saveData.targetPoint.z);

        transform.position = alienPosition;
        targetPoint = alienTargetPoint;
        Health = saveData.health;
        this.player = player;

        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
    }
}
