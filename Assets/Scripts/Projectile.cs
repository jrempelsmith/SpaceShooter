using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 direction;

    public void AimAt(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
    }

    private void Update()
    {
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
