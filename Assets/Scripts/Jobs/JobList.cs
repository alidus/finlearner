using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobList", menuName = "ScriptableObjects/Job/JobList")]
public class JobList : ScriptableObject
{
    [SerializeField]
    private List<Job> jobs = new List<Job>();
	public System.Collections.Generic.List<Job> Jobs
	{
		get { return jobs; }
	}
}
