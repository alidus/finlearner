using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System;

[CreateAssetMenu(menuName = "SO/Education/Certificate", fileName = "Certificate")]
public class Certificate : Item, IEquipable
{
    public EducationDirection EducationDirection;
    public EducationEntityType EducationEntityType;

    public bool CanBeEquipped { get; set; }

    public bool IsEquipped { get; private set; }

    public event Action OnEquipStateChanged;
    public event Action OnEquippableStateChanged;
    public event EquippableInstanceHandler OnInstanceEquipStateChanged;
    public event EquippableInstanceHandler OnInstanceEquippableStateChanged;


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

    public void Equip()
    {
        IsEquipped = true;
        OnEquipStateChanged?.Invoke();
    }

    public void NotifyOnInstanceEquippableStateChanged(IEquipable equipable)
    {
        OnInstanceEquippableStateChanged?.Invoke(equipable);
    }

    public void NotifyOnInstanceEquipStateChanged(IEquipable equipable)
    {
        OnInstanceEquipStateChanged?.Invoke(equipable);
    }

    public void Uneqip()
    {
        IsEquipped = false;
        OnEquipStateChanged?.Invoke();
    }
}
