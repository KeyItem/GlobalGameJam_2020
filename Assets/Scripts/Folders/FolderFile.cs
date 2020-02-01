using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FolderFile", menuName = "FolderFile/NewFolder", order = 2)]
public abstract class FolderFile : ScriptableObject
{
    public FileData[] files;
    public virtual void InitializeFolder()
    {
        
    }
}
