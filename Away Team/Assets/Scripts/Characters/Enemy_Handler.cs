using UnityEngine;
using System.Collections;

public class Enemy_Handler : MonoBehaviour {

    public NPC_Character myChar { get; protected set; }
    public int currActionPoints { get; protected set; }
    public PathController path_controller { get; protected set; }

    int myEnemyIndex;

    int tileCoordX, tileCoordY;

    Enemy_BattleController battle_control;

    Vector2 currBattlePosition;

    public void GetRange()
    {
        path_controller = GetComponent<PathController>();
        path_controller.SetRange(myChar.movementRange);
    }

    public void SetCharacter(NPC_Character c, int index)
    {
        myChar = c;

        currActionPoints = myChar.actionPoints;

        myEnemyIndex = index;

        Debug.Log(myChar.Name + " initialized!");
        Debug.Log("HP: " + myChar.charStats.Hitpoints);
    }

    void Start()
    {
        if (path_controller == null)
            path_controller = GetComponent<PathController>();
    }


    
    public void StartTurn()
    {
        battle_control = Enemy_BattleController.Instance;

        // First set the current tile coordinates:
        GetCurrentTileCoords();

        // Reset the Sprite's color
        GetComponent<SpriteRenderer>().color = Color.white;

        // Reset Action Points
        ResetActionPoints();

        // Set Battle Position at start
        SetBattlePosition(transform.position);

        StartThinkingNextDecision();

    }

    public void StartThinkingNextDecision()
    {

        StartCoroutine("Thinking");
    }

    IEnumerator Thinking()
    {
        while (true)
        {
            Debug.Log("Enemy is THINKING...");

            yield return new WaitForSeconds(2f);

            if (currActionPoints > 1)
            {
                FirstAction();
            }
            else
            {
                SecondAction();
            }

            yield break;
            
        }
    }

    void FirstAction()
    {
        
        //if (battle_control.currTarget != null)
        //{
        //    var heading = battle_control.currTarget.transform.position - transform.position;
        //    var distance = heading.magnitude;
        //    var direction = heading / distance;
        //    Debug.Log("Distance " + distance);
        //    Debug.Log("Direction = " + direction);
        //}
    

        // Attack if any players are currently in range
        if (battle_control.CheckForPlayers(tileCoordX, tileCoordY, myChar.attackRange))
        {
            // Attack the nearest player ...
            battle_control.EnemyRangedAttack(10);
        }
        // If can hit during second action
        else if(battle_control.currTarget != null)
        {
            // Move to closest position to be in range for second round.
            battle_control.EnemyMoveKnownTarget();
        }
        else
        {
            // Move to a desirable location
            battle_control.EnemyMoveToSpecificPath(transform.position + Vector3.down);
        }
    }

    public void SecondAction()
    {
        // Attack if any players are currently in range
        if (battle_control.CheckForPlayers(tileCoordX, tileCoordY, myChar.attackRange))
        {
            // Attack the nearest player ...
            battle_control.EnemyRangedAttack(10);
        }
        else if (battle_control.currTarget != null)
        {
            // Move to closest position to be in range for second round.
            battle_control.EnemyMoveKnownTarget();
        }
        else
        {
            // Move to a desirable location
            battle_control.EnemyMoveToSpecificPath(transform.position + Vector3.down);
        }
    }


    bool CheckIfTargetInRangeOnSecondMove(float distance)
    {
        if ((distance - myChar.attackRange) < myChar.movementRange * 2)
        {
            return true;
        }
        return false;
    }


    float DistanceToTarget()
    {
        if (battle_control.currTarget != null && battle_control.currTarget.gameObject.activeSelf)
        {
            return (battle_control.currTarget.transform.position - transform.position).sqrMagnitude;
      
        }

        return 1000;
    }


    public void TakeAP()
    {
        currActionPoints--;
    }
    public void ResetActionPoints()
    {
        currActionPoints = myChar.actionPoints;
    }

    public void TakeDamage(float damage)
    {
        myChar.charStats.Hitpoints -= damage;

        if (myChar.charStats.Hitpoints <= 0)
        {

        }
    }

    public void EndTurn()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        GetCurrentTileCoords();

        // Re Set the ending Battle Position
        SetBattlePosition(transform.position);
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
            Battle_Controller.Instance.ReSetNPCPosition(currBattlePosition, pos, this);

            // ...then store the current position to later pass it to the Battle Controller as the last position.
            currBattlePosition = pos;
        }
        else
        {
            // If = null then this is the first time setting a position.

            // Pass the new position to the Battle Controller ...
            Battle_Controller.Instance.SetNewNPCPosition(pos, this);

            // ... and store the new position as the current position.
            currBattlePosition = pos;
        }


    }
}
