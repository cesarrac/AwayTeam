using UnityEngine;
using System.Collections;


public enum BattleState
{
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    LOSE,
    WIN,
    NEXT
}
public class Battle_StateManager : MonoBehaviour {

    public static Battle_StateManager Instance { get; protected set; }

    BattleState _state = BattleState.START;
    BattleState lastState;

    void OnEnable()
    {
        Instance = this;
    }

    void Update()
    {
        if (lastState != _state)
        {
            Debug.Log("BATTLE STATE is " + _state.ToString());
            lastState = _state;
        }

        StateMachine(_state);

        

    }

    void StateMachine(BattleState curState)
    {
        switch (curState)
        {
            case BattleState.START:
                // Start the battle by loading the necessary resources
                Debug.Log("Starting the Battle!");
                // Load the Player Characters
                if (Battle_Loader.Instance.isPlayerSquadLoaded == false)
                {
                    Battle_Loader.Instance.SpawnPlayerSquad();
                }
                else if (Battle_Loader.Instance.isEnemiesLoaded == false)
                {
                    // Load the Enemies
                    Battle_Loader.Instance.SpawnEnemies();
                }
                else
                {
                    _state = BattleState.PLAYER_TURN;
                }
                

                break;
            case BattleState.PLAYER_TURN:
                // Start the player's turn with the first available Squad member selected
                break;
            case BattleState.ENEMY_TURN:
                // Start the enemy's turn with first available enemy, then continue with the rest of the enemy unit's until they have all taken an action
                break;
            case BattleState.LOSE:
                // Tell the player they have lost the battle and give them an option to restart the level
                break;
            case BattleState.WIN:
                // Tell the player they have won and go back to the ship level showing the results of the battle
                break;
            case BattleState.NEXT:
                // Reset all unit movement and action point values then go to Player's turn again

            default:
                // Default: do nothing
                break;

        }
    }

    public void EndPlayerTurn()
    {
        if (_state == BattleState.PLAYER_TURN)
        {
            Debug.Log("STATE: Ending Player turn");
            _state = BattleState.ENEMY_TURN;
        }
    }

    public void EndEnemyTurn()
    {
        if (_state == BattleState.ENEMY_TURN)
        {
            // Check if Player has Won or Lost. If neither, go to Next

        }
    }


    public void TestStates()
    {
        // Change states at press of a button. This is a TEST!
        if (_state == BattleState.START)
        {
            _state = BattleState.PLAYER_TURN;
        }

        else if (_state == BattleState.PLAYER_TURN)
        {
            _state = BattleState.ENEMY_TURN;
        }

        else if (_state == BattleState.ENEMY_TURN)
        {
            _state = BattleState.LOSE;
        }

        else if (_state == BattleState.LOSE)
        {
            _state = BattleState.WIN;
        }

        Debug.Log(_state.ToString());
    }
    
}
