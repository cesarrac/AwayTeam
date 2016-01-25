using UnityEngine;
using System.Collections;

public enum PCClass
{
    Wildshot,
    Technobrat
}

public class PC_Character : Character {

    public PCClass pc_class { get; protected set; }

    public PC_Character(string name, PCClass _class)
    {
        Name = name;
        pc_class = _class;
    }
}
