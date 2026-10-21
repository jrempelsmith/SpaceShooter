using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public event EventHandler OnHealthUpdated;
    public event EventHandler OnLostHealth;
    public event EventHandler OnZeroHealth;

    [Header("References")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Ammo ammo;

    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] private float maxXPosition;
    [SerializeField] private float maxZPosition;

    public int Health
    {
        get => health;
        private set
        {
            health = value;
            OnHealthUpdated?.Invoke(this, EventArgs.Empty);
        }
    }


    private void Update()
    {
        Vector3? mouseWorldPosition = GetMouseWorldPosition();
        
        if (mouseWorldPosition != null)
        {
            HandleFiringInput((Vector3)mouseWorldPosition);
            transform.rotation = Quaternion.LookRotation(((Vector3)mouseWorldPosition - transform.position).normalized, Vector3.up);
        }
        
        HandleMovementInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            TakeDamage();
            Destroy(enemy.gameObject);
        }
        else if (other.TryGetComponent(out Comet comet))
        {
            TakeDamage();
        }
        else if (other.TryGetComponent(out Coin coin))
        {
            ammo.AddAmmo(coin.AmmoValue);
            Destroy(coin.gameObject);
        }
    }
    
    public void TakeDamage()
    {
        health -= 1;

        OnLostHealth?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            OnZeroHealth?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleFiringInput(Vector3 mouseWorldPosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (ammo.CurrentAmmo > 0)
            {
                const int projectileCost = 1;
                ammo.SubtractAmmo(projectileCost);
                Projectile newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newProjectile.AimAt(mouseWorldPosition);
            }
        }
    }

    private void HandleMovementInput()
    {
        Vector2 movementInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed && transform.position.z < maxZPosition)
        {
            movementInput.y = 1;
        }
        if (Keyboard.current.sKey.isPressed && transform.position.z > -maxZPosition)
        {
            movementInput.y = -1;
        }
        if (Keyboard.current.aKey.isPressed && transform.position.x > -maxXPosition)
        {
            movementInput.x = -1;
        }
        if (Keyboard.current.dKey.isPressed && transform.position.x < maxXPosition)
        {
            movementInput.x = 1;
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

    public PlayerSaveData GetSaveData()
    {
        Vector3Data playerPosition = new Vector3Data
        {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };

        PlayerSaveData playerSaveData = new PlayerSaveData
        {
            position = playerPosition,
            health = Health,
            ammo = ammo.CurrentAmmo
        };

        return playerSaveData;
    }

    public void LoadFromSaveData(PlayerSaveData saveData)
    {
        Vector3 playerPosition = new Vector3(saveData.position.x, saveData.position.y, saveData.position.z);

        transform.position = playerPosition;
        Health = saveData.health;
        ammo.CurrentAmmo = saveData.ammo;
    }
}
