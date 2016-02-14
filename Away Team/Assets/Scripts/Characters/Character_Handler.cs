using UnityEngine;
using System.Collections;


public class Character_Handler : MonoBehaviour {


    public PC_Character myChar { get; protected set; }
    public int currActionPoints { get; protected set; }
    public PathController path_controller { get; protected set; }

    int tileCoordX, tileCoordY;


    public void GetRange()
    {
        path_controller = GetComponent<PathController>();
        path_controller.SetRange(myChar.movementRange);
    }

    public void SetCharacter(PC_Character c)
    {
        myChar = c;

        currActionPoints = myChar.actionPoints;

        Debug.Log(myChar.Name + " initialized!");
        Debug.Log("HP: " + myChar.charStats.Hitpoints);
    }

    void Start()
    {
        if (path_controller == null)
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
        GetComponent<SpriteRenderer>().color = Color.white;
        GetCurrentTileCoords();
    }

    public void EndTurn()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        GetCurrentTileCoords();
    }

    public void ResetActionPoints()
    {
        currActionPoints = myChar.actionPoints;
    }

    public Vector2 GetCurrentTileCoords()
    {
        tileCoordX = Mathf.RoundToInt(transform.position.x);
        tileCoordY = Mathf.RoundToInt(transform.position.y);

        return new Vector2(tileCoordX, tileCoordY);
    }

}
