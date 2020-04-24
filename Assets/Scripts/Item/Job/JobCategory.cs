using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "SO/Items/JobCategory", fileName = "JobCategory")]
public class JobCategory : ScriptableObject
{
    [SerializeField]
    private string title;
    public string Title { get => title; set => title = value; }
}
