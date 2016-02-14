using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle_Controller : MonoBehaviour {

	// This script will take care of executing actions for the Unit whose turn it is

    public static Battle_Controller Instance { get; protected set; }

    public PathController selectedPathController { get; protected set; }
    public Character_Handler selectedCharacter { get; protected set; }
    public Enemy_Handler selectedNPC { get; protected set; }

    public GameObject selectionCircle { get; protected set; }

    int totalPCcharacters;
    public int TotalPCChars { get { return totalPCcharacters; } set { totalPCcharacters = value; } }
    int activePCChars;
    public int ActivePCChars { get { return activePCChars; } set { activePCChars = value; } }

    int totalNPCcharacters;
    public int TotalNPCChars { get { return totalNPCcharacters; } set { totalNPCcharacters = value; } }
    int activeNPCChars;
    public int ActiveNPCChars { get { return activeNPCChars; } set { activeNPCChars = value; } }

    int pcSquadIndex = 0;
    public int PCSquadIndex { get { return pcSquadIndex; } set { pcSquadIndex = value; } }

    int npcSquadIndex = 0;
    public int NPCSquadIndex { get { return npcSquadIndex; } set { npcSquadIndex = value; } }

    public Dictionary<Vector2, Character_Handler> PCPositionsMap { get; protected set; }
    public Dictionary<Vector2, Enemy_Handler> NPCPositionsMap { get; protected set; }

    void Awake()
    {
        Instance = this;
        PCPositionsMap = new Dictionary<Vector2, Character_Handler>();
        NPCPositionsMap = new Dictionary<Vector2, Enemy_Handler>();
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


    public void StartPlayerTurn()
    {
        PCSquadIndex = 0;
        ActivePCChars = totalPCcharacters;
        // null the selected player
        selectedCharacter = null;

        ChooseWhatPCToSelect();
        Debug.Log("Starting Player Turn!");
    }

    void ChooseWhatPCToSelect()
    {
        if (Battle_StateManager.Instance._state == BattleState.PLAYER_TURN && selectedCharacter == null)
        {
            // Select the first character in the player squad that is still alive (still in the squad map)
            if (Battle_Loader.Instance.playerSquadMap.Count > 0)
            {
                for (int i = 0; i < Battle_Loader.Instance.playerSquad.Length; i++)
                {
                    if (Battle_Loader.Instance.playerSquadMap.ContainsKey(Battle_Loader.Instance.playerSquad[i].Name))
                    {
                        SelectPCCharacter(Battle_Loader.Instance.playerSquad[i].Name);
                        break;
                    }
                }
            }
            else
            {
                Battle_StateManager.Instance.Lose();
            }
        }
    }

    public void SelectPCCharacter(string charName)
    {
        if (Battle_Loader.Instance.playerSquadMap.Count > 0)
        {
            if (Battle_Loader.Instance.playerSquadMap.ContainsKey(charName))
            {
                // Set the character handler
                selectedCharacter = Battle_Loader.Instance.playerSquadMap[charName];

                // Pool any selection circles that are already active
                if (selectionCircle != null)
                {
                    ObjectPool.instance.PoolObject(selectionCircle);
                }

                // Get a new Selection circle from Pool and place under selected character
                selectionCircle = ObjectPool.instance.GetObjectForType("Selection Circle", true, selectedCharacter.transform.position);

                // Center camera on Selected character
                Camera_Controller.Instance.CenterOnCharacter(selectedCharacter.transform.position);

                // Set the path controller
                selectedPathController = selectedCharacter.path_controller;

                // Start the path cursor display
                Player_BattleController.Instance.StartPathCursor();

                // Reset its Start of Turn values
                selectedCharacter.StartTurn();

                // Display starting AP
                UI_Manager.Instance.DisplayCharacterAP(selectedCharacter.currActionPoints);
            }
        }
        else
        {
            Battle_StateManager.Instance.Lose();
        }

      
    }

    public void SetNewPCPosition(Vector2 newPos, Character_Handler pc)
    {
        if (!PCPositionsMap.ContainsKey(newPos))
        {
            PCPositionsMap.Add(newPos, pc);
            Debug.Log("BATTLE CONTROL: Set new PC position at " + newPos);
        }
    }

    public void ReSetPCPosition(Vector2 oldPos, Vector2 newPos, Character_Handler pc)
    {
        if (PCPositionsMap.ContainsKey(oldPos))
        {
            PCPositionsMap.Remove(oldPos);
            Debug.Log("BATTLE CONTROL: Removed PC position at " + oldPos);

            SetNewPCPosition(newPos, pc);
        }
        else
        {
            SetNewPCPosition(newPos, pc);
        }
    }

    public Character_Handler GetPCAtVector2(Vector2 pos)
    {
        if (PCPositionsMap.ContainsKey(pos))
        {
            return PCPositionsMap[pos];
        }
        else
            return null;
    }


    public void StartEnemyTurn()
    {
        NPCSquadIndex = 0;
        activeNPCChars = totalNPCcharacters;
        SelectNPCCharacter();
    }

    public void ContinueEnemyTurn()
    {
        // Here we check if there are any enemies left in the NPC Squad

        // If there are, select next one...

        // If not, go to Player's turn.
    }

    public void SelectNPCCharacter()
    {
        // Get the enemy using the NPC Squad Index
        if (Battle_Loader.Instance.enemySquadMap.ContainsKey(NPCSquadIndex))
        {
            // Set the enemy handler
            selectedNPC = Battle_Loader.Instance.enemySquadMap[NPCSquadIndex];

            // Set the path controller
            selectedPathController = selectedNPC.path_controller;

            // Start their turn
            selectedNPC.StartTurn();
        }
        else
        {
            Battle_StateManager.Instance.EndEnemyTurn();
        }
    }


    public void SetNewNPCPosition(Vector2 newPos, Enemy_Handler NPC)
    {
        if (!NPCPositionsMap.ContainsKey(newPos))
        {
            NPCPositionsMap.Add(newPos, NPC);
            Debug.Log("BATTLE CONTROL: Set new NPC position at " + newPos);
        }
    }

    public void ReSetNPCPosition(Vector2 oldPos, Vector2 newPos, Enemy_Handler NPC)
    {
        if (NPCPositionsMap.ContainsKey(oldPos))
        {
            NPCPositionsMap.Remove(oldPos);
            Debug.Log("BATTLE CONTROL: Removed NPC position at " + oldPos);

            SetNewNPCPosition(newPos, NPC);
        }
        else
        {
            SetNewNPCPosition(newPos, NPC);
        }
    }

    public Enemy_Handler GetNPCAtVector2(Vector2 pos)
    {
        if (NPCPositionsMap.ContainsKey(pos))
        {
            return NPCPositionsMap[pos];
        }
        else
            return null;
    }





    public void StopMovement()
    {
        if (selectedPathController != null)
        {
            selectedPathController.StopMovement();
        }
    }

    public void KillPC(Character_Handler pc)
    {
        if (Battle_Loader.Instance.playerSquadMap.ContainsKey(pc.myChar.Name))
        {
            // Remove from player squad dictionary
            Battle_Loader.Instance.playerSquadMap.Remove(pc.myChar.Name);
        }
     

        // Check if player has lost all their units
        if (Battle_Loader.Instance.playerSquadMap.Count < 1)
        {
            Battle_StateManager.Instance.Lose();
        }
    }

    public void KillNPC(int enemyIndex)
    {
        if (Battle_Loader.Instance.enemySquadMap.ContainsKey(enemyIndex))
        {
            // Remove from enemy squad dictionary
            Battle_Loader.Instance.enemySquadMap.Remove(enemyIndex);
        }


        // Check if enemy has lost all their units
        if (Battle_Loader.Instance.enemySquadMap.Count < 1)
        {
            Battle_StateManager.Instance.Win();
        }
    }
}
