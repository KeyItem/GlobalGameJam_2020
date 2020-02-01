using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu (menuName = "Command/OpenImage")]
public class CommandOpenImage : Command
{
    [Header("Image Attributes")] 
    public Sprite imageFile;

    private UIManager UI;

    public override void InitializeCommand()
    {
        UI = GameObject.FindObjectOfType<UIManager>();
    }

    public override void ExecuteCommand()
    {
        UI.LoadImage(imageFile);
    }
}
