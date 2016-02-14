using UnityEngine;
using System.Collections;

public class Enemy_BattleController : MonoBehaviour {

    public static Enemy_BattleController Instance { get; protected set; }

    public Character_Handler currTarget { get; protected set; }

   // GameObject pathEnd_cursor;

    Battle_Controller battle_Control;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        battle_Control = Battle_Controller.Instance;
    }

    public bool CheckForPlayers(int originX, int originY, int range)
    {
        // Looping through the tiles in an area surrounding this enemy, limited by range
        for (int x = originX - range; x < originX + range; x++)
        {
            for (int y = originY - range; y < originY + range; y++)
            {
               foreach(Character_Handler pc in Battle_Loader.Instance.playerSquadMap.Values)
                {
                    if (pc.GetCurrentTileCoords() == new Vector2(x, y))
                    {
                        Debug.Log("ENEMY: Found target at " + new Vector2(x, y));
                        currTarget = pc;
                        return true;
                    }
                }
            }
        }

        Debug.Log("ENEMY BATTLE: Found no player characters in range!");
        return false;
     
    }

    public void SetTarget(Character_Handler pc)
    {
        if (currTarget == null || currTarget.gameObject.activeSelf == false)
            currTarget = pc;
    }

    // Attack Controls
    public void EnemyRangedAttack(float damage)
    {
        if (battle_Control.selectedNPC != null)
        {
            // Do Damage
            if (currTarget != null)
            {
                DoDamage(currTarget, damage);
            }

            // Subtract AP
            Enemy_ChargeActionPoints();

            // End Action
            EndAction();
        }


    }

    void DoDamage(Character_Handler character, float dmg)
    {
        Debug.Log("ENEMY: Damaging " + character.myChar.Name + " for " + dmg);
        character.TakeDamage(dmg);
    }



    public void EnemyMoveKnownTarget()
    {
        if (currTarget != null)
        {
            if (battle_Control.selectedPathController != null && battle_Control.selectedNPC != null)
            {
                if (battle_Control.selectedNPC.currActionPoints > 0 && battle_Control.selectedPathController.CurrMovePoints > 0)
                {
                    battle_Control.selectedPathController.RequestPath(currTarget.transform.position);

                    // Subtract an Action Point
                    Enemy_ChargeActionPoints();
                }

            }
        }

    }

    public void EnemyMoveToSpecificPath(Vector3 pathLocation)
    {
        if (battle_Control.selectedPathController != null && battle_Control.selectedNPC != null)
        {
            if (battle_Control.selectedNPC.currActionPoints > 0 && battle_Control.selectedPathController.CurrMovePoints > 0)
            {
                battle_Control.selectedPathController.RequestPath(pathLocation);

                // Subtract an Action Point
                Enemy_ChargeActionPoints();
            }

        }
    }




    public void Enemy_ChargeActionPoints()
    {
        if (battle_Control.selectedNPC != null)
        {
            if (battle_Control.selectedNPC.currActionPoints > 1)
            {
                battle_Control.selectedNPC.TakeAP();
            }
            else
            {
                battle_Control.selectedNPC.TakeAP();

                //EndTurn();
            }

        }
    }

    public void EndAction()
    {
        if (battle_Control.selectedNPC.currActionPoints < 1)
        {
            EndTurn();
        }
        else
        {
            battle_Control.selectedNPC.StartThinkingNextDecision();
        }
    }

    public void EndTurn()
    {
        if (battle_Control.selectedNPC != null)
        {
            battle_Control.selectedNPC.EndTurn();

            // Check if this is the end of player's turn
            battle_Control.ActiveNPCChars--;

            if (battle_Control.ActiveNPCChars == 0)
            {
                // Check for Victory conditions and if not met, start Player's turn
                Battle_StateManager.Instance.EndEnemyTurn();
            }
            else
            {
                // Select the next NPC
                battle_Control.NPCSquadIndex++;

                if (battle_Control.NPCSquadIndex < Battle_Loader.Instance.enemySquad.Length)
                {

                    battle_Control.SelectNPCCharacter();
                }
                else
                {
                    Battle_StateManager.Instance.EndEnemyTurn();
                }

            }
        }
    }
}
