using UnityEngine;
using System.Collections;

public enum NPCClass
{
    Grunt,
    Ranged_Grunt,
    Captain,
    Commander,
    General,
    Boss
}

public class NPC_Character : Character {

    public NPCClass npc_class { get; protected set; }

    public NPC_Character(string n, NPCClass _class)
    {
        Name = n;
        npc_class = _class;
    }
}
