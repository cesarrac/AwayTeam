using UnityEngine;
using System.Collections;

public class Camera_Controller : MonoBehaviour {

	public static Camera_Controller Instance { get; protected set; }

    Vector3 centerPosition;

    void OnEnable()
    {
        Instance = this;
    }

    public void Drag(Vector3 diff)
    {
        Camera.main.transform.Translate(diff);
    }

    public void CenterOnCharacter(Vector3 pos)
    {
        pos.z = transform.position.z;
        centerPosition = pos;

        transform.position = centerPosition;

        //StartCoroutine("CenterOnChar");
    }

    IEnumerator CenterOnChar()
    {
        while (true)
        {
            if (transform.position != centerPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, centerPosition, 5 * Time.deltaTime);

            }
            else
            {
                yield break;
            }

            yield return null;
        }
    }
}
