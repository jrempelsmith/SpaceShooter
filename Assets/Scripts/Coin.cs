using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 targetPoint;

    public void RunSetup(float maxSpawnPositionX, float maxSpawnPositionZ)
    {
        // spawn at a random position along the edges
        Vector3 spawnPosition;
        int edgeIndex = Random.Range(0, 4);
        if (edgeIndex == 0)
        {
            spawnPosition = new Vector3(Random.Range(-maxSpawnPositionX, maxSpawnPositionX), 0, -maxSpawnPositionZ);
        }
        else if (edgeIndex == 1)
        {
            spawnPosition = new Vector3(Random.Range(-maxSpawnPositionX, maxSpawnPositionX), 0, maxSpawnPositionZ);
        }
        else if (edgeIndex == 2)
        {
            spawnPosition = new Vector3(-maxSpawnPositionX, 0, Random.Range(-maxSpawnPositionZ, maxSpawnPositionZ));
        }
        else
        {
            spawnPosition = new Vector3(maxSpawnPositionX, 0, Random.Range(-maxSpawnPositionZ, maxSpawnPositionZ));
        }

        transform.position = spawnPosition;

        // Aim towards the middle-ish, and keep going beyond to the edge
        const float centralRegionRange = 6f;

        Vector3 centralTargetPoint = new Vector3(Random.Range(-centralRegionRange, centralRegionRange), 0, Random.Range(-centralRegionRange, centralRegionRange));
        targetPoint = transform.position + (centralTargetPoint - transform.position) * 10f;
    }

    private void Update()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Boundary>())
        {
            Destroy(gameObject);
        }
    }
}
