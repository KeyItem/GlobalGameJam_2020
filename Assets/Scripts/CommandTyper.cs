using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTyper : MonoBehaviour
{
    [Header("UI Attributes")] 
    private UIManager UI;
    
    [Header("Command Attributes")] 
    public Command[] allCommands;
    
    [Space(10)]
    public Command[] possibleCommands;

    [Space(10)] 
    public Command errorCommand;
    
    [Space(10)]
    public Command lastCommandEntered;
    
    [Header("Input Attributes")] 
    private string currentString;
    private string currentFunctionString;
    private string currentArguementString;
    
    private char lastChar;
    
    [SerializeField]
    private bool canInput = true;

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
            
            if (IsInputBasicCommand(newSplitString))
            {
                Command newBasicCommand = ReturnBasicCommand(newSplitString);
                    
                EnterBasicCommand(newBasicCommand);
            }
            else if (IsInputAvailableCommand(newSplitString, possibleCommands))
            {
                Command newCommand = ReturnCommand(newSplitString, possibleCommands);
                    
                EnterCommand(newCommand);
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
            UI.SetNewBacklogText(command.info.commandText);
            
            command.InitializeCommand();
            command.ExecuteCommand();
        }

        protected void EnterBasicCommand(Command command)
        {
            UI.SetNewBacklogText(command.info.commandText);
            
            command.InitializeCommand();
            command.ExecuteCommand();
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
            for (int i = 0; i < allCommands.Length; i++)
            {
                if (commandString[0] == allCommands[i].info.commandString || commandString[0] == allCommands[i].info.alternativeCommandString)
                {
                    return allCommands[i];
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
            for (int i = 0; i < allCommands.Length; i++)
            {
                if (newString[0] == allCommands[i].info.commandString || newString[0] == allCommands[i].info.alternativeCommandString)
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool IsInputAvailableCommand(string[] newCommand, Command[] listOfAvailableCommands)
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
    
        protected CommandInfo ReturnCommandFromString(string newString, CommandInfo[] listOfAvailableCommands)
        {
            CommandInfo newCommand = new CommandInfo();
        
            for (int i = 0; i < listOfAvailableCommands.Length; i++)
            {
                if (newString == listOfAvailableCommands[i].commandString)
                {
                    newCommand = listOfAvailableCommands[i];
                }
            }

            return newCommand;
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
