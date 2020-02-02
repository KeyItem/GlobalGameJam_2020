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
    
    private void Awake()
    {
        Initialize();
    }
    
    protected void Initialize()
    {
        currentFolder = baseFolder;
        currentAvailableFiles = ReturnFilesFromFolder(baseFolder);
    }

    public void MoveToFolder(FolderFile newFolder)
    {
        currentFolder = newFolder;
        currentAvailableFiles = ReturnFilesFromFolder(newFolder);

        previousFolder = newFolder.parentFolder;
    }

    public void ReturnToParentFolder()
    {
        if (previousFolder != null)
        {
            MoveToFolder(previousFolder);
        }
    }
    public FileData[] ReturnFilesInFolder()
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

    public FolderFile[] ReturnFoldersInFolder()
    {
        return currentFolder.folders;
    }

    public FolderFile ReturnParentFolder()
    {
        return currentFolder.parentFolder;
    }
    
    public bool IsFolderAvailableToMove(string[] newCommand)
    {
        if (newCommand.Length > 1)
        {
            foreach (FolderFile folder in currentFolder.folders)
            {
                if (newCommand[1] == folder.name)
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

    public FILE_TYPE type;
    
    public string lastAccessDate;

    public int sizeBytes;

    [Space(10)] 
    public string filePassword;

    [Space(10)]
    [TextArea(15, 60)]
    public string text;

    [Space(10)]
    public Sprite image;

    public bool DoesInputMatchPassword(string[] inputString)
    {
        if (inputString.Length > 3)
        {
            if (inputString[3] == filePassword)
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public enum FILE_TYPE
{
    NONE,
    TEXT,
    IMAGE,
    PASSWORD
}
