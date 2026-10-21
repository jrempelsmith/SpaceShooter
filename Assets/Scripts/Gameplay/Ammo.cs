using System;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public event EventHandler OnAmmoChanged;

    [SerializeField] private int currentAmmo;

    public int CurrentAmmo
    {
        get => currentAmmo;
        set
        {
            currentAmmo = value;
            OnAmmoChanged?.Invoke(this, new EventArgs());
        }
    }

    private void Start()
    {
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SubtractAmmo(int amount)
    {
        if (currentAmmo - amount < 0)
        {
            Debug.LogWarning("Not enough ammo to subtract " + amount);
            return;
        }
        
        currentAmmo -= amount;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }
}
