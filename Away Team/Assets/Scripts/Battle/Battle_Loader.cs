using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle_Loader : MonoBehaviour {

    // This script will need to know what Player Characters are currently in the squad. 
    // The Squad will be constructed before this is called, so that data can be handed to this.

    public static Battle_Loader Instance { get; protected set; }

    // FIX THIS! None of the character data should be hardcoded!
    public PC_Character[] playerSquad { get; protected set; }
    public bool isPlayerSquadLoaded { get; protected set; }

    NPC_Character[] enemySquad;
    public bool isEnemiesLoaded { get; protected set; }

    public Dictionary<string, Character_Handler> playerSquadMap { get; protected set; }

    void OnEnable()
    {
        Instance = this;
    }

    void Awake()
    {
        InitPlayerSquad();
        InitEnemySquad();
    }

    void InitPlayerSquad()
    {
        playerSquad = new PC_Character[]
        {
            new PC_Character("Chess", PCClass.Wildshot),
            new PC_Character("Iza", PCClass.Technobrat)
        };

        playerSquad[0].InitStats(20, 6, 7, 5, 9, 6);
        playerSquad[1].InitStats(20, 5, 10, 5, 7, 8);

    }

    void InitEnemySquad()
    {
        enemySquad = new NPC_Character[]
        {
            new NPC_Character("Grunt", NPCClass.Grunt),
            new NPC_Character("Grunt", NPCClass.Grunt)
        };

        enemySquad[0].InitStats(20, 5, 5, 5, 5, 5);
        enemySquad[1].InitStats(20, 5, 5, 5, 5, 5);
    }


    public void SpawnPlayerSquad()
    {
        // Each class will have a unique sprite, so we spawn a gameobject by the character class.
        float x = 0;

        // Init dictionary to store references to Character Handlers
        playerSquadMap = new Dictionary<string, Character_Handler>();

        foreach (PC_Character character in playerSquad)
        {
            GameObject pc = ObjectPool.instance.GetObjectForType(character.pc_class.ToString(), true, new Vector3(x, 0, 0));

            pc.GetComponent<Character_Handler>().SetCharacter(character);

            x += 3;

            playerSquadMap.Add(character.Name, pc.GetComponent<Character_Handler>());
        }

        isPlayerSquadLoaded = true;

        // Set total # of units on Battle_Controller
        Battle_Controller.Instance.InitPCCharacters(playerSquad.Length);
    }

    public void SpawnEnemies()
    {
        // Each class will have a unique sprite, so we spawn a gameobject by the character class.
        float x = 0;
        foreach (NPC_Character character in enemySquad)
        {
            GameObject npc = ObjectPool.instance.GetObjectForType(character.npc_class.ToString(), true, new Vector3(x, 9, 0));

            npc.GetComponent<Enemy_Handler>().SetCharacter(character);

            x += 3;
        }

        isEnemiesLoaded = true;

        // Set total # of units on Battle_Controller
        Battle_Controller.Instance.InitNPCCharacters(enemySquad.Length);
    }

}
