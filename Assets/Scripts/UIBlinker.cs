using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIBlinker : MonoBehaviour
{
    [Header("UI Blinker Attributes")] 
    private TextMeshProUGUI blinkObject;

    [Space(10)] 
    [SerializeField]
    private float blinkIncrement;

    [Header("Timer")] 
    private Timer blinkTimer;
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        blinkObject = GetComponent<TextMeshProUGUI>();

        blinkTimer = gameObject.AddComponent<Timer>();

        blinkTimer.OnTimerComplete += BlinkTimerComplete;
        
        StartBlink();
    }

    public void StartBlink()
    {
        if (blinkObject != null)
        {
           StartBlinkTimer();
        }
    }

    public void StopBlink()
    {
        
    }

    private void StartBlinkTimer()
    {
        blinkTimer.StartTimer(blinkIncrement);
    }
    private void BlinkTimerComplete()
    {
        blinkTimer.ResetTimer();

        blinkObject.enabled = !blinkObject.enabled;
        
        StartBlink();
    }
    
}
