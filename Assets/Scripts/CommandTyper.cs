using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTyper : MonoBehaviour
{
    [Header("Input Attributes")] 
    private string currentString;
    private char lastChar;
    
    [SerializeField]
    private bool canInput = true;

    [Header("Command Attributes")] 
    public CommandList allCommands;
    
    [Space(10)]
    public CommandInfo[] possibleCommands;
    
    [Space(10)]
    public CommandInfo lastCommandEntered;

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
        currentString = "";

        possibleCommands = allCommands.commandList;
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
        
        Debug.Log(currentString);
    }

    protected void Backspace()
    {
        char[] inputChars = currentString.ToCharArray();

        if (inputChars.Length > 0)
        {
            currentString = currentString.Substring(0, currentString.Length - 1);
            
            Debug.Log(currentString);
        }
    }

    protected void Enter()
    {
        if (IsInputCommand(currentString))
        {
            if (IsCommandAvailable(currentString, possibleCommands))
            {
                Debug.Log("Command");
            }
        }
        
        currentString = string.Empty;
    }
    
    protected void EnterCommand(CommandInfo commandInfo, CommandArguments commandArguments)
    {
        
    }

    protected CommandInfo ReturnLastCommand()
    {
        return lastCommandEntered;
    }

    protected bool IsInputCommand(string newString)
    {
        for (int i = 0; i < allCommands.commandList.Length; i++)
        {
            if (newString == allCommands.commandList[i].commandString || newString == allCommands.commandList[i].alternativeString)
            {
                return true;
            }
        }

        return false;
    }
    
    protected bool IsCommandAvailable(string newString, CommandInfo[] listOfAvailableCommands)
    {
        for (int i = 0; i < listOfAvailableCommands.Length; i++)
        {
            if (newString == listOfAvailableCommands[i].commandString)
            {
                return true;
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

[System.Serializable]
public struct CommandList
{
    [Header("Command List")] 
    public CommandInfo[] commandList;
}

[System.Serializable]
public struct CommandInfo
{
    [Header("Command Info Attributes")] 
    public string name;
    
    [Space(10)]
    public COMMAND_TYPE commandType;

    [Space(10)] 
    public string commandString;
    public string alternativeString;
}

[System.Serializable]
public struct CommandArguments
{
    [Header("Command Arguments")] 
    public string rawArguments;
}

[System.Serializable]
public enum COMMAND_TYPE
{
    NONE,
    MOVE_DIRECTORY,
    LIST,
    OPEN,
}
