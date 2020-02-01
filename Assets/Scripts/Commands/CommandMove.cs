using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Command/Move")]
public class CommandMove : Command
{
    [Header("Folder Data")] 
    public FolderFile folder;

    private FileSystem filesystem;
    
    public override void InitializeCommand()
    {
        filesystem = GameObject.FindObjectOfType<FileSystem>();
    }

    public override void ExecuteCommand()
    {
        filesystem.MoveToFolder(folder);
    }
}
