using UnityEngine;
using System.Collections;

public class Player_BattleController : MonoBehaviour {

    public static Player_BattleController Instance { get; protected set; }

    Enemy_Handler currTargetEnemy;

    GameObject pathEnd_cursor;

    Battle_Controller battle_Control;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        battle_Control = Battle_Controller.Instance;
    }

    public void StartPathCursor()
    {
        StartCoroutine("PathCursorDisplay");
    }

    IEnumerator PathCursorDisplay()
    {
        while (true)
        {
            Mouse_Controller.Instance.DisplayPathCursor();

            yield return null;
        }
    }

    // Attack Controls
    public void PlayerRangedAttack(float damage)
    {
        if (battle_Control.selectedCharacter != null)
        {
            // Stop displaying cursor
            StopCoroutine("PathCursorDisplay");
            if (pathEnd_cursor != null)
            {
                ObjectPool.instance.PoolObject(pathEnd_cursor);
            }

            // Do Damage
            if (Mouse_Controller.Instance.EnemyUnderMouse != null)
            {
                currTargetEnemy = Mouse_Controller.Instance.EnemyUnderMouse;
                DamageEnemy(10, currTargetEnemy);
            }

            // Subtract AP
            Player_ChargeActionPoints();

            // Set a new Enemy Target if it doesn't have one already
            Enemy_BattleController.Instance.SetTarget(battle_Control.selectedCharacter);

            // End Action to check for end of turn
            EndAction();
        }


    }

    void DamageEnemy(float damage, Enemy_Handler enemy)
    {
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            enemy.TakeDamage(damage);
            Debug.Log(enemy.myChar.Name + " takes " + damage + " damage!");
        }

    }

    public void PlayerMove(Vector3 pathTarget)
    {
        if (battle_Control.selectedPathController != null && battle_Control.selectedCharacter != null)
        {
            if (battle_Control.selectedCharacter.currActionPoints > 0 && battle_Control.selectedPathController.CurrMovePoints > 0)
            {
                battle_Control.selectedPathController.RequestPath(pathTarget);

                // Stop displaying cursor
                StopCoroutine("PathCursorDisplay");
                if (pathEnd_cursor != null)
                {
                    ObjectPool.instance.PoolObject(pathEnd_cursor);
                }

                // Stop displaying selection circle
                if (battle_Control.selectionCircle != null)
                {
                    ObjectPool.instance.PoolObject(battle_Control.selectionCircle);
                }

                // Subtract an Action Point
                Player_ChargeActionPoints();
            }

        }
    }


    public void DisplayCursor(Vector3 position)
    {
        if (battle_Control.selectedPathController != null && battle_Control.selectedCharacter != null)
        {
            if (pathEnd_cursor != null)
            {
                ObjectPool.instance.PoolObject(pathEnd_cursor);
            }

            pathEnd_cursor = ObjectPool.instance.GetObjectForType("PathEnd", true, position);


            if (CalcDistanceToCursor(battle_Control.selectedPathController.transform.position) > battle_Control.selectedPathController.CurrMovePoints)
            {
                pathEnd_cursor.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (Mouse_Controller.Instance.MouseOverEnemy && CalcDistanceToCursor(battle_Control.selectedPathController.transform.position) <= battle_Control.selectedPathController.CurrMovePoints)
            {
                pathEnd_cursor.GetComponent<SpriteRenderer>().color = Color.cyan;
            }

            else
            {
                pathEnd_cursor.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public int CalcDistanceToCursor(Vector3 origin)
    {
        var distance = Vector2.Distance(origin, pathEnd_cursor.transform.position);
        return Mathf.FloorToInt(distance);
    }

    public void Player_ChargeActionPoints()
    {
        if (battle_Control.selectedCharacter != null)
        {
            if (battle_Control.selectedCharacter.currActionPoints > 1)
            {
                battle_Control.selectedCharacter.TakeAP();
                // If this character still has AP left, display cursor again
                StartCoroutine("PathCursorDisplay");
            }
            else
            {
                battle_Control.selectedCharacter.TakeAP();

                // Stop displaying cursor in case it's running
                StopCoroutine("PathCursorDisplay");

                //PlayerEndTurn();
            }

        }
    }

    public void EndAction()
    {
        if (battle_Control.selectedCharacter.currActionPoints < 1)
        {
            PlayerEndTurn();
        }
    }

    public void PlayerEndTurn()
    {
        if (battle_Control.selectedCharacter != null)
        {
            battle_Control.selectedCharacter.EndTurn();

            Debug.Log(battle_Control.selectedCharacter.myChar.Name + " ENDING TURN");
            if (pathEnd_cursor != null)
            {
                ObjectPool.instance.PoolObject(pathEnd_cursor);
            }

            // Check if this is the end of player's turn
            battle_Control.ActivePCChars--;

            if (battle_Control.ActivePCChars == 0)
            {
                // Check for Victory conditions and if not met, start Enemy's turn
                Battle_StateManager.Instance.EndPlayerTurn();

            }
            else
            {
                // Select the next character
                battle_Control.PCSquadIndex++;
                if (battle_Control.PCSquadIndex < Battle_Loader.Instance.playerSquad.Length)
                {
                    battle_Control.SelectPCCharacter(Battle_Loader.Instance.playerSquad[battle_Control.PCSquadIndex].Name);
                }
            }
        }
    }
}
