using UnityEngine;
using System.Collections;

public class Enemy_Detect : MonoBehaviour {


	void OnMouseOver()
    {
        Mouse_Controller.Instance.MouseOverEnemy = true;
        Mouse_Controller.Instance.EnemyUnderMouse = GetComponent<Enemy_Handler>();
    }

    void OnMouseExit()
    {
        Mouse_Controller.Instance.MouseOverEnemy = false;
        Mouse_Controller.Instance.EnemyUnderMouse = null;
    }
}
