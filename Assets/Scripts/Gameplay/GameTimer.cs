using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public event EventHandler OnTimerFinished;
    private bool timerFinishedEventFired;

    [SerializeField] private float remainingTime;

    public float RemainingTime
    {
        get => remainingTime;
        set => remainingTime = value;
    }

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

    public GameTimerSaveData GetSaveData()
    {
        return new GameTimerSaveData
        {
            remainingTime = remainingTime
        };
    }

    public void LoadFromSaveData(GameTimerSaveData saveData)
    {
        remainingTime = saveData.remainingTime;
    }
}

