using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public string title;
    public string desc;
    public List<Modifier> modifiers;
    public Job(JobSOTemplate template)
    {
        title = template.title;
        desc = template.title;
        modifiers = template.modifiers;
    }
}
