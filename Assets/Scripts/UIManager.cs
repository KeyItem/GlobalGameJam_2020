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

    [Space(10)] [SerializeField] 
    public Image imageBox;
    
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

    public void SetNewBacklogText(string newBacklogText)
    {
        backlogText.text = newBacklogText;
    }

    public void LoadImage(Sprite targetImage)
    {
        imageBox.enabled = true;
        imageBox.sprite = targetImage;
    }
}

[System.Serializable]
public struct TextElement
{
    [Header("Text Element Attributes")] 
    public string text;
}
