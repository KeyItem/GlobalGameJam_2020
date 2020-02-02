using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    [Header("File System Attributes")] 
    private FolderFile currentFolder;
    private FolderFile previousFolder;
    
    private FileData[] currentAvailableFiles;

    [Space(10)] 
    public FolderFile baseFolder;

    [Header("Command Manager")] 
    private CommandManager manager;

    private void Awake()
    {
        Initialize();
    }
    
    protected void Initialize()
    {
        currentFolder = baseFolder;
        currentAvailableFiles = ReturnFilesFromFolder(baseFolder);

        manager = GetComponent<CommandManager>();
    }

    public void MoveToFolder(FolderFile newFolder)
    {
        currentFolder = newFolder;
        currentAvailableFiles = ReturnFilesFromFolder(newFolder);

        previousFolder = newFolder.parentFolder;
        
        manager.SetAvailableCommands(newFolder.commands);
    }

    public void ReturnToParentFolder()
    {
        if (previousFolder != null)
        {
            MoveToFolder(previousFolder);
        }
    }
    public FileData[] ReturnFiles()
    {
        return currentAvailableFiles;
    }

    private FileData[] ReturnFilesFromFolder(FolderFile folder)
    {
        return folder.files;
    }

    public FileData ReturnFile(string[] newCommand)
    {
        for (int i = 0; i < currentAvailableFiles.Length; i++)
        {
            if (newCommand.Length > 1)
            {
                if (newCommand[1] == currentAvailableFiles[i].name)
                {
                    return currentAvailableFiles[i];
                }
            }
        }

        return new FileData();
    }
    
    public bool IsFileAvailableToOpen(string[] newCommand)
    {
        for (int i = 0; i < currentAvailableFiles.Length; i++)
        {
            if (newCommand.Length > 1)
            {
                if (newCommand[1] == currentAvailableFiles[i].name)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public FolderFile ReturnFolder(string[] newCommand)
    {
        for (int i = 0; i < currentFolder.folders.Length; i++)
        {
            if (newCommand.Length > 1)
            {
                if (newCommand[1] == currentFolder.folders[i].name)
                {
                    return currentFolder.folders[i];
                }
            }
        }

        return null;
    }
    
    public bool IsFolderAvailableToMove(string[] newCommand)
    {
        for (int i = 0; i < currentFolder.folders.Length; i++)
        {
            if (newCommand.Length > 1)
            {
                if (newCommand[1] == currentFolder.files[i].name)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

[System.Serializable]
public struct FolderData
{
    [Header("Folder Data Attributes")] 
    public string name;
    public string lastAccessDate;

    public int sizeBytes;

    [Space(10)] 
    public FileData[] files;
}

[System.Serializable]
public struct FileData
{
    [Header("File Data Attributes")] 
    public string name;
    public string lastAccessDate;

    public int sizeBytes;
}
