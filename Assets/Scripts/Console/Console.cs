using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public static Console instance;
    Text outputTextComponent;
    InputField inputFieldComponent;
    Animator animator;
    ScrollRect outputScrollRect;
    const int LINE_LIMIT = 200;
    bool isShown;
    List<string> inputHistory = new List<string>();
    int inputHistoryPointerIndex;

    public List<ConsoleCommand> Commands { get; set; } = new List<ConsoleCommand>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        outputTextComponent = transform.Find("OutputScrollView").Find("Viewport").Find("Content").Find("OutputText").GetComponent<Text>();
        inputFieldComponent = transform.Find("InputField").GetComponent<InputField>();
        outputScrollRect = GetComponentInChildren<ScrollRect>();
        animator = GetComponent<Animator>();
        InitCommands();
    }

    private void InitCommands()
    {
        // Help command
        Commands.Add(new ConsoleCommand("help", "Show available console commands and params", delegate
        {
            Print();
            Print("**** Available command list:");
            Print();
            foreach (ConsoleCommand consoleCommand in Commands)
            {
                Print("-" + consoleCommand.Name);
                Print("   " + consoleCommand.Description);
            }
        }));

        Commands.Add(new ConsoleCommand("jobs", "Show all jobs from job exchange", delegate
        {
            Print();
            Print("**** Jobs:");
            Print();
            if (JobExchange.instance != null)
            {
                foreach (Job job in JobExchange.instance.ItemDatabase)
                {
                    Print(job.ToString());
                }
                Print();
                Print("* Active jobs");
                Print();
                var activeJobs = (JobExchange.instance as JobExchange).ActiveJobs;
                if (activeJobs.Count == 0)
                {
                    Print("None");
                } else
                {
                    foreach (Job job in activeJobs)
                    {
                        Print(job.ToString());
                    }
                }
                    
                
            } else
            {
                Print("Job exchange is not instantiated. Are you in playing state?");
            }
        }));
    }

    public static void Print(string msg)
    {
        instance.outputTextComponent.text += "\n" + msg;
        HandleLineLimit();
    }

    public static void Print()
    {
        instance.outputTextComponent.text += "\n";
        HandleLineLimit();
    }

    static void HandleLineLimit()
    {
        var numberOfLines = instance.outputTextComponent.text.Count(c => c.Equals('\n')) + 1;
        if (numberOfLines > LINE_LIMIT)
        {
            instance.outputTextComponent.text = instance.outputTextComponent.text.Substring(instance.outputTextComponent.text.IndexOf('\n') + 1);
        }
    }

    public static void Show()
    {
        instance.inputFieldComponent.ActivateInputField();
        instance.animator.SetBool("IsShown", true);
        instance.isShown = true;
    }
    public static void Hide()
    {
        instance.inputFieldComponent.DeactivateInputField();
        instance.animator.SetBool("IsShown", false);
        instance.isShown = false;

    }
    public static void Toggle()
    {
        if (instance.isShown)
        {
            Hide();
        } else
        {
            Show();
        }
    }


    private void Update()
    {
        if (instance.isShown)
        { 
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Sumbit(inputFieldComponent.text);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (inputHistoryPointerIndex > 0)
                {
                    inputHistoryPointerIndex -= 1;
                    inputFieldComponent.text = inputHistory[inputHistoryPointerIndex];
                }
                inputFieldComponent.caretPosition = inputFieldComponent.text.Length;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (inputHistoryPointerIndex < inputHistory.Count - 1)
                {
                    inputHistoryPointerIndex += 1;
                    inputFieldComponent.text = inputHistory[inputHistoryPointerIndex];
                }
                inputFieldComponent.caretPosition = inputFieldComponent.text.Length;
            }
        }
    }

    private void Sumbit(string msg)
    {
        ConsoleCommand cmd = GetCommand(msg);
        inputHistory.Add(msg);
        inputHistoryPointerIndex = inputHistory.Count;
        Print("> " + msg);
        if (cmd != null)
        {
            cmd.Execute();
        } else
        {
            Print("Invalid command. Type -help for list of commands.");
        }

        inputFieldComponent.text = "";
        inputFieldComponent.ActivateInputField();
        instance.outputScrollRect.verticalNormalizedPosition = 0;
    }

    private ConsoleCommand GetCommand(string msg)
    {
        foreach (ConsoleCommand consoleCommand in Commands)
        {
            if (("-" + consoleCommand.Name) == msg)
            {
                return consoleCommand;
            }
        }
        return null;
    }
}
