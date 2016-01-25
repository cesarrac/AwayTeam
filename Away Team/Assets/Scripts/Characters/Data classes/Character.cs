using UnityEngine;
using System.Collections;
using System;


public class Stats
{
    float hitPoints;
    int strength;
    int perception;
    int will;
    int reflex;
    int intelligence;

    public float Hitpoints { get { return hitPoints; } set { hitPoints = value; } }
    public int Strength { get { return strength; } set { strength = value; } }
    public int Perception { get { return perception; } set { perception = value; } }
    public int Will { get { return will; } set { will = value; } }
    public int Reflex { get { return reflex; } set { reflex = value; } }
    public int Intelligence { get { return intelligence; } set { intelligence = value; } }

    public Stats(float hp, int str, int per, int _will, int refl, int intl)
    {
        hitPoints = hp;
        strength = str;
        perception = per;
        will = _will;
        reflex = refl;
        intelligence = intl;
    }
}

public class Character {

    string name;
    public string Name { get { return name; } set { name = value; } }

    public Stats charStats { get; protected set; }
    bool statsInitialized;

    public int actionPoints { get; protected set; }
    public int movementRange { get; protected set; }



    //public Character (string n, CharacterType t)
    //{
    //    name = n;
    //    charType = t;
    //}

    public void InitStats(float hp, int str, int per, int _will, int refl, int intl)
    {
        if (!statsInitialized)
        {
            statsInitialized = true;
            charStats = new Stats(hp, str, per, _will, refl, intl);
            actionPoints = 2;

            // FIX THIS: Right now I'm giving everyone a hardcoded int for move range. This represents 5 tile spaces that they can move.
            movementRange = 5;
        }

    }

    public Stats GetStats()
    {
        return charStats;
    }



}
