using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DAO
{
    public const string InvalidFromJson = "Could not obtain type '{0}' from requested URL.";
}

[Serializable]
public class BlockDAO : DAO
{
    public int id;
    public string subject;
    public string grade;
    public int mastery;
    public string domainid;
    public string domain;
    public string cluster;
    public string standardid;
    public string standarddescription;
}
