using UnityEngine;

public abstract class Enemy : MonoBehaviour, IShootable
{
    protected Player player;

    [SerializeField] protected float speed;
    [SerializeField] protected int health;
    protected Vector3 targetPoint;

    public abstract void OnShot();

    protected virtual void Update()
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



    public virtual void RunSetup(Player player, float maxSpawnPositionX, float maxSpawnPositionZ)
    {
        if (player == null)
        {
            Debug.LogError("Player reference is null in Enemy.SetPlayer");
        }
        this.player = player;


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
    }
}
