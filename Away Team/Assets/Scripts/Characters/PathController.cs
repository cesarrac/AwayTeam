using UnityEngine;
using System.Collections;

public class PathController : MonoBehaviour {
    Vector3[] path;

    int targetIndex = 0;

    Vector3 currPathTarget;

    int maxMovePoints, curMovePoints;

    void Start()
    {
        maxMovePoints = GetComponent<Character_Handler>().myChar.movementRange;
        curMovePoints = maxMovePoints;
    }

    void Update()
    {
        Debug.Log("Current Move Points: " + curMovePoints);
    }



    public void RequestPath(Vector3 pathPosition)
    {
        currPathTarget = pathPosition;
        PathRequestManager.RequestPath(transform.position, currPathTarget, gameObject, OnPathFound);
    }

    

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            Debug.Log("Found Path!");
            // Move
            path = newPath;
            StartCoroutine("FollowPath");
        }
        else
        {
            Debug.Log("Could not get path to " + currPathTarget);
        }
    }


    IEnumerator FollowPath()
    {
        //if (path.Length > 0)
        //{
          
        //}
        //else
        //{

        //    yield break;
        //}

        Vector3 currWayPoint = path[0];
        Debug.Log("Path: Current Waypoint is " + currWayPoint);
        while (true)
        {
            // Movement is limited to movement points available
            if (curMovePoints > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, currWayPoint, 2f * Time.deltaTime);
            }
            else
            {
                // No Movement Points left on this action

                if (GetComponent<Character_Handler>().currActionPoints > 0)
                {
                    // If this character has AP left, reset its move range
                    ResetMovementRange();
                }

                targetIndex = 0;
                path = null;
                yield break;
            }
         

            if (transform.position == currWayPoint)
            {
                targetIndex++;

                // Once we reach the next waypoint, subtract from movement points
                curMovePoints--;

                if (targetIndex >= path.Length)
                {
                    // Reached end of path, check if any AP left
                    if (GetComponent<Character_Handler>().currActionPoints > 0)
                    {
                        // If this character has AP left, reset its move range
                        ResetMovementRange();
                    }

                    targetIndex = 0;
                    path = null;
                    //StopCoroutine("RequestPath");

                    yield break;
                }

                currWayPoint = path[targetIndex];

            }




            yield return null;
        }

    }

    public void StopMovement()
    {
        StopCoroutine("FollowPath");
        targetIndex = 0;
    }


    // Movement Range will need to be reset after each turn
    public void ResetMovementRange()
    {
        curMovePoints = maxMovePoints;
    }

    public int GetCurMovePoints()
    {
        return curMovePoints;
    }
}
