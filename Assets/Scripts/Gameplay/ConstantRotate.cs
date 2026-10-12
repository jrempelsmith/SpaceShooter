using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationsPerSecond * 360 * Time.deltaTime);
    }
}
