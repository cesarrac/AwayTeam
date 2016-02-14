using UnityEngine;
using System.Collections;

public class Wildshot_Controller : MonoBehaviour {

    Character_Handler char_handler;

    int range = 5;
    bool isInRange;

    void Start()
    {
        char_handler = GetComponent<Character_Handler>();
    }

    void Update()
    {
        CheckForEnemies();

        if (isInRange)
        {
            ListenForAttack();
        }
    }

    void CheckForEnemies()
    {
        if (Mouse_Controller.Instance.MouseOverEnemy)
        {
            CheckRange();
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
            }
        }
    }

    void CheckRange()
    {
        if (Player_BattleController.Instance.CalcDistanceToCursor(transform.position) <= char_handler.myChar.attackRange)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }

    void ListenForAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Player_BattleController.Instance.PlayerRangedAttack(10);

        }
    }



}
