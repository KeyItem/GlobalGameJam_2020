﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Attributes")] 
    public TextDisplayAttributes displayAttributes;

    private IEnumerator activeTyper;
    
    [Space(10)]
    [SerializeField]
    private TextMeshProUGUI inputText;

    [Space(10)] 
    [SerializeField]
    private TextMeshProUGUI backlogText;

    [Space(10)] 
    [SerializeField] 
    private Image imageBox;
    [SerializeField]
    private TextMeshProUGUI textBox;
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        
    }

    public void SetNewInputText(string newText)
    {
        inputText.text = newText;
    }

    public void ClearInputText()
    {
        inputText.text = string.Empty;
    }

    public void SetNewBacklogText(string newBacklogText)
    {
        StartDisplayTyperBacklog(newBacklogText);
    }

    public void ClearBacklogText()
    {
        backlogText.text = string.Empty;
    }

    public void LoadImage(Sprite targetImage)
    {
        imageBox.sprite = targetImage;
        imageBox.enabled = true;
    }

    public void LoadText(string newText)
    {
        textBox.enabled = true;
        
        StartDisplayTyperTextBox(newText);
    }

    public void CloseOpenWindows()
    {
        imageBox.enabled = false;
        imageBox.sprite = null;

        textBox.enabled = false;
        textBox.text = string.Empty;
    }

    private void StartDisplayTyperBacklog(string newText)
    {
        if (activeTyper != null)
        {
            StopCoroutine(activeTyper);
        }

        backlogText.text = string.Empty;

        activeTyper = DisplayTextBacklog(newText, displayAttributes.textDisplayDelay);
        
        StartCoroutine(activeTyper);
    }
    
    private void StartDisplayTyperTextBox(string newText)
    {
        if (activeTyper != null)
        {
            StopCoroutine(activeTyper);
        }

        backlogText.text = string.Empty;

        activeTyper = DisplayTextWindow(newText, displayAttributes.textDisplayDelay);
        
        StartCoroutine(activeTyper);
    }
    
    private IEnumerator DisplayTextBacklog(string newText, float delay)
    {
        char[] letters = newText.ToCharArray();
        
        foreach (char letter in letters)
        {
            backlogText.text += letter;
            
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }
    
    private IEnumerator DisplayTextWindow(string newText, float delay)
    {
        char[] letters = newText.ToCharArray();
        
        foreach (char letter in letters)
        {
            textBox.text += letter;
            
            yield return new WaitForSeconds(delay);
        }

        yield return null;
    }
}

[System.Serializable]
public struct TextElement
{
    [Header("Text Element Attributes")] 
    public string text;
}

[System.Serializable]
public struct TextDisplayAttributes
{
    [Header("Text Display Attributes")] 
    public float textDisplayDelay;
}