using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Command/GoUp")]
public class CommandGoUp : Command
{
    [Header("File System Attributes")] 
    private FileSystem filesystem;
    public override void InitializeCommand()
    {
        filesystem = GameObject.FindObjectOfType<FileSystem>();
    }

    public override void ExecuteCommand()
    {
        filesystem.ReturnToParentFolder();
    }
}
