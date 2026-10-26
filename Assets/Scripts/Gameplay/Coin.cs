using UnityEngine;

public class Coin : MonoBehaviour, IShootable
{
    [SerializeField] private float speed;
    [SerializeField] private int ammoValue;

    private Vector3 targetPoint;

    public int AmmoValue => ammoValue;

    public void OnShot()
    {
        Destroy(gameObject);
    }

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

    public CoinSaveData GetSaveData()
    {
        Vector3Data coinPosition = new Vector3Data
        {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };

        Vector3Data coinTargetPoint = new Vector3Data
        {
            x = targetPoint.x,
            y = targetPoint.y,
            z = targetPoint.z
        };


        CoinSaveData coinSaveData = new CoinSaveData
        {
            position = coinPosition,
            targetPoint = coinTargetPoint
        };

        return coinSaveData;
    }

    public void LoadFromSaveData(CoinSaveData saveData)
    {
        Vector3 coinPosition = new Vector3(saveData.position.x, saveData.position.y, saveData.position.z);
        Vector3 coinTargetPoint = new Vector3(saveData.targetPoint.x, saveData.targetPoint.y, saveData.targetPoint.z);

        transform.position = coinPosition;
        targetPoint = coinTargetPoint;
    }
}
