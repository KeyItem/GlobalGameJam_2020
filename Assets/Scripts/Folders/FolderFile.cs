using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FolderFile", menuName = "FolderFile/NewFolder", order = 2)]
public abstract class FolderFile : ScriptableObject
{
    [Header("Folder File Attributes")]
    public Command[] commands;

    [Space(10)] 
    public FolderFile parentFolder;
    
    [Space(10)]
    public FolderFile[] folders;
    
    [Space(10)]
    public FileData[] files;
    public virtual void InitializeFolder()
    {
        
    }
}
