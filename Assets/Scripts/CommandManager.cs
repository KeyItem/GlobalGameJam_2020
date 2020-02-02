using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [Header("levelManager Manager")] 
    private LevelManager levelManager;
    
    [Header("Audio Attributes")] 
    public AudioManager audio;
    
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
    public Command openPasswordErrorCommand;
    public Command errorCommand;
    
    [Space(10)]
    public Command lastCommandEntered;

    [Space(10)] 
    public FileData finalFile;
    
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
        audio = GetComponent<AudioManager>();
        levelManager = GetComponent<LevelManager>();
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
            
            audio.StartSFX();
        }

        protected void Backspace()
        {
            char[] inputChars = currentString.ToCharArray();

            if (inputChars.Length > 0)
            {
                currentString = currentString.Substring(0, currentString.Length - 1);
            
                UI.SetNewInputText(currentString);
            }
            
            audio.StartSFX();
        }

        protected void Enter()
        {
            string[] commandString = SplitStringIntoParts(currentString);

            COMMAND_TYPE commandType = ReturnCommandTypeFromString(commandString);

            if (IsCustomCommandAvailable(commandString, customCommands))
            {
                Command customCommand = ReturnCustomCommand(commandString, customCommands);
                
                customCommand.ExecuteCommand();
            }
            else
            {
                CommandParser(commandString, commandType);
            }

            currentString = string.Empty;
            UI.SetNewInputText(currentString);
            
            audio.StartSFX();
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
                        
                            MoveToCommand(newFolder, commandString);
                            break;
                        }
                        else if (commandString[1].Equals("..", StringComparison.OrdinalIgnoreCase))
                        {
                            FolderFile newFolder = filesystem.ReturnParentFolder();

                            MoveToParentCommand(newFolder, commandString);
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
                    
                    ListCommand(listOfFolders, listOfFiles, commandString);
                    break;
                
                case COMMAND_TYPE.OPEN:
                    if (filesystem.IsFileAvailableToOpen(commandString))
                    {
                        FileData newFile = filesystem.ReturnFile(commandString);
                        
                        OpenCommand(newFile, commandString);
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
                
                case COMMAND_TYPE.QUIT:
                    QuitCommand();
                    break;
                
                default:
                    ErrorCommand();
                    break;
            }
        }

        private void MoveToCommand(FolderFile folder, string[] commandString)
        {
            if (folder != null)
            {
                UI.CloseOpenWindows();
                UI.ClearBacklogText();
                
                filesystem.MoveToFolder(folder);
                
                SetAvailableCustomCommands(folder.commands);
                
                ListCommand(filesystem.ReturnFoldersInFolder(), filesystem.ReturnFilesInFolder(), commandString);

                Debug.Log("Move To Command");
            }
        }

        private void MoveToParentCommand(FolderFile folder, string[] commandString)
        {
            if (folder != null)
            {
                UI.CloseOpenWindows();
                UI.ClearBacklogText();
                
                filesystem.MoveToFolder(folder);
                
                ListCommand(filesystem.ReturnFoldersInFolder(), filesystem.ReturnFilesInFolder(), commandString);
                
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

        private void ListCommand(FolderFile[] folders, FileData[] files, string[] commandString)
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

        private void OpenCommand(FileData file, string[] commandString)
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            switch (file.type)
            {
                case FILE_TYPE.TEXT:
                    if (file.hasPassword)
                    {
                        if (file.DoesInputMatchPassword(commandString))
                        {
                            if (file.name == finalFile.name)
                            {
                                levelManager.GoToNextScene();
                            }
                            
                            UI.LoadText(file.text);
                            
                            Debug.Log("Opening text file :: " + file.name);
                            break;
                        }
                        
                        OpenPasswordErrorCommand();
                    }
                    else
                    {
                        UI.LoadText(file.text);
                        
                        Debug.Log("Opening text file :: " + file.name);
                    }
                    
                    break;
                
                case FILE_TYPE.IMAGE:
                    if (file.hasPassword)
                    {
                        if (file.DoesInputMatchPassword(commandString))
                        {
                            if (file.name == finalFile.name)
                            {
                                levelManager.GoToNextScene();
                            }

                            UI.LoadImage(file.image);
                            
                            Debug.Log("Opening image file :: " + file.name);
                            break;
                        }
                        
                        OpenPasswordErrorCommand();
                    }
                    else
                    {
                        UI.LoadImage(file.image);
                        
                        Debug.Log("Opening image file :: " + file.name);
                    }                    
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

        private void OpenPasswordErrorCommand()
        {
            UI.CloseOpenWindows();
            UI.ClearBacklogText();
            
            UI.SetNewBacklogText(openPasswordErrorCommand.info.commandText);
            
            Debug.Log("Open Password Error Command");
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

        private void QuitCommand()
        {
            Debug.Log("Quit Command");

            Application.Quit();
        }
        public void SetAvailableCustomCommands(Command[] newCommands)
        {
            customCommands = newCommands;
        }
        
        protected Command ReturnCustomCommand(string[] commandString, Command[] listOfAvailableCommands)
        {
            if (commandString.Length > 1)
            {
                for (int i = 0; i < listOfAvailableCommands.Length; i++)
                {
                    if (commandString[0].Equals(listOfAvailableCommands[i].info.commandString, StringComparison.OrdinalIgnoreCase)  || commandString[0].Equals(listOfAvailableCommands[i].info.alternativeCommandString, StringComparison.OrdinalIgnoreCase))
                    {
                        if (commandString[1].Equals(listOfAvailableCommands[i].arguments.rawArguments, StringComparison.OrdinalIgnoreCase))
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
                    if (commandString[0].Equals(allCommands[i].info.commandString, StringComparison.OrdinalIgnoreCase) || commandString[0].Equals(allCommands[i].info.alternativeCommandString, StringComparison.OrdinalIgnoreCase))
                    {
                        if (commandString[1].Equals(allCommands[i].arguments.rawArguments, StringComparison.OrdinalIgnoreCase))
                        {
                            Debug.Log(commandString[1]);
                            
                            return allCommands[i];
                        }
                    }
                }
            }

            for (int j = 0; j < allCommands.Length; j++)
            {
                if (commandString[0].Equals(allCommands[j].info.commandString, StringComparison.OrdinalIgnoreCase) || commandString[0].Equals(allCommands[j].info.alternativeCommandString, StringComparison.OrdinalIgnoreCase))
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
            if (listOfAvailableCommands.Length > 0)
            {
                if (newCommand.Length > 1)
                {
                    for (int i = 0; i < listOfAvailableCommands.Length; i++)
                    {
                        if (newCommand[0].Equals(listOfAvailableCommands[i].info.commandString, StringComparison.OrdinalIgnoreCase) || newCommand[0].Equals(listOfAvailableCommands[i].info.alternativeCommandString, StringComparison.OrdinalIgnoreCase))
                        {
                            if (newCommand[1].Equals(listOfAvailableCommands[i].arguments.rawArguments, StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
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
                if (newString[0].Equals(allCommands[i].info.commandString, StringComparison.OrdinalIgnoreCase) || newString[0].Equals(allCommands[i].info.alternativeCommandString, StringComparison.OrdinalIgnoreCase))
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
