using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [Header("UI Attributes")] 
    private UIManager UI;
    
    [Header("Command Attributes")] 
    public Command[] allCommands;
    
    [Space(10)]
    public Command[] customCommands;

    [Space(10)] 
    public Command errorCommand;
    
    [Space(10)]
    public Command lastCommandEntered;
    
    [Header("Input Attributes")] 
    private string currentString;

    private char lastChar;
    
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
            string[] newSplitString = SplitStringIntoParts(currentString);
            
            if (IsCustomCommandAvailable(newSplitString, customCommands))
            {
                Command newCommand = ReturnCommand(newSplitString, customCommands);
                    
                EnterCommand(newCommand);
            }

            COMMAND_TYPE newCommandType = ReturnCommandTypeFromString(newSplitString);

            if (newCommandType != COMMAND_TYPE.NONE)
            {
                Command newBasicCommand = ReturnBasicCommand(newSplitString);
                
                EnterBasicCommand(newBasicCommand);
            }
            else
            {
                EnterBasicCommand(errorCommand);
            }

            currentString = string.Empty;
            UI.SetNewInputText(currentString);
        }
    
        protected void EnterCommand(Command command)
        {
            UI.CloseOpenWindows();

            UI.SetNewBacklogText(command.info.commandText);
            
            command.InitializeCommand();
            command.ExecuteCommand();
        }

        protected void EnterBasicCommand(Command command)
        {
            UI.CloseOpenWindows();
            
            UI.SetNewBacklogText(command.info.commandText);
            
            command.InitializeCommand();
            command.ExecuteCommand();
        }

        public void SetAvailableCommands(Command[] newCommands)
        {
            customCommands = newCommands;
        }
        
        protected Command ReturnLastCommand()
        {
            return lastCommandEntered;
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
                            return allCommands[i];
                        }
                    }
                }
            }

            for (int o = 0; o < allCommands.Length; o++)
            {
                if (commandString[0] == allCommands[o].info.commandString || commandString[0] == allCommands[o].info.alternativeCommandString)
                {
                    return allCommands[o];
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
