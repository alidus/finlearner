using UnityEngine;
using System.Collections;

public enum EduDegreeLevel { Bachelor, Specialist, Master }

[CreateAssetMenu(menuName = "SO/Education/Degree", fileName = "EducationDegree")]
public class EduDegree : EducationEntity
{
    public EduDegreeLevel EduDegreeLevel;
}
