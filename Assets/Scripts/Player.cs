using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Money money;
    [SerializeField] private float speed;
    [SerializeField] private int health;

    private void Awake()
    {
        
    }

    private void Update()
    {
        Vector3? mouseWorldPosition = GetMouseWorldPosition();
        
        if (mouseWorldPosition != null)
        {
            HandleMouseClick((Vector3)mouseWorldPosition);
            transform.rotation = Quaternion.LookRotation(((Vector3)mouseWorldPosition - transform.position).normalized, Vector3.up);
        }
        
        HandleKeyboardInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            health -= 1;
            Destroy(enemy.gameObject);

            if (health <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
        else if (other.TryGetComponent(out Coin coin))
        {
            money.AddMoney(coin.MoneyValue);
            Destroy(coin.gameObject);
        }
    }

    private void HandleMouseClick(Vector3 mouseWorldPosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (money.CurrentMoney > 0)
            {
                const int projectileCost = 1;
                money.SubtractMoney(projectileCost);
                Projectile newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newProjectile.AimAt(mouseWorldPosition);
            }
        }
    }

    private void HandleKeyboardInput()
    {
        Vector2 movementInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed)
        {
            movementInput.y += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            movementInput.y -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            movementInput.x -= 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            movementInput.x += 1;
        }

        Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }

    private Vector3? GetMouseWorldPosition()
    {
        Ray pointerRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // normal is (0,1,0) and point on plane is (0,0,0)

        if (groundPlane.Raycast(pointerRay, out float distanceToPlane))
        {
            return pointerRay.GetPoint(distanceToPlane);
        }
        else
        {
            return null; // Return null if the ray does not intersect the plane
        }
    }
}
