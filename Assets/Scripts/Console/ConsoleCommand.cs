using UnityEngine;
using System.Collections;
using System;

public class ConsoleCommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Action ExecuteAction;

    public ConsoleCommand(string name, string description, Action executeAction)
    {
        Name = name;
        Description = description;
        ExecuteAction = executeAction;
    }

    public void Execute()
    {
        ExecuteAction();
    }
}
