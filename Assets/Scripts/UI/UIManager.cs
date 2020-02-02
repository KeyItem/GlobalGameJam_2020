using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Attributes")] 
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
        backlogText.text = newBacklogText;
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
        textBox.text = newText;
        textBox.enabled = true;
    }

    public void CloseOpenWindows()
    {
        imageBox.enabled = false;
        imageBox.sprite = null;

        textBox.enabled = false;
        textBox.text = string.Empty;
    }
}

[System.Serializable]
public struct TextElement
{
    [Header("Text Element Attributes")] 
    public string text;
}
