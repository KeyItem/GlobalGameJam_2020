using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer Attributes")]
    private float currentTime = float.MaxValue;
    private float maxTime = float.MaxValue;

    private bool isTimerActive = false;

    public delegate void TimerComplete();
    public event TimerComplete OnTimerComplete;

    private void Awake()
    {
        InitializeTimer();
    }

    private void Update()
    {
        ManageTimer();
    }

    private void InitializeTimer()
    {
        currentTime = float.MaxValue;
        maxTime = float.MaxValue;

        isTimerActive = false;
    }

    private void ManageTimer()
    {
        if (isTimerActive)
        {
            currentTime -= Time.deltaTime;

            if (CheckIfTimerIsDone(currentTime))
            {
                CompleteTimer();
            }
        }
    }

    public void StartTimer(float newTime)
    {
        currentTime = newTime;
        maxTime = newTime;
        
        isTimerActive = true;
    }

    public void PauseTimer()
    {
        isTimerActive = false;
    }

    public void ResetTimer()
    {
        currentTime = float.MaxValue;
        maxTime = float.MaxValue;

        isTimerActive = false;
    }

    private void CompleteTimer()
    {
        isTimerActive = false;
        
        currentTime = float.MaxValue;
        maxTime = float.MaxValue;
        
        if (OnTimerComplete != null)
        {
            OnTimerComplete();
        }
    }

    public float ReturnTimeRatio()
    {
        if (isTimerActive)
        {
            return currentTime / maxTime;
        }

        return 0f;
    }

    private bool CheckIfTimerIsDone(float time)
    {
        return time <= 0;
    }
    
    public bool ReturnTimerStatus()
    {
        return isTimerActive;
    }
}
