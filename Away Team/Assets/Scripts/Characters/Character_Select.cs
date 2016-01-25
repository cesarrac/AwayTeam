using UnityEngine;
using System.Collections;

public class Character_Select : MonoBehaviour
{
    Character_Handler char_handler;
    void Awake()
    {
       
        char_handler = GetComponent<Character_Handler>();
    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Battle_Controller.Instance.SelectCharacter(char_handler);

            // Display info in portrait panel
            UI_Manager.Instance.DisplaySelectedCharacter(char_handler.myChar.Name);
        }
    }
}
