using UnityEngine;
using System.Collections;
using System;

public class Character_Handler : MonoBehaviour {


    public PC_Character myChar { get; protected set; }
    public int currActionPoints { get; protected set; }
    public PathController path_controller { get; protected set; }



    public void SetCharacter(PC_Character c)
    {
        myChar = c;

        currActionPoints = myChar.actionPoints;

        Debug.Log(myChar.Name + " initialized!");
        Debug.Log("HP: " + myChar.charStats.Hitpoints);
    }

    void Start()
    {
        path_controller = GetComponent<PathController>();
    }

    public void TakeAP()
    {
        currActionPoints--;

        // Display current action points. This should work correctly since we are only calling Take AP on the seleted unit.
        UI_Manager.Instance.DisplayCharacterAP(currActionPoints);
    }

    public void TakeDamage(float damage)
    {
        myChar.charStats.Hitpoints -= damage;
    }

    public void StartTurn()
    {
        ResetActionPoints();
        path_controller.ResetMovementRange();
        path_controller.StopMovement();
    }

    public void EndTurn()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void ResetActionPoints()
    {
        currActionPoints = myChar.actionPoints;
    }

}
