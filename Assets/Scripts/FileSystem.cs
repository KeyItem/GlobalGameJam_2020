using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    [Header("File System Attributes")] 
    public FolderData currentFolder;
    public FileData[] currentAvailableFiles;

    [Space(10)] 
    public FolderData baseFolder;

    public void MoveToFolder(FolderData newFolder)
    {
        currentFolder = newFolder;
        currentAvailableFiles = ReturnFilesFromFolder(newFolder);
    }
    public FileData[] ReturnFiles()
    {
        return currentAvailableFiles;
    }

    private FileData[] ReturnFilesFromFolder(FolderData folder)
    {
        return folder.files;
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
