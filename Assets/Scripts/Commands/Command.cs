using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "Command/NewCommand", order = 1)]
public abstract class Command : ScriptableObject
{
    public CommandInfo info;

    [Space(10)] 
    public CommandArguments arguments;
    
    public abstract void InitializeCommand();
    public abstract void ExecuteCommand();
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
    public string alternativeCommandString;

    [Space(10)] 
    [TextArea(15, 50)]
    public string commandText;
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
    HELP,
    ERROR
}
