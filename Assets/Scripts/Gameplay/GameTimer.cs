using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public event EventHandler OnTimerFinished;
    private bool timerFinishedEventFired;

    [SerializeField] private float remainingTime;

    public float RemainingTime => remainingTime;

    private void Awake()
    {
        timerFinishedEventFired = false;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0 && !timerFinishedEventFired)
        {
            OnTimerFinished?.Invoke(this, EventArgs.Empty);
            timerFinishedEventFired = true;
        }
    }
}
