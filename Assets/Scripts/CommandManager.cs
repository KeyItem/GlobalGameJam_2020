﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class CommandManager : MonoBehaviour
{
    [Header("UI Attributes")] 
    private UIManager UI;
    
    [Header("Command Attributes")] 
    public Command[] allCommands;
    
    [Space(10)]
    public Command[] customCommands;

    [Space(10)]
    public Command helpCommand;
    public Command moveErrorCommand;
    public Command openErrorCommand;
    public Command errorCommand;
    
    [Space(10)]
    public Command lastCommandEntered;
    
    [Header("Input Attributes")] 
    private string currentString;
    
    [SerializeField]
    private bool canInput = true;

    [Header("File System Attributes")] 
    private FileSystem filesystem;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        ManageInput();
    }

    protected void Initialize()
    {
        UI = GetComponent<UIManager>();
        filesystem = GetComponent<FileSystem>();

        currentString = "";
    }

    protected void ManageInput()
    {
        if (canInput)
        {
            string keyInput = Input.inputString;

            foreach (char key in keyInput)
            {
                if (IsBackspaceInput(key))
                {
                    Backspace();
                }
                else if (IsEnterInput(key))
                {
                    Enter();
                }
                else
                {
                    AddLetter(key);
                }
            }
        }
    }

    protected void AddLetter(char key)
        {
            currentString += key;
            
            UI.SetNewInputText(currentString);
        }

        protected void Backspace()
        {
            char[] inputChars = currentString.ToCharArray();

            if (inputChars.Length > 0)
            {
                currentString = currentString.Substring(0, currentString.Length - 1);
            
                UI.SetNewInputText(currentString);
            }
        }

        protected void Enter()
        {
            string[] commandString = SplitStringIntoParts(currentString);

            COMMAND_TYPE commandType = ReturnCommandTypeFromString(commandString);
            
            CommandParser(commandString, commandType);

            currentString = string.Empty;
            UI.SetNewInputText(currentString);
        }

        private void CommandParser(string[] commandString, COMMAND_TYPE commandType)
        {
            switch (commandType)
            {
                case COMMAND_TYPE.MOVE_DIRECTORY:
                    if (commandString.Length > 1)
                    {
                        if (filesystem.IsFolderAvailableToMove(commandString))
                        {
                            FolderFile newFolder = filesystem.ReturnFolder(commandString);
                        
                            MoveToCommand(newFolder);
                            break;
                        }
                        else if (commandString[1] == "..")
                        {
                            FolderFile newFolder = filesystem.ReturnParentFolder();

                            MoveToParentCommand(newFolder);
                            break;
                        }
                    }
                    else
                    {
                        MoveToErrorCommand();
                        break;
                    }

                    break;
                
                case COMMAND_TYPE.LIST:
                    FolderFile[] listOfFolders = filesystem.ReturnFoldersInFolder();
                    FileData[] listOfFiles = filesystem.ReturnFilesInFolder();
                    
                    ListCommand(listOfFolders, listOfFiles);
                    break;
                
                case COMMAND_TYPE.OPEN:
                    if (filesystem.IsFileAvailableToOpen(commandString))
                    {
                        FileData newFile = filesystem.ReturnFile(commandString);
                        
                        OpenCommand(newFile);
                        break;
                    }
                    else
                    {
                        OpenErrorCommand();
                        break;
                    }
                
                case COMMAND_TYPE.HELP:
                    HelpCommand();
                    break;
                
                default:
                    ErrorCommand();
                    break;
            }
        }

        private void MoveToCommand(FolderFile folder)
        {
            if (folder != null)
            {
                UI.CloseOpenWindows();
                UI.ClearBacklogText();
                
                filesystem.MoveToFolder(folder);
                
                SetAvailableCustomCommands(folder.commands);
                
                ListCommand(filesystem.ReturnFoldersInFolder(), filesystem.ReturnFilesInFolder());

                Debug.Log("Move To Command");
            }
        }

        private void MoveToParentCommand(FolderFile folder)
        {
            if (folder != null)
            {
                UI.CloseOpenWindows();
                UI.ClearBacklogText();
                
                filesystem.MoveToFolder(folder);
                
                ListCommand(filesystem.ReturnFoldersInFolder(), filesystem.ReturnFilesInFolder());
                
                Debug.Log("Move To Parent Command");
            }
        }

        private void MoveToErrorCommand()
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();

            
            UI.SetNewBacklogText(moveErrorCommand.info.commandText);
            
            Debug.Log("Move Error Command");
        }

        private void ListCommand(FolderFile[] folders, FileData[] files)
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            string listString = String.Empty;

            for (int i = 0; i < folders.Length; i++)
            {
                listString += folders[i].name;
                listString += " - ";
                listString += folders[i].lastAccessDate;
                listString += " - ";
                listString += folders[i].sizeBytes;
                listString += "\n";
            }
            
            for (int j = 0; j < files.Length; j++)
            {
                listString += files[j].name;
                listString += " - ";
                listString += files[j].lastAccessDate;
                listString += " - ";
                listString += files[j].sizeBytes;
                listString += "\n";
            }
      
            UI.SetNewBacklogText(listString);
            
            Debug.Log("List Command");
        }

        private void OpenCommand(FileData file)
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            switch (file.type)
            {
                case FILE_TYPE.TEXT:
                    UI.LoadText(file.text);
                    break;
                
                case FILE_TYPE.IMAGE:
                    UI.LoadImage(file.image);
                    break;
            }
        }

        private void OpenErrorCommand()
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            UI.SetNewBacklogText(openErrorCommand.info.commandText);
            
            Debug.Log("Open Error Command");
        }

        private void HelpCommand()
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            UI.SetNewBacklogText(helpCommand.info.commandText);
            
            Debug.Log("Help Command");
        }
        private void ErrorCommand()
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            UI.SetNewBacklogText(errorCommand.info.commandText);
            
            Debug.Log("Error Command");
        }
        public void SetAvailableCustomCommands(Command[] newCommands)
        {
            customCommands = newCommands;
        }
        
        protected Command ReturnCommand(string[] commandString, Command[] listOfAvailableCommands)
        {
            if (commandString.Length > 1)
            {
                for (int i = 0; i < listOfAvailableCommands.Length; i++)
                {
                    if (commandString[0] == listOfAvailableCommands[i].info.commandString || commandString[0] == listOfAvailableCommands[i].info.alternativeCommandString)
                    {
                        if (commandString[1] == listOfAvailableCommands[i].arguments.rawArguments)
                        {
                            return listOfAvailableCommands[i];
                        }
                    }
                }
            }

            return null;
        }

        protected Command ReturnBasicCommand(string[] commandString)
        {
            if (commandString.Length > 1)
            {
                for (int i = 0; i < allCommands.Length; i++)
                {
                    if (commandString[0] == allCommands[i].info.commandString || commandString[0] == allCommands[i].info.alternativeCommandString)
                    {
                        if (commandString[1] == allCommands[i].arguments.rawArguments)
                        {
                            Debug.Log(commandString[1]);
                            
                            return allCommands[i];
                        }
                    }
                }
            }

            for (int j = 0; j < allCommands.Length; j++)
            {
                if (commandString[0] == allCommands[j].info.commandString || commandString[0] == allCommands[j].info.alternativeCommandString)
                {
                    if (allCommands[j].arguments.rawArguments == string.Empty)
                    {
                        return allCommands[j];
                    }
                }
            }
            
            return null;
        }

        protected string[] SplitStringIntoParts(string myString)
        {
            string[] newSplitString = myString.Split(' ');
            
            return newSplitString;
        }
        
        protected bool IsInputBasicCommand(string[] newString)
        {
            if (newString.Length > 1)
            {
                for (int i = 0; i < allCommands.Length; i++)
                {
                    if (newString[0] == allCommands[i].info.commandString || newString[0] == allCommands[i].info.alternativeCommandString)
                    {
                        if (newString[1] == allCommands[i].arguments.rawArguments)
                        {
                            return true;
                        }
                    }
                }
            }

            for (int o = 0; o < allCommands.Length; o++)
            {
                if (newString[0] == allCommands[o].info.commandString || newString[0] == allCommands[o].info.alternativeCommandString)
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool IsCustomCommandAvailable(string[] newCommand, Command[] listOfAvailableCommands)
        {
            if (newCommand.Length > 1)
            {
                for (int i = 0; i < listOfAvailableCommands.Length; i++)
                {
                    if (newCommand[0] == listOfAvailableCommands[i].info.commandString || newCommand[0] == listOfAvailableCommands[i].info.alternativeCommandString)
                    {
                        if (newCommand[1] == listOfAvailableCommands[i].arguments.rawArguments)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected COMMAND_TYPE ReturnCommandTypeFromString(string[] newString)
        {
            COMMAND_TYPE newCommandType = COMMAND_TYPE.NONE;
        
            for (int i = 0; i < allCommands.Length; i++)
            {
                if (newString[0] == allCommands[i].info.commandString || newString[0] == allCommands[i].info.alternativeCommandString)
                {
                    return allCommands[i].info.commandType;
                }
            }

            return newCommandType;
        }

        private bool IsBackspaceInput(char key)
        {
            if (key == '\b' )
            {
                return true;
            }

            return false;
        }

        private bool IsEnterInput(char key)
        {
            if ((key == '\n') || (key == '\r') )
            {
                return true;
            }

            return false;
        }
}
