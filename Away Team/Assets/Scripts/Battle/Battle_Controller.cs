using UnityEngine;
using System.Collections;

public class Battle_Controller : MonoBehaviour {

	// This script will take care of executing actions for the Unit whose turn it is

    public static Battle_Controller Instance { get; protected set; }

    public PathController selectedPathController { get; protected set; }
    public Character_Handler selectedCharacter { get; protected set; }

    GameObject selectionCircle, pathEnd;

    Enemy_Handler currEnemy;

    int totalPCcharacters;
    public int TotalPCChars { get { return totalPCcharacters; } set { totalPCcharacters = value; } }
    int activePCChars;

    int totalNPCcharacters;
    public int TotalNPCChars { get { return totalNPCcharacters; } set { totalNPCcharacters = value; } }
    int activeNPCChars;


    void OnEnable()
    {
        Instance = this;
    }

    public void InitPCCharacters(int total)
    {
        totalPCcharacters = total;
        activePCChars = total;

    }

    public void InitNPCCharacters(int total)
    {

        totalNPCcharacters = total;
        activeNPCChars = total;
    }

    public void SelectCharacter(Character_Handler char_handler)
    {
    
        // Pool any selection circles that are already active
        if (selectionCircle != null)
        {
            ObjectPool.instance.PoolObject(selectionCircle);
        }
        
        // Get a new Selecition circle from Pool and place under selected character
        selectionCircle = ObjectPool.instance.GetObjectForType("Selection Circle", true, char_handler.transform.position);

        // Center camera on Selected character
        Camera_Controller.Instance.CenterOnCharacter(char_handler.transform.position);

        // Set the path controller
        selectedPathController = char_handler.path_controller;

        // Set the character handler
        selectedCharacter = char_handler;

        // Display starting AP
        UI_Manager.Instance.DisplayCharacterAP(selectedCharacter.currActionPoints);
    }

    public void DisplayCursor(Vector3 position)
    {
        if (selectedPathController != null && selectedCharacter != null)
        {
            if (pathEnd != null)
            {
                ObjectPool.instance.PoolObject(pathEnd);
            }

            pathEnd = ObjectPool.instance.GetObjectForType("PathEnd", true, position);


            if (CalcDistanceToCursor(selectedPathController.transform.position) > selectedPathController.GetCurMovePoints())
            {
                pathEnd.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (Mouse_Controller.Instance.MouseOverEnemy && CalcDistanceToCursor(selectedPathController.transform.position) <= selectedPathController.GetCurMovePoints())
            {
                pathEnd.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
              
            else
            {
                pathEnd.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public int CalcDistanceToCursor(Vector3 origin)
    {
        var distance = Vector2.Distance(origin, pathEnd.transform.position);
        return Mathf.FloorToInt(distance);
    }


    public void Move(Vector3 pathTarget)
    {
        if (selectedPathController != null && selectedCharacter != null)
        {
            if (selectedCharacter.currActionPoints > 0 && selectedPathController.GetCurMovePoints() > 0)
            {
                selectedPathController.RequestPath(pathTarget);


                if (selectionCircle != null)
                {
                    ObjectPool.instance.PoolObject(selectionCircle);
                }

                // Subtract an Action Point
                TakeActionPoint();
            }
    
        }
    }

    public void Stop()
    {
        if (selectedPathController != null)
        {
            selectedPathController.StopMovement();
        }
    }

    public void TakeActionPoint()
    {
        if (selectedCharacter != null)
        {
            if (selectedCharacter.currActionPoints > 1)
            {
                selectedCharacter.TakeAP();
            }
            else
            {
                selectedCharacter.TakeAP();
                EndTurn();
            }
            
        }
    }

    // Attack Controls
    public void RangedAttack(float damage)
    {
        if (selectedCharacter != null)
        {
            // Do Damage
            if (Mouse_Controller.Instance.EnemyUnderMouse != null)
            {
                currEnemy = Mouse_Controller.Instance.EnemyUnderMouse;
                DamageUnit(10, currEnemy);
            }

            // Subtract AP
            TakeActionPoint();
        }
   

    }

    void DamageUnit(float damage, Enemy_Handler enemy)
    {
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            enemy.TakeDamage(damage);
            Debug.Log(enemy.myChar.Name + " takes " + damage + " damage!");
        }

    }

    public void EndTurn()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.EndTurn();

            if (pathEnd != null)
            {
                ObjectPool.instance.PoolObject(pathEnd);
            }

            // Check if this is the end of player's turn
            activePCChars--;

            if (activePCChars == 0)
            {
                // Start enemy's turn
                Battle_StateManager.Instance.EndPlayerTurn();
            }
        }
    }
}
