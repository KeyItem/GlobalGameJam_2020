using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Command/OpenText")]
public class CommandOpenText : Command
{
    [Header("Text Attributes")] 
    [TextArea(15, 50)]
    public string textFile;

    private UIManager UI;

    public override void InitializeCommand()
    {
        UI = GameObject.FindObjectOfType<UIManager>();
    }

    public override void ExecuteCommand()
    {
        UI.LoadText(textFile);
    }
}
