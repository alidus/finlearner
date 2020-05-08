using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

[CreateAssetMenu(menuName = "SO/Education/Certificate", fileName = "Certificate")]
public class Certificate : Item
{
    public EducationDirection EducationDirection;
    public EducationEntityType EducationEntityType;


    /// <summary>
    /// Check if list of given certificates fulfill object demands for them
    /// </summary>
    /// <param name="listOfCertificatesToProvide"></param>
    /// <param name="entityThatDemandCertificates"></param>
    /// <returns></returns>
    public static bool SertificateCheck(Collection<Certificate> listOfCertificatesToProvide,IDemandCertificate entityThatDemandCertificates)
    {
        var result = !entityThatDemandCertificates.MandatoryCertificates.Except(listOfCertificatesToProvide).Any();
        return result;
    }
}
