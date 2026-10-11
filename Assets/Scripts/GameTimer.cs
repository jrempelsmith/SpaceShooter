using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public event EventHandler OnTimerFinished;

    [SerializeField] private float remainingTime;

    public float RemainingTime => remainingTime;

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            OnTimerFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
