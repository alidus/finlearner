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
    public bool IsShown;
    List<string> inputHistory = new List<string>();
    int inputHistoryPointerIndex;
    Button submitButton;

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
        var inputPanel = transform.Find("InputPanel");
        inputFieldComponent = inputPanel.Find("InputField").GetComponent<InputField>();
        submitButton = inputPanel.Find("SubmitButton").GetComponent<Button>();
        outputScrollRect = GetComponentInChildren<ScrollRect>();
        animator = GetComponent<Animator>();
        InitCommands();

        submitButton.onClick.AddListener(delegate { Sumbit(inputFieldComponent.text); });
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

        Commands.Add(new ConsoleCommand("edu", "Show available education entities", delegate
        {
            Print();
            Print("**** Education entities:");
            Print();
            var counter = 1;
            foreach (EducationEntity entity in (EducationHub.instance as EducationHub).ItemDatabase)
            {
                Console.Print(counter.ToString() + ") " + entity.ToString());
                var sertificateCheck = Certificate.SertificateCheck((EducationHub.instance as EducationHub).Certificates, entity);
                Console.Print("Certificate check: " + sertificateCheck);
                if (!sertificateCheck)
                {
                    Console.Print("__________Demand certificates:");
                    foreach (Certificate certificate in entity.MandatoryCertificates)
                    {
                        Console.Print("______________________(" + ((EducationHub.instance as EducationHub).Certificates.Contains(certificate) ? "✓" : "X") + ") " + certificate.ToString());
                    }
                    Console.Print();
                }
                counter++;
            }
        }));

        Commands.Add(new ConsoleCommand("time", "Show information about time and time management", delegate
        {
            Print("Current date: " + GameDataManager.instance.CurrentDateTime);
            Print();
            Print("Default free hours in a week: " + GameDataManager.TOTAL_FREE_HOURS_IN_A_WEEK);
            Print("Current free hours in a week: " + GameDataManager.instance.FreeHoursOfWeekLeft);
        }));

        Commands.Add(new ConsoleCommand("cert", "Show available and owned certificates", delegate
        {
            Print();
            Print("**** Certificates:");
            Print();
            var counter = 0;
            foreach (Certificate certificate in Resources.LoadAll("ScriptableObjects/Education/Certificates", typeof(Certificate)).ToList().ConvertAll(go => (Certificate)go))
            {
                Print(counter.ToString() + ") " + certificate.ToString());
                counter++;
            }
            Print("** Owned by player:");
            counter = 1;
            foreach (Certificate certificate in (EducationHub.instance as EducationHub).Certificates)
            {
                Print(counter.ToString() + ") " + certificate.ToString());
                counter++;
            }
        }));

        // Jobs
        Commands.Add(new ConsoleCommand("jobs", "Show all jobs from job exchange", delegate
        {
            Print();
            Print("**** Jobs:");
            Print();
            if (JobExchange.instance != null)
            {
                Print("Jobs count: " + JobExchange.instance.ItemDatabase.Count.ToString());

                var counter = 1;
                Print(JobExchange.instance.ItemDatabase.ToString());
                Print(JobExchange.instance.ItemDatabase[0].ToString());
                foreach (Job job in (JobExchange.instance as JobExchange).ItemDatabase)
                {
                    Print(job.ToString());
                    if (job is null)
                    {
                        Print("job is null");
                    }
                    Print(counter.ToString() + ") " + job.ToString());
                    counter++;
                }
                Print();
                Print("* Active jobs");
                Print();

                var activeJobs = (JobExchange.instance as JobExchange).ActiveJobs;
                Print("Active jobs count: " + activeJobs.Count.ToString());

                counter = 1;
                foreach (Job job in activeJobs)
                {
                   
                    Print(counter.ToString() + ") " + job.ToString());
                    counter++;

                }


            } else
            {
                Print("Job exchange is not instantiated. Are you in playing state?");
            }
        }));
        Commands.Add(new ConsoleCommand("showhint", "Show hint for debug purposes", delegate
        {
            HintsManager.instance.ShowHint("DEBUG HINT", "THIS HINT WAS CREATED BY CONSOLE COMMAND");
        }));

        Commands.Add(new ConsoleCommand("se", "Show list of active status effects", delegate
        {
            Print("*** Status effects:");
            if (StatusEffectsManager.instance.StatusEffects.Count == 0)
            {
                Print("None");
            } else
            {
                foreach (StatusEffect se in StatusEffectsManager.instance.StatusEffects)
                {
                    Print(se.ToString());
                }
            }
        }));
    }

    public static void Print(string msg)
    {
        if (instance != null)
        {
            instance.outputTextComponent.text += "\n" + msg;
            HandleLineLimit();
        }
    }

    public static void Print()
    {
        if (instance != null)
        {
            instance.outputTextComponent.text += "\n";
            HandleLineLimit();
        }
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
        instance.IsShown = true;
    }
    public static void Hide()
    {
        instance.inputFieldComponent.DeactivateInputField();
        instance.animator.SetBool("IsShown", false);
        instance.IsShown = false;

    }
    public static void Toggle()
    {
        if (instance.IsShown)
        {
            Hide();
        } else
        {
            Show();
        }
    }


    private void Update()
    {
        if (instance.IsShown)
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
        StartCoroutine(ResetOutputScrollViewPosition());
    }

    IEnumerator ResetOutputScrollViewPosition()
    {
        yield return null;

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
