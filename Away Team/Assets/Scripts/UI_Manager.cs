using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager Instance { get; protected set; }

    public Text charName, charAP;
    public Image charPortrait;

    void OnEnable()
    {
        Instance = this;
    }

    public void DisplaySelectedCharacter(string name)
    {
        //string path = "Sprites/" + name;
        //Debug.Log("PATH " + path);

        //charPortrait.sprite = Resources.Load("Sprites/" + name.ToString()) as Sprite;

        charName.text = name;
    }

    public void DisplayCharacterAP(int ap)
    {
        charAP.text = ap.ToString();
    }

}
