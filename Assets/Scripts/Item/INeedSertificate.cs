using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDemandCertificate
{
    List<Certificate> MandatoryCertificates { get; set; }
}
