using UnityEngine;
using System.Collections;

public class Enemy_Handler : MonoBehaviour {

    public NPC_Character myChar { get; protected set; }
    public int currActionPoints { get; protected set; }
    public PathController path_controller { get; protected set; }



    public void SetCharacter(NPC_Character c)
    {
        myChar = c;

        currActionPoints = myChar.actionPoints;

        Debug.Log(myChar.Name + " initialized!");
        Debug.Log("HP: " + myChar.charStats.Hitpoints);
    }

    public void ResetActionPoints()
    {
        currActionPoints = myChar.actionPoints;
    }


    void Start()
    {
        path_controller = GetComponent<PathController>();
    }

    public void TakeAP()
    {
        currActionPoints--;
    }


    public void TakeDamage(float damage)
    {
        myChar.charStats.Hitpoints -= damage;
    }
}
