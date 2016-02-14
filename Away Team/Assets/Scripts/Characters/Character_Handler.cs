using UnityEngine;
using System.Collections;


public class Character_Handler : MonoBehaviour {


    public PC_Character myChar { get; protected set; }
    public int currActionPoints { get; protected set; }
    public PathController path_controller { get; protected set; }

    int tileCoordX, tileCoordY;

    // Record the Position of this character as a Vector2 when:
    // 1 - During Spawn
    // 2 - At End of Turn

    // Keep track of the Position at the Start of Turn and at the End remove it to be replaced with a new position
    Vector2 currBattlePosition;

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

        // Set the starting Battle Position
        SetBattlePosition(transform.position);
    }

    public void EndTurn()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        GetCurrentTileCoords();

        // Re Set the ending Battle Position
        SetBattlePosition(transform.position);
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

    public void SetBattlePosition(Vector2 pos)
    {
        if (currBattlePosition != null)
        {
            // If != null there was a position set before this was called.

            // Give the battle controller the last position and the current position...
            Battle_Controller.Instance.ReSetPCPosition(currBattlePosition, pos, this);

            // ...then store the current position to later pass it to the Battle Controller as the last position.
            currBattlePosition = pos;
        }
        else
        {
            // If = null then this is the first time setting a position.

            // Pass the new position to the Battle Controller ...
            Battle_Controller.Instance.SetNewPCPosition(pos, this);

            // ... and store the new position as the current position.
            currBattlePosition = pos;
        }

     
    }

}
