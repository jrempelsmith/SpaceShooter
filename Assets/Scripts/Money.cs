using System;
using UnityEngine;

public class Money : MonoBehaviour
{
    public event EventHandler OnMoneyChanged;

    [SerializeField] private int currentMoney;

    public int CurrentMoney
    {
        get => currentMoney;
        private set => currentMoney = value;
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SubtractMoney(int amount)
    {
        if (currentMoney - amount < 0)
        {
            Debug.LogWarning("Not enough money to subtract " + amount);
            return;
        }
        
        currentMoney -= amount;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }
}
